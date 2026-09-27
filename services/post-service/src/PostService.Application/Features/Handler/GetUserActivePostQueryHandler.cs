using MediatR;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Features.Common;
using PostService.Application.Mapping;
using PostService.Application.Features.Queries;
using PostService.Application.Posts;
using PostService.Application.Security;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Features.Handler
{
    public class GetUserActivePostQueryHandler : IRequestHandler<GetUserActivePostQuery, StandardResponse<PostResponse>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICurrentUser _currentUser;
        public GetUserActivePostQueryHandler(IPostRepository postRepository, ICurrentUser currentUser)
        {
            _postRepository = postRepository;
            _currentUser = currentUser;
        }

        public async Task<StandardResponse<PostResponse>> Handle(GetUserActivePostQuery request, CancellationToken cancellationToken)
        {
            if (!request.UserId.HasValue)
                request.UserId = _currentUser.UserId;
            var ownerId = request.UserId.ToString();
            var response = new StandardResponse<PostResponse>();
            var activePost = await _postRepository.GetAll().AsNoTracking()
                .Include(x => x.Hospital)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Country)
                .WhereLive()
                .FirstOrDefaultAsync(x => x.OwnerId == ownerId, cancellationToken);
            if (activePost == null)
                return response;
            response.Response = activePost.ToResponse();
            return response;
        }
    }
}
