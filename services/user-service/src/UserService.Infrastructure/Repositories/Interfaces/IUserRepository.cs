using UserService.Domain.Entities;
using System.Linq.Expressions;

namespace UserService.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User> AddAsync(User entity);
        /// <summary>Stages a new user; persisted by IUnitOfWork.SaveChangesAsync.</summary>
        void Add(User entity);
        Task<User?> GetByExpressionAsync(Expression<Func<User, bool>> predicate);
    }
}
