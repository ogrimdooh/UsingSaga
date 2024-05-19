using AutoMapper;
using UsingSaga.Domain.Messages.Commands;
using UsingSaga.Domain.States;

namespace UsingSaga.Domain.Mappings.Profiles
{
    public class CommandsToEntitiesProfile : Profile
    {
        public CommandsToEntitiesProfile()
        {
            CreateMap<Etapa01Command, SagaState>();
            CreateMap<Etapa02Command, SagaState>();
        }
    }
}
