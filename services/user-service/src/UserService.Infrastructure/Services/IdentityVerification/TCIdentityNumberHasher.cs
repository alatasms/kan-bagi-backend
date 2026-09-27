using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace UserService.Infrastructure.Services.IdentityVerification
{
    /// <summary>
    /// Identity numbers are never stored in clear text. A keyed hash (HMAC-SHA256) still lets us detect a
    /// second registration with the same number, but unlike a plain hash it cannot be reversed by trying
    /// all 11-digit numbers without the secret key.
    /// </summary>
    public class TCIdentityNumberHasher
    {
        private readonly byte[] _key;

        public TCIdentityNumberHasher(IOptions<IdentityVerificationOptions> options)
        {
            _key = Encoding.UTF8.GetBytes(options.Value.HashKey ?? string.Empty);
        }

        public string Hash(string tcIdentityNumber)
        {
            var hash = HMACSHA256.HashData(_key, Encoding.UTF8.GetBytes(tcIdentityNumber));
            return Convert.ToBase64String(hash);
        }
    }
}
