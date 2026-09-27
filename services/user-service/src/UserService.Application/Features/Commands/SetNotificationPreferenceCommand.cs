using UserService.Application.Common;
using UserService.Domain.BussinesModels;
using MediatR;

namespace UserService.Application.Features.Commands
{
    public class SetNotificationPreferenceCommand: IRequest<StandardResponse<SetNotificationPreferenceResponse>>
    {
        public bool Email { get; set; } = true;
        public bool PhoneNumber { get; set; } = true;
        public bool PushNotification { get; set; } = false;
        public List<string> PreferredHospitals { get; set; } = [];
        public List<BloodType> PreferredBloodTypes { get; set; } = [];
    }
}
