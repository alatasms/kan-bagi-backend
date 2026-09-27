using MediatR;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Exceptions;
using PostService.Application.Features.Common;
using PostService.Application.Mapping;
using PostService.Application.Features.Queries;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Features.Handler
{
    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, StandardResponse<PostResponse>>
    {
        private readonly IPostRepository _postRepository;

        public GetPostQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<StandardResponse<PostResponse>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetAll().AsNoTracking()
                .AsNoTracking()
                .Include(x => x.Hospital)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (post == null)
                throw new NotFoundException("Post not found!");
            
            var response = post.ToResponse();
            return new StandardResponse<PostResponse>
            {
                Response = response
            };
        }
    }
}
