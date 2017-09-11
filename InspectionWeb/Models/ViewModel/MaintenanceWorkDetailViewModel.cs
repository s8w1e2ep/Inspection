using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class MaintenanceWorkDetailViewModel
    {
        public string recordId { get; set; }
        public string itemName { get; set; }
        public string sourceName { set; get; }
        public string deviceId { get; set; }
        public string abnormalId { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> happendedTime { get; set; }
        public string description { get; set; }
        public Nullable<short> fixMethod { get; set; }
        public Nullable<byte> isClose { get; set; }
        public Nullable<byte> isDelete { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> createTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> lastUpdateTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> fixDate { set; get; }
    }
}
