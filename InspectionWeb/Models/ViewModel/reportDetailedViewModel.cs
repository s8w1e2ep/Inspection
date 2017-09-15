using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class reportDetailedViewModel
    {
        public ReportJobViewModel data { get; set; }
        public List<reportSource> sources = new List<reportSource>();
        public List<abnormalDefinition> abnormals = new List<abnormalDefinition>();
    }
}