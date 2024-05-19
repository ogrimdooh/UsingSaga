using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingSaga.Domain.Settings
{
    public class RabbitMqSettings
    {
        public string ConnectionString { get; set; }
        public int RetryCount { get; set; }
        public int RetryInterval { get; set; }
        public string SchedulerEndpoint { get; set; }
        public QueueSettings IniciarSaga { get; set; }
        public QueueSettings ScheduleEtapa02 { get; set; }
        public QueueSettings RescheduleEtapa02 { get; set; }
    }
}
