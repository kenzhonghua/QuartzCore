using Quartz;
using Quartz.Spi;
using Quartz.Util;
using System;
using System.Collections.Generic;

namespace QuartzCore.WebApi.Models
{
    public class TriggerDetailModel
    {
        protected TriggerDetailModel(ITrigger trigger, ICalendar calendar)
        {
            Description = trigger.Description;
            TriggerType = trigger.GetType().AssemblyQualifiedNameWithoutVersion();
            Name = trigger.Key.Name;
            Group = trigger.Key.Group;
            CalendarName = trigger.CalendarName;
            Priority = trigger.Priority;
            StartTimeUtc = trigger.StartTimeUtc;
            EndTimeUtc = trigger.EndTimeUtc;
            NextFireTimes = TriggerUtils.ComputeFireTimes((IOperableTrigger)trigger, calendar, 10);
        }

        public string Name { get; set; }
        public string Group { get; set; }
        public string TriggerType { get; set; }
        public string Description { get; set; }

        public string CalendarName { get; set; }
        public int Priority { get; set; }
        public DateTimeOffset StartTimeUtc { get; set; }
        public DateTimeOffset? EndTimeUtc { get; set; }

        public IList<DateTimeOffset> NextFireTimes { get; set; }

        public static TriggerDetailModel Create(ITrigger trigger, ICalendar calendar)
        {
            var simpleTrigger = trigger as ISimpleTrigger;
            if (simpleTrigger != null)
            {
                return new SimpleTriggerDetailModel(simpleTrigger, calendar);
            }
            var cronTrigger = trigger as ICronTrigger;
            if (cronTrigger != null)
            {
                return new CronTriggerDetailModel(cronTrigger, calendar);
            }
            var calendarIntervalTrigger = trigger as ICalendarIntervalTrigger;
            if (calendarIntervalTrigger != null)
            {
                return new CalendarIntervalTriggerDetailModel(calendarIntervalTrigger, calendar);
            }
            var dailyTimeIntervalTrigger = trigger as IDailyTimeIntervalTrigger;
            if (dailyTimeIntervalTrigger != null)
            {
                return new DailyTimeIntervalTriggerDetailModel(dailyTimeIntervalTrigger, calendar);
            }

            return new TriggerDetailModel(trigger, calendar);
        }


        public class CronTriggerDetailModel : TriggerDetailModel
        {
            public CronTriggerDetailModel(ICronTrigger trigger, ICalendar calendar) : base(trigger, calendar)
            {
                CronExpression = trigger.CronExpressionString;
                TimeZone = new TimeZone(trigger.TimeZone);
            }

            public string CronExpression { get; }
            public TimeZone TimeZone { get; }
        }

        public class SimpleTriggerDetailModel : TriggerDetailModel
        {
            public SimpleTriggerDetailModel(ISimpleTrigger trigger, ICalendar calendar) : base(trigger, calendar)
            {
                RepeatCount = trigger.RepeatCount;
                RepeatInterval = trigger.RepeatInterval;
                TimesTriggered = trigger.TimesTriggered;
            }

            public TimeSpan RepeatInterval { get; }
            public int RepeatCount { get; }
            public int TimesTriggered { get; }
        }

        public class CalendarIntervalTriggerDetailModel : TriggerDetailModel
        {
            public CalendarIntervalTriggerDetailModel(ICalendarIntervalTrigger trigger, ICalendar calendar) : base(trigger, calendar)
            {
                RepeatInterval = trigger.RepeatInterval;
                TimesTriggered = trigger.TimesTriggered;
                RepeatIntervalUnit = trigger.RepeatIntervalUnit;
                PreserveHourOfDayAcrossDaylightSavings = trigger.PreserveHourOfDayAcrossDaylightSavings;
                TimeZone = new TimeZone(trigger.TimeZone);
                SkipDayIfHourDoesNotExist = trigger.SkipDayIfHourDoesNotExist;
            }

            public TimeZone TimeZone { get; }
            public bool SkipDayIfHourDoesNotExist { get; }
            public bool PreserveHourOfDayAcrossDaylightSavings { get; }
            public IntervalUnit RepeatIntervalUnit { get; }
            public int RepeatInterval { get; }
            public int TimesTriggered { get; }
        }

        public class DailyTimeIntervalTriggerDetailModel : TriggerDetailModel
        {
            public DailyTimeIntervalTriggerDetailModel(IDailyTimeIntervalTrigger trigger, ICalendar calendar) : base(trigger, calendar)
            {
                RepeatInterval = trigger.RepeatInterval;
                TimesTriggered = trigger.TimesTriggered;
                RepeatIntervalUnit = trigger.RepeatIntervalUnit;
                TimeZone = new TimeZone(trigger.TimeZone);
            }

            public TimeZone TimeZone { get; }
            public IntervalUnit RepeatIntervalUnit { get; }
            public int TimesTriggered { get; }
            public int RepeatInterval { get; }
        }
    }
}
