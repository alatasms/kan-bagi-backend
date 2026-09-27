using MediatR;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Features.Common;
using PostService.Application.Mapping;
using PostService.Application.Features.Queries;
using PostService.Application.Posts;
using PostService.Application.Security;
using PostService.Domain.BusinessModels;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Features.Handler
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, StandardResponse<PagedResponse<PostResponse>>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICurrentUser _currentUser;

        public GetAllPostsQueryHandler(IPostRepository postRepository, ICurrentUser currentUser)
        {
            _postRepository = postRepository;
            _currentUser = currentUser;
        }

        public async Task<StandardResponse<PagedResponse<PostResponse>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var response = new StandardResponse<PagedResponse<PostResponse>>();
            var pagedResponse = new PagedResponse<PostResponse>();
            var query = _postRepository.GetAll().AsNoTracking();

            if (request.PostIds != null && request.PostIds.Count != 0)
            {
                // Explicit id lookup (used by the gateway aggregate); the validator caps the list size.
                var parsedIds = request.PostIds.Select(id => Guid.TryParse(id, out var guid) ? guid : Guid.Empty).ToHashSet();
                query = query.Where(x => parsedIds.Contains(x.Id));
                request.ShowingResultsFrom = 0;
                request.Paging = parsedIds.Count;
            }
            else
            {
                if (request.UserId.HasValue)
                {
                    if (request.UserId.Value != _currentUser.UserId)
                        throw new UnauthorizedAccessException("Not Authorized!");

                    query = query.Where(x => x.OwnerId == request.UserId.ToString());
                }
                else
                {
                    var currentUserId = _currentUser.UserId.ToString();
                    query = query.WhereLive().Where(x => x.OwnerId != currentUserId);
                }

                if (request.Hospitals != null && request.Hospitals.Count > 0)
                    query = query.Where(x => request.Hospitals.Contains(x.HospitalId));
                if (request.BloodTypes != null && request.BloodTypes.Count > 0)
                    query = query.Where(x => request.BloodTypes.Contains(x.BloodType));

            }

            // Counted after filtering, so TotalCount describes the result the client is paging through.
            pagedResponse.TotalCount = await query.CountAsync(cancellationToken);

            query = request.Sorting switch
            {
                SortingType.Newest => query.OrderByDescending(x => x.CreationTime),
                SortingType.Oldest => query.OrderBy(x => x.CreationTime),
                _ => query.OrderBy(x => x.Hospital.Name).ThenByDescending(x => x.CreationTime)
            };

            var posts = await query
                .Include(x => x.Hospital)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Country)
                .Skip(request.ShowingResultsFrom)
                .Take(request.Paging)
                .ToListAsync(cancellationToken);
            pagedResponse.Items = posts.Select(p => p.ToResponse()).ToList();

            response.Response = pagedResponse;
            return response;
        }
    }
}
