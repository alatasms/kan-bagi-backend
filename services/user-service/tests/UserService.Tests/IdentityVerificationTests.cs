using Microsoft.Extensions.Options;
using UserService.Infrastructure.Services.IdentityVerification;

namespace UserService.Tests;

public class IdentityVerificationTests
{
    private static IdentityVerificationRequest Request(string number) => new(number, "Test", "User", new DateOnly(1990, 1, 1));

    [Theory]
    [InlineData("10000000146", true)]
    [InlineData("10000000147", false)] // wrong last digit
    [InlineData("00000000146", false)] // cannot start with 0
    [InlineData("1000000014", false)]  // too short
    [InlineData("1000000014a", false)]
    public async Task DevChecksum_accepts_only_checksum_valid_numbers(string number, bool expected)
    {
        Assert.Equal(expected, await new DevChecksumIdentityVerifier().VerifyAsync(Request(number), CancellationToken.None));
    }

    [Fact]
    public void Hash_is_deterministic_and_depends_on_the_key()
    {
        var a = new TCIdentityNumberHasher(Options.Create(new IdentityVerificationOptions { HashKey = "key-a" }));
        var b = new TCIdentityNumberHasher(Options.Create(new IdentityVerificationOptions { HashKey = "key-b" }));

        Assert.Equal(a.Hash("10000000146"), a.Hash("10000000146"));
        Assert.NotEqual(a.Hash("10000000146"), b.Hash("10000000146"));
        Assert.DoesNotContain("10000000146", a.Hash("10000000146"));
    }
}
