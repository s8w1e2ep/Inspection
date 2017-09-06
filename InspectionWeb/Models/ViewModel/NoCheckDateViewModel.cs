using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class NoCheckDateViewModel
    {
        public string id { get; set; }

        public string noCheckDate { get; set; }

        public Nullable<byte> am { get; set; }

        public Nullable<byte> pm { get; set; }

        public string description { get; set; }

        public string setupUserId { get; set; }

        public Nullable<short> isDelete { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> createTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> lastUpdateTime { get; set; }

        public string ErrorMsg { get; set; }

    }
}
