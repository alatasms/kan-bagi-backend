using PostService.Domain.Entities;
using System.Linq.Expressions;

namespace PostService.Infrastructure.Repositories
{
    public interface IPostRepository
    {
        Task AddAsync(Post post);
        /// <summary>Stage changes; persisted by IUnitOfWork.SaveChangesAsync.</summary>
        void Add(Post post);
        void Remove(Post post);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task<IEnumerable<Post>> GetByExpressionAsync(Expression<Func<Post, bool>> expression);
        Task<Post?> GetByIdAsync(Guid id);
        IQueryable<Post> GetAll(Expression<Func<Post, bool>>? expression = null);
        Task UpdateAsync(Post post);
    }
}
