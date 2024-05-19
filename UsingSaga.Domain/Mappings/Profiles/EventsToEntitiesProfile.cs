using AutoMapper;
using UsingSaga.Domain.Messages.Events;
using UsingSaga.Domain.States;

namespace UsingSaga.Domain.Mappings.Profiles
{
    public class EventsToEntitiesProfile : Profile
    {
        public EventsToEntitiesProfile()
        {
            CreateMap<IniciarSagaEvent, SagaState>();
            CreateMap<ScheduleEtapa02Event, SagaState>();
            CreateMap<RescheduleEtapa02Event, SagaState>();
            CreateMap<ErroSagaEvent, SagaState>();
        }
    }
}
