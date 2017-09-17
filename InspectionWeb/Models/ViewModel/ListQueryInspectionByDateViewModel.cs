using System;
using System.Web;
using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using System.Collections.Generic;

namespace InspectionWeb.Models.ViewModel
{
    public class ListQueryInspectionByDateViewModel
    {
        public IEnumerable<itemInspectionDispatchDetail> itemInspectionDispatch { get; set; }
        public IEnumerable<roomInspectionDispatchDetail> roomInspectionDispatch { get; set; }
        public IEnumerable<queryInspectionByDateStatusDetail> inspectionStatus { get; set; }
        public string ErrorMsg { get; set; }
        public string checkDate { get; set; }
        public int dateType { get; set; }
    }
}
