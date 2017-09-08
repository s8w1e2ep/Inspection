using System;
using System.Web;
using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using System.Collections.Generic;

namespace InspectionWeb.Models.ViewModel
{
    public class ListQueryInspectionByItemViewModel
    {
        public IEnumerable<itemInspectionDispatchDetail> itemInspectionDispatch { get; set; }
        public List<exhibitionItemList> item { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
