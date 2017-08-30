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

        public class inpectionTimeJson
        {
            public string name { get; set; }
            public string pk { get; set; }
            public string value { get; set; }
        }

        public InspectionDispatchController(IRoomInspectionDispatchService roomInpectionDispatchService, IItemInspectionDispatchService itemInspectionDispatchService)
        {
            this._RoomInspectionDispatchService = roomInpectionDispatchService;
            this._ItemInspectionDispatchService = itemInspectionDispatchService;
        }

        [HttpGet]
        public ActionResult ListRoomInspectionDispatch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListRoomInspectionDispatch(inpectionTimeJson timeJson)
        {


            return View();
        }

        private RoomInspectionDispatchViewModel data2ViewModel(roomInspectionDispatch roomInspectionDispatchInstance, exhibitionRoom roomInstance, user userInstance)
        {
            RoomInspectionDispatchViewModel viewModel = new RoomInspectionDispatchViewModel();

            viewModel.roomId = roomInspectionDispatchInstance.roomId;
            viewModel.roomName = roomInstance.roomName;
            viewModel.inspectorId1 = roomInspectionDispatchInstance.inspectorId1;
            viewModel.inspectorId2 = roomInspectionDispatchInstance.inspectorId2;
            viewModel.inspectorName1 = userInstance.userName;
            viewModel.inspectorName2 = userInstance.userName;
            viewModel.createTime = roomInspectionDispatchInstance.createTime;
            viewModel.lastUpdateTime = roomInspectionDispatchInstance.lastUpdateTime;

            return viewModel;
        }

        [HttpGet]
        public ActionResult ListItemInspectionDispatch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListItemInspectionDispatch(inpectionTimeJson inpectionTime)
        {
            return View();
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