#region License

/* 
 * All content copyright Terracotta, Inc., unless otherwise indicated. All rights reserved. 
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not 
 * use this file except in compliance with the License. You may obtain a copy 
 * of the License at 
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0 
 *   
 * Unless required by applicable law or agreed to in writing, software 
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations 
 * under the License.
 * 
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;

using Quartz.Util;

namespace Quartz.Impl.Calendar
{
    /// <summary>
    /// This implementation of the Calendar excludes a set of days of the year. You
    /// may use it to exclude bank holidays which are on the same date every year.
    /// </summary>
    /// <seealso cref="ICalendar" />
    /// <seealso cref="BaseCalendar" />
    /// <author>Juergen Donnerstag</author>
    /// <author>Marko Lahma (.NET)</author>
#if BINARY_SERIALIZATION
    [Serializable]
#endif // BINARY_SERIALIZATION
    public class AnnualCalendar : BaseCalendar
    {
        private SortedSet<DateTime> excludeDays = new SortedSet<DateTime>();

        // year to use as fixed year
        private const int FixedYear = 2000;

        /// <summary>
        /// Constructor
        /// </summary>
        public AnnualCalendar()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseCalendar">The base calendar.</param>
        public AnnualCalendar(ICalendar baseCalendar) : base(baseCalendar)
        {
        }

#if BINARY_SERIALIZATION // NetCore versions of Quartz can't use old serialized data. 
        // Make sure that future calendar version changes are done in a DCS-friendly way (with [OnSerializing] and [OnDeserialized] methods).

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected AnnualCalendar(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            int version;
            try
            {
                version = info.GetInt32("version");
            }
            catch
            {
                version = 0;
            }

            switch (version)
            {
                case 0:
                    // 1.x
                    object o = info.GetValue("excludeDays", typeof(object));
                    ArrayList oldFormat = o as ArrayList;
                    if (oldFormat != null)
                    {
                        foreach (DateTime dateTime in oldFormat)
                        {
                            excludeDays.Add(dateTime);
                        }
                    }
                    else
                    {
                        // must be new..
                        var timeOffsets = (List<DateTimeOffset>) o;
                        excludeDays = new SortedSet<DateTime>(timeOffsets.Select(x => x.Date));
                    }
                    break;
                case 1:
                    var dateTimeOffsets = (List<DateTimeOffset>) info.GetValue("excludeDays", typeof(List<DateTimeOffset>));
                    excludeDays = new SortedSet<DateTime>(dateTimeOffsets.Select(x => x.Date));
                    break;
                case 2:
                    excludeDays = (SortedSet<DateTime>) info.GetValue("excludeDays", typeof(SortedSet<DateTime>));
                    break;
                default:
                    throw new NotSupportedException("Unknown serialization version");
            }
        }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("version", 2);
            info.AddValue("excludeDays", excludeDays);
        }
#endif // BINARY_SERIALIZATION

        /// <summary> 
        /// Get or the array which defines the exclude-value of each day of month.
        /// Setting will redefine the array of days excluded. The array must of size greater or
        /// equal 31.
        /// </summary>
        public virtual ISet<DateTime> DaysExcluded
        {
            get { return new SortedSet<DateTime>(excludeDays); }
            set
            {
                if (value == null)
                {
                    excludeDays = new SortedSet<DateTime>();
                }
                else
                {
                    excludeDays = new SortedSet<DateTime>(value);
                }
            }
        }

        /// <summary>
        /// Return true, if day is defined to be excluded.
        /// </summary>
        public virtual bool IsDayExcluded(DateTimeOffset day)
        {
            return IsDateTimeExcluded(day, true);
        }

        private bool IsDateTimeExcluded(DateTimeOffset day, bool checkBaseCalendar)
        {
            // Check baseCalendar first
            if (checkBaseCalendar && !base.IsTimeIncluded(day))
            {
                return true;
            }

            int dmonth = day.Month;
            int dday = day.Day;

            foreach (DateTime cl in excludeDays)
            {
                // remember, the list is sorted
                if (dmonth < cl.Month)
                {
                    return false;
                }

                if (dday != cl.Day)
                {
                    continue;
                }

                if (dmonth != cl.Month)
                {
                    continue;
                }

                return true;
            }

            // not found
            return false;
        }

        /// <summary>
        /// Redefine a certain day to be excluded (true) or included (false).
        /// </summary>
        public virtual void SetDayExcluded(DateTime day, bool exclude)
        {
            DateTime d = new DateTime(FixedYear, day.Month, day.Day, 0, 0, 0);

            if (exclude)
            {
                if (!IsDateTimeExcluded(day, false))
                {
                    excludeDays.Add(d);
                }
            }
            else
            {
                // include
                if (IsDateTimeExcluded(day, false))
                {
                    excludeDays.Remove(d);
                }
            }
        }

        /// <summary>
        /// Determine whether the given UTC time (in milliseconds) is 'included' by the
        /// Calendar.
        /// <para>
        /// Note that this Calendar is only has full-day precision.
        /// </para>
        /// </summary>
        public override bool IsTimeIncluded(DateTimeOffset dateUtc)
        {
            // Test the base calendar first. Only if the base calendar not already
            // excludes the time/date, continue evaluating this calendar instance.
            if (!base.IsTimeIncluded(dateUtc))
            {
                return false;
            }

            //apply the timezone
            dateUtc = TimeZoneUtil.ConvertTime(dateUtc, TimeZone);

            return !(IsDayExcluded(dateUtc));
        }

        /// <summary>
        /// Determine the next UTC time (in milliseconds) that is 'included' by the
        /// Calendar after the given time. Return the original value if timeStampUtc is
        /// included. Return 0 if all days are excluded.
        /// <para>
        /// Note that this Calendar is only has full-day precision.
        /// </para>
        /// </summary>
        public override DateTimeOffset GetNextIncludedTimeUtc(DateTimeOffset timeStampUtc)
        {
            // Call base calendar implementation first
            DateTimeOffset baseTime = base.GetNextIncludedTimeUtc(timeStampUtc);
            if ((baseTime != DateTimeOffset.MinValue) && (baseTime > timeStampUtc))
            {
                timeStampUtc = baseTime;
            }

            //apply the timezone
            timeStampUtc = TimeZoneUtil.ConvertTime(timeStampUtc, TimeZone);

            // Get timestamp for 00:00:00, in the correct timezone offset
            DateTimeOffset day = new DateTimeOffset(timeStampUtc.Date, timeStampUtc.Offset);

            if (!IsDayExcluded(day))
            {
                // return the original value
                return timeStampUtc;
            }

            while (IsDayExcluded(day))
            {
                day = day.AddDays(1);
            }

            return day;
        }

        public override int GetHashCode()
        {
            int baseHash = 13;
            if (GetBaseCalendar() != null)
            {
                baseHash = GetBaseCalendar().GetHashCode();
            }

            return excludeDays.GetHashCode() + 5*baseHash;
        }

        public bool Equals(AnnualCalendar obj)
        {
            if (obj == null)
            {
                return false;
            }

            bool toReturn = GetBaseCalendar() == null || GetBaseCalendar().Equals(obj.GetBaseCalendar());

            toReturn = toReturn && (DaysExcluded.Count == obj.DaysExcluded.Count);
            if (toReturn)
            {
                foreach (DateTime date in DaysExcluded)
                {
                    toReturn = toReturn && obj.excludeDays.Contains(date);
                }
            }
            return toReturn;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AnnualCalendar))
            {
                return false;
            }

            return Equals((AnnualCalendar) obj);
        }

        public override ICalendar Clone()
        {
            var clone = new AnnualCalendar();
            CloneFields(clone);
            clone.excludeDays = new SortedSet<DateTime>(excludeDays);
            return clone;
        }
    }
}