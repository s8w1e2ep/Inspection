using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models
{
    public class itemInspectionDispatchDetail
    {
        public Nullable<System.DateTime> checkDate { get; set; }

        public string dispatchId { get; set; }

        public string itemId { get; set; }

        public string itemName { get; set; }

        public string inspectorId1 { get; set; }

        public string inspectorName1 { get; set; }

        public string inspectorId2 { get; set; }

        public string inspectorName2 { get; set; }

    }
}
