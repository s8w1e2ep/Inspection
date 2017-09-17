using System;
using System.Web;
using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using System.Collections.Generic;

namespace InspectionWeb.Models.ViewModel
{
    public class ListRoomCheckRecordViewModel
    {
        public List<roomInspectionDispatchDetail> roomInspectionDispatch { get; set; }
        public string ErrorMsg { get; set; }
        public int ErrorType { get; set; }
        public string checkDate { get; set; }
    }
}
