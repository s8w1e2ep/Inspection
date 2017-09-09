

using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class FieldAddViewModel
    {
        public string fieldId { get; set; }

        [Required]
        [Display(Name = "場域名稱")]
        public string FieldName { get; set; }

        [Display(Name = "說明")]
        public string Description { get; set; }

        public string MapFileName { get; set; }

        public string Photo { get; set; }

        [Display(Name = "版本")]
        public string Version { get; set; }


        [Display(Name = "建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "最後更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime LastUpdateTime { get; set; }

        public string ErrorMsg { get; set; }
    }
}