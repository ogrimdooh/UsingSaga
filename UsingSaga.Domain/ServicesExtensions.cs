using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsingSaga.Domain.Interfaces.Services;
using UsingSaga.Domain.Mappings.Profiles;
using UsingSaga.Domain.Services;

namespace UsingSaga.Domain
{
    public static class ServicesExtensions
    {

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.RegisterServices();
            services.RegisterMappers();
            return services;
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<ISagaService, SagaService>();
        }

        private static void RegisterMappers(this IServiceCollection services)
        {
            services.AddSingleton(p => new MapperConfiguration(c => {
                c.AddProfile(new ViewModelToEntitiesProfile());
                c.AddProfile(new EntitiesToEventsProfile());
                c.AddProfile(new EventsToEntitiesProfile());
                c.AddProfile(new CommandsToEntitiesProfile());
                c.AddProfile(new CommandsToEventsProfile());
                c.AddProfile(new EventsToCommandsProfile());
                c.AddProfile(new MessagesToErroProfile());
                c.AddProfile(new CommandsToCommandsProfile());                
            }).CreateMapper());
        }

    }
}
