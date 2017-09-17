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
        private IMailService _mailService;
        private IRoomInspectionDispatchService _roomInspectionDispatchService;
        private IItemInspectionDispatchService _itemInspectionDispatchService;
        private MailController _mailController;

        public ReportJobController(IExhibitionRoomService service, IExhibitionItemService service2, IReportSourceService service3,
            IAbnormalDefinitionService service4, IAbnormalRecordService service5, IOtherAbnormalRecordService service6,
            IMailService service7, IRoomInspectionDispatchService service8, IItemInspectionDispatchService service9,
            MailController controller)
        {
            this._exhibitionRoomService = service;
            this._exhibitionItemService = service2;
            this._reportSourceService = service3;
            this._abnormalDefinitionService = service4;
            this._abnormalRecordService = service5;
            this._otherAbnormalRecordService = service6;
            this._mailService = service7;
            this._roomInspectionDispatchService = service8;
            this._itemInspectionDispatchService = service9;
            this._mailController = controller;
        }

        
        // GET: /ReportJob/AddExperience
        public ActionResult AddExperience()
        {
            ViewBag.exhibitionRooms = this._exhibitionRoomService.GetAll().Where(x => x.active == 1);
            ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
            ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);

            return View();
        }

        
        public ActionResult AddOther()
        {
            ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
            ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);
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
            ViewBag.reportSources = this._reportSourceService.GetAll().Where(x => x.isDelete == 0);
            ViewBag.abnormals = this._abnormalDefinitionService.GetAll().Where(x => x.isDelete == 0);
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

        
        // POST: /User/AddUser
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

                var now = DateTime.Now;
                var startDate = now.ToString("yyyy/MM/dd");
                var endDate = now.ToString("yyyy/MM/dd");
                var roomId = _exhibitionItemService.GetById(itemId).roomId;
                var systemSettings = _mailService.GetAll().ToList(); // get systemSettings<list> to 'from' value.
                var roominspectionList = _roomInspectionDispatchService.GetAll().ToList();
                var nowtime = Convert.ToDateTime(now.ToString("hh:mm:ss"));
                var time = Convert.ToDateTime("12:00:00");
                var userId = string.Empty;
                roomInspectionDispatch roomInspection = new roomInspectionDispatch();

                foreach (var item in roominspectionList)
                {
                    if(item.checkDate == now)
                    {
                        if (item.roomId == roomId)
                        {
                            roomInspection = item;
                            break;
                        }
                    }
                }
                
                if (DateTime.Compare(nowtime, time) < 0)   // am inspectionuser
                    userId = roomInspection.inspectorId1;
                else
                    userId = roomInspection.inspectorId2;
                

                // get inspectionUser<list> to 'to' value.
                var emailJson = new MailController.EmailJson(){
                    from = systemSettings[0].keyName,
                    to = "P76064342@ncku.edu.tw",
                    subject = "單元故障通知，故障單元：" + this._exhibitionItemService.GetById(itemId).itemName,
                    msg = "您好：\n\n" + DateTime.Now.ToString("yyyy/MM/dd") + "通報故障之「" +
                        this._exhibitionItemService.GetById(itemId).itemName + "」，已派" + itemId +
                        "處理。\n系統在此告知。"
                };
                this._mailController.SendEmail(emailJson);

                return RedirectToAction("ItemDetailedData", new { id = result.Message });
            }
            else
            {
                return View("AddExhibitionItem");
            }
        }

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

        [HttpPost]
        public ActionResult Addother(string name, string sourceId, string abnormalId)
        {
            if ( !string.IsNullOrEmpty(name) && ModelState.IsValid )
            {
                IResult result = this._otherAbnormalRecordService.Create(name, sourceId, abnormalId);

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

        [HttpPost]
        public ActionResult GetItems(roomJson data)
        {
            var roomId = data.id;
            var vms = new List<ReportAddViewModel>();
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

        [HttpPost]
        public ActionResult GetExperience(roomJson data)
        {
            var roomId = data.id;
            var vms = new List<ReportAddViewModel>();
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


        public ReportAddViewModel Item2ViewModel_item(exhibitionItem instance)
        {
            ReportAddViewModel vm = new ReportAddViewModel();

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