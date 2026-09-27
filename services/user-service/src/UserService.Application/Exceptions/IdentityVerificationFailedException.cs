namespace UserService.Application.Exceptions
{
    /// <summary>The identity details could not be verified (HTTP 422).</summary>
    public class IdentityVerificationFailedException : Exception
    {
        public IdentityVerificationFailedException(string message) : base(message) { }
    }
}
