using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PostService.Application.Exceptions;
using PostService.Application.Features.Commands;
using PostService.Application.Features.Common;
using PostService.Application.Mapping;
using PostService.Application.Posts;
using PostService.Application.Security;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Features.Handler
{
    public class SetPostActivenessCommandHandler : IRequestHandler<SetPostActivenessCommand, StandardResponse<PostResponse>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICurrentUser _currentUser;
        private readonly PostOptions _postOptions;
        private readonly OwnerPostExpirer _ownerPostExpirer;

        public SetPostActivenessCommandHandler(IPostRepository postRepository, ICurrentUser currentUser, IOptions<PostOptions> postOptions, OwnerPostExpirer ownerPostExpirer)
        {
            _ownerPostExpirer = ownerPostExpirer;
            _postRepository = postRepository;
            _currentUser = currentUser;
            _postOptions = postOptions.Value;
        }

        public async Task<StandardResponse<PostResponse>> Handle(SetPostActivenessCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            var response = new StandardResponse<PostResponse>();

            var post = await _postRepository.GetAll()
                .Include(x => x.Hospital)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken);

            if (post == null)
                throw new NotFoundException("Post not found.");

            if (userId.ToString() != post.OwnerId)
                throw new UnauthorizedAccessException("Not authorized!");

            // "Live" rather than IsActive: an expired post may not have been deactivated by PostExpiryService yet,
            // and reactivating it must still start a new period.
            var isLive = post.IsActive && post.ExpiresAt > DateTime.UtcNow;
            if (!request.IsActive.HasValue || request.IsActive.Value == isLive)
            {
                response.Response = post.ToResponse();
                return response;
            }

            if (request.IsActive.Value)
            {
                await _ownerPostExpirer.ExpireDuePostsAsync(post.OwnerId, cancellationToken);
                if (await _postRepository.GetAll().WhereLive().AnyAsync(x => x.OwnerId == post.OwnerId && x.Id != post.Id, cancellationToken))
                    throw new ConflictException("You already have an active post.");

                post.IsActive = true;
                post.ExpiresAt = DateTime.UtcNow.Add(_postOptions.ActiveDuration);
            }
            else
            {
                post.IsActive = false;
            }

            await _postRepository.UpdateAsync(post);
            response.ResultMessage = "Post activation status updated successfully.";
            response.Response = post.ToResponse();
            return response;
        }
    }
}
