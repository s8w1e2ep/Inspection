using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class DeviceEditViewModel
    {
        public string ItemId;
        public string ItemName;
        public string FieldId;
        public string ItemCode;
        public Nullable<byte> IsLock;
        public Nullable<short> Active;
        public user Inspector;
        public DateTime? CreateTime;
        public DateTime? LastUpdateTime;
        public Nullable<int> X;
        public Nullable<int> Y;

        public List<user> Inspectors;
        public List<fieldMap> Fields;

    }
}