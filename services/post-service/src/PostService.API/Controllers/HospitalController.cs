using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.Features.Common;
using PostService.Application.Features.Queries;

namespace PostService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HospitalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HospitalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-hospitals")]
        public async Task<StandardResponse<PagedResponse<HospitalResponse>>> GetAllHospitals([FromQuery] GetAllHospitalsQuery requestDto)
        {
            return await _mediator.Send(requestDto);
        }

        [HttpGet("get-hospital")]
        public async Task<StandardResponse<HospitalResponse>> GetHospital([FromQuery] GetHospitalQuery requestDto)
        {
            return await _mediator.Send(requestDto);
        }
    }
}
