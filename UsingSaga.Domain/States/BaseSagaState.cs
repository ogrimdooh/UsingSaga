using MassTransit;
using UsingSaga.Domain.Interfaces.Infra.Bus;

namespace UsingSaga.Domain.States
{
    public abstract class BaseSagaState : ISagaState, ISagaVersion
    {

        public required string CurrentState { get; set; }
        public Guid CorrelationId { get; set; }
        public int Version { get; set; }

    }
}
