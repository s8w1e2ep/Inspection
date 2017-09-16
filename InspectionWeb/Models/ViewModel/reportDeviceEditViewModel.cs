using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class ReportDeviceEditViewModel
    {
        public string DeviceId;
        public string DeviceCode;
        public string SourceId;
        public string Name;
        public string Photo;
        public string MapFileName;
        public string Description;
        public exhibitionRoom Room;
        public exhibitionItem Item;
        public fieldMap Field;
        public int? X;
        public int? Y;
        public DateTime? CreateTime;
        public DateTime? LastUpdateTime;

        public List<reportSource> ReportSources;
        public List<exhibitionRoom> Rooms;
        public List<exhibitionItem> Items;
        public List<fieldMap> Fields;
    }
}