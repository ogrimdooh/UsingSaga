using AutoMapper;
using UsingSaga.Domain.Messages;
using UsingSaga.Domain.Messages.Commands;
using UsingSaga.Domain.Messages.Events;

namespace UsingSaga.Domain.Mappings.Profiles
{
    public class MessagesToErroProfile : Profile
    {
        public MessagesToErroProfile()
        {
            ConfigurarMapperDeErro<IniciarSagaEvent>();
            ConfigurarMapperDeErro<Etapa01Command>();
            ConfigurarMapperDeErro<Etapa02Command>();
            ConfigurarMapperDeErro<ScheduleEtapa02Event>();
            ConfigurarMapperDeErro<RescheduleEtapa02Event>();
        }

        private void ConfigurarMapperDeErro<M>() where M : BaseMessage
        {
            CreateMap<(M, Exception), ErroSagaEvent>()
                .ForMember(d => d.CorrelationId, o => o.MapFrom(p => p.Item1.CorrelationId))
                .ForMember(d => d.ExecuteWaitSchedule, o => o.MapFrom(p => p.Item1.ExecuteWaitSchedule))
                .ForMember(d => d.ExecuteReschedule, o => o.MapFrom(p => p.Item1.ExecuteReschedule))
                .ForMember(d => d.RescheduleCount, o => o.MapFrom(p => p.Item1.RescheduleCount))
                .ForMember(d => d.ScheduleInterval, o => o.MapFrom(p => p.Item1.ScheduleInterval))
                .ForMember(d => d.MensagemErro, o => o.MapFrom(p => p.Item2.Message));
        }
    }
}
