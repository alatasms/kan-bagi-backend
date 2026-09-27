using UserService.Domain.BussinesModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Domain.Entities
{
    public class NotificationPreference: FullAuditableEntity<Guid>
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public bool Email { get; set; }
        public bool PhoneNumber { get; set; }
        public bool PushNotification { get; set; }
        [Column(TypeName = "jsonb")]
        public List<string> PreferredHospitals { get; set; } = [];
        [Column(TypeName = "jsonb")]
        public List<BloodType> PreferredBloodTypes { get; set; } = [];
    }
}
