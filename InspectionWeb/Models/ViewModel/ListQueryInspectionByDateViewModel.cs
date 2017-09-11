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
        public string checkDate { get; set; }
    }
}
