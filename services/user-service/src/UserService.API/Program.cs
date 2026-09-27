using UserService.API.Telemetry;
using UserService.API.Auth;
using UserService.API.ExceptionHandler;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureLayerServiceConfiguration(builder.Configuration, builder.Environment.IsProduction());
builder.Services.AddApplicationLayerServiceConfiguration(builder.Configuration);

builder.Services.AddTracing(builder.Configuration, builder.Environment);

builder.Services.AddKeycloakJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// /health/live: the process is up. /health/ready: the database and the RabbitMQ bus (registered by MassTransit) are reachable.
builder.Services.AddHealthChecks().AddDbContextCheck<UserServiceDbContext>("database", tags: ["ready"]);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });

app.Run();
