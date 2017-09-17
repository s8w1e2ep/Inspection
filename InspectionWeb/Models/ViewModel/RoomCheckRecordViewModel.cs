using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class RoomCheckRecordViewModel
    {
        public string checkId { get; set; }

        public string dispatchId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime checkDate { get; set; }

        public string roomId { get; set; }

        public string inspectorId { get; set; }

        public int status { get; set; }

        public int checkTimeType { get; set; }

        public int isDelete { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime createTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime lastUpdateTime { get; set; }
    }
}
