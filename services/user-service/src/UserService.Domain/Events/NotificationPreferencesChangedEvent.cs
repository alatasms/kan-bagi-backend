using UserService.Domain.BussinesModels;

namespace UserService.Domain.Events
{
    /// <summary>
    /// Published whenever a user's notification preferences are created or changed. The notification
    /// service keeps its own copy of subscribers from these events; see docs/events.md.
    /// </summary>
    public record NotificationPreferencesChangedEvent : IDomainEvent
    {
        public Guid UserId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public bool EmailEnabled { get; init; }
        public bool PhoneNumberEnabled { get; init; }
        public bool PushNotificationEnabled { get; init; }
        /// <summary>Hospital ids to be notified about. Empty means every hospital.</summary>
        public List<string> PreferredHospitalIds { get; init; } = [];
        /// <summary>Blood types of posts to be notified about.</summary>
        public List<BloodType> PreferredBloodTypes { get; init; } = [];
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    }
}
