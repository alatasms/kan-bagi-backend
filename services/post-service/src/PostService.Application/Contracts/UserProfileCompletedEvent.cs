using MassTransit;

namespace PostService.Application.Contracts
{
    /// <summary>
    /// Published by user-service when a profile is completed. This is post-service's copy of the contract;
    /// the shared URN lets MassTransit match it to the publisher's type. See docs/events.md.
    /// </summary>
    [MessageUrn("user-profile-completed")]
    public record UserProfileCompletedEvent
    {
        public Guid UserId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Surname { get; init; } = string.Empty;
        public DateTime OccurredOn { get; init; }
    }
}
