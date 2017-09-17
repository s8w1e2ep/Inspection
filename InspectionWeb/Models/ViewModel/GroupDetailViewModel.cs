using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class GroupDetailViewModel
    {
        public string groupId { get; set; }
        public string groupName { get; set; }
        public Nullable<short> superUser { get; set; }
        public Nullable<short> system { get; set; }
        public Nullable<short> user { get; set; }
        public Nullable<short> dispatch { get; set; }
        public Nullable<short> normal { get; set; }
        public short isDelete { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> createTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> lastUpdateTime { get; set; }
    }
}