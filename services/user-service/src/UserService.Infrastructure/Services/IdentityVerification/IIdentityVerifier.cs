namespace UserService.Infrastructure.Services.IdentityVerification
{
    /// <summary>
    /// Checks that an identity number belongs to the person described. The only official source in Türkiye
    /// is NVİ's KPS, available to authorised institutions; see docs/identity-verification.md.
    /// </summary>
    public interface IIdentityVerifier
    {
        Task<bool> VerifyAsync(IdentityVerificationRequest request, CancellationToken cancellationToken);
    }

    public record IdentityVerificationRequest(string TCIdentityNumber, string Name, string Surname, DateOnly BirthDate);
}
