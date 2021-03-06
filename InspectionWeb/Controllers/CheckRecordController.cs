﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, Dispatch = true)]
    public class CheckRecordController : Controller
    {

        private IRoomCheckRecordService _RoomCheckRecordService;
        private IItemCheckRecordService _ItemCheckRecordServices;
        private IRoomInspectionDispatchService _RoomInspectionDispatchService;
        private IItemInspectionDispatchService _ItemInspectionDispatchService;
        private INoCheckDateService _NoCheckDateService;


        public class newItemRecordDataJson
        {
            public string dispatchId { get; set; }
            public string itemId { get; set; }
            public string checkDate { get; set; }
            public string inspectorId { get; set; }
            public int status { get; set; }
            public int checkTimeType { get; set; }
        }

        public class newRoomRecordDataJson
        {
            public string dispatchId { get; set; }
            public string roomId { get; set; }
            public string checkDate { get; set; }
            public string inspectorId { get; set; }
            public int status { get; set; }
            public int checkTimeType { get; set; }
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
            System.DateTime checkDate = DateTime.Today; ;
            //檢查非巡檢日
            IResult nonCkeck = new Result(false);
            int isNonCheckDate = this._NoCheckDateService.IsExists(checkDate);
            if (isNonCheckDate == 1)
            {
                nonCkeck.ErrorMsg = "此日非巡檢日";
                nonCkeck.Message = "1";
                return View(Data2RoomViewModel(null, nonCkeck, checkDate));
            }
            else if (isNonCheckDate == 2)
            {
                nonCkeck.ErrorMsg = "上午非巡檢日";
                nonCkeck.Message = "2";
            }
            else if (isNonCheckDate == 3)
            {
                nonCkeck.ErrorMsg = "下午非巡檢日";
                nonCkeck.Message = "3";
            }
            else
            {
                nonCkeck.ErrorMsg = "";
                nonCkeck.Message = "0";
            }
            List<roomInspectionDispatchDetail> allDetailData = new List<roomInspectionDispatchDetail>();
            var allData = this._RoomCheckRecordService.GetAllByDate(checkDate);
            foreach (var item in allData)
            {
                allDetailData.Add(item);
            }
            return View(Data2RoomViewModel(allDetailData, nonCkeck, checkDate));
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
                nonCkeck.Message = "1";
                return View(Data2RoomViewModel(null, nonCkeck, checkDate));
            }
            else if (isNonCheckDate == 2)
            {
                nonCkeck.ErrorMsg = "上午非巡檢日";
                nonCkeck.Message = "2";
            }
            else if (isNonCheckDate == 3)
            {
                nonCkeck.ErrorMsg = "下午非巡檢日";
                nonCkeck.Message = "3";
            }
            else
            {
                nonCkeck.ErrorMsg = "";
                nonCkeck.Message = "0";
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
                viewModel.ErrorType = Convert.ToInt16(result.Message);
            }
            else
            {
                viewModel.ErrorMsg = null;
            }
            return viewModel;
        }

        public ActionResult AddNewRoomRecord(newRoomRecordDataJson json)
        {
            string roomId = json.roomId;
            string inspectorId = json.inspectorId;
            string checkDate = json.checkDate;
            string dispatchId = json.dispatchId;
            int status = json.status;
            int checkTimeType = json.checkTimeType;

            IResult result = this._RoomCheckRecordService.Create(roomId, inspectorId, checkDate, dispatchId, status, checkTimeType);
            if (result.Success == false)
            {
                System.Diagnostics.Debug.WriteLine("insert error");
                System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
                return Json(new { result = 0, ErrorMsg = result.ErrorMsg });
            }
            else
            {
                return Json(new { result = 1 });
            }
        }

        [HttpGet]
        public ActionResult AddItemCheckRecord()
        {
            System.DateTime checkDate = DateTime.Today;
            //檢查非巡檢日
            IResult nonCkeck = new Result(false);
            int isNonCheckDate = this._NoCheckDateService.IsExists(checkDate);
            if (isNonCheckDate == 1)
            {
                nonCkeck.ErrorMsg = "此日非巡檢日";
                nonCkeck.Message = "1";
                return View(Data2ItemViewModel(null, nonCkeck, checkDate));
            }
            else if (isNonCheckDate == 2)
            {
                nonCkeck.ErrorMsg = "上午非巡檢日";
                nonCkeck.Message = "2";
            }
            else if (isNonCheckDate == 3)
            {
                nonCkeck.ErrorMsg = "下午非巡檢日";
                nonCkeck.Message = "3";
            }
            else
            {
                nonCkeck.ErrorMsg = "";
                nonCkeck.Message = "0";
            }
            List<itemInspectionDispatchDetail> allDetailData = new List<itemInspectionDispatchDetail>();
            var allData = this._ItemCheckRecordServices.GetAllByDate(checkDate);
            foreach (var item in allData)
            {
                allDetailData.Add(item);
            }
            return View(Data2ItemViewModel(allDetailData, nonCkeck, checkDate));
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
                nonCkeck.Message = "1";
                return View(Data2ItemViewModel(null, nonCkeck, checkDate));
            }
            else if (isNonCheckDate == 2)
            {
                nonCkeck.ErrorMsg = "上午非巡檢日";
                nonCkeck.Message = "2";
            }
            else if (isNonCheckDate == 3)
            {
                nonCkeck.ErrorMsg = "下午非巡檢日";
                nonCkeck.Message = "3";
            }
            else
            {
                nonCkeck.ErrorMsg = "";
                nonCkeck.Message = "0";
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
                viewModel.ErrorType = Convert.ToInt16(result.Message);
            }
            else
            {
                viewModel.ErrorMsg = null;
            }
            return viewModel;
        }

        public ActionResult AddNewItemRecord(newItemRecordDataJson json)
        {
            string itemId = json.itemId;
            string inspectorId = json.inspectorId;
            string checkDate = json.checkDate;
            string dispatchId = json.dispatchId;
            int status = json.status;
            int checkTimeType = json.checkTimeType;

            IResult result = this._ItemCheckRecordServices.Create(itemId, inspectorId, checkDate, dispatchId, status, checkTimeType);
            if (result.Success == false)
            {
                System.Diagnostics.Debug.WriteLine("insert error");
                System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
                return Json(new { result = 0, ErrorMsg = result.ErrorMsg });
            }
            else
            {
                return Json(new { result = 1 });
            }
        }
    }
}