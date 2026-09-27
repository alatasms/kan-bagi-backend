using UserService.Application.Common;
using UserService.Application.Features.Commands;
using UserService.Application.Features.Common;
using UserService.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("config")]
        public async Task<StandardResponse<ProfileConfigResponse>> GetConfig()
        {
            return await _mediator.Send(new GetProfileConfigQuery());
        }

        [HttpPost("complete-profile")]
        public async Task<StandardResponse<UserInfoResponse>> CompleteProfile([FromBody] CompleteProfileCommand requestDto)
        {
            return await _mediator.Send(requestDto);
        }

        [HttpGet("get-session-info")]
        public async Task<StandardResponse<UserInfoResponse>> GetSessionInfo()
        {
            return await _mediator.Send(new GetSessionInfoQuery());
        }
        [HttpGet("is-profile-completed")]
        public async Task<StandardResponse<IsProfileCompletedQueryResponse>> IsProfileCompleted()
        {
            return await _mediator.Send(new IsProfileCompletedQuery());
        }
        [HttpPost("set-notification-preferences")]
        public async Task<StandardResponse<SetNotificationPreferenceResponse>> SetNotificationPreferences([FromBody] SetNotificationPreferenceCommand requestDto)
        {
            return await _mediator.Send(requestDto);
        }
        [HttpGet("get-user-info")]
        public async Task<StandardResponse<UserInfoResponse>> GetUserInfo([FromQuery] GetUserInfoQuery requestDto)
        {
            return await _mediator.Send(requestDto);
        }
        [HttpGet("get-notification-preferences")]
        public async Task<StandardResponse<NotificationPreferenceResponse>> GetUserInfo()
        {
            return await _mediator.Send(new GetNotificatiionPreferenceQuery());
        }
    }
}
