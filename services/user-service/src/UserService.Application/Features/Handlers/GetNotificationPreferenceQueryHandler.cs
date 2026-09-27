using UserService.Application.Common;
using UserService.Application.Exceptions;
using UserService.Application.Features.Common;
using UserService.Application.Features.Queries;
using UserService.Application.Security;
using UserService.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Mapping;

namespace UserService.Application.Features.Handlers
{
    public class GetNotificationPreferenceQueryHandler : IRequestHandler<GetNotificatiionPreferenceQuery, StandardResponse<NotificationPreferenceResponse>>
    {
        private readonly INotificationPreferenceRepository _notificationPreferenceRepository;
        private readonly ICurrentUser _currentUser;

        public GetNotificationPreferenceQueryHandler(INotificationPreferenceRepository notificationPreferenceRepository, ICurrentUser currentUser)
        {
            _notificationPreferenceRepository = notificationPreferenceRepository;
            _currentUser = currentUser;
        }

        public async Task<StandardResponse<NotificationPreferenceResponse>> Handle(GetNotificatiionPreferenceQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            var notificationPreference = await _notificationPreferenceRepository.GetAllAsQueryable()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken)
                ?? throw new NotFoundException("Notification preference not found!");

            return new StandardResponse<NotificationPreferenceResponse>
            {
                Response = notificationPreference.ToResponse()
            };
        }
    }
}
