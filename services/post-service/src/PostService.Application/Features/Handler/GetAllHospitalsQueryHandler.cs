using MediatR;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Features.Common;
using PostService.Application.Mapping;
using PostService.Application.Features.Queries;
using PostService.Domain.BusinessModels;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Features.Handler
{
    public class GetAllHospitalsQueryHandler : IRequestHandler<GetAllHospitalsQuery, StandardResponse<PagedResponse<HospitalResponse>>>
    {
        private readonly IHospitalRepository _hospitalRepository;

        public GetAllHospitalsQueryHandler(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        public async Task<StandardResponse<PagedResponse<HospitalResponse>>> Handle(GetAllHospitalsQuery request, CancellationToken cancellationToken)
        {
            var response = new StandardResponse<PagedResponse<HospitalResponse>>();
            var pagedResponse = new PagedResponse<HospitalResponse>();

            var query = _hospitalRepository.GetAll().AsNoTracking();

            if (request.LatStart.HasValue && request.LatEnd.HasValue)
                query = query.Where(x => x.Lat >= request.LatStart.Value && x.Lat <= request.LatEnd.Value);
            if (request.LonStart.HasValue && request.LonEnd.HasValue)
                query = query.Where(x => x.Lon >= request.LonStart.Value && x.Lon <= request.LonEnd.Value);
            if (request.DistrictId.HasValue)
                query = query.Where(x => x.DistrictId == request.DistrictId);
            if (request.CityId.HasValue)
                query = query.Where(x => x.District.CityId == request.CityId);
            if (request.CountryId.HasValue)
                query = query.Where(x => x.District.City.CountryId == request.CountryId);
            if (!string.IsNullOrWhiteSpace(request.Name))
                // ILIKE lets PostgreSQL do case-insensitive matching; ToLowerInvariant mishandles Turkish İ/ı.
                query = query.Where(x => EF.Functions.ILike(x.Name, $"%{request.Name.Trim()}%"));

            // Counted after filtering, so TotalCount describes the result the client is paging through.
            pagedResponse.TotalCount = await query.CountAsync(cancellationToken);

            query = request.Sorting switch
            {
                SortingType.Newest => query.OrderByDescending(x => x.CreationTime),
                SortingType.Oldest => query.OrderBy(x => x.CreationTime),
                _ => query.OrderBy(x => x.Name)
            };

            var hospitals = await query
                .Include(x => x.District)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Country)
                .Skip(request.ShowingResultsFrom)
                .Take(request.Paging)
                .ToListAsync(cancellationToken);
            pagedResponse.Items = hospitals.Select(h => h.ToResponse()).ToList();

            response.Response = pagedResponse;
            return response;
        }
    }
}
