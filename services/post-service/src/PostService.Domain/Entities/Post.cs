using PostService.Domain.BusinessModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Domain.Entities
{
    public class Post: FullAuditableEntity<Guid>
    {
        public Post()
        {
            CorrelationId = Guid.NewGuid();
        }
        public string OwnerId { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerSurname { get; set; } = string.Empty;
        public string PatientFullName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhoneNumbers { get; private set; } = string.Empty;
        public BloodType BloodType { get; set; }
        public int HospitalId { get; set; }
        [ForeignKey(nameof(HospitalId))]
        public Hospital Hospital { get; set; } = null!;
        [NotMapped]
        public override long CreatedBy { get; set; }
        [NotMapped]
        public override long LastModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public Guid CorrelationId { get; set; }
        /// <summary>After this moment the post is no longer shown; PostExpiryService then deactivates it.</summary>
        public DateTime ExpiresAt { get; set; }
        // Phone numbers are stored as one comma-separated column; these two methods are the only
        // place that knows the format.
        private const char PhoneNumberSeparator = ',';

        public void SetPhoneNumbers(List<string>? phoneNumbers)
        {
            PhoneNumbers = phoneNumbers == null ? string.Empty : string.Join(PhoneNumberSeparator, phoneNumbers);
        }

        public List<string> GetPhoneNumbers()
        {
            if (string.IsNullOrEmpty(PhoneNumbers))
            {
                return new List<string>();
            }
            return PhoneNumbers.Split(PhoneNumberSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        }
    }
}
