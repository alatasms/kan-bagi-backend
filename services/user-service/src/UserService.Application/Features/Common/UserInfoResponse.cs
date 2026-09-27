using UserService.Domain.BussinesModels;

namespace UserService.Application.Features.Common
{
    public class UserInfoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string BloodType { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public bool IsIdentityVerified { get; set; }
    }
}
