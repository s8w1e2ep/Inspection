using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.Misc
{
    public class DateTimeHelper
    {
        static double CurrentTimeZoneUTCOffset = 99;


        public DateTime Timestamp2DateTime(double? timestamp)
        {
            if (CurrentTimeZoneUTCOffset == 99)
            {
                CurrentTimeZoneUTCOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
            }

            DateTime dt = new DateTime();


            if (timestamp == null)
            {
                dt = default(DateTime);
            }
            else
            {
                double timestamp2 = timestamp.GetValueOrDefault();
                dt = (new DateTime(1970, 1, 1)).AddHours(CurrentTimeZoneUTCOffset).AddSeconds(timestamp2);
            }


            return dt;
        }

        public double DateTime2Timestamp(DateTime dateTime)
        {
            if (CurrentTimeZoneUTCOffset == 99)
            {
                CurrentTimeZoneUTCOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
            }

            double timestamp = (dateTime.AddHours(-CurrentTimeZoneUTCOffset) - new DateTime(1970, 1, 1)).TotalSeconds;

            return timestamp;
        }
    }
}