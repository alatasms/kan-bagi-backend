using MediatR;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Exceptions;
using PostService.Application.Features.Commands;
using PostService.Application.Features.Common;
using PostService.Application.Mapping;
using PostService.Application.Security;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Features.Handler
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, StandardResponse<PostResponse>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly ICurrentUser _currentUser;

        public UpdatePostCommandHandler(IPostRepository postRepository, IHospitalRepository hospitalRepository, ICurrentUser currentUser)
        {
            _postRepository = postRepository;
            _hospitalRepository = hospitalRepository;
            _currentUser = currentUser;
        }

        public async Task<StandardResponse<PostResponse>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            // Only the editable fields change; owner, creation time, correlation id, activeness and expiry stay.
            var post = await _postRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException("Post not found!");

            if (post.OwnerId != _currentUser.UserId.ToString())
                throw new UnauthorizedAccessException("Not Authorized!");

            if (post.HospitalId != request.HospitalId && await _hospitalRepository.GetByIdAsync(request.HospitalId) == null)
                throw new NotFoundException("Hospital not found");

            post.PatientFullName = request.PatientFullName;
            post.PatientAge = request.PatientAge;
            post.Title = request.Title;
            post.Description = request.Description;
            post.SetPhoneNumbers(request.PhoneNumbers);
            post.BloodType = request.BloodType;
            post.HospitalId = request.HospitalId;

            await _postRepository.UpdateAsync(post);

            var updated = await _postRepository.GetAll().AsNoTracking()
                .Include(x => x.Hospital)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Country)
                .FirstAsync(x => x.Id == post.Id, cancellationToken);

            return new StandardResponse<PostResponse>
            {
                Response = updated.ToResponse()
            };
        }
    }
}
