using UserService.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.Data
{
    public class UserServiceDbContext : DbContext, IUnitOfWork
    {
        public UserServiceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<NotificationPreference> NotificationPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            // One active profile per verified identity number, enforced by the database as well.
            modelBuilder.Entity<User>()
                .HasIndex(u => u.TCIdentityNumberHash)
                .IsUnique()
                .HasFilter("\"TCIdentityNumberHash\" IS NOT NULL AND \"IsDeleted\" = false");
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                            .Where(e => (e.Entity is FullAuditableEntity<Guid> || e.Entity is FullAuditableEntity<int>) &&
                                        (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                // Handle both Guid and int base types
                if (entry.Entity is FullAuditableEntity<Guid> guidEntity)
                {
                    UpdateAuditFields(guidEntity, entry.State);
                }
                else if (entry.Entity is FullAuditableEntity<int> intEntity)
                {
                    UpdateAuditFields(intEntity, entry.State);
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        private void UpdateAuditFields<T>(FullAuditableEntity<T> entity, EntityState state)
        {
            if (state == EntityState.Added)
            {
                entity.CreationTime = DateTime.UtcNow;
                entity.IsDeleted = false;
            }
            else if (state == EntityState.Modified)
            {
                entity.IsDeleted = false;
                entity.LastModified = DateTime.UtcNow;
            }
            else if (state == EntityState.Deleted)
            {
                // Implement soft delete
                entity.IsDeleted = true;
                entity.DeletedTime = DateTime.UtcNow;
                // Change state to Modified to update instead of delete
                Entry(entity).State = EntityState.Modified;
            }
        }
    }
}
