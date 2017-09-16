using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class ReportJobViewModel
    {
        public string recordId { get; set; }

        public string deviceId { get; set; }

        public string itemtypeName { get; set; }

        public string roomId { get; set; }
        public string roomName { get; set; }

        public string itemId { get; set; }
        public string itemName { get; set; }

        public string sourceId { get; set; }
        public string sourceName { get; set; }

        public string abnormalId { get; set; }
        public string abnormalName { get; set; }
        public string abnormalDescription { get; set; }

        public string happenedTime { get; set; }
        public string description { get; set; }
        public string fixDate { get; set; }
        public string fixMethod { get; set; }
        public string isClose { get; set; }
        public Nullable<short> isClose_s { get; set; }

        public string createTime { get; set; }
        public string lastUpdateTime { get; set; }

    }
}