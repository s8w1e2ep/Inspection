using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class reportDetailedViewModel
    {
        //通報紀錄資料
        public ReportJobViewModel reportRecord { get; set; }
        //用途 : 通報來源選單
        public List<reportSource> sources = new List<reportSource>();
        //用途 : 異常狀況選單
        public List<abnormalDefinition> abnormals = new List<abnormalDefinition>();

        //用途 : 人工維修紀錄

        //用途 : 維修人員選單
        public List<user> repairUsers = new List<user>();

        //用途 : 人工故障排除紀錄
        public ManRepairViewModel ManRecord { get; set; }
    }
}