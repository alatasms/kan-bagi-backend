using UserService.Domain.Entities;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UserService.Infrastructure.Repositories.Concretes
{
    public class UserRepository : IUserRepository
    {
        public readonly UserServiceDbContext _context;
        public readonly DbSet<User> _dbSet;

        public UserRepository(UserServiceDbContext context)
        {
            _context = context;
            _dbSet = context.Users;
        }

        public async Task<User> AddAsync(User entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public void Add(User entity)
        {
            _dbSet.Add(entity);
        }

        public Task<User?> GetByExpressionAsync(Expression<Func<User, bool>> predicate)
        {
            return _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }
    }
}
