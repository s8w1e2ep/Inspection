using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class ReportSourceViewModel
    {
        public string sourceId { get; set; }

        public string sourceCode { get; set; }

        public string sourceName { get; set; }

        public string description { get; set; }

        public int? isDelete { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> createTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> lastUpdateTime { get; set; }

        public string ErrorMsg { get; set; }
    }
}
