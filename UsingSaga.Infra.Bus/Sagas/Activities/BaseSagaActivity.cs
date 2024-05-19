using MassTransit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsingSaga.Domain.Messages;
using UsingSaga.Domain.States;

namespace UsingSaga.Infra.Bus.Sagas.Activities
{
    public abstract class BaseSagaActivity<S, M> : IStateMachineActivity<S, M>
        where S : BaseSagaState
        where M : BaseMessage
    {

        protected readonly ILogger _logger;

        public BaseSagaActivity()
        {
            _logger = Log.ForContext<BaseSagaActivity<S, M>>();
        }

        public void Accept(StateMachineVisitor visitor)
        {
            _logger.Information($"{GetType().FullName} : Accept");
        }

        protected abstract Task OnExecute(BehaviorContext<S, M> context, IBehavior<S, M> next);

        public async Task Execute(BehaviorContext<S, M> context, IBehavior<S, M> next)
        {
            _logger.Information($"{GetType().FullName} : Execute");

            await OnExecute(context, next);

            await next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<S, M, TException> context, IBehavior<S, M> next) where TException : Exception
        {
            _logger.Information($"{GetType().FullName} : Faulted");

            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            _logger.Information($"{GetType().FullName} : Probe");
        }

    }
}
