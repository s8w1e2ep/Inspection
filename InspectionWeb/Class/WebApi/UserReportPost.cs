using System;

namespace InspectionWeb.WebSupport.WebApi
{
    public class UserReportPost
    {
        public string itemCode { get; set; }
        public string email { get; set; }
        public string desc { get; set; }
        public string timeStamp { get; set; }
    }
}

