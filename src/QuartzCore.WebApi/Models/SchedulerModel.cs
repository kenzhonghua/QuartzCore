using Quartz;
using Quartz.Util;

namespace QuartzCore.WebApi.Models
{
    public class SchedulerModel
    {
        public SchedulerModel(IScheduler scheduler, SchedulerMetaData metaData)
        {
            Name = scheduler.SchedulerName;
            SchedulerInstanceId = scheduler.SchedulerInstanceId;
            Status = SchedulerHeaderModel.TranslateStatus(scheduler);

            ThreadPool = new SchedulerThreadPoolModel(metaData);
            JobStore = new SchedulerJobStoreModel(metaData);
            Statistics = new SchedulerStatisticsModel(metaData);
        }

        public string SchedulerInstanceId { get; }
        public string Name { get; }
        public SchedulerStatus Status { get; }

        public SchedulerThreadPoolModel ThreadPool { get; }
        public SchedulerJobStoreModel JobStore { get; }
        public SchedulerStatisticsModel Statistics { get; }
    }

    public class SchedulerThreadPoolModel
    {
        public SchedulerThreadPoolModel(SchedulerMetaData metaData)
        {
            Type = metaData.ThreadPoolType.AssemblyQualifiedNameWithoutVersion();
            Size = metaData.ThreadPoolSize;
        }

        public string Type { get; private set; }
        public int Size { get; private set; }
    }

    public class SchedulerJobStoreModel
    {
        public SchedulerJobStoreModel(SchedulerMetaData metaData)
        {
            Type = metaData.JobStoreType.AssemblyQualifiedNameWithoutVersion();
            Clustered = metaData.JobStoreClustered;
            Persistent = metaData.JobStoreSupportsPersistence;
        }

        public string Type { get; private set; }
        public bool Clustered { get; private set; }
        public bool Persistent { get; private set; }
    }

    public class SchedulerStatisticsModel
    {
        public SchedulerStatisticsModel(SchedulerMetaData metaData)
        {
            NumberOfJobsExecuted = metaData.NumberOfJobsExecuted;
        }

        public int NumberOfJobsExecuted { get; private set; }
    }
}
