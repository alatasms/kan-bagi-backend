using System.Security.Claims;
using PostService.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace PostService.Application.Security
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal Principal
        {
            get
            {
                var principal = _httpContextAccessor.HttpContext?.User;
                if (principal?.Identity?.IsAuthenticated != true)
                    throw new UnauthorizedException("No authenticated user.");
                return principal;
            }
        }

        public Guid UserId => Guid.TryParse(Principal.FindFirst("sub")?.Value, out var userId)
            ? userId
            : throw new UnauthorizedException("Access token has no valid subject.");

        public string Email => Principal.FindFirst("email")?.Value
            ?? throw new UnauthorizedException("Access token has no email claim.");

        public bool IsInRole(string role) => Principal.IsInRole(role);
    }
}
