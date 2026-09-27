using PostService.Domain.BusinessModels;

namespace PostService.Domain.Events
{
    /// <summary>
    /// A post expired. The owner is identified by id; the notification service looks up how to reach them.
    /// </summary>
    public record UserPostExpiredEvent : IDomainEvent
    {
        public Guid PostId { get; init; }
        public string OwnerId { get; init; } = string.Empty;
        public string OwnerName { get; init; } = string.Empty;
        public string OwnerSurname { get; init; } = string.Empty;
        public string PatientFullName { get; init; } = string.Empty;
        public int PatientAge { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string PhoneNumbers { get; init; } = string.Empty;
        public string BloodType { get; init; } = string.Empty;
        public HospitalModel Hospital { get; init; } = new();
        public Guid CorrelationId { get; init; }
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    }
}
