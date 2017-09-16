using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class ManRepairViewModel
    {
        public string filluserName { get; set; }
        public string repairUserName { get; set; }
        public string fixnote { get; set; }
        public string cost { get; set; }


        public string imgDesc1 { get; set; }
        public string imgDesc2 { set; get; }
        public string imgDesc3 { get; set; }
        public string imgFixDesc1 { get; set; }
       

        public short isDelete { get; set; }

        
        public string expectDate { get; set; }
      
        public string createTime { get; set; }
     
        public string lastUpdateTime { get; set; }
    }
}