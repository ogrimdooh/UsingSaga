namespace UsingSaga.Domain.Messages.Events
{
    public abstract class BaseErroSagaEvent : BaseEvent
    {

        public string MensagemErro { get; set; }

    }
}
