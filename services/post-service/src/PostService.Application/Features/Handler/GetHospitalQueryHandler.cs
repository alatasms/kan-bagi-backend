using MediatR;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Exceptions;
using PostService.Application.Features.Common;
using PostService.Application.Mapping;
using PostService.Application.Features.Queries;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Features.Handler
{
    public class GetHospitalQueryHandler : IRequestHandler<GetHospitalQuery, StandardResponse<HospitalResponse>>
    {
        private readonly IHospitalRepository _hospitalRepository;

        public GetHospitalQueryHandler(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        public async Task<StandardResponse<HospitalResponse>> Handle(GetHospitalQuery request, CancellationToken cancellationToken)
        {
            var hospital = await _hospitalRepository.GetAll().AsNoTracking()
                .Include(x => x.District)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (hospital == null)
                throw new NotFoundException("Hospital not found.");

            return new StandardResponse<HospitalResponse>
            {
                Response = hospital.ToResponse()
            };
        }
    }
}
