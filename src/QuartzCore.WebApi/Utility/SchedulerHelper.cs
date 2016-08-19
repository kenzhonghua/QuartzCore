using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace QuartzCore.WebApi.Utility
{
    public class SchedulerHelper
    {
        public static async Task<IScheduler> GetScheduler(string schedulerName)
        {
            var scheduler = await SchedulerRepository.Instance.Lookup(schedulerName).ConfigureAwait(false);
            if (scheduler == null)
            {
                throw new Exception("未能找到Scheduler");
            }
            return scheduler;
        }
    }
}
