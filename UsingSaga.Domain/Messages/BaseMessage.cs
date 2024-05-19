using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingSaga.Domain.Messages
{
    public abstract class BaseMessage
    {

        public Guid CorrelationId { get; set; }

        public bool ExecuteWaitSchedule { get; set; }
        public bool ExecuteReschedule { get; set; }
        public int RescheduleCount { get; set; }
        public int ScheduleInterval { get; set; }

    }
}
