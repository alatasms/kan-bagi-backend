using PostService.Application.Behaviors;
using PostService.Application.Features.Commands;
using PostService.Application.Features.Queries;
using PostService.Application.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MassTransit;
using PostService.Domain.Events;
using PostService.Application.Security;
using PostService.Application.Consumers;
using PostService.Application.Contracts;
using PostService.Application.Posts;
using PostService.Infrastructure.Data;

namespace PostService.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayerServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            });
            services.AddScoped<IValidator<CreatePostCommand>, CreatePostCommandValidator>();
            services.AddScoped<IValidator<UpdatePostCommand>, UpdatePostCommandValidator>();
            services.AddScoped<IValidator<GetAllHospitalsQuery>, GetAllHospitalsQueryValidator>();
            services.AddScoped<IValidator<GetAllPostsQuery>, GetAllPostsQueryValidator>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<OwnerPostExpirer>();

            services.Configure<PostOptions>(configuration.GetSection(PostOptions.SectionName));
            services.AddHostedService<PostExpiryService>();

            services.AddMassTransit(x =>
            {
                // Transactional outbox: events published in a request are stored in the same database
                // transaction as the data change and delivered afterwards, so neither can happen without
                // the other. Handlers therefore publish first and call SaveChangesAsync last. Consumers get
                // an inbox as well, so a redelivered message is not processed twice.
                x.AddEntityFrameworkOutbox<PostServiceDbContext>(o =>
                {
                    o.UsePostgres();
                    o.UseBusOutbox();
                });

                x.AddConsumer<UserProfileCompletedConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqHost = configuration.GetRequiredSection("MessageBroker:RabbitMQ:Host").Value;
                    var rabbitMqUsername = configuration.GetRequiredSection("MessageBroker:RabbitMQ:Username").Value;
                    var rabbitMqPassword = configuration.GetRequiredSection("MessageBroker:RabbitMQ:Password").Value;
                    cfg.Host($"{rabbitMqHost}", h =>
                    {
                        h.Username(rabbitMqUsername!);
                        h.Password(rabbitMqPassword!);
                    });

                    // Fanout exchanges with stable names so non-.NET consumers can bind to them; see docs/events.md.
                    cfg.Message<NewPostCreatedEvent>(m => m.SetEntityName("new-post-created"));
                    cfg.Publish<NewPostCreatedEvent>(p => p.ExchangeType = "fanout");
                    cfg.Message<PostDeletedEvent>(m => m.SetEntityName("post-deleted"));
                    cfg.Publish<PostDeletedEvent>(p => p.ExchangeType = "fanout");
                    cfg.Message<UserPostExpiredEvent>(m => m.SetEntityName("user-post-expired"));
                    cfg.Publish<UserPostExpiredEvent>(p => p.ExchangeType = "fanout");

                    // Owner names for posts come from user-service. Queue names are prefixed with the service so
                    // each subscriber gets its own copy of the fanout.
                    cfg.Message<UserProfileCompletedEvent>(m => m.SetEntityName("user-profile-completed"));
                    cfg.ReceiveEndpoint("post-service.user-profile-completed", e =>
                    {
                        e.Durable = true;
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.UseEntityFrameworkOutbox<PostServiceDbContext>(context);
                        e.ConfigureConsumer<UserProfileCompletedConsumer>(context);
                    });
                });
            });

        }
    }
}
