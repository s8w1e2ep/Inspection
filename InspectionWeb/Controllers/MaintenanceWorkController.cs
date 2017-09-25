using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System.ComponentModel.DataAnnotations;
using System.Web;

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

        public MaintenanceWorkController(IAbnormalRecordService abnormalRecordService, 
            IExhibitionItemService exhibitionItemService, IReportSourceService reportSourceService,
            IExhibitionRoomService exhibitionRoomService, IAbnormalDefinitionService abnormalDefinitionService, 
            IOtherAbnormalRecordService otherAbnormalRecordService)
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

        // GET: /MaintenanceWork/GetQuery
        public ActionResult GetQuery(string startDate, string endDate, string sourceId, string abnormalId)
        {
            var queryStartDate = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            var queryEndDate = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            var success = true;
            var vms = new List<MaintenanceWorkDetailViewModel>();
            var abnormalLists = this._abnormalRecordService.GetAll().ToList();
            var otherLists = this._otherAbnormalRecordService.GetAll().ToList();
            foreach (var item in abnormalLists)
            {
                if (item.createTime >= queryStartDate && item.createTime <= queryEndDate && item.sourceId == sourceId && item.abnormalId == abnormalId)
                    vms.Add(this.MaintenanceWorkDetailViewModule(item));
            }
            foreach (var item in otherLists)
            {
                if (item.createTime >= queryStartDate && item.createTime <= queryEndDate && item.sourceId == sourceId && item.abnormalId == abnormalId)
                    vms.Add(this.MaintenanceWorkDetailViewModule(item));
            }
            if (vms.Count == 0)
                success = false;
            return Json(new { Success = success, vms  },JsonRequestBehavior.AllowGet);
        }

        private MaintenanceWorkDetailViewModel MaintenanceWorkDetailViewModule(abnormalRecord instance)
        {
            MaintenanceWorkDetailViewModel vm = new MaintenanceWorkDetailViewModel()
            {
                recordId = instance.recordId,
                itemName = this._exhibitionItemService.GetById(instance.itemId).itemName,
                roomName = this._exhibitionRoomService.GetById(this._exhibitionItemService.GetById(instance.itemId).roomId).roomName,
                sourceName = this._reportSourceService.GetById(instance.sourceId).sourceName,
                deviceId = instance.deviceId,
                abnormalId = instance.abnormalId,
                happendedTime = instance.happenedTime.HasValue ? instance.happenedTime?.ToString("yyyy-MM-dd hh:mm:ss") : "",
                description = instance.description,
                isClose = instance.isClose,
                isDelete = instance.isDelete,
                createTime = instance.createTime.HasValue ? instance.createTime?.ToString("yyyy-MM-dd hh:mm:ss") : "",
                lastUpdateTime = instance.lastUpdateTime.HasValue ? instance.lastUpdateTime?.ToString("yyyy-MM-dd hh:mm:ss") : "",
                fixDate = instance.fixDate.HasValue ? instance.fixDate?.ToString("yyyy-MM-dd hh:mm:ss") : ""
            };
            if (this._exhibitionItemService.GetById(instance.itemId).itemType == 0)
                vm.listName = "Item";
            else
                vm.listName = "Experience";
            return vm;
        }
        
            private MaintenanceWorkDetailViewModel MaintenanceWorkDetailViewModule(otherAbnormalRecord instance)
        {
            MaintenanceWorkDetailViewModel vm = new MaintenanceWorkDetailViewModel()
            {
                recordId = instance.recordId,
                itemName = instance.name,
                roomName = "",
                sourceName = this._reportSourceService.GetById(instance.sourceId).sourceName,
                deviceId = instance.deviceId,
                abnormalId = instance.abnormalId,
                happendedTime = instance.happenedTime.HasValue ? instance.happenedTime?.ToString("yyyy-MM-dd hh:mm:ss") : "",
                description = instance.description,
                isClose = instance.isClose,
                isDelete = instance.isDelete,
                createTime = instance.createTime.HasValue ? instance.createTime?.ToString("yyyy-MM-dd hh:mm:ss") : "",
                lastUpdateTime = instance.lastUpdateTime.HasValue ? instance.lastUpdateTime?.ToString("yyyy-MM-dd hh:mm:ss") : "",
                fixDate = instance.fixDate.HasValue ? instance.fixDate?.ToString("yyyy-MM-dd hh:mm:ss") : "",
                listName = "Other"
            };
            return vm;
        }
    }
}