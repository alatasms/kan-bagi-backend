using UserService.Application.Common;
using UserService.Application.Events;
using UserService.Application.Exceptions;
using UserService.Application.Features.Commands;
using UserService.Application.Security;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Mapping;

namespace UserService.Application.Features.Handlers
{
    public class SetNotificationPreferenceCommandHandler : IRequestHandler<SetNotificationPreferenceCommand, StandardResponse<SetNotificationPreferenceResponse>>
    {
        private readonly INotificationPreferenceRepository _notificationPreferenceRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUserRepository _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;

        public SetNotificationPreferenceCommandHandler(INotificationPreferenceRepository notificationPreferenceRepository, ICurrentUser currentUser,
            IUserRepository userRepository, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _notificationPreferenceRepository = notificationPreferenceRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<StandardResponse<SetNotificationPreferenceResponse>> Handle(SetNotificationPreferenceCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException("Profile not found. Complete your profile first.");

            var entityDto = request.ToEntity();
            entityDto.UserId = userId;
            entityDto.PreferredHospitals ??= [];
            entityDto.PreferredBloodTypes ??= [];

            var notificationPreference = await _notificationPreferenceRepository.GetAllAsQueryable()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (notificationPreference == null)
            {
                _notificationPreferenceRepository.Add(entityDto);
            }
            else
            {
                entityDto.Id = notificationPreference.Id;
                entityDto.CreationTime = notificationPreference.CreationTime;
                _notificationPreferenceRepository.Update(entityDto);
            }
            var result = entityDto;

            // The notification service decides who receives which notification from these events.
            // Published through the outbox, so it is committed together with the preference change.
            await _publishEndpoint.Publish(IntegrationEvents.PreferencesChanged(user, result), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new StandardResponse<SetNotificationPreferenceResponse>
            {
                Response = result.ToSetResponse()
            };
        }
    }
}
