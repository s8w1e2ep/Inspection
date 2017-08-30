using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class GetAllRoomDispatchViewModel
    {
        public string roomId { get; set; }

        public string roomName { get; set; }

        public int active { get; set; }

        public int isDelete { get; set; }

        public string ErrorMsg { get; set; }
    }
}
