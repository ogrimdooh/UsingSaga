using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsingSaga.Domain.Entities;
using UsingSaga.Model.ViewModels;

namespace UsingSaga.Domain.Mappings.Profiles
{
    public class ViewModelToEntitiesProfile : Profile
    {
        public ViewModelToEntitiesProfile() 
        {
            CreateMap<SolicitarSagaViewModel.Request, SolicitacaoSaga>();
        }
    }
}
