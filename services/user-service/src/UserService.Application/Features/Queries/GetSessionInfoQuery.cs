using UserService.Application.Common;
using UserService.Application.Features.Common;
using MediatR;
namespace UserService.Application.Features.Queries
{
    public class GetSessionInfoQuery : IRequest<StandardResponse<UserInfoResponse>>
    {
    }
}
