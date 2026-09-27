using MediatR;
using PostService.Application.Features.Common;

namespace PostService.Application.Features.Queries
{
    public class GetAllHospitalsQuery:PagedRequest, IRequest<StandardResponse<PagedResponse<HospitalResponse>>>
    { 
        public string? Name { get; set; }
        public double? LatStart { get; set; }
        public double? LatEnd { get; set; }
        public double? LonStart { get; set; }
        public double? LonEnd { get; set; }

        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? CountryId { get; set; }
    }
}
