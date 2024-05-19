using AutoMapper;
using UsingSaga.Domain.Messages.Commands;
using UsingSaga.Domain.Messages.Events;

namespace UsingSaga.Domain.Mappings.Profiles
{
    public class CommandsToEventsProfile : Profile
    {
        public CommandsToEventsProfile()
        {
            CreateMap<Etapa01Command, ScheduleEtapa02Event>();
            CreateMap<Etapa02Command, RescheduleEtapa02Event>();
        }
    }
}
