using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class RoomInspectionDispatchViewModel
    {
        public string dispatchId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> checkDate { get; set; }

        public string roomId { get; set; }

        public string roomName { get; set; }

        public string inspectorId1 { get; set; }

        public string inspectorName1 { get; set; }

        public string inspectorId2 { get; set; }

        public string inspectorName2 { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> createTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> lastUpdateTime { get; set; }
    }
}
