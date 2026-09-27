using PostService.Application.Helper;
using PostService.Domain.BusinessModels;
using PostService.Domain.Entities;
using PostService.Domain.Events;

namespace PostService.Application.Events
{
    /// <summary>Builds post events from a post loaded with Hospital → District → City → Country.</summary>
    public static class PostEvents
    {
        public static NewPostCreatedEvent Created(Post post) => new()
        {
            PostId = post.Id,
            OwnerId = post.OwnerId,
            OwnerName = post.OwnerName,
            OwnerSurname = post.OwnerSurname,
            PatientFullName = post.PatientFullName,
            PatientAge = post.PatientAge,
            Title = post.Title,
            Description = post.Description,
            PhoneNumbers = post.PhoneNumbers,
            BloodType = post.BloodType.GetDisplayName(),
            BloodTypeCode = post.BloodType.ToString(),
            HospitalId = post.HospitalId,
            Hospital = HospitalOf(post),
            CorrelationId = post.CorrelationId,
        };

        public static UserPostExpiredEvent Expired(Post post, Guid correlationId) => new()
        {
            PostId = post.Id,
            OwnerId = post.OwnerId,
            OwnerName = post.OwnerName,
            OwnerSurname = post.OwnerSurname,
            PatientFullName = post.PatientFullName,
            PatientAge = post.PatientAge,
            Title = post.Title,
            Description = post.Description,
            PhoneNumbers = post.PhoneNumbers,
            BloodType = post.BloodType.GetDisplayName(),
            Hospital = HospitalOf(post),
            CorrelationId = correlationId,
        };

        private static HospitalModel HospitalOf(Post post) => new()
        {
            HospitalName = post.Hospital.Name,
            HospitalAddress = post.Hospital.Address,
            District = post.Hospital.District.Name,
            City = post.Hospital.District.City.Name,
            Country = post.Hospital.District.City.Country.Name
        };
    }
}
