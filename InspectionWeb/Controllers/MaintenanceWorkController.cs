using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, Dispatch = true)]
    public class MaintenanceWorkController : Controller
    {
        private IAbnormalRecordService _abnormalRecordService;
        private IExhibitionItemService _exhibitionItemService;
        private IReportSourceService _reportSourceService;

        public MaintenanceWorkController(IAbnormalRecordService service, IExhibitionItemService service2, IReportSourceService service3)
        {
            this._abnormalRecordService = service;
            this._exhibitionItemService = service2;
            this._reportSourceService = service3;
        }

        // GET: MaintenanceWork
        public ActionResult Index()
        {
            return View();
        }
        // GET: /ReportJob/PendingList
        
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

        // POST: /ReportJob/PendingList
        [HttpPost]
        public ActionResult PendingList1(string recordId, string fixDate)
        {
            abnormalRecord record = this._abnormalRecordService.GetById(recordId);
            IResult result = this._abnormalRecordService.Update(record, "fixDate", DateTime.ParseExact(fixDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
            System.Diagnostics.Debug.WriteLine(result.Exception);
            return Json(new { Success = result.Success });
        }

        // GET: /ReportJob/ExtendedWork
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

        // GET: /ReportJob/Query
        public ActionResult Query()
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

        // GET: /ReportJob/DetailedData
        public ActionResult DetailedData()
        {
            return View();
        }

        // GET: /ReportJob/WriteDetailedData
        public ActionResult WriteDetailedData()
        {
            return View();
        }

        // POST: /User/UpdateUser
        [HttpPost]
        public ActionResult Updatedate(PendingJson pendingJson)
        {
            var id = pendingJson.pk;
            var item = this._abnormalRecordService.GetById(id);
            if (item != null && ModelState.IsValid)
            {
                IResult result = this._abnormalRecordService.Update(item, pendingJson.name, pendingJson.value);
                if (result.Success)
                {
                    return Json(result);
                }
                else
                {
                    return RedirectToAction("ExtendedWork");
                }
            }
            else
            {
                return RedirectToAction("PendingList");
            }
        }

        private MaintenanceWorkDetailViewModel MaintenanceWorkDetailViewModule(abnormalRecord instance)
        {
            MaintenanceWorkDetailViewModel vm = new MaintenanceWorkDetailViewModel();
            vm.recordId = instance.recordId;
            vm.itemName = this._exhibitionItemService.GetById(instance.itemId).itemName;
            //vm.roomName = this._exhibitionRoomService.GetById(this._exhibitionItemService.GetById(instance.itemId).roomId).roomName;
            vm.sourceName = this._reportSourceService.GetBySourceCode(instance.sourceId).sourceName;
            vm.deviceId = instance.deviceId;
            vm.abnormalId = instance.abnormalId;
            vm.happendedTime = instance.happenedTime;
            vm.description = instance.description;
            vm.fixMethod = instance.fixMethod;
            vm.isClose = instance.isClose;
            vm.isDelete = instance.isDelete;
            vm.createTime = instance.createTime;
            vm.lastUpdateTime = instance.lastUpdateTime;
            vm.fixDate = instance.fixDate;
            return vm;
        }

        public class PendingJson
        {
            public string pk { get; set; }
            public string name { get; set; }
            public string value { get; set; }
        }
    }
}