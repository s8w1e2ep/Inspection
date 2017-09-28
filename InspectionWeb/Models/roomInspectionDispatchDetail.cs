using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models
{
    public class roomInspectionDispatchDetail
    {
        public Nullable<System.DateTime> checkDate { get; set; }

        public string dispatchId { get; set; }

        public string roomId { get; set; }

        public string roomName { get; set; }

        public short roomStatus { get; set; }

        public string inspectorId1 { get; set; }

        public string inspectorCode1 { get; set; }

        public string inspectorName1 { get; set; }

        public string inspectorId2 { get; set; }

        public string inspectorCode2 { get; set; }

        public string inspectorName2 { get; set; }

        public byte ridStatus { get; set; }
    }
}
