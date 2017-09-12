using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class ExhibitionItemEditViewModel
    {
        public string RoomId;
        public string ItemId;
        public string ItemCode;
        public string ItemName;
        public Nullable<short> ItemType;
        public string Description;
        public string Picture;
        public string FieldId;

        public Nullable<int> X;
        public Nullable<int> Y;
        public Nullable<byte> IsLock;
        public Nullable<short> Active;
        public Nullable<int> PeriodReportTime;
        public Nullable<DateTime> CreateTime;
        public Nullable<DateTime> LastUpdateTime;
        
        public List<company> Companys;
        
        public string ErrorMsg;
    }
}