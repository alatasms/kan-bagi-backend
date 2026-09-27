namespace PostService.Application.Features.Common
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerSurname { get; set; } = string.Empty;
        public string PatientFullName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> PhoneNumbers { get; set; } = [];
        public string BloodType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime ExpiresAt { get; set; }

        public HospitalResponse Hospital { get; set; } = null!;
    }
}
