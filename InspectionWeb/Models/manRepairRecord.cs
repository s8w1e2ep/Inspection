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
    
    public partial class manRepairRecord
    {
        public string recordId { get; set; }
        public string fillUserId { get; set; }
        public string repairUserrId { get; set; }
        public string fixNote { get; set; }
        public string cost { get; set; }
        public Nullable<System.DateTime> expectDate { get; set; }
        public string imgFile1 { get; set; }
        public string imgDesc1 { get; set; }
        public string imgFile2 { get; set; }
        public string imgDesc2 { get; set; }
        public string imgFile3 { get; set; }
        public string imgDesc3 { get; set; }
        public string imgFixFile1 { get; set; }
        public string imgFixDesc1 { get; set; }
        public Nullable<byte> isDelete { get; set; }
        public Nullable<System.DateTime> createTime { get; set; }
        public Nullable<System.DateTime> lastUpdateTime { get; set; }
    }
}
