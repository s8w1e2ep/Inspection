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
        private IRoomInspectionDispatchService _RoomInspectionDispatchService;
        private IItemInspectionDispatchService _ItemInspectionDispatchService;
        private INoCheckDateService _NoCheckDateService;


        public class getInspectionDataJson
        {
            public string date { get; set; }
        }

        public CheckRecordController(IRoomCheckRecordService roomCheckRecordService, 
                                     IItemCheckRecordService itemCheckRecordServices,
                                     IRoomInspectionDispatchService roomInspectionDispatchService,
                                     IItemInspectionDispatchService itemInspectionDispatchService,
                                     INoCheckDateService noCheckDateService)
        {
            this._RoomCheckRecordService = roomCheckRecordService;
            this._ItemCheckRecordServices = itemCheckRecordServices;
            this._RoomInspectionDispatchService = roomInspectionDispatchService;
            this._ItemInspectionDispatchService = itemInspectionDispatchService;
            this._NoCheckDateService = noCheckDateService;

        }

        [HttpGet]
        public ActionResult AddRoomCheckRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRoomCheckRecord(string dispatchDate)
        {
            System.DateTime checkDate = Convert.ToDateTime(dispatchDate);
            //檢查非巡檢日
            IResult nonCkeck = new Result(false);
            int isNonCheckDate = this._NoCheckDateService.IsExists(checkDate);
            if (isNonCheckDate == 1)
            {
                nonCkeck.ErrorMsg = "此日非巡檢日";
                return View(Data2ItemViewModel(null, nonCkeck, checkDate));
            }
            else if (isNonCheckDate == 2)
            {
                nonCkeck.ErrorMsg = "上午非巡檢日";
            }
            else if (isNonCheckDate == 3)
            {
                nonCkeck.ErrorMsg = "下午非巡檢日";
            }
            else
            {
                nonCkeck.ErrorMsg = "";
            }
            List<roomInspectionDispatchDetail> allDetailData = new List<roomInspectionDispatchDetail>();
            var allData = this._RoomCheckRecordService.GetAllByDate(checkDate);
            foreach (var item in allData)
            {
                allDetailData.Add(item);
            }
            return View(Data2RoomViewModel(allDetailData, nonCkeck, checkDate));
        }

        private ListRoomCheckRecordViewModel Data2RoomViewModel(List<roomInspectionDispatchDetail> dispatchList, IResult result, System.DateTime checkDate)
        {
            ListRoomCheckRecordViewModel viewModel = new ListRoomCheckRecordViewModel();
            viewModel.roomInspectionDispatch = dispatchList;
            viewModel.checkDate = checkDate.ToString("d");
            if (result != null)
            {
                viewModel.ErrorMsg = result.ErrorMsg;
                System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
            }
            else
            {
                viewModel.ErrorMsg = null;
            }
            return viewModel;
        }


        [HttpGet]
        public ActionResult AddItemCheckRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddItemCheckRecord(string dispatchDate)
        {
            System.DateTime checkDate = Convert.ToDateTime(dispatchDate);
            //檢查非巡檢日
            IResult nonCkeck = new Result(false);
            int isNonCheckDate = this._NoCheckDateService.IsExists(checkDate);
            if (isNonCheckDate == 1)
            {
                nonCkeck.ErrorMsg = "此日非巡檢日";
                return View(Data2ItemViewModel(null, nonCkeck, checkDate));
            }
            else if (isNonCheckDate == 2)
            {
                nonCkeck.ErrorMsg = "上午非巡檢日";
            }
            else if (isNonCheckDate == 3)
            {
                nonCkeck.ErrorMsg = "下午非巡檢日";
            }
            else
            {
                nonCkeck.ErrorMsg = "";
            }
            List<itemInspectionDispatchDetail> allDetailData = new List<itemInspectionDispatchDetail>();
            var allData = this._ItemCheckRecordServices.GetAllByDate(checkDate);
            foreach (var item in allData)
            {
                allDetailData.Add(item);
            }
            return View(Data2ItemViewModel(allDetailData,nonCkeck,checkDate));
        }

        private ListItemCheckRecordViewModel Data2ItemViewModel(List<itemInspectionDispatchDetail> dispatchList, IResult result, System.DateTime checkDate)
        {
            ListItemCheckRecordViewModel viewModel = new ListItemCheckRecordViewModel();
            viewModel.itemInspectionDispatch = dispatchList;
            viewModel.checkDate = checkDate.ToString("d");
            if (result != null)
            {
                viewModel.ErrorMsg = result.ErrorMsg;
                System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
            }
            else
            {
                viewModel.ErrorMsg = null;
            }
            return viewModel;
        }
    }
}