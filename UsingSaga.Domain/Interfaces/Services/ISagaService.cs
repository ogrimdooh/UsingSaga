using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsingSaga.Domain.Entities;

namespace UsingSaga.Domain.Interfaces.Services
{
    public interface ISagaService
    {
        Task<Guid> SolicitarSaga(SolicitacaoSaga solicitacao);
    }
}
