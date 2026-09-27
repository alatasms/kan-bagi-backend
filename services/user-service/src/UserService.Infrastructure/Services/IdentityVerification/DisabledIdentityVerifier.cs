namespace UserService.Infrastructure.Services.IdentityVerification
{
    /// <summary>Registered when Provider is None. Callers check IdentityVerificationOptions.Enabled first.</summary>
    public class DisabledIdentityVerifier : IIdentityVerifier
    {
        public Task<bool> VerifyAsync(IdentityVerificationRequest request, CancellationToken cancellationToken)
            => throw new InvalidOperationException("Identity verification is disabled (IdentityVerification:Provider=None).");
    }
}
