using MassTransit;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Events;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Posts
{
    /// <summary>
    /// Deactivates an owner's posts that are past ExpiresAt but not yet picked up by PostExpiryService,
    /// so they neither block a new active post (IX_Posts_OneActivePerOwner) nor skip their expiry event.
    /// Changes are staged; the caller's SaveChangesAsync commits them together with its own.
    /// </summary>
    public class OwnerPostExpirer
    {
        private readonly IPostRepository _postRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public OwnerPostExpirer(IPostRepository postRepository, IPublishEndpoint publishEndpoint)
        {
            _postRepository = postRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task ExpireDuePostsAsync(string ownerId, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var duePosts = await _postRepository.GetAll()
                .Include(x => x.Hospital)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Country)
                .Where(x => x.OwnerId == ownerId && x.IsActive && x.ExpiresAt <= now)
                .ToListAsync(cancellationToken);

            foreach (var post in duePosts)
            {
                post.IsActive = false;
                await _publishEndpoint.Publish(PostEvents.Expired(post, post.CorrelationId), cancellationToken);
            }
        }
    }
}
