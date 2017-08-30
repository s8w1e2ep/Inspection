using System;

namespace InspectionWeb.Models.Misc
{
    public class IdGenerator
    {
        private string GetUniqID()
        {
            var ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double t = ts.TotalMilliseconds / 1000;

            int a = (int)Math.Floor(t);
            int b = (int)((t - Math.Floor(t)) * 1000000);

            return a.ToString("x8") + b.ToString("x5");
        }


        public string GetUserNewID()
        {
            string id = "user_" + this.GetUniqID().Replace(",", "_");
            return id;
        }

<<<<<<< HEAD
        public string GetUserGroupNewID()
        {
            string id = "group" + this.GetUniqID().Replace(",", "_");
            return id;
        }

=======
>>>>>>> abnormal
        public string GetFieldNewID()
        {
            string id = "field_" + this.GetUniqID().Replace(",", "_");
            return id;
        }

        public string GetBeaconNewID()
        {
            string id = "beacon_" + this.GetUniqID().Replace(",", "_");
            return id;
        }

        public string GetParkingblockNewID()
        {
            string id = "parkingBlock_" + this.GetUniqID().Replace(",", "_");
            return id;
        }

<<<<<<< HEAD
=======
        public string GetAbnormalDefinitionNewID()
        {
            string id = "abnormalDefinition_" + this.GetUniqID().Replace(",", "_");
            return id;
        }

        public string GetReportSourceNewID()
        {
            string id = "reportSource_" + this.GetUniqID().Replace(",", "_");
            return id;
        }

>>>>>>> abnormal
        public string GetID(string Header)
        {
            string id = Header + "_" + this.GetUniqID().Replace(",", "_");
            return id;
        }
    }
}