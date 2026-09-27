using PostService.Domain.Entities;

namespace PostService.Infrastructure.Repositories
{
    public interface IPostOwnerRepository
    {
        Task<PostOwner?> GetAsync(Guid userId, CancellationToken cancellationToken = default);
        Task UpsertAsync(PostOwner owner, CancellationToken cancellationToken = default);
    }
}
