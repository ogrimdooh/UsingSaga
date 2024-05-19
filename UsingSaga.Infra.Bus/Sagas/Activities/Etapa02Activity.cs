using MassTransit;
using UsingSaga.Domain.Exeptions;
using UsingSaga.Domain.Messages.Commands;
using UsingSaga.Domain.States;

namespace UsingSaga.Infra.Bus.Sagas.Activities
{    public class Etapa02Activity : BaseSagaActivity<SagaState, Etapa02Command>
    {

        protected override async Task OnExecute(BehaviorContext<SagaState, Etapa02Command> context, IBehavior<SagaState, Etapa02Command> next)
        {

            if (context.Saga.ExecuteReschedule && context.Saga.ExecutedReschedules < context.Saga.RescheduleCount)
            {
                context.Saga.ExecutedReschedules++;
                var msg = $"{GetType().Name} : Reschedule : {context.Saga.ExecutedReschedules} of {context.Saga.RescheduleCount}";
                _logger.Warning(msg);
                throw new RescheduleException(msg);
            }

        }

    }
}
