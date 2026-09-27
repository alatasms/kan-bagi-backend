using MediatR;
using PostService.Application.Features.Common;

namespace PostService.Application.Features.Queries
{
    public class GetHospitalQuery: IRequest<StandardResponse<HospitalResponse>>
    {
        public int Id { get; set; }
    }
}
