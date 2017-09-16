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
        private IUserService _userService;
        private IManRepairRecordService _manRepairRecordService;


        public ReportJobController(IExhibitionRoomService service, IExhibitionItemService service2, IReportSourceService service3,
            IAbnormalDefinitionService service4, IAbnormalRecordService service5, IOtherAbnormalRecordService service6, IUserService service7,
            IManRepairRecordService service8)
        {
            this._exhibitionRoomService = service;
            this._exhibitionItemService = service2;
            this._reportSourceService = service3;
            this._abnormalDefinitionService = service4;
            this._abnormalRecordService = service5;
            this._otherAbnormalRecordService = service6;
            this._userService = service7;
            this._manRepairRecordService = service8;
        }

        // GET: /ReportJob/EditExhibitionItem
        public ActionResult EditExhibitionItem()
        {
            return View();
        }

        // GET: /ReportJob/DetailedData/id
        public ActionResult ItemDetailedData(string id)
        {
            reportDetailedViewModel vm = new reportDetailedViewModel();
            vm = GainDetail(id);
            

            return View(vm);
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
            ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
            ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);
            return View();
        }

        //======================功能區==========================//

        
        // 新增通報 :展項通報 POST: /User/AddUser 
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

        //新增通報選單功能 : 取得隸屬於某展示廳下的展項
        [HttpPost]
        public ActionResult GetItems(roomJson data)
        {
            var roomId = data.id;
            var vms = new List<ReportJobViewModel>();
            //取出一般展項
            var items = this._exhibitionItemService.GetAll().Where(x => x.roomId == roomId && x.itemType == 0
             && x.active == 1 && x.isDelete == 0);

            foreach (var item in items)
            {
                vms.Add(this.Item2ViewModel_item(item));
            }


            //for loop convert items to ItemviewModel

            return Json(vms);
        }

        //新增通報 :體驗設施通報
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

        //新增通報選單功能 : 取得隸屬於某展示廳下的體驗設施 
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

        //新增通報 : 其他設施通報
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


        //通報查詢 : 取得查詢結果
        [HttpPost]
        public ActionResult GetQuery(string sourceId, string abnormalId, byte isClose, string startDate,
            string endDate, string type)
        {
            List<ReportJobViewModel> vms = new List<ReportJobViewModel>();

            //只有某些條件的還沒寫
            if (type == "item")
            {
                var records = GainRecords(sourceId, abnormalId, isClose, startDate, endDate);

                foreach (var record in records)
                {
                    var exhibitionItem = this._exhibitionItemService.GetById(record.itemId);
                    if (exhibitionItem.itemType == 0)
                    {
                        vms.Add(this.Query2ViewModel(record));
                    }
                }
            }
            else if (type == "exp")
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
            else if (type == "other")
            {
                var records = GainRecords_other(sourceId, abnormalId, isClose, startDate, endDate);

                foreach (var record in records)
                {
                    vms.Add(this.Query2ViewModel_other(record));
                }
            }
            return Json(vms);
        }

        //更新異常紀錄
        [HttpPost]
        public ActionResult UpdateRecord(string name, string pk, string value)
        {
            var id = pk;
            var record = this._abnormalRecordService.GetById(id);
            if (record != null && ModelState.IsValid)
            {
                if (value == "") value = null;
                IResult result = this._abnormalRecordService.Update(record, name, value);

                if (result.Success)
                {
                    if(name == "abnormalId")
                    {
                        result.Message = this._abnormalDefinitionService.GetById(value).description;
                    }
                    return Json(result);
                }
                else
                {
                    return RedirectToAction("ItemDetailedData");
                }
            }
            else
            {
                return RedirectToAction("Query");
            }
        }

        //新增人工維修紀錄
        [HttpPost]
        public ActionResult AddFixRecord(string recordId)
        {
            if (!string.IsNullOrEmpty(recordId) && ModelState.IsValid)
            {
                if (Session["userId"].ToString() == null)
                {
                    RedirectToAction("Login", "Authentication");
                }


                var filluserId = Session["userId"].ToString();
                IResult result = this._manRepairRecordService.Create(recordId, filluserId);

                if (result.Success)
                {
                    ManRepairViewModel vm = new ManRepairViewModel();
                    var manRecord = this._manRepairRecordService.GetByID(recordId); 
                    vm.filluserName = this._userService.GetByID(filluserId).userName;
                    vm.createTime = manRecord.createTime?.ToString("yyyy-MM-dd HH:mm:ss");
                    vm.lastUpdateTime = manRecord.lastUpdateTime?.ToString("yyyy-MM-dd HH:mm:ss");

                    return Json(vm);
                }
                else
                {
                    return RedirectToAction("ItemDetailedData");
                }
            }
            else
            {
                return RedirectToAction("ItemDetailedData");
            }

        }

        //更新人工排除故障紀錄
        [HttpPost]
        public ActionResult UpdateManRecord(string name, string pk, string value)
        {
            var id = pk;
            var record = this._manRepairRecordService.GetByID(id);
            if (record != null && ModelState.IsValid)
            {
                if (value == "") value = null;
                IResult result = this._manRepairRecordService.Update(record, name, value);

                if (result.Success)
                {
                    return Json(result);
                }
                else
                {
                    return RedirectToAction("ItemDetailedData");
                }
            }
            else
            {
                return RedirectToAction("Query");
            }
        }
        

        //取得通報詳細資料
        public reportDetailedViewModel GainDetail(string id)
        {
            reportDetailedViewModel vmD = new reportDetailedViewModel();
            ReportJobViewModel vm = new ReportJobViewModel();
            abnormalRecord instance = new abnormalRecord();
            manRepairRecord instanceM = new manRepairRecord();
            ManRepairViewModel vmm = new ManRepairViewModel();

            List<user> users = new List<user>();
            List<reportSource> sources = new List<reportSource>();
            List<abnormalDefinition> abnormals = new List<abnormalDefinition>();

            instance = this._abnormalRecordService.GetById(id);
            instanceM = this._manRepairRecordService.GetByID(id);
            
            //選單
            sources = this._reportSourceService.GetAll().ToList();
            abnormals = this._abnormalDefinitionService.GetAll().ToList();
            users = this._userService.GetAll().ToList();

            //通報紀錄
            vm.recordId = instance.recordId;
            vm.roomName = this._exhibitionRoomService.GetById(this._exhibitionItemService.GetById(instance.itemId).roomId).roomName;
            vm.itemName = this._exhibitionItemService.GetById(instance.itemId).itemName;
            vm.sourceName = this._reportSourceService.GetById(instance.sourceId).sourceName;
            var abnormal = this._abnormalDefinitionService.GetById(instance.abnormalId);
            vm.abnormalName = abnormal.abnormalName;
            vm.abnormalDescription = abnormal.description;
            vm.happenedTime = instance.happenedTime?.ToString("yyyy/MM/dd HH:mm:ss");
            vm.description = instance.description;
            vm.fixDate = instance.fixDate?.ToString("yyyy/MM/dd HH:mm:ss");
            if (instance.fixMethod == 1)
                vm.fixMethod = "人員排除";
            else if (instance.fixMethod == 0)
                vm.fixMethod = "自動排除";

            vm.isClose_s = instance.isClose;
            vm.createTime = instance.createTime?.ToString("yyyy-MM-dd HH:mm:ss");
            vm.lastUpdateTime = instance.lastUpdateTime?.ToString("yyyy-MM-dd HH:mm:ss");

            //人員維修紀錄
            if(instanceM != null)
            {
                if(instanceM.repairUserId != null)
                {
                    vmm.repairUserName = this._userService.GetByID(instanceM.repairUserId).userName;
                }
                vmm.filluserName = this._userService.GetByID(instanceM.fillUserId).userName;
                vmm.fixnote = instanceM.fixNote;
                vmm.cost = instanceM.cost;
                vmm.expectDate = instanceM.expectDate?.ToString("yyyy-MM-dd");
                vmm.imgDesc1 = instanceM.imgDesc1;
                vmm.imgDesc2 = instanceM.imgDesc2;
                vmm.imgDesc3 = instanceM.imgDesc3;
                vmm.imgFixDesc1 = instanceM.imgFixDesc1;
                vmm.createTime = instanceM.createTime?.ToString("yyyy-MM-dd HH:mm:ss");
                vmm.lastUpdateTime = instanceM.lastUpdateTime?.ToString("yyyy-MM-dd HH:mm:ss");

                //封裝
                vmD.ManRecord = vmm;
            }
            

            //封裝起來
            vmD.reportRecord = vm;
            vmD.sources = sources;
            vmD.abnormals = abnormals;
            vmD.repairUsers = users;
            
            return vmD;
        }

        //通報查詢 :取得展項異常紀錄
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

        //通報查詢 :取得其他異常紀錄
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

        //record to ReportJobViewModel 
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
        public class RcordJson
        {
            public string name { get; set; }
            public string pk { get; set; }
            public string value { get; set; }

        }
    }
}