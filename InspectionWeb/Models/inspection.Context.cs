﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class inspectionEntities : DbContext
    {
        public inspectionEntities()
            : base("name=inspectionEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<abnormalDefinition> abnormalDefinition { get; set; }
        public virtual DbSet<abnormalRecord> abnormalRecord { get; set; }
        public virtual DbSet<company> company { get; set; }
        public virtual DbSet<exhibitionItem> exhibitionItem { get; set; }
        public virtual DbSet<exhibitionRoom> exhibitionRoom { get; set; }
        public virtual DbSet<fieldMap> fieldMap { get; set; }
        public virtual DbSet<itemCheckRecord> itemCheckRecord { get; set; }
        public virtual DbSet<itemInspectionDispatch> itemInspectionDispatch { get; set; }
        public virtual DbSet<manRepairRecord> manRepairRecord { get; set; }
        public virtual DbSet<noCheckDate> noCheckDate { get; set; }
        public virtual DbSet<quickSolution> quickSolution { get; set; }
        public virtual DbSet<reportDevice> reportDevice { get; set; }
        public virtual DbSet<reportSource> reportSource { get; set; }
        public virtual DbSet<roomActiveRecord> roomActiveRecord { get; set; }
        public virtual DbSet<roomCheckRecord> roomCheckRecord { get; set; }
        public virtual DbSet<roomInspectionDispatch> roomInspectionDispatch { get; set; }
        public virtual DbSet<softwareVersion> softwareVersion { get; set; }
        public virtual DbSet<systemSettings> systemSettings { get; set; }
        public virtual DbSet<user> user { get; set; }
        public virtual DbSet<userGroup> userGroup { get; set; }
        public virtual DbSet<temp> temp { get; set; }
        public virtual DbSet<temp1> temp1 { get; set; }
        public virtual DbSet<temp2> temp2 { get; set; }
        public virtual DbSet<otherAbnormalRecord> otherAbnormalRecord { get; set; }
    }
}
