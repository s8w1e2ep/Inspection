//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace InspectionWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class itemCheckRecord
    {
        public string checkId { get; set; }
        public string itemId { get; set; }
        public Nullable<System.DateTime> checkDate { get; set; }
        public string inspectorId { get; set; }
        public Nullable<short> status { get; set; }
        public Nullable<short> checkTimeType { get; set; }
        public Nullable<byte> isDelete { get; set; }
        public Nullable<System.DateTime> createTime { get; set; }
        public Nullable<System.DateTime> lastUpdateTime { get; set; }
        public string dispatchId { get; set; }
    }
}
