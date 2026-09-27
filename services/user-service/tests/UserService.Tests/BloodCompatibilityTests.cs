using UserService.Domain.BussinesModels;

namespace UserService.Tests;

public class BloodCompatibilityTests
{
    [Fact]
    public void O_negative_can_donate_to_every_blood_type()
    {
        Assert.Equal(Enum.GetValues<BloodType>().Order(), BloodCompatibility.RecipientsOf(BloodType.O_Negative).Order());
    }

    [Fact]
    public void AB_positive_can_donate_only_to_AB_positive()
    {
        Assert.Equal([BloodType.AB_Positive], BloodCompatibility.RecipientsOf(BloodType.AB_Positive));
    }

    [Theory]
    [InlineData(BloodType.A_Positive, BloodType.B_Positive)]
    [InlineData(BloodType.B_Negative, BloodType.A_Negative)]
    [InlineData(BloodType.O_Positive, BloodType.O_Negative)]
    public void Incompatible_recipient_is_excluded(BloodType donor, BloodType recipient)
    {
        Assert.DoesNotContain(recipient, BloodCompatibility.RecipientsOf(donor));
    }

    [Fact]
    public void Every_blood_type_can_donate_to_itself()
    {
        foreach (var type in Enum.GetValues<BloodType>())
            Assert.Contains(type, BloodCompatibility.RecipientsOf(type));
    }
}
