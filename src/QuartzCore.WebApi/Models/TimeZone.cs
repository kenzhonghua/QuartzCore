using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
