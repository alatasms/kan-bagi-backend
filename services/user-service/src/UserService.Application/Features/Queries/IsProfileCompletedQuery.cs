using UserService.Application.Common;
using MediatR;

namespace UserService.Application.Features.Queries
{
    public class IsProfileCompletedQuery: IRequest<StandardResponse<IsProfileCompletedQueryResponse>>
    {
    }
}
