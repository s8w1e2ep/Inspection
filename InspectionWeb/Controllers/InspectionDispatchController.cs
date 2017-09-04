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
        private INoCheckDateService _noCheckDateService;
        private IExhibitionRoomService _ExhibitionRoomService;
        private IExhibitionItemService _ExhibitionItemService;
        private IUserService _UserService;


        public class inpectionDateJson
        {
            public string dispatchDate { get; set; }
        }

        public class inspectionDispatchJson
        {
            public string name { get; set; }
            public string pk { get; set; }
            public string value { get; set; }
        }

        public class nonInspectionDispatchJson
        {
            public System.DateTime nonCheckDate { get; set; }
            public string description { get; set; }
            public bool morning { get; set; }
            public bool afternoon { get; set; }
        }


        public InspectionDispatchController(IRoomInspectionDispatchService roomInpectionDispatchService, 
                                            IItemInspectionDispatchService itemInspectionDispatchService, 
                                            INoCheckDateService noCheckDateService,
                                            IExhibitionRoomService exhibitionRoomService, 
                                            IExhibitionItemService exhibitionItemService,
                                            IUserService userService)
        {
            this._RoomInspectionDispatchService = roomInpectionDispatchService;
            this._ItemInspectionDispatchService = itemInspectionDispatchService;
            this._noCheckDateService = noCheckDateService;
            this._ExhibitionRoomService = exhibitionRoomService;
            this._ExhibitionItemService = exhibitionItemService;
            this._UserService = userService;
        }

        [HttpGet]
        public ActionResult ListRoomInspectionDispatch()
        {
            ListRoomInspectionDispatchViewModel empty = null;
            return View(empty);
        }

        [HttpPost]
        public ActionResult ListRoomInspectionDispatch(inpectionDateJson timeJson)
        {
            System.DateTime date = Convert.ToDateTime(timeJson.dispatchDate);
            bool isExist = this._RoomInspectionDispatchService.IsExists(date);
            bool roomNumCheck = this._RoomInspectionDispatchService.checkRoomInsert(date);
            IEnumerable<exhibitionRoom> rooms = this._ExhibitionRoomService.GetAll();
            //檢查有無分派的紀錄
            if (isExist)
            {
                //檢查有無新增展覽廳
                System.Diagnostics.Debug.WriteLine("has dispatch.");
                if (!roomNumCheck)
                {
                    System.Diagnostics.Debug.WriteLine("number of rooms is same");
                    List<roomInspectionDispatchDetail> roomDispatchDetail = new List<roomInspectionDispatchDetail>();
                    var roomDispatchs = this._RoomInspectionDispatchService.GetAllByDate(date);
                    foreach (var roomDispatch in roomDispatchs)
                    {
                        roomDispatchDetail.Add(roomDispatch);
                    }

                    List<userListForInspectionViweModel> userList = GetUserListForInspection();
                    return View(Data2RoomViewModel(roomDispatchDetail, userList, null, date));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("number of rooms is different");
                    //找出還未建檔的展覽廳
                    rooms = this._ExhibitionRoomService.GetUndispatchRoom(date);
                    IResult result = this._RoomInspectionDispatchService.Create(date, rooms);
                    if (result.Success == false)
                    {
                        System.Diagnostics.Debug.WriteLine("insert error");
                        //result.ErrorMsg = "Dispatch List construct error";
                        System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
                        return View(Data2RoomViewModel(null, null, result,date));
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("insert success");
                        List<roomInspectionDispatchDetail> roomDispatchDetail = new List<roomInspectionDispatchDetail>();
                        var roomDispatchs = this._RoomInspectionDispatchService.GetAllByDate(date);
                        foreach (var roomDispatch in roomDispatchs)
                        {
                            roomDispatchDetail.Add(roomDispatch);
                        }
                        List<userListForInspectionViweModel> userList = GetUserListForInspection();
                        return View(Data2RoomViewModel(roomDispatchDetail, userList, result, date));
                    }
                }

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("has not dispatch yet");
                IResult result = this._RoomInspectionDispatchService.Create(date, rooms);
                if (result.Success == false)
                {
                    System.Diagnostics.Debug.WriteLine("insert error");
                    System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
                    //result.ErrorMsg = "Dispatch List construct error";
                    return View(Data2RoomViewModel(null, null, result, date));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("insert success");
                    List<roomInspectionDispatchDetail> roomDispatchDetail = new List<roomInspectionDispatchDetail>();
                    var roomDispatchs = this._RoomInspectionDispatchService.GetAllByDate(date);
                    foreach (var roomDispatch in roomDispatchs)
                    {
                        roomDispatchDetail.Add(roomDispatch);
                    }
                    List<userListForInspectionViweModel> userList = GetUserListForInspection();
                    return View(Data2RoomViewModel(roomDispatchDetail, userList, result, date));
                }
            }
        }

        public ActionResult UpdateRoomInspectionDispatch(inspectionDispatchJson dispatchJson)
        {
            var roomDispatchId = dispatchJson.pk;
            var roomDispatch = this._RoomInspectionDispatchService.GetById(roomDispatchId);
            if (roomDispatch != null && ModelState.IsValid)
            {
                IResult result = this._RoomInspectionDispatchService.Update(roomDispatch, dispatchJson.name, dispatchJson.value);
                roomDispatch = this._RoomInspectionDispatchService.GetById(roomDispatchId);
                if (result.Success)
                {
                    string lastUpdateTime = roomDispatch.lastUpdateTime.ToString();
                    return Json(new { result = 1, roomDispatchId = roomDispatchId });
                }
                else
                {
                    return Json(new { result = 0, roomDispatchId = roomDispatchId });
                }
            }
            else
            {
                return Json(new { result = 0, roomDispatchId = roomDispatchId });
                //return RedirectToAction("ListReportSource");
            }
        }

        private ListRoomInspectionDispatchViewModel Data2RoomViewModel(List<roomInspectionDispatchDetail> dispatchList, List<userListForInspectionViweModel> userList, IResult result,System.DateTime checkDate)
        {
            ListRoomInspectionDispatchViewModel viewModel = new ListRoomInspectionDispatchViewModel();
            viewModel.roomInspectionDispatch = dispatchList;
            viewModel.userList = userList;
            viewModel.checkDate = checkDate.ToString("d");
            if (result!=null) {
                viewModel.ErrorMsg = result.ErrorMsg;
                System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
            }
            else
            {
                viewModel.ErrorMsg = null;
            }
            return viewModel;
        }

        private List<userListForInspectionViweModel> GetUserListForInspection()
        {
            IEnumerable<user> users = this._UserService.GetAll();
            System.Diagnostics.Debug.WriteLine(users.Count());
            List<userListForInspectionViweModel> userList = new List<userListForInspectionViweModel>();
            foreach (var user in users)
            {
                userListForInspectionViweModel u = new userListForInspectionViweModel();
                u.userId = user.userId;
                u.userCode = user.userCode;
                u.userName = user.userName;
                userList.Add(u);
            }
            return userList;
        }

        [HttpGet]
        public ActionResult ListItemInspectionDispatch()
        {
            ListItemInspectionDispatchViewModel empty = null;
            return View(empty);
        }

        [HttpPost]
        public ActionResult ListItemInspectionDispatch(inpectionDateJson timeJson)
        {
            System.DateTime date = Convert.ToDateTime(timeJson.dispatchDate);
            bool isExist = this._ItemInspectionDispatchService.IsExists(date);
            bool itemNumCheck = this._ItemInspectionDispatchService.checkItemInsert(date);
            IEnumerable<exhibitionItem> items = this._ExhibitionItemService.GetAll();
            //檢查有無分派的紀錄
            if (isExist)
            {
                //檢查有無新增展覽廳
                System.Diagnostics.Debug.WriteLine("has dispatch.");
                if (!itemNumCheck)
                {
                    System.Diagnostics.Debug.WriteLine("number of rooms is same");
                    List<itemInspectionDispatchDetail> itemDispatchDetail = new List<itemInspectionDispatchDetail>();
                    var itemDispatchs = this._ItemInspectionDispatchService.GetAllByDate(date);
                    foreach (var itemDispatch in itemDispatchs)
                    {
                        itemDispatchDetail.Add(itemDispatch);
                    }

                    List<userListForInspectionViweModel> userList = GetUserListForInspection();
                    return View(Data2ItemViewModel(itemDispatchDetail, userList, null, date));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("number of rooms is different");
                    //找出還未建檔的展覽廳
                    items = this._ExhibitionItemService.GetUndispatchItem(date);
                    IResult result = this._ItemInspectionDispatchService.Create(date, items);
                    if (result.Success == false)
                    {
                        System.Diagnostics.Debug.WriteLine("insert error");
                        //result.ErrorMsg = "Dispatch List construct error";
                        System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
                        return View(Data2ItemViewModel(null, null, result,date));
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("insert success");
                        List<itemInspectionDispatchDetail> itemDispatchDetail = new List<itemInspectionDispatchDetail>();
                        var itemDispatchs = this._ItemInspectionDispatchService.GetAllByDate(date);
                        foreach (var itemDispatch in itemDispatchs)
                        {
                            itemDispatchDetail.Add(itemDispatch);
                        }
                        List<userListForInspectionViweModel> userList = GetUserListForInspection();
                        return View(Data2ItemViewModel(itemDispatchDetail, userList, result, date));
                    }
                }

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("has not dispatch yet");
                IResult result = this._ItemInspectionDispatchService.Create(date, items);
                if (result.Success == false)
                {
                    System.Diagnostics.Debug.WriteLine("insert error");
                    System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
                    //result.ErrorMsg = "Dispatch List construct error";
                    return View(Data2ItemViewModel(null, null, result, date));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("insert success");
                    List<itemInspectionDispatchDetail> itemDispatchDetail = new List<itemInspectionDispatchDetail>();
                    var itemDispatchs = this._ItemInspectionDispatchService.GetAllByDate(date);
                    foreach (var itemDispatch in itemDispatchs)
                    {
                       itemDispatchDetail.Add(itemDispatch);
                    }
                    List<userListForInspectionViweModel> userList = GetUserListForInspection();
                    return View(Data2ItemViewModel(itemDispatchDetail, userList, result, date));
                }
            }
        }

        public ActionResult UpdateItemInspectionDispatch(inspectionDispatchJson dispatchJson)
        {
            var itemDispatchId = dispatchJson.pk;
            var itemDispatch = this._ItemInspectionDispatchService.GetById(itemDispatchId);
            if (itemDispatch != null && ModelState.IsValid)
            {
                IResult result = this._ItemInspectionDispatchService.Update(itemDispatch, dispatchJson.name, dispatchJson.value);
                itemDispatch = this._ItemInspectionDispatchService.GetById(itemDispatchId);
                if (result.Success)
                {
                    string lastUpdateTime = itemDispatch.lastUpdateTime.ToString();
                    return Json(new { result = 1, itemDispatchId = itemDispatchId });
                }
                else
                {
                    return Json(new { result = 0, itemDispatchId = itemDispatchId });
                }
            }
            else
            {
                return Json(new { result = 0, itemDispatchId = itemDispatchId });
            }
        }

        private ListItemInspectionDispatchViewModel Data2ItemViewModel(List<itemInspectionDispatchDetail> dispatchList, List<userListForInspectionViweModel> userList, IResult result, System.DateTime checkDate)
        {
            ListItemInspectionDispatchViewModel viewModel = new ListItemInspectionDispatchViewModel();
            viewModel.itemInspectionDispatch = dispatchList;
            viewModel.userList = userList;
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

        public ActionResult ListNonInspectionDispatchDate()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddNonInspectionDispatchDate()
        {
            var TotalViewModel = new List<NoCheckDateViewModel>();
            var noCheckDates = this._noCheckDateService.GetAll().ToList();

            foreach (var item in noCheckDates)
            {
                NoCheckDateViewModel noCheckDateViewModel = this.Data2NoChekDateViewModel(item);
                TotalViewModel.Add(noCheckDateViewModel);
            }
            return View(TotalViewModel);
        }

        [HttpPost]
        public ActionResult AddNonInspectionDispatchDate(nonInspectionDispatchJson noCheckDateJson)
        {
            System.DateTime noCheckDate = Convert.ToDateTime(noCheckDateJson.nonCheckDate);
            string description = noCheckDateJson.description;
            bool type1 = noCheckDateJson.morning;
            bool type2 = noCheckDateJson.afternoon;
            if (type1)
            {
                IResult result = this._noCheckDateService.Create(noCheckDate, description, Convert.ToInt16(1));
                System.Diagnostics.Debug.WriteLine("msg: " + result.ErrorMsg);
                if (result.Success == false)
                {
                    NoCheckDateViewModel vm = new NoCheckDateViewModel();
                    vm.ErrorMsg = result.ErrorMsg;
                    return View(vm);
                }
            }

            if (type2)
            {
                IResult result = this._noCheckDateService.Create(noCheckDate, description, Convert.ToInt16(2));
                System.Diagnostics.Debug.WriteLine("msg: " + result.ErrorMsg);
                if (result.Success == false)
                {
                    NoCheckDateViewModel vm = new NoCheckDateViewModel();
                    vm.ErrorMsg = result.ErrorMsg;
                    return View(vm);
                }
            }
            var TotalViewModel = new List<NoCheckDateViewModel>();
            var noCheckDates = this._noCheckDateService.GetAll().ToList();
            foreach (var item in noCheckDates)
            {
                NoCheckDateViewModel noCheckDateViewModel = this.Data2NoChekDateViewModel(item);
                TotalViewModel.Add(noCheckDateViewModel);
            }
            return View(TotalViewModel);
        }

        private NoCheckDateViewModel Data2NoChekDateViewModel(noCheckDate noCheckDate)
        {
            NoCheckDateViewModel viewModel = new NoCheckDateViewModel();
            viewModel.id = noCheckDate.id;
            viewModel.description = noCheckDate.description;
            viewModel.checkTimeType = noCheckDate.checkTimeType;
            viewModel.setupUserId = noCheckDate.setupUserId;
            viewModel.isDelete = noCheckDate.isDelete;
            viewModel.createTime = noCheckDate.createTime;
            viewModel.lastUpdateTime = noCheckDate.lastUpdateTime;

            return viewModel;
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