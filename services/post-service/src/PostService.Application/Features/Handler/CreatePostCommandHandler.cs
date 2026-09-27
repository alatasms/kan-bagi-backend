using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PostService.Application.Events;
using PostService.Application.Exceptions;
using PostService.Application.Features.Commands;
using PostService.Application.Features.Common;
using PostService.Application.Mapping;
using PostService.Application.Posts;
using PostService.Application.Security;
using PostService.Infrastructure.Data;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Features.Handler
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, StandardResponse<PostResponse>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IPostOwnerRepository _postOwnerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICurrentUser _currentUser;
        private readonly PostOptions _postOptions;
        private readonly OwnerPostExpirer _ownerPostExpirer;

        public CreatePostCommandHandler(IPostRepository postRepository, IPublishEndpoint publishEndpoint,
            ICurrentUser currentUser, IHospitalRepository hospitalRepository, IPostOwnerRepository postOwnerRepository,
            IUnitOfWork unitOfWork, IOptions<PostOptions> postOptions, OwnerPostExpirer ownerPostExpirer)
        {
            _ownerPostExpirer = ownerPostExpirer;
            _postRepository = postRepository;
            _publishEndpoint = publishEndpoint;
            _currentUser = currentUser;
            _hospitalRepository = hospitalRepository;
            _postOwnerRepository = postOwnerRepository;
            _unitOfWork = unitOfWork;
            _postOptions = postOptions.Value;
        }

        public async Task<StandardResponse<PostResponse>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            // The name shown on the post comes from the owner's profile, not from the request.
            var owner = await _postOwnerRepository.GetAsync(userId, cancellationToken)
                ?? throw new ConflictException("Complete your profile before creating a post.");

            var ownerId = userId.ToString();
            await _ownerPostExpirer.ExpireDuePostsAsync(ownerId, cancellationToken);
            if (await _postRepository.GetAll().WhereLive().AnyAsync(x => x.OwnerId == ownerId, cancellationToken))
                throw new ConflictException("User already has an active post.");

            // Loaded with its location because the event and the response both describe the hospital.
            var hospital = await _hospitalRepository.GetAll()
                .Include(x => x.District)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == request.HospitalId, cancellationToken)
                ?? throw new NotFoundException("Hospital not found");

            var entity = request.ToPost();
            entity.OwnerId = ownerId;
            entity.OwnerName = owner.Name;
            entity.OwnerSurname = owner.Surname;
            entity.IsActive = true;
            entity.ExpiresAt = DateTime.UtcNow.Add(_postOptions.ActiveDuration);
            entity.Hospital = hospital;
            _postRepository.Add(entity);

            // Who gets notified is decided by the notification service. The event goes through the outbox:
            // it is stored with the post and delivered only if the post is saved.
            await _publishEndpoint.Publish(PostEvents.Created(entity), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new StandardResponse<PostResponse>
            {
                Response = entity.ToResponse()
            };
        }
    }
}
