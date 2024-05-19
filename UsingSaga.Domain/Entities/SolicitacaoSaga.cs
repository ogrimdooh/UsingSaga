using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingSaga.Domain.Entities
{
    public class SolicitacaoSaga
    {

        public bool ExecuteWaitSchedule { get; set; }
        public bool ExecuteReschedule { get; set; }
        public int RescheduleCount { get; set; }
        public int ScheduleInterval { get; set; }

    }
}
