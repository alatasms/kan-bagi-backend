using PostService.Domain.Entities;

namespace PostService.Application.Posts
{
    public static class PostQueries
    {
        /// <summary>
        /// Posts that are active and not yet past ExpiresAt. PostExpiryService deactivates expired posts
        /// periodically; this keeps them out of results in the meantime.
        /// </summary>
        public static IQueryable<Post> WhereLive(this IQueryable<Post> posts)
        {
            var now = DateTime.UtcNow;
            return posts.Where(x => x.IsActive && x.ExpiresAt > now);
        }
    }
}
