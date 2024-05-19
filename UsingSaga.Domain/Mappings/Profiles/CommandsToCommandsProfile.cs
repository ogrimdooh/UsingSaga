using AutoMapper;
using UsingSaga.Domain.Messages.Commands;

namespace UsingSaga.Domain.Mappings.Profiles
{
    public class CommandsToCommandsProfile : Profile
    {
        public CommandsToCommandsProfile()
        {
            CreateMap<Etapa01Command, Etapa02Command>();
        }
    }
}
