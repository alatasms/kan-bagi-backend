using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace APIGateway.Auth
{
    public static class KeycloakAuthenticationExtensions
    {
        /// <summary>
        /// Validates Keycloak access tokens: signature, issuer, audience and lifetime.
        /// Configuration section "Auth": Authority (issuer URL), MetadataAddress (optional, internal
        /// discovery URL), Audience, RequireHttpsMetadata.
        /// </summary>
        public static IServiceCollection AddKeycloakJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var auth = configuration.GetRequiredSection("Auth");
            var authority = auth["Authority"];
            if (string.IsNullOrWhiteSpace(authority))
                throw new InvalidOperationException("Auth:Authority is not configured.");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    if (!string.IsNullOrWhiteSpace(auth["MetadataAddress"]))
                        options.MetadataAddress = auth["MetadataAddress"]!;
                    options.Audience = auth["Audience"];
                    options.RequireHttpsMetadata = auth.GetValue("RequireHttpsMetadata", true);
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        NameClaimType = "preferred_username",
                        RoleClaimType = "roles"
                    };
                });

            return services;
        }
    }
}
