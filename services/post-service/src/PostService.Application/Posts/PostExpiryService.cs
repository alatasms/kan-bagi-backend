using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PostService.Application.Events;
using PostService.Infrastructure.Data;

namespace PostService.Application.Posts
{
    /// <summary>
    /// Deactivates posts whose ExpiresAt has passed and announces it with `user-post-expired`.
    /// </summary>
    public class PostExpiryService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly PostOptions _options;
        private readonly ILogger<PostExpiryService> _logger;

        public PostExpiryService(IServiceScopeFactory scopeFactory, IOptions<PostOptions> options, ILogger<PostExpiryService> logger)
        {
            _scopeFactory = scopeFactory;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_options.ExpiryCheckInterval);
            do
            {
                try
                {
                    var expired = await ExpireDuePostsAsync(stoppingToken);
                    if (expired > 0)
                        _logger.LogInformation("Expired {Count} post(s)", expired);
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "Expiring posts failed; retrying on the next tick");
                }
            } while (await timer.WaitForNextTickAsync(stoppingToken));
        }

        private async Task<int> ExpireDuePostsAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PostServiceDbContext>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            // One atomic statement claims the due posts, so with several instances running each post is
            // deactivated (and announced) by exactly one of them.
            var expiredIds = await dbContext.Database.SqlQuery<Guid>($"""
                UPDATE "Posts" SET "IsActive" = false, "LastModified" = now()
                WHERE "IsActive" AND NOT "IsDeleted" AND "ExpiresAt" <= now()
                RETURNING "Id" AS "Value"
                """).ToListAsync(cancellationToken);

            if (expiredIds.Count > 0)
            {
                var posts = await dbContext.Posts.AsNoTracking()
                    .Include(x => x.Hospital)
                    .ThenInclude(x => x.District)
                    .ThenInclude(x => x.City)
                    .ThenInclude(x => x.Country)
                    .Where(x => expiredIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                // Stored in the outbox inside this transaction: the deactivation and its event commit together.
                foreach (var post in posts)
                    await publishEndpoint.Publish(PostEvents.Expired(post, post.CorrelationId), cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            return expiredIds.Count;
        }
    }
}
