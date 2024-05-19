using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UsingSaga.Domain.Entities;
using UsingSaga.Domain.Interfaces.Services;
using UsingSaga.Model.ViewModels;

namespace UsingSaga.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SagaController : ControllerBase
    {

        private readonly ISagaService _sagaService;
        private readonly IMapper _mapper;

        public SagaController(ISagaService sagaService, IMapper mapper)
        {
            _sagaService = sagaService;
            _mapper = mapper;
        }

        [HttpPost("solicitar")]
        [ProducesResponseType<SolicitarSagaViewModel.Response>((int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostSolicitarSaga([FromBody] SolicitarSagaViewModel.Request request)
        {
            var solicitacao = _mapper.Map<SolicitacaoSaga>(request);
            var id = await _sagaService.SolicitarSaga(solicitacao);
            return Ok(new SolicitarSagaViewModel.Response() { CorrelationId = id });
        }
    }
}
