namespace UserService.Application.Exceptions
{
    /// <summary>The request conflicts with existing data (HTTP 409).</summary>
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
