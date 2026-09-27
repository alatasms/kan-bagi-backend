namespace PostService.Application.Posts
{
    public class PostOptions
    {
        public const string SectionName = "Posts";

        /// <summary>How long a post stays active after it is created or reactivated.</summary>
        public TimeSpan ActiveDuration { get; set; } = TimeSpan.FromDays(3);

        /// <summary>How often PostExpiryService looks for expired posts.</summary>
        public TimeSpan ExpiryCheckInterval { get; set; } = TimeSpan.FromMinutes(1);
    }
}
