using UserService.Application.Common;
using UserService.Application.Features.Common;
using UserService.Domain.BussinesModels;
using MediatR;
namespace UserService.Application.Features.Commands
{
    public class CompleteProfileCommand : IRequest<StandardResponse<UserInfoResponse>>
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        /// <summary>Required only when the server has identity verification enabled (see GET /api/profile/config).</summary>
        public string? TCIdentityNumber { get; set; }
        public DateOnly BirthDate { get; set; }
        public BloodType BloodType { get; set; }
        public GenderType Gender { get; set; }
    }
}
