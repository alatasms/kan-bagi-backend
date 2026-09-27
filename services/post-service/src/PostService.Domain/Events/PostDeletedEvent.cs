namespace PostService.Domain.Events
{
    public class PostDeletedEvent: IDomainEvent
    {
        public PostDeletedEvent(Guid postId, Guid correlationId)
        {
            PostId = postId;
            CorrelationId = correlationId;
            OccurredOn = DateTime.Now;
        }

        public Guid PostId { get; }
        public Guid CorrelationId { get; }
        public DateTime OccurredOn { get; }
    }
}
