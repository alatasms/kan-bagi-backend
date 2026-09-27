using PostService.Application.Features.Commands;
using PostService.Application.Features.Common;
using PostService.Application.Helper;
using PostService.Domain.Entities;

namespace PostService.Application.Mapping
{
    public static class Mappings
    {
        public static Post ToPost(this CreatePostCommand command)
        {
            var post = new Post
            {
                PatientFullName = command.PatientFullName,
                PatientAge = command.PatientAge,
                Title = command.Title,
                Description = command.Description,
                BloodType = command.BloodType,
                HospitalId = command.HospitalId,
            };
            post.SetPhoneNumbers(command.PhoneNumbers);
            return post;
        }

        /// <summary>Expects Hospital → District → City → Country to be loaded when Hospital is set.</summary>
        public static PostResponse ToResponse(this Post post) => new()
        {
            Id = post.Id,
            OwnerId = post.OwnerId,
            OwnerName = post.OwnerName,
            OwnerSurname = post.OwnerSurname,
            PatientFullName = post.PatientFullName,
            PatientAge = post.PatientAge,
            Title = post.Title,
            Description = post.Description,
            PhoneNumbers = post.GetPhoneNumbers(),
            BloodType = post.BloodType.GetDisplayName(),
            IsActive = post.IsActive,
            ExpiresAt = post.ExpiresAt,
            Hospital = post.Hospital?.ToResponse()!,
        };

        public static HospitalResponse ToResponse(this Hospital hospital) => new()
        {
            Id = hospital.Id,
            Name = hospital.Name,
            Address = hospital.Address,
            PhoneNumber = hospital.PhoneNumber!,
            Email = hospital.Email!,
            WebSite = hospital.WebSite!,
            Lat = hospital.Lat,
            Lon = hospital.Lon,
            DistrictId = hospital.DistrictId,
            DistrictName = hospital.District?.Name!,
            CityId = hospital.District?.CityId ?? 0,
            CityName = hospital.District?.City?.Name!,
            CountryId = hospital.District?.City?.CountryId ?? 0,
            CountryName = hospital.District?.City?.Country?.Name!,
        };
    }
}
