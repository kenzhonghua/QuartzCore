using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using Quartz.Util;
using QuartzCore.WebApi.Utility;
using QuartzCore.WebApi.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace QuartzCore.WebApi.Controllers
{
    [Route("api/{schedulerName}/jobs")]
    [AuthorizeFilter]
    public class JobsController : Controller
    {
        [HttpGet]
        [Route("{jobGroup}/{jobName}/details")]
        public async Task<JobDetailModel> JobDetails(string schedulerName, string jobGroup, string jobName)
        {
            var scheduler = await GetScheduler(schedulerName).ConfigureAwait(false);
            var jobDetail = await scheduler.GetJobDetail(new JobKey(jobName, jobGroup)).ConfigureAwait(false);
            return new JobDetailModel(jobDetail);
        }

        private static async Task<IScheduler> GetScheduler(string schedulerName)
        {
            var scheduler = await SchedulerRepository.Instance.Lookup(schedulerName).ConfigureAwait(false);
            if (scheduler == null)
            {
                throw new System.Exception("");
            }
            return scheduler;
        }
    }

}
