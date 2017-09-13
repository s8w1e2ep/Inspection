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
        public Nullable<byte> IsLock;
        public Nullable<short> Active;
        public user Inspector;
        public DateTime? CreateTime;
        public DateTime? LastUpdateTime;

        public List<user> Inspectors;

    }
}