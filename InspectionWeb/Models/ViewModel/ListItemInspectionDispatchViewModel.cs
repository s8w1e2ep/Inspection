using System;
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
        public int ErrorType { get; set; }
        //0 or nonDefine normal
        //1 整日非巡檢日
        //2 上午非巡檢日
        //3 下午非巡檢日
        //4 other type
        public string ErrorMsg { get; set; }
        public string checkDate { get; set; }
    }
}
