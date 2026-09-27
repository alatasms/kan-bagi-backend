using MassTransit;

namespace UserService.Domain.Events
{
    /// <summary>Published once a profile is completed; other services keep the display name they need. The URN is fixed
    /// so consumers in other services can declare their own copy of the contract.</summary>
    [MessageUrn("user-profile-completed")]
    public record UserProfileCompletedEvent : IDomainEvent
    {
        public Guid UserId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Surname { get; init; } = string.Empty;
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    }
}
