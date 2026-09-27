namespace PostService.Application.Exceptions
{
    /// <summary>The request conflicts with the current state (HTTP 409).</summary>
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
