using PostService.API.Telemetry;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using PostService.API.Auth;
using PostService.API.ExceptionHandler;
using PostService.Application;
using PostService.Infrastructure;
using PostService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureLayerServiceConfiguration(builder.Configuration);
builder.Services.AddApplicationLayerServiceConfiguration(builder.Configuration);

builder.Services.AddTracing(builder.Configuration, builder.Environment);

builder.Services.AddKeycloakJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// /health/live: the process is up. /health/ready: the database and the RabbitMQ bus (registered by MassTransit) are reachable.
builder.Services.AddHealthChecks().AddDbContextCheck<PostServiceDbContext>("database", tags: ["ready"]);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PostServiceDbContext>();
    await dbContext.Database.MigrateAsync();
    await dbContext.SeedData();
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
