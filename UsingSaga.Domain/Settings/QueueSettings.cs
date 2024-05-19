namespace UsingSaga.Domain.Settings
{
    public class QueueSettings
    {
        public string EndpointName { get; set; }
        public int ConcurrentMessageLimit { get; set; }
        public int PrefetchCount { get; set; }
    }
}
