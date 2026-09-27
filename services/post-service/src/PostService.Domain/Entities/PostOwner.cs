namespace PostService.Domain.Entities
{
    /// <summary>
    /// Local copy of the owner details post-service needs, filled from user-service events.
    /// Owner names on posts come from here, never from the request.
    /// </summary>
    public class PostOwner
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
    }
}
