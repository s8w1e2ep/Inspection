using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.WebSupport
{
    public class IdGenerator
    {
        private static string GetUniqId()
        {
            var ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double t = ts.TotalMilliseconds / 1000;

            int a = (int)Math.Floor(t);
            int b = (int)((t - Math.Floor(t)) * 1000000);

            return a.ToString("x8") + b.ToString("x5");
        }

        public static string GetId(string Header)
        {
            string id = Header + "_" + GetUniqId().Replace(",", "_");
            return id;
        }
    }
}