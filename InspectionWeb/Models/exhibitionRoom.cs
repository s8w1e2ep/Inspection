//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
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
        public string roomCode { get; set; }
    }
}
