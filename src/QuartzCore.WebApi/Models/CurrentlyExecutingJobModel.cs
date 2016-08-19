using Quartz;
using System;

namespace QuartzCore.WebApi.Models
{
    public class CurrentlyExecutingJobModel
    {
        public CurrentlyExecutingJobModel(IJobExecutionContext context)
        {
            FireInstanceId = context.FireInstanceId;
            FireTime = context.FireTimeUtc;
            Trigger = new KeyModel(context.Trigger.Key);
            Job = new KeyModel(context.JobDetail.Key);
            JobRunTime = context.JobRunTime;
            RefireCount = context.RefireCount;

            Recovering = context.Recovering;
            if (context.Recovering)
            {
                RecoveringTrigger = new KeyModel(context.RecoveringTriggerKey);
            }
        }

        public string FireInstanceId { get; private set; }
        public DateTimeOffset? FireTime { get; private set; }
        public KeyModel Trigger { get; private set; }
        public KeyModel Job { get; private set; }
        public TimeSpan JobRunTime { get; private set; }
        public int RefireCount { get; private set; }
        public KeyModel RecoveringTrigger { get; private set; }
        public bool Recovering { get; private set; }
    }
}
