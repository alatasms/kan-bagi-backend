using PostService.Domain.Interfaces;

namespace PostService.Domain.Entities
{
    public class FullAuditableEntity<T> : IFullAuditableEntity<T>
    {
        public T Id { get; set; } = default!;
        public DateTime CreationTime { get; set; }
        public virtual long CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public virtual long LastModifiedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
