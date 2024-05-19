using AutoMapper;
using MassTransit;
using System;
using UsingSaga.Domain.Exeptions;
using UsingSaga.Domain.Messages;
using UsingSaga.Domain.Messages.Commands;
using UsingSaga.Domain.Messages.Events;
using UsingSaga.Domain.States;
using UsingSaga.Infra.Bus.Sagas.Activities;

namespace UsingSaga.Infra.Bus.Sagas.StateMachines
{
    public class SagaStateMachine : BaseSagaStateMachine<IniciarSagaEvent, ErroSagaEvent, SagaState>
    {

        public State EmEtapa01 { get; set; }
        public State EmScheduleEtapa02 { get; set; }
        public State EmEtapa02 { get; set; }
        public State EmRescheduleEtapa02 { get; set; }

        public Event<Etapa01Command> Etapa01 { get; set; }
        public Event<Etapa02Command> Etapa02 { get; set; }
        public Event<ScheduleEtapa02Event> ScheduleEtapa02 { get; set; }
        public Event<RescheduleEtapa02Event> RescheduleEtapa02 { get; set; }

        public SagaStateMachine(IMapper mapper) 
            : base(mapper) 
        { 

        }

        protected override void ConfigurarEstados()
        {
            base.ConfigurarEstados();
            State(() => EmEtapa01);
            State(() => EmScheduleEtapa02);
            State(() => EmEtapa02);
            State(() => EmRescheduleEtapa02);
        }

        protected override void ConfigurarEventos()
        {
            base.ConfigurarEventos();
            Event(() => ScheduleEtapa02, c =>
            {
                c.CorrelateById(m => m.Message.CorrelationId);
                c.SelectId(m => m.Message.CorrelationId);
            });
            Event(() => RescheduleEtapa02, c => {
                c.CorrelateById(m => m.Message.CorrelationId);
                c.SelectId(m => m.Message.CorrelationId);
            });
            Event(() => Etapa01, c => c.CorrelateById(m => m.Message.CorrelationId));
            Event(() => Etapa02, c => c.CorrelateById(m => m.Message.CorrelationId));
        }

        protected override State GetStartState()
        {
            return EmEtapa01;
        }

        protected override Task OnIniciarSaga(BehaviorContext<SagaState, IniciarSagaEvent> action)
        {
            return action.Raise(Etapa01, _mapper.Map<Etapa01Command>(action.Message));
        }

        protected override void DefinirSaga()
        {
            base.DefinirSaga();

            During(
                EmEtapa01,
                When(Etapa01)
                    .Then(c => _mapper.Map(c.Message, c.Saga))
                    .Then(c => LogOperacao(c.Saga, c.Message))
                    .Activity(c => c.OfType<Etapa01Activity>())
                    .IfElse(
                        c => c.Message.ExecuteWaitSchedule,
                        a => a.TransitionTo(EmScheduleEtapa02)
                            .ThenAsync(x => 
                                x.ScheduleSend(DateTime.Now.AddSeconds(x.Message.ScheduleInterval), _mapper.Map<ScheduleEtapa02Event>(x.Message))
                            ),
                        b => b.TransitionTo(EmEtapa02)
                            .ThenAsync(x =>
                                x.Raise(Etapa02, _mapper.Map<Etapa02Command>(x.Message))
                            )
                    )
                    .Catch<Exception>(c => EnviarErro(c))
            );

            During(
                EmScheduleEtapa02,
                When(ScheduleEtapa02)
                    .Then(c => _mapper.Map(c.Message, c.Saga))
                    .Then(c => LogOperacao(c.Saga, c.Message))
                    .TransitionTo(EmEtapa02)
                    .ThenAsync(x =>
                        x.Raise(Etapa02, _mapper.Map<Etapa02Command>(x.Message))
                    )
                    .Catch<Exception>(c => EnviarErro(c))
            );

            During(
                EmEtapa02,
                When(Etapa02)
                    .Then(c => _mapper.Map(c.Message, c.Saga))
                    .Then(c => LogOperacao(c.Saga, c.Message))
                    .Activity(c => c.OfType<Etapa02Activity>())
                    .Finalize()
                    .Catch<Exception>(c => 
                        c.IfElse(
                            x => x.Exception is RescheduleException,
                            c => ExecutarReschedule(c),
                            c => EnviarErro(c)
                        )
                    )
            );

            During(
                EmRescheduleEtapa02,
                When(RescheduleEtapa02)
                    .Then(c => _mapper.Map(c.Message, c.Saga))
                    .Then(c => LogOperacao(c.Saga, c.Message))
                    .TransitionTo(EmEtapa02)
                    .ThenAsync(x =>
                        x.Raise(Etapa02, _mapper.Map<Etapa02Command>(x.Message))
                    )
                    .Catch<Exception>(c => EnviarErro(c))
            );

        }

        protected ExceptionActivityBinder<SagaState, D, X> ExecutarReschedule<D, X>(ExceptionActivityBinder<SagaState, D, X> context)
            where X : Exception
            where D : BaseMessage
        {
            return context
                .Then(c => LogOperacao(c.Saga, c.Message))
                .Then(c => _mapper.Map(c.Message, c.Saga))
                .TransitionTo(EmRescheduleEtapa02)
                .ThenAsync(x =>
                    x.ScheduleSend(DateTime.Now.AddSeconds(x.Message.ScheduleInterval), _mapper.Map<RescheduleEtapa02Event>(x.Message))
                );
        }

    }
}
