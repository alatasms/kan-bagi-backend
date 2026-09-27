using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Infrastructure.Data;

namespace PostService.Infrastructure.Repositories
{
    public class PostOwnerRepository : IPostOwnerRepository
    {
        private readonly PostServiceDbContext _dbContext;

        public PostOwnerRepository(PostServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<PostOwner?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return _dbContext.PostOwners.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }

        public async Task UpsertAsync(PostOwner owner, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.PostOwners.FirstOrDefaultAsync(x => x.UserId == owner.UserId, cancellationToken);
            if (existing == null)
            {
                await _dbContext.PostOwners.AddAsync(owner, cancellationToken);
            }
            else
            {
                existing.Name = owner.Name;
                existing.Surname = owner.Surname;
                existing.UpdatedAt = owner.UpdatedAt;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
