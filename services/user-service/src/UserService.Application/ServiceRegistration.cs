using UserService.Application.Behaviors;
using UserService.Application.Features.Commands;
using UserService.Application.Security;
using UserService.Application.Validators;
using UserService.Domain.Events;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories.Concretes;
using UserService.Infrastructure.Repositories.Interfaces;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace UserService.Application
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationPreferenceRepository, NotificationPreferenceRepository>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IValidator<CompleteProfileCommand>, RegisterCommandValidator>();
            services.AddHttpContextAccessor();
            services.AddMassTransit(x =>
            {
                // Transactional outbox: events published in a request are stored in the same database
                // transaction as the data change and delivered afterwards, so neither can happen without
                // the other. Handlers therefore publish first and call SaveChangesAsync last.
                x.AddEntityFrameworkOutbox<UserServiceDbContext>(o =>
                {
                    o.UsePostgres();
                    o.UseBusOutbox();
                });

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
                    cfg.Message<UserRegisteredEvent>(m => m.SetEntityName("user-registered"));
                    cfg.Publish<UserRegisteredEvent>(p => p.ExchangeType = "fanout");
                    cfg.Message<UserProfileCompletedEvent>(m => m.SetEntityName("user-profile-completed"));
                    cfg.Publish<UserProfileCompletedEvent>(p => p.ExchangeType = "fanout");
                    cfg.Message<NotificationPreferencesChangedEvent>(m => m.SetEntityName("notification-preferences-changed"));
                    cfg.Publish<NotificationPreferencesChangedEvent>(p => p.ExchangeType = "fanout");

                    cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                });
            });

        }
    }
}
