using System;

namespace InspectionWeb.WebSupport.WebApi
{
    public class AutoReportPost
    {
        public string deviceCode { get; set; }
        public string abnormalCode { get; set; }
        public string timeStamp { get; set; }
    }
}

