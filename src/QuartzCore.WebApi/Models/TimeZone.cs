using System;

namespace QuartzCore.WebApi.Models
{
    public class TimeZone
    {
        public TimeZone(TimeZoneInfo timeZone)
        {
            Id = timeZone.Id;
            StandardName = timeZone.StandardName;
            DisplayName = timeZone.DisplayName;
        }

        public string Id { get; }
        public string StandardName { get; }
        public string DisplayName { get; }
    }
}
