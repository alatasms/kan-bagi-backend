using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Infrastructure.Data;
using System.Linq.Expressions;

namespace PostService.Infrastructure.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly PostServiceDbContext _dbContext;

        public HospitalRepository(PostServiceDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Hospital?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Hospital>().FindAsync(id);
        }

        public async Task<IEnumerable<Hospital>> GetAllAsync()
        {
            return await _dbContext.Set<Hospital>().ToListAsync();
        }

        public async Task AddAsync(Hospital hospital)
        {
            await _dbContext.Set<Hospital>().AddAsync(hospital);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Hospital hospital)
        {
            _dbContext.Set<Hospital>().Update(hospital);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hospital = await GetByIdAsync(id);
            if (hospital != null)
            {
                _dbContext.Set<Hospital>().Remove(hospital);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Hospital>> GetByExpressionAsync(Expression<Func<Hospital, bool>> expression)
        {
            return await GetAll(expression).ToListAsync();
        }

        public IQueryable<Hospital> GetAll(Expression<Func<Hospital, bool>>? expression = null)
        {
            if (expression == null)
            {
                return _dbContext.Set<Hospital>().AsQueryable();
            }
            return _dbContext.Set<Hospital>().Where(expression);
        }
    }
}
