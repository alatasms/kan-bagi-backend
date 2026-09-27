namespace PostService.Application.Security
{
    /// <summary>
    /// The authenticated caller, read from the validated access token. Never from headers, route or body.
    /// </summary>
    public interface ICurrentUser
    {
        Guid UserId { get; }
        string Email { get; }
        bool IsInRole(string role);
    }
}
