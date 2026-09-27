using UserService.Domain.Entities;
using UserService.Domain.Events;

namespace UserService.Application.Events
{
    /// <summary>Builds the events other services rely on, so every publisher sends the same shape.</summary>
    public static class IntegrationEvents
    {
        public static NotificationPreferencesChangedEvent PreferencesChanged(User user, NotificationPreference preference) => new()
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email,
            EmailEnabled = preference.Email,
            PhoneNumberEnabled = preference.PhoneNumber,
            PushNotificationEnabled = preference.PushNotification,
            PreferredHospitalIds = preference.PreferredHospitals ?? [],
            PreferredBloodTypes = preference.PreferredBloodTypes ?? [],
        };

        public static UserProfileCompletedEvent ProfileCompleted(User user) => new()
        {
            UserId = user.Id,
            Name = user.Name,
            Surname = user.Surname,
        };
    }
}
