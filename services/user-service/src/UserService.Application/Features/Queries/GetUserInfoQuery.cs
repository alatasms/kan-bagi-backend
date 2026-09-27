using UserService.Application.Common;
using UserService.Application.Features.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Features.Queries
{
    public class GetUserInfoQuery: IRequest<StandardResponse<UserInfoResponse>>
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
