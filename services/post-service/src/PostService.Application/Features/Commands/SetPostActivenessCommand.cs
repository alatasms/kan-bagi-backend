using MediatR;
using PostService.Application.Features.Common;

namespace PostService.Application.Features.Commands
{
    public class SetPostActivenessCommand: IRequest<StandardResponse<PostResponse>>
    {
        public Guid PostId { get; set; }
        public bool? IsActive { get; set; }
    }
}
