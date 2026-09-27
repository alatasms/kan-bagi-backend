using System.ComponentModel.DataAnnotations;

namespace UserService.Domain.Events
{
    public class UserRegisteredEvent : IDomainEvent
    {
        public UserRegisteredEvent(Guid userId, string name, string email, Guid correlationId)
        {
            UserId = userId;
            Name = name;
            Email = email;
            CorrelationId = correlationId;
            OccurredOn = DateTime.UtcNow;
        }
        [Required]
        public Guid UserId { get; }
        [Required]
        public string Name { get; }
        [Required]
        public string Email { get; }
        [Required]
        public Guid CorrelationId { get; }
        public DateTime OccurredOn { get; }
    }
}
