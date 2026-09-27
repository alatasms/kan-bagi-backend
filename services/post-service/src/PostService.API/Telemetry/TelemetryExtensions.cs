using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PostService.API.Telemetry
{
    public static class TelemetryExtensions
    {
        /// <summary>
        /// Distributed tracing for incoming requests, outgoing HTTP calls and MassTransit messages.
        /// Traces are exported over OTLP to Otlp:Endpoint (Jaeger in deploy/docker-compose.yml); without an
        /// endpoint nothing is exported.
        /// </summary>
        public static IServiceCollection AddTracing(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var otlpEndpoint = configuration["Otlp:Endpoint"];

            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                    .AddService("PostService")
                    .AddAttributes(new Dictionary<string, object> { ["deployment.environment"] = environment.EnvironmentName }))
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation(options => options.RecordException = true)
                        .AddHttpClientInstrumentation(options => options.RecordException = true)
                        .AddSource("MassTransit");

                    if (!string.IsNullOrWhiteSpace(otlpEndpoint))
                        tracing.AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint));
                });

            return services;
        }
    }
}
