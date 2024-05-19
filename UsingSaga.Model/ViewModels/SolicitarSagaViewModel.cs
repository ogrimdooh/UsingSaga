using System;

namespace UsingSaga.Model.ViewModels
{
    public class SolicitarSagaViewModel
    {

        public class Request
        {

            public bool ExecuteWaitSchedule { get; set; }
            public bool ExecuteReschedule { get; set; }
            public int RescheduleCount { get; set; }
            public int ScheduleInterval { get; set; }

        }

        public class Response
        {

            public Guid CorrelationId { get; set; }

        }

    }
}
