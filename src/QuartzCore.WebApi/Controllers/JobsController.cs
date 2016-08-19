using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using QuartzCore.WebApi.Models;
using QuartzCore.WebApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.WebApi.Controllers
{
    [Route("api/{schedulerName}/jobs")]
    [AuthorizeFilter]
    public class JobsController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<IReadOnlyList<KeyModel>> Jobs(string schedulerName, GroupMatcherModel groupMatcher)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var matcher = (groupMatcher ?? new GroupMatcherModel()).GetJobGroupMatcher();
            var jobKeys = await scheduler.GetJobKeys(matcher).ConfigureAwait(false);
            return jobKeys.Select(x => new KeyModel(x)).ToList();
        }

        [HttpGet]
        [Route("{jobGroup}/{jobName}/details")]
        public async Task<JobDetailModel> JobDetails(string schedulerName, string jobGroup, string jobName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var jobDetail = await scheduler.GetJobDetail(new JobKey(jobName, jobGroup)).ConfigureAwait(false);
            return new JobDetailModel(jobDetail);
        }

        [HttpGet]
        [Route("currently-executing")]
        public async Task<IReadOnlyList<CurrentlyExecutingJobModel>> CurrentlyExecutingJobs(string schedulerName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var currentlyExecutingJobs = await scheduler.GetCurrentlyExecutingJobs().ConfigureAwait(false);
            return currentlyExecutingJobs.Select(x => new CurrentlyExecutingJobModel(x)).ToList();
        }

        [HttpPost]
        [Route("{jobGroup}/{jobName}/pause")]
        public async Task PauseJobs(string schedulerName, string jobGroup, string jobName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.PauseJob(new JobKey(jobName, jobGroup)).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("pause")]
        public async Task PauseJobs(string schedulerName, GroupMatcherModel groupMatcher)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var matcher = (groupMatcher ?? new GroupMatcherModel()).GetJobGroupMatcher();
            await scheduler.PauseJobs(matcher).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("{jobGroup}/{jobName}/resume")]
        public async Task ResumeJob(string schedulerName, string jobGroup, string jobName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.ResumeJob(new JobKey(jobName, jobGroup)).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("resume")]
        public async Task ResumeJobs(string schedulerName, GroupMatcherModel groupMatcher)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var matcher = (groupMatcher ?? new GroupMatcherModel()).GetJobGroupMatcher();
            await scheduler.ResumeJobs(matcher).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("{jobGroup}/{jobName}/trigger")]
        public async Task TriggerJob(string schedulerName, string jobGroup, string jobName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.TriggerJob(new JobKey(jobName, jobGroup)).ConfigureAwait(false);
        }

        [HttpDelete]
        [Route("{jobGroup}/{jobName}")]
        public async Task DeleteJob(string schedulerName, string jobGroup, string jobName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.DeleteJob(new JobKey(jobName, jobGroup)).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("{jobGroup}/{jobName}/interrupt")]
        public async Task InterruptJob(string schedulerName, string jobGroup, string jobName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.Interrupt(new JobKey(jobName, jobGroup)).ConfigureAwait(false);
        }

        [HttpPut]
        [Route("{jobGroup}/{jobName}")]
        public async Task AddJob(string schedulerName, string jobGroup, string jobName, string jobType, bool durable, bool requestsRecovery, bool replace = false)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var jobDetail = new JobDetailImpl(jobName, jobGroup, Type.GetType(jobType), durable, requestsRecovery);
            await scheduler.AddJob(jobDetail, replace).ConfigureAwait(false);
        }
    }
}