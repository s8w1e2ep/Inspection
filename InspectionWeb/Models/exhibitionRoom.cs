
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
    
public partial class exhibitionRoom
{

    public string roomId { get; set; }

    public string roomName { get; set; }

    public string fieldId { get; set; }

    public string description { get; set; }

    public string picture { get; set; }

    public string inspectionUserId { get; set; }

    public string companyId { get; set; }

    public Nullable<short> active { get; set; }

    public string floor { get; set; }

    public Nullable<int> x { get; set; }

    public Nullable<int> y { get; set; }

    public Nullable<int> width { get; set; }

    public Nullable<int> height { get; set; }

    public Nullable<byte> isDelete { get; set; }

    public Nullable<System.DateTime> createTime { get; set; }

    public Nullable<System.DateTime> lastUpdateTime { get; set; }

}

}
