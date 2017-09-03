﻿using System;
using System.Web;
using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using System.Collections.Generic;

namespace InspectionWeb.Models.ViewModel
{
    public class ListItemInspectionDispatchViewModel
    {
        public List<itemInspectionDispatchDetail> itemInspectionDispatch { get; set; }
        public List<userListForInspectionViweModel> userList { get; set; }
        public string ErrorMsg { get; set; }
    }
}
