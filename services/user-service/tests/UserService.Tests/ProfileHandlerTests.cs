using MassTransit;
using Microsoft.Extensions.Options;
using NSubstitute;
using UserService.Application.Exceptions;
using UserService.Application.Features.Commands;
using UserService.Application.Features.Handlers;
using UserService.Application.Features.Queries;
using UserService.Application.Security;
using UserService.Domain.BussinesModels;
using UserService.Domain.Entities;
using UserService.Domain.Events;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories.Interfaces;
using UserService.Infrastructure.Services.IdentityVerification;

namespace UserService.Tests;

public class ProfileHandlerTests
{
    private static ICurrentUser Caller(Guid id, params string[] roles)
    {
        var user = Substitute.For<ICurrentUser>();
        user.UserId.Returns(id);
        user.Email.Returns("caller@example.com");
        user.IsInRole(Arg.Any<string>()).Returns(call => roles.Contains(call.Arg<string>()));
        return user;
    }

    private static IUserRepository RepositoryWith(User user)
    {
        var repository = Substitute.For<IUserRepository>();
        repository.GetByIdAsync(user.Id).Returns(user);
        return repository;
    }

    [Fact]
    public async Task Owner_can_read_own_profile()
    {
        var owner = new User { Id = Guid.NewGuid(), Name = "Ayşe" };
        var handler = new GetUserInfoQueryHandler(RepositoryWith(owner), Caller(owner.Id));

        var result = await handler.Handle(new GetUserInfoQuery { UserId = owner.Id }, CancellationToken.None);

        Assert.Equal("Ayşe", result.Response!.Name);
    }

    [Fact]
    public async Task Other_donor_cannot_read_a_profile()
    {
        var owner = new User { Id = Guid.NewGuid() };
        var handler = new GetUserInfoQueryHandler(RepositoryWith(owner), Caller(Guid.NewGuid(), Roles.Donor));

        await Assert.ThrowsAsync<ForbiddenException>(() => handler.Handle(new GetUserInfoQuery { UserId = owner.Id }, CancellationToken.None));
    }

    [Theory]
    [InlineData(Roles.HospitalStaff)]
    [InlineData(Roles.Admin)]
    public async Task Staff_and_admin_can_read_a_profile(string role)
    {
        var owner = new User { Id = Guid.NewGuid() };
        var handler = new GetUserInfoQueryHandler(RepositoryWith(owner), Caller(Guid.NewGuid(), role));

        var result = await handler.Handle(new GetUserInfoQuery { UserId = owner.Id }, CancellationToken.None);

        Assert.Equal(owner.Id, result.Response!.Id);
    }

    [Fact]
    public async Task Completing_profile_without_verifier_stores_no_identity_number_and_saves_once()
    {
        var userId = Guid.NewGuid();
        var users = Substitute.For<IUserRepository>();
        var preferences = Substitute.For<INotificationPreferenceRepository>();
        var publisher = Substitute.For<IPublishEndpoint>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var options = Options.Create(new IdentityVerificationOptions { Provider = IdentityVerificationOptions.None });
        var handler = new CompleteProfileCommandHandler(users, new DisabledIdentityVerifier(), new TCIdentityNumberHasher(options),
            options, publisher, Caller(userId), preferences, unitOfWork);

        var result = await handler.Handle(new CompleteProfileCommand
        {
            Name = "Ömer", Surname = "Test", PhoneNumber = "05551112233", TCIdentityNumber = "10000000146",
            BirthDate = new DateOnly(1990, 1, 1), BloodType = BloodType.O_Negative, Gender = GenderType.Male
        }, CancellationToken.None);

        users.Received(1).Add(Arg.Is<User>(u => u.Id == userId && u.TCIdentityNumberHash == null && !u.IsIdentityVerified));
        preferences.Received(1).Add(Arg.Is<NotificationPreference>(p => p.PreferredBloodTypes.Count == 8 && p.PreferredHospitals.Count == 0));
        await publisher.Received(1).Publish(Arg.Any<NotificationPreferencesChangedEvent>(), Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        Assert.False(result.Response!.IsIdentityVerified);
    }

    [Fact]
    public async Task Completing_an_existing_profile_is_a_conflict()
    {
        var existing = new User { Id = Guid.NewGuid() };
        var options = Options.Create(new IdentityVerificationOptions());
        var handler = new CompleteProfileCommandHandler(RepositoryWith(existing), new DisabledIdentityVerifier(), new TCIdentityNumberHasher(options),
            options, Substitute.For<IPublishEndpoint>(), Caller(existing.Id), Substitute.For<INotificationPreferenceRepository>(), Substitute.For<IUnitOfWork>());

        await Assert.ThrowsAsync<ConflictException>(() => handler.Handle(new CompleteProfileCommand(), CancellationToken.None));
    }
}
