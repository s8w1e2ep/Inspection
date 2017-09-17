using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models
{
    public class queryInspectionByDateStatusDetail
    {

        public string roomId { get; set; }

        public int allItemNum { get; set; }

        public int hasInspection { get; set; }

        public int abnormalNum { get; set; }

        public int solveNum { get; set; }

    }
}
