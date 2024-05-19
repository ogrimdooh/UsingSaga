using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingSaga.Domain.Interfaces.Infra.Bus
{
    public interface ISagaState : SagaStateMachineInstance
    {

        string CurrentState { get; set; }

    }
}
