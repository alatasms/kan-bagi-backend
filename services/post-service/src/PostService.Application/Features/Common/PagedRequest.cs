using PostService.Domain.BusinessModels;

namespace PostService.Application.Features.Common
{
    public class PagedRequest
    {
        public int ShowingResultsFrom { get; set; } = 0;
        public int Paging { get; set; } = 10;
        public SortingType? Sorting { get; set; }
    }
}
