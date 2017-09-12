using System;
using System.Web;
using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using System.Collections.Generic;

namespace InspectionWeb.Models.ViewModel
{
    public class ListQueryInspectionByRoomViewModel
    {
        public IEnumerable<roomInspectionDispatchDetail> roomInspectionDispatch { get; set; }
        public List<exhibitionRoomList> room { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
