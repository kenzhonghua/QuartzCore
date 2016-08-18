using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var result = Run().Result;

            Console.WriteLine("===========Start============");
            Console.ReadKey();
        }

        public static async Task<string> Run()
        {
            var properties = new NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = "XmlConfiguredInstance",
                ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.threadPool.threadCount"] = "5",
                ["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz",
                ["quartz.plugin.xml.fileNames"] = "~/quartz_jobs.xml",
                ["quartz.serializer.type"] = "json"
            };

            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler sched = await sf.GetScheduler();
            var dailyCalendar = new DailyCalendar("00:01", "23:59");
            dailyCalendar.InvertTimeRange = true;
            await sched.AddCalendar("cal1", dailyCalendar, false, false);

            await sched.Start();

            WebApi.Program.Main(null);

            return "OK";
        }
    }

    public class SimpleJob : IJob
    {
        public virtual Task Execute(IJobExecutionContext context)
        {
            JobKey jobKey = context.JobDetail.Key;
            Console.WriteLine("SimpleJob says: {0} executing at {1}", jobKey, DateTime.Now.ToString("r"));
            return Task.FromResult(0);
        }
    }
}
