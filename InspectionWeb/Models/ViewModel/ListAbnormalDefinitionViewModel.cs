using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class ListAbnormalDefinitionViewModel
    {
        public string abnormalDefinitionId { get; set; }

        public string abnormalDefinitionCode { get; set; }

        public string abnormalDefinitionName { get; set; }

        public string abnormalDefinitionDescription { get; set; }

        public int? isDelete { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> createTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> lastUpdateTime { get; set; }
    }
}
