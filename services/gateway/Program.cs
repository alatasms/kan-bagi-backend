using APIGateway.Telemetry;
using APIGateway.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    // Ocelot merges Routes arrays by index, so exactly one route file is loaded per environment.
    .AddJsonFile(builder.Environment.IsDevelopment() ? "ocelot.Development.json" : "ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Ocelot configurations
builder.Services.AddOcelot(builder.Configuration).
    AddPolly();

// Keycloak access tokens. Ocelot routes reference this scheme via "AuthenticationProviderKey": "Bearer".
// Downstream services validate the same token again; the gateway is not the only line of defence.
builder.Services.AddKeycloakJwtAuthentication(builder.Configuration);
builder.Services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{
    // Browsers cannot set headers on WebSocket requests, so chat connections may pass the token as access_token.
    options.Events ??= new JwtBearerEvents();
    options.Events.OnMessageReceived = context =>
    {
        if (context.HttpContext.WebSockets.IsWebSocketRequest && context.Request.Query.TryGetValue("access_token", out var token))
            context.Token = token;
        return Task.CompletedTask;
    };
});

builder.Services.AddTracing(builder.Configuration, builder.Environment);



builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
});

// Services identify callers from the token only; strip identity headers so none can be forged downstream.
app.Use(async (context, next) =>
{
    foreach (var header in IdentityHeaders.Spoofable)
        context.Request.Headers.Remove(header);
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

// Ocelot does not run its authentication step for WebSocket routes, so enforce it here.
app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest && context.User.Identity?.IsAuthenticated != true)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.MapControllers();
app.MapHealthChecks("/health/live");

app.Use(async (context, next) =>
{
    var activity = System.Diagnostics.Activity.Current;
    if (activity != null)
    {
        string routeTemplate = string.Empty;
        if (context.Items.TryGetValue("DownstreamRoute", out var route))
        {
            // DownstreamRoute type is internal to Ocelot, so use dynamic to access UpstreamPathTemplate
            var upstreamPathTemplate = route?.GetType().GetProperty("UpstreamPathTemplate")?.GetValue(route)?.ToString();
            if (!string.IsNullOrEmpty(upstreamPathTemplate))
            {
                routeTemplate = upstreamPathTemplate;
                activity.SetTag("api.route", routeTemplate);
            }
        }
        else
        {
            // If DownstreamRoute is not present, use the request path
            routeTemplate = context.Request.Path.ToString();
            activity.SetTag("api.route", routeTemplate);
        }
        activity.DisplayName = $"{context.Request.Method} {routeTemplate}";
    }
    await next();
});

// Ocelot is terminal middleware, so it only gets requests the gateway does not answer itself.
app.MapWhen(
    context => !context.Request.Path.StartsWithSegments("/aggregate") && !context.Request.Path.StartsWithSegments("/health"),
    ocelot => ocelot.UseOcelot().Wait());

if (!app.Environment.IsDevelopment())
{
    app.Urls.Add("http://0.0.0.0:8080");
}

app.Run();
