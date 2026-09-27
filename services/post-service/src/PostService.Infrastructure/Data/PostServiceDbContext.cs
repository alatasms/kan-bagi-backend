using MassTransit;
using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Infrastructure.Data.Seeds;

namespace PostService.Infrastructure.Data
{
    public class PostServiceDbContext : DbContext, IUnitOfWork
    {
        public PostServiceDbContext(DbContextOptions options) : base(options)
        {
        }

        protected PostServiceDbContext()
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Hospital> Hospitals { get; set; } 
        public DbSet<District> Districts { get; set; } 
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<PostOwner> PostOwners { get; set; }

        public async Task SeedData()
        {
            await LocationSeeds.Seed(this);
            await HospitalSeeds.Seed(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            modelBuilder.Entity<Post>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Hospital>().HasQueryFilter(h => !h.IsDeleted);
            modelBuilder.Entity<District>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<City>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Country>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<PostOwner>().HasKey(o => o.UserId);
            modelBuilder.Entity<Post>().HasIndex(p => new { p.IsActive, p.ExpiresAt });
            // At most one active post per owner, enforced by the database so concurrent requests cannot both succeed.
            modelBuilder.Entity<Post>()
                .HasIndex(p => p.OwnerId)
                .IsUnique()
                .HasFilter("\"IsActive\" AND NOT \"IsDeleted\"")
                .HasDatabaseName("IX_Posts_OneActivePerOwner");
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
