using UserService.Domain.Entities;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.Repositories.Concretes
{
    public class NotificationPreferenceRepository : INotificationPreferenceRepository
    {
        public readonly UserServiceDbContext _context;
        public readonly DbSet<NotificationPreference> _dbSet;

        public NotificationPreferenceRepository(UserServiceDbContext context)
        {
            _context = context;
            _dbSet = context.NotificationPreferences;
        }

        public async Task<NotificationPreference> AddAsync(NotificationPreference entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void Add(NotificationPreference entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(NotificationPreference entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<NotificationPreference> GetAllAsQueryable()
        {
            return _dbSet.AsNoTracking().Where(x => !x.IsDeleted);
        }

        public async Task<IEnumerable<NotificationPreference>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<NotificationPreference?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<NotificationPreference> UpdateAsync(NotificationPreference entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
