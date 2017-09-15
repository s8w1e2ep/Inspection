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
        public string roomName { get; set; }
        public string happendedTime { get; set; }
        public string description { get; set; }
        public Nullable<short> fixMethod { get; set; }
        public Nullable<byte> isClose { get; set; }
        public Nullable<byte> isDelete { get; set; }
        public string createTime { get; set; }
        public string lastUpdateTime { get; set; }
        public string fixDate { get; set; }
        public string listName { get; set; }
    }
}
