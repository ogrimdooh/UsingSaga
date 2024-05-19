using AutoMapper;
using MassTransit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsingSaga.Domain.Messages;
using UsingSaga.Domain.Messages.Events;
using UsingSaga.Domain.States;
using UsingSaga.Infra.Bus.Sagas.Activities;

namespace UsingSaga.Infra.Bus.Sagas.StateMachines
{
    public abstract class BaseSagaStateMachine<T, E, S> : MassTransitStateMachine<S> 
        where T : BaseIniciarSagaEvent
        where E : BaseErroSagaEvent
        where S : BaseSagaState
    {

        public State ComErro { get; set; }

        public Event<T> IniciarSaga { get; set; }
        public Event<E> ErroSaga { get; set; }

        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;

        protected BaseSagaStateMachine(IMapper mapper)
        {
            _mapper = mapper;
            _logger = Log.ForContext<BaseSagaStateMachine<T, E, S>>();

            InstanceState(x => x.CurrentState);

            ConfigurarEstados();
            ConfigurarEventos();
            DefinirSaga();

            SetCompletedWhenFinalized();
        }

        protected virtual void ConfigurarEstados()
        {
            State(() => ComErro);
        }

        protected virtual void ConfigurarEventos()
        {
            Event(() => IniciarSaga, c => { 
                c.CorrelateById(m => m.Message.CorrelationId); 
                c.SelectId(m => m.Message.CorrelationId);
                c.InsertOnInitial = true;
            });
            Event(() => ErroSaga, c => c.CorrelateById(m => m.Message.CorrelationId));
        }

        protected virtual void DefinirSaga()
        {
            Initially(
                When(IniciarSaga)
                    .Then(c => _mapper.Map(c.Message, c.Saga))
                    .Then(c => LogOperacao(c.Saga, c.Message))
                    .TransitionTo(GetStartState())
                    .ThenAsync(OnIniciarSaga)
                    .Catch<Exception>(c => EnviarErro(c))
            );

            During(
                ComErro,
                When(ErroSaga)
                    .Then(c => _mapper.Map(c.Message, c.Saga))
                    .Then(c => LogOperacao(c.Saga, c.Message))
                    .Finalize()
            );
        }

        protected abstract State GetStartState();
        protected abstract Task OnIniciarSaga(BehaviorContext<S, T> action);

        protected void LogOperacao<M>(S state, M message) 
            where M : BaseMessage
        {
            _logger.Information($"{GetType().Name} : CurrentState = {state.CurrentState}, CorrelationId = {message.CorrelationId}");
        }

        protected void LogException<M, X>(S state, M message, X ex)
            where M : BaseMessage
            where X : Exception
        {
            _logger.Error($"{GetType().Name} : CurrentState = {state.CurrentState}, CorrelationId = {message.CorrelationId}, Exception[{typeof(X).Name}] = Message : {ex.Message} | StackTrace : {ex.StackTrace}");
        }

        protected ExceptionActivityBinder<S, D, X> EnviarErro<D, X>(ExceptionActivityBinder<S, D, X> context) 
            where X : Exception
            where D : BaseMessage
        {
            return context
                .Then(c => LogException(c.Saga, c.Message, c.Exception))
                .Then(c => _mapper.Map(c.Message, c.Saga))
                .TransitionTo(ComErro)
                .ThenAsync(c => 
                    c.Raise(ErroSaga, _mapper.Map<E>((c.Message, c.Exception)))
                );
        }

    }
}
