using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsingSaga.Domain.Messages.Commands;
using UsingSaga.Domain.States;

namespace UsingSaga.Infra.Bus.Sagas.Activities
{
    public class Etapa01Activity : BaseSagaActivity<SagaState, Etapa01Command>
    {

        protected override async Task OnExecute(BehaviorContext<SagaState, Etapa01Command> context, IBehavior<SagaState, Etapa01Command> next)
        {

        }

    }
}
