using UserService.Application.Common;
using UserService.Application.Exceptions;
using UserService.Application.Features.Common;
using UserService.Application.Features.Queries;
using UserService.Application.Security;
using UserService.Infrastructure.Repositories.Interfaces;
using MediatR;
using UserService.Application.Mapping;

namespace UserService.Application.Features.Handlers
{
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, StandardResponse<UserInfoResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public GetUserInfoQueryHandler(IUserRepository userRepository, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<StandardResponse<UserInfoResponse>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            // Profiles hold personal data (phone, birth date, blood type). Only the owner, hospital
            // staff (verifying a donation) and admins may read them.
            var isOwner = request.UserId == _currentUser.UserId;
            if (!isOwner && !_currentUser.IsInRole(Roles.HospitalStaff) && !_currentUser.IsInRole(Roles.Admin))
                throw new ForbiddenException("You are not allowed to view this profile.");

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return new StandardResponse<UserInfoResponse>();

            return new StandardResponse<UserInfoResponse>()
            {
                Response = user.ToResponse()
            };
        }
    }
}
