using UserService.Application.Features.Commands;
using UserService.Domain.BussinesModels;

namespace UserService.Application.Features.Common
{
    public class NotificationPreferenceResponse
    {
        public bool Email { get; set; }
        public bool PhoneNumber { get; set; }
        public bool PushNotification { get; set; }
        public List<string> PreferredHospitals { get; set; } = [];
        public List<BloodType> PreferredBloodTypes { get; set; } = [];
    }
}
