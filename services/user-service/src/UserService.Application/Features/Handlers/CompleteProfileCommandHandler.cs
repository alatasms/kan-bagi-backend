using UserService.Application.Common;
using UserService.Application.Events;
using UserService.Application.Exceptions;
using UserService.Application.Features.Commands;
using UserService.Application.Features.Common;
using UserService.Application.Security;
using UserService.Domain.BussinesModels;
using UserService.Domain.Entities;
using UserService.Domain.Events;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories.Interfaces;
using UserService.Infrastructure.Services.IdentityVerification;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using UserService.Application.Mapping;

namespace UserService.Application.Features.Handlers
{
    public class CompleteProfileCommandHandler : IRequestHandler<CompleteProfileCommand, StandardResponse<UserInfoResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityVerifier _identityVerifier;
        private readonly TCIdentityNumberHasher _hasher;
        private readonly IdentityVerificationOptions _identityOptions;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICurrentUser _currentUser;
        private readonly INotificationPreferenceRepository _notificationPreferenceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteProfileCommandHandler(IUserRepository userRepository, IIdentityVerifier identityVerifier, TCIdentityNumberHasher hasher,
            IOptions<IdentityVerificationOptions> identityOptions, IPublishEndpoint publishEndpoint, ICurrentUser currentUser,
            INotificationPreferenceRepository notificationPreferenceRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _identityVerifier = identityVerifier;
            _hasher = hasher;
            _identityOptions = identityOptions.Value;
            _publishEndpoint = publishEndpoint;
            _currentUser = currentUser;
            _notificationPreferenceRepository = notificationPreferenceRepository;
        }

        public async Task<StandardResponse<UserInfoResponse>> Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (await _userRepository.GetByIdAsync(userId) != null)
                throw new ConflictException("Profile is already completed.");

            var user = request.ToUser();
            user.Id = userId;
            user.Email = _currentUser.Email;

            // Without a verifier the identity number would be unverifiable, so it is neither required nor stored.
            if (_identityOptions.Enabled)
            {
                var tcIdentityNumber = request.TCIdentityNumber!;
                var hash = _hasher.Hash(tcIdentityNumber);

                var existingUser = await _userRepository.GetByExpressionAsync(x => x.TCIdentityNumberHash == hash && !x.IsDeleted);
                if (existingUser != null)
                    throw new ConflictException("This identity number is already registered.");

                var verified = await _identityVerifier.VerifyAsync(
                    new IdentityVerificationRequest(tcIdentityNumber, request.Name, request.Surname, request.BirthDate), cancellationToken);
                if (!verified)
                    throw new IdentityVerificationFailedException("The identity number could not be verified with the given name, surname and birth date.");

                user.TCIdentityNumberHash = hash;
                user.IsIdentityVerified = true;
            }

            _userRepository.Add(user);

            // Defaults that actually produce notifications: every post the donor's blood can help with,
            // at any hospital (an empty hospital list means all hospitals).
            var notificationPreference = new NotificationPreference
            {
                UserId = user.Id,
                Email = true,
                PhoneNumber = false,
                PushNotification = false,
                PreferredBloodTypes = [.. BloodCompatibility.RecipientsOf(request.BloodType)],
                PreferredHospitals = []
            };
            _notificationPreferenceRepository.Add(notificationPreference);

            // Published through the outbox: stored with the profile below and delivered only if it commits.
            await _publishEndpoint.Publish(new UserRegisteredEvent(user.Id,
                request.Name, user.Email, user.CorrelationId), cancellationToken);
            await _publishEndpoint.Publish(IntegrationEvents.ProfileCompleted(user), cancellationToken);
            await _publishEndpoint.Publish(IntegrationEvents.PreferencesChanged(user, notificationPreference), cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new StandardResponse<UserInfoResponse>
            {
                Response = user.ToResponse()
            };
        }
    }
}
