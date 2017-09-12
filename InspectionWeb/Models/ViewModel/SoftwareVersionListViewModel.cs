using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class SoftwareVersionListViewModel
    {
        public string SoftwareId;
        public string SoftwareCode;
        public string SoftwareName;
        public string Version;
        public DateTime? CreateTime;
        public DateTime? LastUpdateTime;
    }
}