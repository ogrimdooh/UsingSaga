using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Runtime.CompilerServices;
using UsingSaga.Domain.Interfaces.Infra.Bus;
using UsingSaga.Domain.Messages.Events;
using UsingSaga.Domain.Settings;
using UsingSaga.Domain.States;
using UsingSaga.Infra.Bus.Sagas.StateMachines;

namespace UsingSaga.Infra.Bus
{
    public static class ServicesExtensions
    {

        public static void AddListenerBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSagaWorker(configuration);
        }

        public static void AddPublisherBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBusPublisher(configuration);
        }

        private static void AddBusPublisher(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetRabbitMqSettings();
            services.AddMassTransit((Action<IBusRegistrationConfigurator<ISagaBus>>)(x =>
            {
                x.UsingRabbitMq((provider, cfg) =>
                {
                    cfg.Host(settings.ConnectionString);
                    cfg.ConfigurePublishEndpoints(settings);
                    cfg.UseNewtonsoftJsonSerializer();
                    cfg.UseNewtonsoftJsonDeserializer();
                });
            }));
        }

        private static void AddSagaWorker(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetRabbitMqSettings();
            services.AddMassTransit((Action<IBusRegistrationConfigurator<ISagaBus>>)(x =>
            {
                x.AddDelayedMessageScheduler();
                x.AddSagaStateMachine<SagaStateMachine, SagaState>()
                    .InMemoryRepository();
                x.UsingRabbitMq((provider, cfg) =>
                {
                    cfg.Host(settings.ConnectionString);
                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureSagaEndpoints(provider, settings);
                    cfg.UseNewtonsoftJsonSerializer();
                    cfg.UseNewtonsoftJsonDeserializer();
                });
            }));
        }

        private static void ConfigureSagaEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext provider, RabbitMqSettings settings)
        {
            cfg.ReceiveEndpoint(settings.IniciarSaga.EndpointName, e =>
            {
                e.PrefetchCount = settings.IniciarSaga.PrefetchCount;
                e.ConcurrentMessageLimit = settings.IniciarSaga.ConcurrentMessageLimit;
                e.StateMachineSaga<SagaState>(provider);
                EndpointConvention.Map<IniciarSagaEvent>(e.InputAddress);
            });

            cfg.ReceiveEndpoint(settings.ScheduleEtapa02.EndpointName, e =>
            {
                e.PrefetchCount = settings.ScheduleEtapa02.PrefetchCount;
                e.ConcurrentMessageLimit = settings.ScheduleEtapa02.ConcurrentMessageLimit;
                e.UseInMemoryOutbox();
                e.StateMachineSaga<SagaState>(provider);
                EndpointConvention.Map<ScheduleEtapa02Event>(e.InputAddress);
            });

            cfg.ReceiveEndpoint(settings.RescheduleEtapa02.EndpointName, e =>
            {
                e.PrefetchCount = settings.RescheduleEtapa02.PrefetchCount;
                e.ConcurrentMessageLimit = settings.RescheduleEtapa02.ConcurrentMessageLimit;
                e.UseInMemoryOutbox();
                e.StateMachineSaga<SagaState>(provider);
                EndpointConvention.Map<RescheduleEtapa02Event>(e.InputAddress);
            });
        }

        private static void ConfigurePublishEndpoints(this IRabbitMqBusFactoryConfigurator cfg, RabbitMqSettings settings)
        {
            cfg.Message<IniciarSagaEvent>(x => { x.SetEntityName(settings.IniciarSaga.EndpointName); });
            cfg.Publish<IniciarSagaEvent>();

            cfg.Message<ScheduleEtapa02Event>(x => { x.SetEntityName(settings.ScheduleEtapa02.EndpointName); });
            cfg.Publish<ScheduleEtapa02Event>();

            cfg.Message<RescheduleEtapa02Event>(x => { x.SetEntityName(settings.RescheduleEtapa02.EndpointName); });
            cfg.Publish<RescheduleEtapa02Event>();
        }

        private static RabbitMqSettings GetRabbitMqSettings(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();
        }
    }
}
