﻿using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class ExhibitionRoomListViewModel
    {
        public string roomId { get; set; }
        public string roomName { get; set; }
        public string InspectorName { get; set; }
        public string floor { get; set; }

        public string fieldName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> createTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> lastUpdateTime { get; set; }
    }
}