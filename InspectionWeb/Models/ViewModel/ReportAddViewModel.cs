using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class ReportAddViewModel
    {

        public string roomId { get; set; }
        public string roomName { get; set; }

        public string itemId { get; set; }
        public string itemName { get; set; }

        public string sourceId { get; set; }
        public string sourceName { get; set; }

        public string abnormalId { get; set; }
        public string abnormalName { get; set; }

        public string ErrorMsg { get; set; }
    }
}