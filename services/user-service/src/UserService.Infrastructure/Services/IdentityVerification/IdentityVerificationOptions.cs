namespace UserService.Infrastructure.Services.IdentityVerification
{
    public class IdentityVerificationOptions
    {
        public const string SectionName = "IdentityVerification";

        public const string None = "None";
        public const string DevChecksum = "DevChecksum";

        /// <summary>None (default): no identity number is collected. DevChecksum: development only.</summary>
        public string Provider { get; set; } = None;

        /// <summary>Secret key for hashing identity numbers. Required whenever Provider is not None.</summary>
        public string? HashKey { get; set; }

        public bool Enabled => !string.Equals(Provider, None, StringComparison.OrdinalIgnoreCase);
    }
}
