using Microsoft.AspNetCore.Mvc;
using Quartz;
using QuartzCore.WebApi.Models;
using QuartzCore.WebApi.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.WebApi.Controllers
{
    [Route("api/{schedulerName}/triggers")]
    public class TriggersController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<IReadOnlyList<KeyModel>> Triggers(string schedulerName, GroupMatcherModel groupMatcher)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var matcher = (groupMatcher ?? new GroupMatcherModel()).GetTriggerGroupMatcher();
            var jobKeys = await scheduler.GetTriggerKeys(matcher).ConfigureAwait(false);

            return jobKeys.Select(x => new KeyModel(x)).ToList();
        }

        [HttpGet]
        [Route("{triggerGroup}/{triggerName}/details")]
        public async Task<TriggerDetailModel> TriggerDetails(string schedulerName, string triggerGroup, string triggerName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var trigger = await scheduler.GetTrigger(new TriggerKey(triggerName, triggerGroup)).ConfigureAwait(false);
            var calendar = trigger.CalendarName != null
                ? await scheduler.GetCalendar(trigger.CalendarName).ConfigureAwait(false)
                : null;
            return TriggerDetailModel.Create(trigger, calendar);
        }

        [HttpPost]
        [Route("{triggerGroup}/{triggerName}/pause")]
        public async Task PauseTrigger(string schedulerName, string triggerGroup, string triggerName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.PauseTrigger(new TriggerKey(triggerName, triggerGroup)).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("pause")]
        public async Task PauseTriggers(string schedulerName, GroupMatcherModel groupMatcher)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var matcher = (groupMatcher ?? new GroupMatcherModel()).GetTriggerGroupMatcher();
            await scheduler.PauseTriggers(matcher).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("{triggerGroup}/{triggerName}/resume")]
        public async Task ResumeTrigger(string schedulerName, string triggerGroup, string triggerName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.ResumeTrigger(new TriggerKey(triggerName, triggerGroup)).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("resume")]
        public async Task ResumeTriggers(string schedulerName, GroupMatcherModel groupMatcher)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var matcher = (groupMatcher ?? new GroupMatcherModel()).GetTriggerGroupMatcher();
            await scheduler.ResumeTriggers(matcher).ConfigureAwait(false);
        }
    }
}
