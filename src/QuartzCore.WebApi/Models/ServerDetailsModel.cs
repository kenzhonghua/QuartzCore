using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuartzCore.WebApi.Models
{
    public class ServerDetailsModel
    {
        public ServerDetailsModel(IEnumerable<IScheduler> schedulers)
        {
            Name = Environment.MachineName;
            Address = "localhost";
            Schedulers = schedulers.Select(x => x.SchedulerName).ToList();
        }

        public string Name { get; private set; }
        public string Address { get; private set; }
        public IReadOnlyList<string> Schedulers { get; set; }
    }
}