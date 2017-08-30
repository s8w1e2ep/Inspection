using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class ItemInspectionDispatchViewModel
    {
        public string dispatchId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime checkDate { get; set; }

        public string roomId { get; set; }

        public string inspectorId1 { get; set; }

        public string inspectorId2 { get; set; }

        public string setupUserId { get; set; }

        public int isDelete { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime createTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime lastUpdateTime { get; set; }
    }
}
