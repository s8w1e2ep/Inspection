using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class MailViewModel
    {
        public string id { get; set; }
        public string keyName { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public short isDelete { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> createTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> lastUpdateTime { get; set; }
    }
}