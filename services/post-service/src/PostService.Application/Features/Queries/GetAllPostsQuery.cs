using MediatR;
using PostService.Application.Features.Common;
using PostService.Domain.BusinessModels;

namespace PostService.Application.Features.Queries
{
    public class GetAllPostsQuery: PagedRequest, IRequest<StandardResponse<PagedResponse<PostResponse>>>
    {
        public Guid? UserId { get; set; }
        public List<int>? Hospitals { get; set; }
        public List<BloodType>? BloodTypes { get; set; }
        //public List<string> Locations { get; set; } 
        public List<string>? PostIds { get; set; }
    }
}
