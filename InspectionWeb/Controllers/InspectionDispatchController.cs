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
    public class InspectionDispatchController : Controller
    {
        private IRoomInspectionDispatchService _RoomInspectionDispatchService;
        private IItemInspectionDispatchService _ItemInspectionDispatchService;
        private IExhibitionItemService _ExhibitionItemService;


        public class inpectionDateJson
        {
            public string date { get; set; }
        }

        public class inspectionDispatchJson
        {
            public string name { get; set; }
            public string pk { get; set; }
            public string value { get; set; }
        }

        public InspectionDispatchController(IRoomInspectionDispatchService roomInpectionDispatchService, IItemInspectionDispatchService itemInspectionDispatchService, IExhibitionItemService exhibitionRoomService)
        {
            this._RoomInspectionDispatchService = roomInpectionDispatchService;
            this._ItemInspectionDispatchService = itemInspectionDispatchService;
            this._ExhibitionItemService = exhibitionRoomService;
        }

        [HttpGet]
        public ActionResult ListRoomInspectionDispatch()
        {
            //go to ListRoomInspectionDispatch page wait for time query
            return View();
        }

        [HttpPost]
        public ActionResult ListRoomInspectionDispatch(inpectionDateJson timeJson)
        {
            System.DateTime date = Convert.ToDateTime(timeJson.date);
            bool isExist = this._RoomInspectionDispatchService.IsExists(date);
            bool roomNumCheck = this._RoomInspectionDispatchService.checkRoomInsert();
            this._ExhibitionItemService.GetAll().OrderBy(x => x.roomId);
            //檢查有無新增展覽廳
           if (isExist)
            {
                if(!roomNumCheck)
                {
                    var TotalViewModel = new List<roomInspectionDispatchDetail>();
                    var roomDispatchs = this._RoomInspectionDispatchService.GetAllByDate(date);

                    foreach (var roomDispatch in roomDispatchs)
                    {
                        TotalViewModel.Add(roomDispatch);
                    }
                    return View(TotalViewModel);
                }
                else
                {
                    //找出還未建檔的展覽廳
                    return View();
                }

            }
            else
            {
                IResult result = this._RoomInspectionDispatchService.Create(date);
                if (result.Success == false)
                {
                    //
                    return View();
                }
                else
                {
                    var TotalViewModel = new List<roomInspectionDispatchDetail>();
                    var roomDispatchs = this._RoomInspectionDispatchService.GetAllByDate(date);

                    foreach (var roomDispatch in roomDispatchs)
                    {
                        TotalViewModel.Add(roomDispatch);
                    }
                    return View(TotalViewModel);
                }
            }
        }

        public ActionResult updateRoomInspectionDispatch(inspectionDispatchJson dispatchJson)
        {
            var roomDispatchId = dispatchJson.pk;
            var roomDispatch = this._RoomInspectionDispatchService.GetById(roomDispatchId);
            if (roomDispatch != null && ModelState.IsValid)
            {
                IResult result = this._RoomInspectionDispatchService.Update(roomDispatch, dispatchJson.name, dispatchJson.value);
                roomDispatch = this._RoomInspectionDispatchService.GetById(roomDispatchId);
                if (result.Success)
                {
                    string lastUpdateTime = roomDispatch.lastUpdateTime.ToString();
                    return Json(new { result = 1, roomDispatchId = roomDispatchId, lastUpdateTime = lastUpdateTime });
                }
                else
                {
                    return Json(new { result = 0, roomDispatchId = roomDispatchId });
                }
            }
            else
            {
                return Json(new { result = 0, roomDispatchId = roomDispatchId });
                //return RedirectToAction("ListReportSource");
            }
        }

        private RoomInspectionDispatchViewModel data2ViewModel(roomInspectionDispatchDetail roomDispatch)
        {
            RoomInspectionDispatchViewModel viewModel = new RoomInspectionDispatchViewModel();

            return viewModel;
        }

        [HttpGet]
        public ActionResult ListItemInspectionDispatch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListItemInspectionDispatch(inpectionDateJson inpectionTime)
        {
            return View();
        }

        public ActionResult updateItemInspectionDispatch(inspectionDispatchJson dispatchJson)
        {
            var itemDispatchId = dispatchJson.pk;
            var itemDispatch = this._RoomInspectionDispatchService.GetById(itemDispatchId);
            if (itemDispatch != null && ModelState.IsValid)
            {
                IResult result = this._RoomInspectionDispatchService.Update(itemDispatch, dispatchJson.name, dispatchJson.value);
                itemDispatch = this._RoomInspectionDispatchService.GetById(itemDispatchId);
                if (result.Success)
                {
                    string lastUpdateTime = itemDispatch.lastUpdateTime.ToString();
                    return Json(new { result = 1, itemDispatchId = itemDispatchId, lastUpdateTime = lastUpdateTime });
                }
                else
                {
                    return Json(new { result = 0, itemDispatchId = itemDispatchId });
                }
            }
            else
            {
                return Json(new { result = 0, itemDispatchId = itemDispatchId });
                //return RedirectToAction("ListReportSource");
            }
        }

        public ActionResult ListNonInspectionDispatchDate()
        {
            return View();
        }

        public ActionResult AddNonInspectionDispatchDate()
        {
            return View();
        }

        public ActionResult QueryInspectionByRoom()
        {
            return View();
        }

        public ActionResult QueryInspectionByItem()
        {
            return View();
        }

        public ActionResult QueryInspectionByUserId()
        {
            return View();
        }

        public ActionResult QueryInspectionByDate()
        {
            return View();
        }

    }
}