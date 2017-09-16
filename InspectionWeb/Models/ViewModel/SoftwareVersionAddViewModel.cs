using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class SoftwareVersionAddViewModel
    {
        public string SoftwareId;
        public string SoftwareCode;
        public string SoftwareName;
        public string Description;
        public string FileName;
        public string Version;
        public Nullable<DateTime> CreateTime;
        public Nullable<DateTime> LastUpdateTime;
        public string ErrorMsg;

    }
}