using UserService.Application.Common;
using UserService.Application.Features.Queries;
using UserService.Infrastructure.Services.IdentityVerification;
using MediatR;
using Microsoft.Extensions.Options;

namespace UserService.Application.Features.Handlers
{
    public class GetProfileConfigQueryHandler : IRequestHandler<GetProfileConfigQuery, StandardResponse<ProfileConfigResponse>>
    {
        private readonly IdentityVerificationOptions _identityOptions;

        public GetProfileConfigQueryHandler(IOptions<IdentityVerificationOptions> identityOptions)
        {
            _identityOptions = identityOptions.Value;
        }

        public Task<StandardResponse<ProfileConfigResponse>> Handle(GetProfileConfigQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new StandardResponse<ProfileConfigResponse>
            {
                Response = new ProfileConfigResponse
                {
                    IdentityVerificationEnabled = _identityOptions.Enabled
                }
            });
        }
    }
}
