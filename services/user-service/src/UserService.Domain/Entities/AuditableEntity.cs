using UserService.Domain.Interfaces;

namespace UserService.Domain.Entities
{
    public class AuditableEntity : IAuditableEntity
    {
        public DateTime? CreationTime { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public long LastModifiedBy { get; set; }
    }
}
