using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.WebApi.Models
{
    public enum SchedulerStatus
    {
        Unknown = 0,
        Running = 1,
        Standby = 2,
        Shutdown = 3,
    }
}
