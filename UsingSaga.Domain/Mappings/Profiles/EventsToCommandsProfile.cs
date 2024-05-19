using AutoMapper;
using UsingSaga.Domain.Messages.Commands;
using UsingSaga.Domain.Messages.Events;

namespace UsingSaga.Domain.Mappings.Profiles
{
    public class EventsToCommandsProfile : Profile
    {
        public EventsToCommandsProfile()
        {
            CreateMap<IniciarSagaEvent, Etapa01Command>();
            CreateMap<ScheduleEtapa02Event, Etapa02Command>();
            CreateMap<RescheduleEtapa02Event, Etapa02Command>();
        }
    }
}
