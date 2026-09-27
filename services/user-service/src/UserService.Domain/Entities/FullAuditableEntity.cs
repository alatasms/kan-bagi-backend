using UserService.Domain.Interfaces;

namespace UserService.Domain.Entities
{
    public class FullAuditableEntity<T> : IFullAuditableEntity<T>
    {
        public T Id { get; set; } = default!;
        public DateTime? CreationTime { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public long LastModifiedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
