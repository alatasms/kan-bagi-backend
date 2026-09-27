using UserService.Application.Common;
using UserService.Application.Features.Queries;
using UserService.Application.Security;
using UserService.Infrastructure.Repositories.Interfaces;
using UserService.Infrastructure.Services.IdentityVerification;
using MediatR;
using Microsoft.Extensions.Options;

namespace UserService.Application.Features.Handlers
{
    public class IsProfileCompletedQueryHandler : IRequestHandler<IsProfileCompletedQuery, StandardResponse<IsProfileCompletedQueryResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IdentityVerificationOptions _identityOptions;

        public IsProfileCompletedQueryHandler(IUserRepository userRepository, ICurrentUser currentUser, IOptions<IdentityVerificationOptions> identityOptions)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _identityOptions = identityOptions.Value;
        }

        public async Task<StandardResponse<IsProfileCompletedQueryResponse>> Handle(IsProfileCompletedQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_currentUser.UserId);

            // When a deployment turns verification on later, existing unverified profiles must be completed again.
            var completed = user != null && (!_identityOptions.Enabled || user.IsIdentityVerified);

            return new StandardResponse<IsProfileCompletedQueryResponse>
            {
                Response = new IsProfileCompletedQueryResponse
                {
                    ProfileCompleted = completed
                }
            };
        }
    }
}
