using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System.Globalization;
using System.Threading;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, Dispatch = true)]
    public class ReportJobController : Controller
    {
        private IExhibitionRoomService _exhibitionRoomService;
        private IExhibitionItemService _exhibitionItemService;
        private IReportSourceService _reportSourceService;
        private IAbnormalDefinitionService _abnormalDefinitionService;
        private IAbnormalRecordService _abnormalRecordService;
        private IOtherAbnormalRecordService _otherAbnormalRecordService;
      

        public ReportJobController(IExhibitionRoomService service, IExhibitionItemService service2, IReportSourceService service3,
            IAbnormalDefinitionService service4, IAbnormalRecordService service5, IOtherAbnormalRecordService service6)
        {
            this._exhibitionRoomService = service;
            this._exhibitionItemService = service2;
            this._reportSourceService = service3;
            this._abnormalDefinitionService = service4;
            this._abnormalRecordService = service5;
            this._otherAbnormalRecordService = service6;
        }

        // GET: /ReportJob/EditExhibitionItem
        public ActionResult EditExhibitionItem()
        {
            return View();
        }

        // GET: /ReportJob/DetailedData/id
        public ActionResult ItemDetailedData(string id)
        {
            return View();
        }

        // GET: /ReportJob/AddExhibitionItem
        public ActionResult AddExhibitionItem()
        {
            //取出展示廳資料
            
            ViewBag.exhibitionRooms = this._exhibitionRoomService.GetAll().Where(x => x.active == 1);
            ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
            ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);

            return View();
        }

        // GET: /ReportJob/AddExperience
        public ActionResult AddExperience()
        {
            ViewBag.exhibitionRooms = this._exhibitionRoomService.GetAll().Where(x => x.active == 1);
            ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
            ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);

            return View();
        }

        // GET: /ReportJob/AddOther
        public ActionResult AddOther()
        {
            ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
            ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);
            return View();
        }

        // GET: /ReportJob/Query
        public ActionResult Query()
        {
            string str;
            ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
            ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);
            ViewBag.record = this._otherAbnormalRecordService.GetAll().Where(x => x.isDelete == 0);
            
            foreach (var item in ViewBag.record)
            {
                str = this._reportSourceService.GetById(item.sourceId).sourceName;
            }
            
            return View();
        }

        //======================功能區==========================//

        // POST: /User/AddUser ; 新增展項通報紀錄於資料表中
        [HttpPost]
        public ActionResult AddItem(string itemId, string sourceId, string abnormalId, string reporter)
        {
            if (ModelState.IsValid)
            {
                IResult result = this._abnormalRecordService.Create(itemId, sourceId, abnormalId, reporter);

                if (result.Success == false)
                {
                    //取出展示廳資料
                    ViewBag.exhibitionRooms = this._exhibitionRoomService.GetAll().Where(x => x.active == 1);
                    ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
                    ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);
                    ViewBag.ErrorMsg = result.ErrorMsg;
                    
                    return View("AddExhibitionItem");
                }

                return RedirectToAction("ItemDetailedData", new { id = result.Message });
            }
            else
            {
                return View("AddExhibitionItem");
            }
        }

        //新增體驗設施通報紀錄於資料表中
        [HttpPost]
        public ActionResult AddExp(string itemId, string sourceId, string abnormalId, string reporter)
        {
            if (ModelState.IsValid)
            {
                IResult result = this._abnormalRecordService.Create(itemId, sourceId, abnormalId, reporter);

                if (result.Success == false)
                {
                    //取出展示廳資料
                    ViewBag.exhibitionRooms = this._exhibitionRoomService.GetAll().Where(x => x.active == 1);
                    ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
                    ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);
                    ViewBag.ErrorMsg = result.ErrorMsg;

                    return View("AddExperience");
                }

                return RedirectToAction("ItemDetailedData", new { id = result.Message });           ////////////////////actionName待修改
            }
            else
            {
                return View("AddExperience");
            }
        }

        //新增其他設施通報紀錄於資料表中
        [HttpPost]
        public ActionResult Addother(string name, string sourceId, string abnormalId, string reporter)
        {
            if ( !string.IsNullOrEmpty(name) && ModelState.IsValid )
            {
                IResult result = this._otherAbnormalRecordService.Create(name, sourceId, abnormalId, reporter);

                if (result.Success == false)
                {
                    //取出展示廳資料
                    ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
                    ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);
                    ViewBag.ErrorMsg = result.ErrorMsg;

                    return View("AddOther");
                }

                return RedirectToAction("ItemDetailedData", new { id = result.Message });           ////////////////////actionName待修改
            }
            else
            {
                //取出展示廳資料
                ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
                ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);
                ViewBag.ErrorMsg = "設施名稱空白";
                return View("AddOther");
            }
        }

       
        

        //取得隸屬於某展示廳下的展項
        [HttpPost]
        public ActionResult GetItems(roomJson data)
        {
            var roomId = data.id;
            var vms = new List<ReportJobViewModel>();
            //取出一般展項
            var items = this._exhibitionItemService.GetAll().Where(x => x.roomId == roomId && x.itemType == 0
             && x.active == 1 && x.isDelete == 0);

            foreach(var item in items)
            {
                vms.Add(this.Item2ViewModel_item(item));
            }
            

            //for loop convert items to ItemviewModel
           
            return Json(vms);
        }

        //取得隸屬於某展示廳下的體驗設施
        [HttpPost]
        public ActionResult GetExperience(roomJson data)
        {
            var roomId = data.id;
            var vms = new List<ReportJobViewModel>();
            //取出體驗設施
            var items = this._exhibitionItemService.GetAll().Where(x => x.roomId == roomId && x.itemType == 1
             && x.active == 1 && x.isDelete == 0);

            foreach (var item in items)
            {
                vms.Add(this.Item2ViewModel_item(item));
            }


            //for loop convert items to ItemviewModel

            return Json(vms);
        }

        //按下查詢確定鈕後所接收資料的函數
        [HttpPost]
        public ActionResult GetQuery(string sourceId, string abnormalId, byte isClose, string startDate, 
            string endDate, string type)
        {
            List<ReportJobViewModel> vms = new List<ReportJobViewModel>();
            
            //只有某些條件的還沒寫
            if(type == "item")
            {
                var records = GainRecords(sourceId, abnormalId, isClose, startDate, endDate);
                
                foreach (var record in records)
                {
                    var exhibitionItem = this._exhibitionItemService.GetById(record.itemId);
                    if(exhibitionItem.itemType == 0)
                    {
                        vms.Add(this.Query2ViewModel(record));
                    }
                }
            }
            else if(type == "exp")
            {
                var records = GainRecords(sourceId, abnormalId, isClose, startDate, endDate);
                
                foreach (var record in records)
                {
                    var exhibitionItem = this._exhibitionItemService.GetById(record.itemId);
                    if (exhibitionItem.itemType == 1)
                    {
                        vms.Add(this.Query2ViewModel(record));
                    }
                }
            }
            else if(type == "other")
            {
                //取得其他異常紀錄
                var records = GainRecords_other(sourceId, abnormalId, isClose, startDate, endDate);

                foreach (var record in records)
                {
                    vms.Add(this.Query2ViewModel_other(record));
                }
            }
            return Json(vms);
        }

        public List<abnormalRecord> GainRecords(string sourceId, string abnormalId, byte isClose, string startDate, string endDate)
        {
            List<abnormalRecord> records = new List<abnormalRecord>();

            records = this._abnormalRecordService.GetAll().Where( x=> x.createTime >= Convert.ToDateTime(startDate) && 
            x.createTime <= Convert.ToDateTime(endDate)).ToList();
            if (sourceId != "N")
            {
                records = records.Where(x => x.sourceId == sourceId).ToList();
            }
            if(abnormalId != "N")
            {
                records = records.Where(x => x.abnormalId == abnormalId).ToList();
            }
            if(isClose != 2)
            {
                records = records.Where(x => x.isClose == isClose).ToList();
            }
            
            return records;
        }
        public List<otherAbnormalRecord> GainRecords_other(string sourceId, string abnormalId, byte isClose, string startDate, string endDate)
        {
            List<otherAbnormalRecord> records = new List<otherAbnormalRecord>();

            records = this._otherAbnormalRecordService.GetAll().Where(x => x.createTime >= Convert.ToDateTime(startDate) &&
           x.createTime <= Convert.ToDateTime(endDate)).ToList();
            if (sourceId != "N")
            {
                records = records.Where(x => x.sourceId == sourceId).ToList();
            }
            if (abnormalId != "N")
            {
                records = records.Where(x => x.abnormalId == abnormalId).ToList();
            }
            if (isClose != 2)
            {
                records = records.Where(x => x.isClose == isClose).ToList();
            }

            return records;
        }

        public ReportJobViewModel Query2ViewModel(abnormalRecord instance)
        {
            ReportJobViewModel vm = new ReportJobViewModel();

            vm.recordId = instance.recordId;
            vm.sourceName = this._reportSourceService.GetById(instance.sourceId).sourceName;
            vm.roomName = this._exhibitionRoomService.GetById(this._exhibitionItemService.GetById(instance.itemId).roomId).roomName;
            vm.itemName = this._exhibitionItemService.GetById(instance.itemId).itemName;
            vm.happenedTime = instance.happenedTime?.ToString("yyyy-MM-dd HH:mm:ss");
            vm.fixDate = instance.fixDate?.ToString("yyyy-MM-dd HH:mm:ss");
            vm.isClose = (instance.isClose == 1) ? "是" : "否";

            return vm;
        }


        //轉換至ReportJJobViewModel (otherRecord版)
        public ReportJobViewModel Query2ViewModel_other(otherAbnormalRecord instance)
        {
            ReportJobViewModel vm = new ReportJobViewModel();

            vm.recordId = instance.recordId;
            vm.sourceName = this._reportSourceService.GetById(instance.sourceId).sourceName;
            vm.itemName = instance.name;
            vm.happenedTime = instance.happenedTime?.ToString("yyyy-MM-dd HH:mm:ss");
            vm.fixDate = instance.fixDate?.ToString("yyyy-MM-dd HH:mm:ss");
            vm.isClose = (instance.isClose == 1) ? "是" : "否";

            return vm;
        }

        public ReportJobViewModel Item2ViewModel_item(exhibitionItem instance)
        {
            ReportJobViewModel vm = new ReportJobViewModel();

            vm.itemId = instance.itemId;
            vm.itemName = instance.itemName;

            return vm;
        }

        public class roomJson
        {
            public string id { set; get; }
        }

    }
}