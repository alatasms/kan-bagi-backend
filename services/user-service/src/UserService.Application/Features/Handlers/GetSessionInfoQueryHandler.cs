using UserService.Application.Common;
using UserService.Application.Features.Common;
using UserService.Application.Features.Queries;
using UserService.Application.Security;
using UserService.Infrastructure.Repositories.Interfaces;
using MediatR;
using UserService.Application.Mapping;

namespace UserService.Application.Features.Handlers
{
    public class GetSessionInfoQueryHandler : IRequestHandler<GetSessionInfoQuery, StandardResponse<UserInfoResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public GetSessionInfoQueryHandler(IUserRepository userRepository, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<StandardResponse<UserInfoResponse>> Handle(GetSessionInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_currentUser.UserId);
            return  new StandardResponse<UserInfoResponse>
            {
                Response = user?.ToResponse()
            };
        }
    }
}
