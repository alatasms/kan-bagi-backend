using PostService.Application.Features.Commands;
using PostService.Application.Features.Queries;
using PostService.Application.Mapping;
using PostService.Application.Posts;
using PostService.Application.Validators;
using PostService.Domain.BusinessModels;
using PostService.Domain.Entities;

namespace PostService.Tests;

public class PostTests
{
    [Fact]
    public void Phone_numbers_round_trip_through_the_stored_column()
    {
        var post = new Post();
        post.SetPhoneNumbers(["05551112233", "05559998877"]);

        Assert.Equal(["05551112233", "05559998877"], post.GetPhoneNumbers());
    }

    [Fact]
    public void Stored_phone_numbers_with_spaces_are_trimmed()
    {
        var post = new Post();
        post.SetPhoneNumbers(["05551112233", " 05559998877"]);

        Assert.Equal("05559998877", post.GetPhoneNumbers()[1]);
    }

    [Fact]
    public void Live_posts_are_active_and_not_past_expiry()
    {
        var now = DateTime.UtcNow;
        var posts = new[]
        {
            new Post { Title = "live", IsActive = true, ExpiresAt = now.AddHours(1) },
            new Post { Title = "expired", IsActive = true, ExpiresAt = now.AddMinutes(-1) },
            new Post { Title = "inactive", IsActive = false, ExpiresAt = now.AddHours(1) },
        }.AsQueryable();

        Assert.Equal(["live"], posts.WhereLive().Select(p => p.Title));
    }

    [Fact]
    public void Create_command_maps_to_post_and_back_to_response()
    {
        var post = new CreatePostCommand
        {
            PatientFullName = "Hasta", PatientAge = 40, Title = "Acil", Description = "Açıklama",
            PhoneNumbers = ["05551112233"], BloodType = BloodType.AB_Negative, HospitalId = 25
        }.ToPost();

        var response = post.ToResponse();

        Assert.Equal(25, post.HospitalId);
        Assert.Equal("AB-", response.BloodType);
        Assert.Equal(["05551112233"], response.PhoneNumbers);
    }

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(0, 1, true)]
    [InlineData(0, 100, true)]
    [InlineData(0, 101, false)]
    [InlineData(-1, 10, false)]
    public void Page_size_and_offset_are_bounded(int from, int size, bool valid)
    {
        var result = new GetAllHospitalsQueryValidator().Validate(new GetAllHospitalsQuery { ShowingResultsFrom = from, Paging = size });

        Assert.Equal(valid, result.IsValid);
    }

    [Fact]
    public void Update_command_is_validated_like_create()
    {
        var result = new UpdatePostCommandValidator().Validate(new UpdatePostCommand
        {
            PatientFullName = "", Title = "x", Description = "short", PhoneNumbers = [], BloodType = BloodType.A_Positive, HospitalId = 25
        });

        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdatePostCommand.PatientFullName));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdatePostCommand.Description));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdatePostCommand.PhoneNumbers));
    }
}
