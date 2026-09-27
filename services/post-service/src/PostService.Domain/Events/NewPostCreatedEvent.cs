using PostService.Domain.BusinessModels;

namespace PostService.Domain.Events
{
    /// <summary>
    /// A blood request was published. Carries only what is shown on the post itself; who gets notified
    /// is decided by the notification service. See docs/events.md.
    /// </summary>
    public record NewPostCreatedEvent : IDomainEvent
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
        /// <summary>Display form, e.g. "A+".</summary>
        public string BloodType { get; init; } = string.Empty;
        /// <summary>Enum name, e.g. "A_Positive"; matches the values in notification preferences.</summary>
        public string BloodTypeCode { get; init; } = string.Empty;
        public int HospitalId { get; init; }
        public HospitalModel Hospital { get; init; } = new();
        public Guid CorrelationId { get; init; }
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    }
}
