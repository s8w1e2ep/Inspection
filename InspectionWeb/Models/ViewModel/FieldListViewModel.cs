using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class FieldListViewModel
    {
        public string FieldId { get; set; }

        public string FieldName { get; set; }

        public string Version { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime LastUpdateTime { get; set; }

    }
}