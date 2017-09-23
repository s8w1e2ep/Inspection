using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class SolutionViewModel
    {
        public string solutionId { get; set; }
        public string solutionName { get; set; }
        public string description { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> lastUpdateTime { get; set; }
    }
}