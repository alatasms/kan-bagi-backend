using UserService.Domain.BussinesModels;

namespace UserService.Domain.Entities
{
    public class User : FullAuditableEntity<Guid>
    {
        public User()
        {
            CorrelationId = Guid.NewGuid();
        }
        public Guid CorrelationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        /// <summary>HMAC of the identity number; null when identity verification is disabled.</summary>
        public string? TCIdentityNumberHash { get; set; }
        public bool IsIdentityVerified { get; set; }
        public DateOnly BirthDate { get; set; }
        public BloodType BloodType { get; set; }
        public GenderType Gender { get; set; }
    }
}
