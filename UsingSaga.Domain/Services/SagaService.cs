using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsingSaga.Domain.Entities;
using UsingSaga.Domain.Interfaces.Infra.Bus;
using UsingSaga.Domain.Interfaces.Services;
using UsingSaga.Domain.Messages.Events;

namespace UsingSaga.Domain.Services
{
    public class SagaService : ISagaService
    {

        private readonly ISagaBus _bus;
        private readonly IMapper _mapper;

        public SagaService(ISagaBus bus, IMapper mapper) 
        {
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<Guid> SolicitarSaga(SolicitacaoSaga solicitacao)
        {
            var evento = _mapper.Map<IniciarSagaEvent>(solicitacao);
            await _bus.Publish(evento);
            return evento.CorrelationId;
        }
    }
}
