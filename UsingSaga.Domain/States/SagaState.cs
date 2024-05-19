namespace UsingSaga.Domain.States
{
    public class SagaState : BaseSagaState
    {

        public bool ExecuteWaitSchedule { get; set; }
        public bool ExecuteReschedule { get; set; }
        public int RescheduleCount { get; set; }
        public int ScheduleInterval { get; set; }

        public int ExecutedReschedules { get; set; }

    }
}
