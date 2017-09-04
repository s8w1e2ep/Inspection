using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class NoCheckDateViewModel
    {
        public string id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> noCheckDate { get; set; }

        public int checkTimeType { get; set; }

        public string description { get; set; }

        public string setupUserId { get; set; }

        public int isDelete { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> createTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> lastUpdateTime { get; set; }
    }
}
