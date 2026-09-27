using MediatR;
using PostService.Application.Features.Common;

namespace PostService.Application.Features.Queries
{
    public class GetUserActivePostQuery: IRequest<StandardResponse<PostResponse>>
    {
        public Guid? UserId { get; set; }
    }
}
