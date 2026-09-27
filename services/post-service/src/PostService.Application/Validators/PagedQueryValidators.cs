using FluentValidation;
using PostService.Application.Features.Common;
using PostService.Application.Features.Queries;

namespace PostService.Application.Validators
{
    /// <summary>Bounds every list endpoint so a single request cannot pull a whole table.</summary>
    public class PagedRequestRules : AbstractValidator<PagedRequest>
    {
        public const int MaxPageSize = 100;

        public PagedRequestRules()
        {
            RuleFor(x => x.ShowingResultsFrom).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Paging).InclusiveBetween(1, MaxPageSize);
        }
    }

    public class GetAllHospitalsQueryValidator : AbstractValidator<GetAllHospitalsQuery>
    {
        public GetAllHospitalsQueryValidator()
        {
            Include(new PagedRequestRules());
            RuleFor(x => x.Name).MaximumLength(100);
        }
    }

    public class GetAllPostsQueryValidator : AbstractValidator<GetAllPostsQuery>
    {
        public GetAllPostsQueryValidator()
        {
            Include(new PagedRequestRules());
            RuleFor(x => x.PostIds).Must(ids => ids == null || ids.Count <= PagedRequestRules.MaxPageSize)
                .WithMessage($"At most {PagedRequestRules.MaxPageSize} post ids can be requested at once.");
        }
    }
}
