using System;
using System.Web;
using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using System.Collections.Generic;

namespace InspectionWeb.Models.ViewModel
{
    public class ListRoomInspectionDispatchViewModel
    {
        public List<roomInspectionDispatchDetail> roomInspectionDispatch { get; set; }
        public List<userListForInspectionViweModel> userList { get; set; }
        public string ErrorMsg { get; set; }
    }
}
