using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.Features.Commands;
using PostService.Application.Features.Common;
using PostService.Application.Features.Queries;

namespace PostService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-post")]
        public async Task<StandardResponse<PostResponse>> CreatePost([FromBody] CreatePostCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("delete-post")]
        public async Task<StandardResponse<object>> DeletePost([FromQuery] Guid id)
        {
            return await _mediator.Send(new DeletePostCommand
            {
                PostId = id
            });
        }
        [HttpPut("update-post")]
        public async Task<StandardResponse<PostResponse>> UpdatePost([FromBody] UpdatePostCommand command)
        {
            return await _mediator.Send(command);
            
        }
        [HttpGet("get-all-posts")]
        public async Task<StandardResponse<PagedResponse<PostResponse>>> GetAllPosts([FromQuery] GetAllPostsQuery command)
        {
            return await _mediator.Send(command);
        }
        [HttpGet("get-post")]
        public async Task<StandardResponse<PostResponse>> GetPost([FromQuery] Guid id)
        {
            return await _mediator.Send(new GetPostQuery
            {
                Id = id
            });
        }
        [HttpGet("get-user-active-post")]
        public async Task<StandardResponse<PostResponse>> GetPost([FromQuery] GetUserActivePostQuery command)
        {
            return await _mediator.Send(command);
        }
        [HttpPost("set-post-activeness")]
        public async Task<StandardResponse<PostResponse>> GetPost([FromBody] SetPostActivenessCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
