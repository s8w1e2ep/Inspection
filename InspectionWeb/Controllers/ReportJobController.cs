using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Services.Interface;

namespace InspectionWeb.Controllers
{
    public class ReportJobController : Controller
    {
        private IExhibitionRoomService _exhibitionRoomService;
        private IExhibitionItemService _exhibitionItemService;

        public ReportJobController(IExhibitionRoomService service, IExhibitionItemService service2)
        {
            this._exhibitionRoomService = service;
            this._exhibitionItemService = service2;
        }

        public ActionResult AddExhibitionItem()
        {
            ViewBag.Message = "Hello ";
            ViewBag.NumTimes = 5;

            ViewBag.exhibitionRooms = this._exhibitionRoomService.GetAll();
            


            return View();
        }

        // GET: /ReportJob/AddExperience
        public ActionResult AddExperience()
        {
            return View();
        }

        
        public ActionResult AddOther()
        {
            return View();
        }

        // GET: /ReportJob/EditExhibitionItem
        public ActionResult EditExhibitionItem()
        {
            return View();
        }

        // GET: /ReportJob/Query
        public ActionResult Query()
        {
            return View();
        }

        // GET: /ReportJob/DetailedData
        public ActionResult ItemDetailedData()
        {
            return View();
        }

        //function
        [HttpPost]
        public ActionResult F_AddExhibitionItem()
        {
            return View();
        }



        [HttpPost]
        public ActionResult GetItems(roomJson data)
        {
            var roomId = data.id;
            var items = this._exhibitionItemService.GetAll().Where(x => x.roomId == roomId);

            //for loop convert items to ItemviewModel

            return Json(items);
        }

        public class roomJson
        {
            public string id { set; get; }
        }

    }
}