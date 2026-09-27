namespace UserService.Infrastructure.Services.IdentityVerification
{
    /// <summary>
    /// Development stand-in used to exercise the verified-profile flow. It only checks the official
    /// checksum digits (e.g. 10000000146 passes), so anyone can produce a number it accepts.
    /// The service refuses to start with it in the Production environment.
    /// </summary>
    public class DevChecksumIdentityVerifier : IIdentityVerifier
    {
        public Task<bool> VerifyAsync(IdentityVerificationRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(HasValidChecksum(request.TCIdentityNumber));
        }

        private static bool HasValidChecksum(string number)
        {
            if (number.Length != 11 || number[0] == '0' || !number.All(char.IsDigit))
                return false;

            var d = number.Select(c => c - '0').ToArray();
            var oddSum = d[0] + d[2] + d[4] + d[6] + d[8];
            var evenSum = d[1] + d[3] + d[5] + d[7];
            var tenth = ((oddSum * 7 - evenSum) % 10 + 10) % 10;
            var eleventh = d.Take(10).Sum() % 10;

            return d[9] == tenth && d[10] == eleventh;
        }
    }
}
