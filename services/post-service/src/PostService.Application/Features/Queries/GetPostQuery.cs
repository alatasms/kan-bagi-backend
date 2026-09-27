using MediatR;
using PostService.Application.Features.Common;

namespace PostService.Application.Features.Queries
{
    public class GetPostQuery: IRequest<StandardResponse<PostResponse>>
    {
        public Guid Id { get; set; }
    }
}
