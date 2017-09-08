using System;
using System.Web;
using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using System.Collections.Generic;

namespace InspectionWeb.Models.ViewModel
{
    public class ListQueryInspectionByUserIdViewModel
    {
        public IEnumerable<user> users { get; set; }
        public IEnumerable<roomInspectionDispatchDetail> roomInspectionDispatch { get; set; }
        public IEnumerable<itemInspectionDispatchDetail> itemInspectionDispatch { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
