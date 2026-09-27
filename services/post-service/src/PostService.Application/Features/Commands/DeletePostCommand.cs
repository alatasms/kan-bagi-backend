using MediatR;
using PostService.Application.Features.Common;

namespace PostService.Application.Features.Commands
{
    public class DeletePostCommand: IRequest<StandardResponse<object>>
    {
        public Guid PostId { get; set; }
    }
}
