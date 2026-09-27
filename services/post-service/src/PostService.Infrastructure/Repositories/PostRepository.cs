using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Infrastructure.Data;
using System.Linq.Expressions;

namespace PostService.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly PostServiceDbContext _dbContext;

        public PostRepository(PostServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Post?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<Post>().FindAsync(id);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _dbContext.Set<Post>().ToListAsync();
        }

        public void Add(Post post)
        {
            _dbContext.Set<Post>().Add(post);
        }

        public void Remove(Post post)
        {
            _dbContext.Set<Post>().Remove(post);
        }

        public async Task AddAsync(Post post)
        {
            await _dbContext.Set<Post>().AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post)
        {
            _dbContext.Set<Post>().Update(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = await GetByIdAsync(id);
            if (post != null)
            {
                _dbContext.Set<Post>().Remove(post);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Post>> GetByExpressionAsync(Expression<Func<Post, bool>> expression)
        {
            return await GetAll(expression).ToListAsync();
        }

        public IQueryable<Post> GetAll(Expression<Func<Post, bool>>? expression = null)
        {
            if (expression == null)
            {
                return _dbContext.Set<Post>().AsQueryable();
            }
            return _dbContext.Set<Post>().Where(expression);
        }

    }
}
