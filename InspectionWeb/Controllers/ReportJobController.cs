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
    public class ReportJobController : Controller
    {
        private IExhibitionRoomService _exhibitionRoomService;
        private IExhibitionItemService _exhibitionItemService;
        private IReportSourceService _reportSourceService;
        private IAbnormalDefinitionService _abnormalDefinitionService;
        private IAbnormalRecordService _abnormalRecordService;

        public ReportJobController(IExhibitionRoomService service, IExhibitionItemService service2, 
            IReportSourceService service3, IAbnormalDefinitionService service4, IAbnormalRecordService service5)
        {
            this._exhibitionRoomService = service;
            this._exhibitionItemService = service2;
            this._reportSourceService = service3;
            this._abnormalDefinitionService = service4;
            this._abnormalRecordService = service5;
        }

        

        // GET: /ReportJob/AddExperience
        public ActionResult AddExperience()
        {
            return View();
        }

        
        public ActionResult AddOther()
        {
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
                    ViewBag.ErrMsg = result.ErrorMsg;
                    return View("AddExhibitionItem");
                }

                return RedirectToAction("ItemDetailedData", new { id = result.Message });
            }
            else
            {
                return View("AddExhibitionItem");
            }
        }

        [HttpPost]
        public ActionResult GetItems(roomJson data)
        {
            var roomId = data.id;
            var vms = new List<ExhibitionItemViewModel>();
            //取出一般展項
            var items = this._exhibitionItemService.GetAll().Where(x => x.roomId == roomId && x.itemType == 0
             && x.active == 1 && x.isDelete == 0);

            foreach(var item in items)
            {
                vms.Add(this.Item2ViewModel(item));
            }
            

            //for loop convert items to ItemviewModel
           
            return Json(vms);
        }

        public ExhibitionItemViewModel Item2ViewModel(exhibitionItem instance)
        {
            ExhibitionItemViewModel vm = new ExhibitionItemViewModel();

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