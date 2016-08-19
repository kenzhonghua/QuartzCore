using Quartz;
using Quartz.Impl.Calendar;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuartzCore.WebApi.Models
{
    public class CalendarDetailModel
    {
        protected CalendarDetailModel(ICalendar calendar)
        {
            CalendarType = calendar.GetType().AssemblyQualifiedNameWithoutVersion();
            Description = calendar.Description;
            if (calendar.CalendarBase != null)
            {
                CalendarBase = Create(calendar.CalendarBase);
            }
        }

        public string CalendarType { get; }
        public string Description { get; }
        public CalendarDetailModel CalendarBase { get; }

        public static CalendarDetailModel Create(ICalendar calendar)
        {
            var annualCalendar = calendar as AnnualCalendar;
            if (annualCalendar != null)
            {
                return new AnnualCalendarDto(annualCalendar);
            }

            var cronCalendar = calendar as CronCalendar;
            if (cronCalendar != null)
            {
                return new CronCalendarDto(cronCalendar);
            }

            var dailyCalendar = calendar as DailyCalendar;
            if (dailyCalendar != null)
            {
                return new DailyCalendarDto(dailyCalendar);
            }

            var holidayCalendar = calendar as HolidayCalendar;
            if (holidayCalendar != null)
            {
                return new HolidayCalendarDto(holidayCalendar);
            }

            var monthlyCalendar = calendar as MonthlyCalendar;
            if (monthlyCalendar != null)
            {
                return new MonthlyCalendarDto(monthlyCalendar);
            }

            var weeklyCalendar = calendar as WeeklyCalendar;
            if (weeklyCalendar != null)
            {
                return new WeeklyCalendarDto(weeklyCalendar);
            }

            return new CalendarDetailModel(calendar);
        }

        public class AnnualCalendarDto : CalendarDetailModel
        {
            public AnnualCalendarDto(AnnualCalendar calendar) : base(calendar)
            {
                DaysExcluded = calendar.DaysExcluded as IReadOnlyList<DateTimeOffset>;
                TimeZone = new TimeZone(calendar.TimeZone);
            }

            public IReadOnlyList<DateTimeOffset> DaysExcluded { get; }
            public TimeZone TimeZone { get; }
        }

        public class CronCalendarDto : CalendarDetailModel
        {
            public CronCalendarDto(CronCalendar calendar) : base(calendar)
            {
                CronExpression = calendar.CronExpression.CronExpressionString;
                TimeZone = new TimeZone(calendar.TimeZone);
            }

            public string CronExpression { get; }
            public TimeZone TimeZone { get; }
        }

        public class DailyCalendarDto : CalendarDetailModel
        {
            public DailyCalendarDto(DailyCalendar calendar) : base(calendar)
            {
                InvertTimeRange = calendar.InvertTimeRange;
                TimeZone = new TimeZone(calendar.TimeZone);
            }

            public bool InvertTimeRange { get; }
            public TimeZone TimeZone { get; }
        }

        public class HolidayCalendarDto : CalendarDetailModel
        {
            public HolidayCalendarDto(HolidayCalendar calendar) : base(calendar)
            {
                ExcludedDates = calendar.ExcludedDates.ToList();
                TimeZone = new TimeZone(calendar.TimeZone);
            }

            public IReadOnlyList<DateTime> ExcludedDates { get; }
            public TimeZone TimeZone { get; }
        }

        public class MonthlyCalendarDto : CalendarDetailModel
        {
            public MonthlyCalendarDto(MonthlyCalendar calendar) : base(calendar)
            {
                DaysExcluded = calendar.DaysExcluded.ToList();
                TimeZone = new TimeZone(calendar.TimeZone);
            }

            public IReadOnlyList<bool> DaysExcluded { get; }
            public TimeZone TimeZone { get; }
        }

        public class WeeklyCalendarDto : CalendarDetailModel
        {
            public WeeklyCalendarDto(WeeklyCalendar calendar) : base(calendar)
            {
                DaysExcluded = calendar.DaysExcluded.ToList();
                TimeZone = new TimeZone(calendar.TimeZone);
            }

            public IReadOnlyList<bool> DaysExcluded { get; }
            public TimeZone TimeZone { get; }
        }
    }
}
