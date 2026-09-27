using UserService.Application.Common;
using MediatR;

namespace UserService.Application.Features.Queries
{
    public class GetProfileConfigQuery : IRequest<StandardResponse<ProfileConfigResponse>>
    {
    }

    /// <summary>Server settings the mobile app needs to render the profile form.</summary>
    public class ProfileConfigResponse
    {
        /// <summary>When true, the profile form asks for the T.C. identity number and it is verified.</summary>
        public bool IdentityVerificationEnabled { get; set; }
    }
}
