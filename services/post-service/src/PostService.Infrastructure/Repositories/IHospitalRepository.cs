using PostService.Domain.Entities;
using System.Linq.Expressions;

namespace PostService.Infrastructure.Repositories
{
    public interface IHospitalRepository
    {
        Task AddAsync(Hospital hospital);
        Task DeleteAsync(int id);
        Task<IEnumerable<Hospital>> GetAllAsync();
        Task<IEnumerable<Hospital>> GetByExpressionAsync(Expression<Func<Hospital, bool>> expression);
        Task<Hospital?> GetByIdAsync(int id);
        IQueryable<Hospital> GetAll(Expression<Func<Hospital, bool>>? expression = null);
        Task UpdateAsync(Hospital hospital);
    }
}
