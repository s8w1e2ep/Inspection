using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class AddAbnormalDefinitionViewModel
    {
        public string abnormalDefinitionId { get; set; }

        public string abnormalDefinitionCode { get; set; }

        public string abnormalDefinitionName { get; set; }

        public string ErrorMsg { get; set; }
    }
}
