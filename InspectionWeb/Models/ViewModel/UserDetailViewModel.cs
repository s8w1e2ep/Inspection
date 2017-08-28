using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class UserDetailViewModel
    {
        public string userId { get; set; }
        public string userCode { get; set; }
        public string groupId { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string agent { get; set; }
        public string email { get; set; }
        public string tel { get; set; }
        public Nullable<short> active { get; set; }
        public short isDelete { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> createTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> lastUpdateTime { get; set; }
    }
}