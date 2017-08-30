using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using InspectionWeb.Models.Misc;
using InspectionWeb.Models.ViewModel;
using System.Web.Hosting;
using System.IO;

namespace InspectionWeb.Controllers
{
    public class CheckRecordController : Controller
    {

        private IRoomCheckRecordService _RoomCheckRecordService;
        private IItemCheckRecordService _ItemCheckRecordServices;
        
        public class getInspectionDataJson
        {
            public string name { get; set; }
            public string pk { get; set; }
            public string value { get; set; }
        }

        public CheckRecordController(IRoomCheckRecordService roomCheckRecordService, IItemCheckRecordService itemCheckRecordServices)
        {
            this._RoomCheckRecordService = roomCheckRecordService;
            this._ItemCheckRecordServices = itemCheckRecordServices;
        }

        [HttpGet]
        public ActionResult AddRoomCheckRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRoomCheckRecord(getInspectionDataJson time)
        {
            var checkTime = time.value;
            var inspectionData = this._RoomCheckRecordService.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult AddItemCheckRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddItemCheckRecord(getInspectionDataJson time)
        {
            return View();
        }
    }
}