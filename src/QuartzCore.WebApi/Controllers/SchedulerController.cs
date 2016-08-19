using Microsoft.AspNetCore.Mvc;
using Quartz.Impl;
using QuartzCore.WebApi.Models;
using QuartzCore.WebApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.WebApi.Controllers
{
    [Route("api/schedulers")]
    public class SchedulerController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<IReadOnlyList<SchedulerHeaderModel>> AllSchedulers()
        {
            var schedulers = await SchedulerRepository.Instance.LookupAll().ConfigureAwait(false);
            return schedulers.Select(x => new SchedulerHeaderModel(x)).ToList();
        }

        [HttpGet]
        [Route("{schedulerName}")]
        public async Task<SchedulerModel> SchedulerDetails(string schedulerName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var metaData = await scheduler.GetMetaData().ConfigureAwait(false);
            return new SchedulerModel(scheduler, metaData);
        }

        [HttpPost]
        [Route("{schedulerName}/start")]
        public async Task Start(string schedulerName, int? delayMilliseconds = null)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            if (delayMilliseconds == null)
            {
                await scheduler.Start().ConfigureAwait(false);
            }
            else
            {
                await scheduler.StartDelayed(TimeSpan.FromMilliseconds(delayMilliseconds.Value)).ConfigureAwait(false);
            }
        }

        [HttpPost]
        [Route("{schedulerName}/standby")]
        public async Task Standby(string schedulerName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.Standby().ConfigureAwait(false);
        }

        [HttpPost]
        [Route("{schedulerName}/shutdown")]
        public async Task Shutdown(string schedulerName, bool waitForJobsToComplete = false)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.Shutdown(waitForJobsToComplete).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("{schedulerName}/clear")]
        public async Task Clear(string schedulerName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.Clear().ConfigureAwait(false);
        }
    }
}
