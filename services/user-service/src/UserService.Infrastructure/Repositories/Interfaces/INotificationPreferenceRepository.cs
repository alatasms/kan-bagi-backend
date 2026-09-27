using UserService.Domain.Entities;

namespace UserService.Infrastructure.Repositories.Interfaces
{
    public interface INotificationPreferenceRepository
    {
        Task<NotificationPreference> AddAsync(NotificationPreference entity);
        /// <summary>Stage changes; persisted by IUnitOfWork.SaveChangesAsync.</summary>
        void Add(NotificationPreference entity);
        void Update(NotificationPreference entity);
        Task<NotificationPreference> UpdateAsync(NotificationPreference entity);
        Task DeleteAsync(Guid id);
        Task<NotificationPreference?> GetByIdAsync(Guid id);
        Task<IEnumerable<NotificationPreference>> GetAllAsync();
        IQueryable<NotificationPreference> GetAllAsQueryable();
    }
}
