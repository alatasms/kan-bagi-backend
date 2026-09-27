using UserService.Application.Features.Commands;
using UserService.Application.Features.Common;
using UserService.Application.Helper;
using UserService.Domain.Entities;

namespace UserService.Application.Mapping
{
    public static class Mappings
    {
        public static User ToUser(this CompleteProfileCommand command) => new()
        {
            Name = command.Name,
            Surname = command.Surname,
            PhoneNumber = command.PhoneNumber,
            BirthDate = command.BirthDate,
            BloodType = command.BloodType,
            Gender = command.Gender,
        };

        public static UserInfoResponse ToResponse(this User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            BirthDate = user.BirthDate,
            BloodType = user.BloodType.GetDisplayName(),
            Gender = user.Gender.GetDisplayName(),
            IsIdentityVerified = user.IsIdentityVerified,
        };

        public static NotificationPreference ToEntity(this SetNotificationPreferenceCommand command) => new()
        {
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            PushNotification = command.PushNotification,
            PreferredHospitals = command.PreferredHospitals ?? [],
            PreferredBloodTypes = command.PreferredBloodTypes ?? [],
        };

        public static NotificationPreferenceResponse ToResponse(this NotificationPreference preference) => new()
        {
            Email = preference.Email,
            PhoneNumber = preference.PhoneNumber,
            PushNotification = preference.PushNotification,
            PreferredHospitals = preference.PreferredHospitals,
            PreferredBloodTypes = preference.PreferredBloodTypes,
        };

        public static SetNotificationPreferenceResponse ToSetResponse(this NotificationPreference preference) => new()
        {
            Email = preference.Email,
            PhoneNumber = preference.PhoneNumber,
            PushNotification = preference.PushNotification,
            PreferredHospitals = preference.PreferredHospitals,
            PreferredBloodTypes = preference.PreferredBloodTypes,
        };
    }
}
