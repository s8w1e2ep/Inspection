using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, Dispatch = true)]
    public class MaintenanceWorkController : Controller
    {
        private IAbnormalRecordService _abnormalRecordService;
        private IExhibitionItemService _exhibitionItemService;
        private IReportSourceService _reportSourceService;
        private IExhibitionRoomService _exhibitionRoomService;
        private IAbnormalDefinitionService _abnormalDefinitionService;
        private IOtherAbnormalRecordService _otherAbnormalRecordService;

        public MaintenanceWorkController(IAbnormalRecordService abnormalRecordService, IExhibitionItemService exhibitionItemService, 
            IReportSourceService reportSourceService, IExhibitionRoomService exhibitionRoomService, 
            IAbnormalDefinitionService abnormalDefinitionService, IOtherAbnormalRecordService otherAbnormalRecordService)
        {
            this._abnormalRecordService = abnormalRecordService;
            this._exhibitionItemService = exhibitionItemService;
            this._reportSourceService = reportSourceService;
            this._exhibitionRoomService = exhibitionRoomService;
            this._abnormalDefinitionService = abnormalDefinitionService;
            this._otherAbnormalRecordService = otherAbnormalRecordService;
        }

        // GET: MaintenanceWork
        public ActionResult Index()
        {
            return View();
        }
        // GET: /MaintenanceWork/PendingList

        public ActionResult PendingList()
        {
            var vms = new List<MaintenanceWorkDetailViewModel>();
            var lists = this._abnormalRecordService.GetAll().ToList();

            foreach (var item in lists)
            {
                if (item.isClose == 0)
                {
                    
                    vms.Add(this.MaintenanceWorkDetailViewModule(item));
                }
            }
            return View(vms);
        }

        // POST: /MaintenanceWork/updateDate
        [HttpPost]
        public ActionResult ExtendedWorkUpdateDate(string recordId, string fixDate)
        {
            abnormalRecord record = this._abnormalRecordService.GetById(recordId);
            IResult result = this._abnormalRecordService.Update(record, "fixDate", DateTime.ParseExact(fixDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
            System.Diagnostics.Debug.WriteLine(result.Exception);
            return Json(new { Success = result.Success });
        }

        // GET: /MaintenanceWork/ExtendedWork
        public ActionResult ExtendedWork()
        {
            var vms = new List<MaintenanceWorkDetailViewModel>();
            var lists = this._abnormalRecordService.GetAll().ToList();

            foreach (var item in lists)
            {
                if (item.isClose == 0)
                    vms.Add(this.MaintenanceWorkDetailViewModule(item));
            }

            return View(vms);
        }

        // GET: /MaintenanceWork/Query
        public ActionResult Query()
        {
            ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
            ViewBag.abnormalDefinition = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);
            return View();
        }

        // POST: /MaintenanceWork/UpdatdQuery
        //[HttpPost]
        public ActionResult RefreshQuery(string startDate, string endDate, string sourceId, string abnormalId)
        {
            var queryStartDate = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            var queryEndDate = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            var success = true;
            var vms = new List<MaintenanceWorkDetailViewModel>();
            var abnormalLists = this._abnormalRecordService.GetAll().ToList();
            var otherLists = this._otherAbnormalRecordService.GetAll().ToList();
            foreach (var item in abnormalLists)
            {
                if (item.isClose == 0 && item.createTime >= queryStartDate && item.createTime <= queryEndDate && item.sourceId == sourceId && item.abnormalId == abnormalId)
                    vms.Add(this.MaintenanceWorkDetailViewModule(item));
            }
            foreach (var item in otherLists)
            {
                if (item.isClose == 0 && item.createTime >= queryStartDate && item.createTime <= queryEndDate && item.sourceId == sourceId && item.abnormalId == abnormalId)
                    vms.Add(this.MaintenanceWorkDetailViewModule(item));
            }
            if (vms.Count == 0)
                success = false;
            return Json(new { Success = success, vms  },JsonRequestBehavior.AllowGet);
        }

        // GET: /MaintenanceWork/DetailedData
        public ActionResult DetailedData()
        {
            return View();
        }

        // GET: /MaintenanceWork/WriteDetailedData
        public ActionResult WriteDetailedData()
        {
            return View();
        }

        private MaintenanceWorkDetailViewModel MaintenanceWorkDetailViewModule(abnormalRecord instance)
        {
            MaintenanceWorkDetailViewModel vm = new MaintenanceWorkDetailViewModel();
            vm.recordId = instance.recordId;
            vm.itemName = this._exhibitionItemService.GetById(instance.itemId).itemName;
            vm.roomName = this._exhibitionRoomService.GetById(this._exhibitionItemService.GetById(instance.itemId).roomId).roomName;
            vm.sourceName = this._reportSourceService.GetById(instance.sourceId).sourceName;
            vm.deviceId = instance.deviceId;
            vm.abnormalId = instance.abnormalId;
            vm.happendedTime = instance.happenedTime.HasValue ? instance.happenedTime?.ToString("yyyy-MM-dd hh:mm:ss") : "";
            vm.description = instance.description;
            vm.fixMethod = instance.fixMethod;
            vm.isClose = instance.isClose;
            vm.isDelete = instance.isDelete;
            vm.createTime = instance.createTime.HasValue ? instance.createTime?.ToString("yyyy-MM-dd hh:mm:ss") : "";
            vm.lastUpdateTime = instance.lastUpdateTime.HasValue ? instance.lastUpdateTime?.ToString("yyyy-MM-dd hh:mm:ss") : "";
            vm.fixDate = instance.fixDate.HasValue ? instance.fixDate?.ToString("yyyy-MM-dd hh:mm:ss") : "";

            if (this._exhibitionItemService.GetById(instance.itemId).itemType == 0)
                vm.listName = "Item";
            else
                vm.listName = "Experience";
            return vm;
        }

        private MaintenanceWorkDetailViewModel MaintenanceWorkDetailViewModule(otherAbnormalRecord instance)
        {
            MaintenanceWorkDetailViewModel vm = new MaintenanceWorkDetailViewModel();
            vm.recordId = instance.recordId;
            vm.itemName = instance.name;
            vm.roomName = "";
            vm.sourceName = this._reportSourceService.GetById(instance.sourceId).sourceName;
            vm.deviceId = instance.deviceId;
            vm.abnormalId = instance.abnormalId;
            vm.happendedTime = instance.happenedTime.HasValue ? instance.happenedTime?.ToString("yyyy-MM-dd hh:mm:ss") : "";
            vm.description = instance.description;
            vm.fixMethod = instance.fixMethod;
            vm.isClose = instance.isClose;
            vm.isDelete = instance.isDelete;
            vm.createTime = instance.createTime.HasValue ? instance.createTime?.ToString("yyyy-MM-dd hh:mm:ss") : "";
            vm.lastUpdateTime = instance.lastUpdateTime.HasValue ? instance.lastUpdateTime?.ToString("yyyy-MM-dd hh:mm:ss") : "";
            vm.fixDate = instance.fixDate.HasValue ? instance.fixDate?.ToString("yyyy-MM-dd hh:mm:ss") : "";
            vm.listName = "Other";
            return vm;
        }
    }
}