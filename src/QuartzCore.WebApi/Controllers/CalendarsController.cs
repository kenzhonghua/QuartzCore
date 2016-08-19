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
    [Route("api/{schedulerName}/calendars")]
    public class CalendarsController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<IReadOnlyList<string>> Calendars(string schedulerName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var calendarNames = await scheduler.GetCalendarNames().ConfigureAwait(false);

            return calendarNames;
        }

        [HttpGet]
        [Route("{calendarName}")]
        public async Task<CalendarDetailModel> CalendarDetails(string schedulerName, string calendarName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            var calendar = await scheduler.GetCalendar(calendarName).ConfigureAwait(false);
            return CalendarDetailModel.Create(calendar);
        }

        [HttpPut]
        [Route("{calendarName}")]
        public async Task AddCalendar(string schedulerName, string calendarName, bool replace, bool updateTriggers)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            ICalendar calendar = null;
            await scheduler.AddCalendar(calendarName, calendar, replace, updateTriggers).ConfigureAwait(false);
        }

        [HttpDelete]
        [Route("{calendarName}")]
        public async Task DeleteCalendar(string schedulerName, string calendarName)
        {
            var scheduler = await SchedulerHelper.GetScheduler(schedulerName).ConfigureAwait(false);
            await scheduler.DeleteCalendar(calendarName).ConfigureAwait(false);
        }

    }
}
