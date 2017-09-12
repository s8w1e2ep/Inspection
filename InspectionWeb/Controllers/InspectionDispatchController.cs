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
    [AuthorizeUser(Super = true, Manager = true, Dispatch = true)]
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

        public class DeleteInspectionDispatchJson
        {
            public string dispatchId { get; set; }
            public string date { get; set; }
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
            //確認是不是非巡檢日
            IResult nonCkeck = new Result(false);
            int isNonCheckDate = this._noCheckDateService.IsExists(date);
            if (isNonCheckDate == 1)
            {
                nonCkeck.ErrorMsg = "此日非巡檢日";
                return View(Data2RoomViewModel(null, null, nonCkeck, date));
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

        public ActionResult DeleteRoomInspectionDispatch(DeleteInspectionDispatchJson deleteJson)
        {
            string dispatchId = deleteJson.dispatchId;

            if (string.IsNullOrEmpty(dispatchId))
            {
                return Json(new { result = 0 });
            }

            var roomInspectionDispatch = this._RoomInspectionDispatchService.GetById(dispatchId);

            if (roomInspectionDispatch == null)
            {
                return Json(new { result = 0 });
            }

            try
            {
                this._RoomInspectionDispatchService.Delete(dispatchId);
            }
            catch (Exception)
            {
                return Json(new { result = 0 });
            }

            return Json(new { result = 1 });
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
            //確認是不是非巡檢日
            IResult nonCkeck = new Result(false);
            int isNonCheckDate = this._noCheckDateService.IsExists(date);
            if (isNonCheckDate == 1)
            {
                nonCkeck.ErrorMsg = "此日非巡檢日";
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
                    return View(Data2ItemViewModel(itemDispatchDetail, userList, nonCkeck, date));
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
                        return View(Data2ItemViewModel(itemDispatchDetail, userList, nonCkeck, date));
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
                    return View(Data2ItemViewModel(itemDispatchDetail, userList, nonCkeck, date));
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

        public ActionResult DeleteItemInspectionDispatch(DeleteInspectionDispatchJson deleteJson)
        {
            string dispatchId = deleteJson.dispatchId;
            if (string.IsNullOrEmpty(dispatchId))
            {
                return Json(new { result = 0 });
            }

            var itemInspectionDispatch = this._ItemInspectionDispatchService.GetById(dispatchId);

            if (itemInspectionDispatch == null)
            {
                return Json(new { result = 0 });
            }

            try
            {
                this._ItemInspectionDispatchService.Delete(dispatchId);
            }
            catch (Exception)
            {
                return Json(new { result = 0 });
            }

            return Json(new { result = 1 });
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

        [HttpGet]
        public ActionResult ListNonInspectionDispatchDate()
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
        public ActionResult ListNonInspectionDispatchDate(string nonCheckDateStart, string nonCheckDateEnd)
        {
            System.DateTime start = Convert.ToDateTime(nonCheckDateStart);
            System.DateTime end = Convert.ToDateTime(nonCheckDateEnd);
            var TotalViewModel = new List<NoCheckDateViewModel>();
            var noCheckDates = this._noCheckDateService.GetAllWithTimeInterval(start, end).ToList();
            foreach (var item in noCheckDates)
            {
                NoCheckDateViewModel noCheckDateViewModel = this.Data2NoChekDateViewModel(item);
                TotalViewModel.Add(noCheckDateViewModel);
            }
            return View(TotalViewModel);
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
        public ActionResult AddNonInspectionDispatchDate(string nonCheckDate, bool morning, bool afternoon, string description)
        {
            System.DateTime noCheckDate = Convert.ToDateTime(nonCheckDate);
            IResult result = this._noCheckDateService.Create(noCheckDate, description, morning, afternoon);
            System.Diagnostics.Debug.WriteLine("msg: " + result.ErrorMsg);
            if (result.Success == false)
            {
                NoCheckDateViewModel vm = new NoCheckDateViewModel();
                vm.ErrorMsg = result.ErrorMsg;
                return View(vm);
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
            viewModel.noCheckDate = noCheckDate.noCheckDate1.Value.ToString("d");
            viewModel.description = noCheckDate.description;
            viewModel.am = noCheckDate.am;
            viewModel.pm = noCheckDate.pm;
            viewModel.setupUserId = noCheckDate.setupUserId;
            viewModel.isDelete = noCheckDate.isDelete;
            viewModel.createTime = noCheckDate.createTime;
            viewModel.lastUpdateTime = noCheckDate.lastUpdateTime;

            return viewModel;
        }

        public ActionResult DeleteNonCheckDateDispatch(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { result = 0 });
            }

            var noCheckDate = this._noCheckDateService.GetById(id);

            if (noCheckDate == null)
            {
                return Json(new { result = 0 });
            }

            try
            {
                this._noCheckDateService.Delete(id);
            }
            catch (Exception)
            {
                return Json(new { result = 0 });
            }

            return Json(new { result = 1 });
        }

        [HttpGet]
        public ActionResult QueryInspectionByRoom()
        {
            IEnumerable<exhibitionRoom> rooms = this._ExhibitionRoomService.GetAllWithoutIsDelete();
            return View(Data2QueryByRoomViewModel(null,rooms,null,null));
        }

        [HttpPost]
        public ActionResult QueryInspectionByRoom(string queryDateStart, string queryDateEnd, List<string> roomId)
        {
            IEnumerable<roomInspectionDispatchDetail> dispatchList = this._RoomInspectionDispatchService.GetAllByRoomCondition(queryDateStart, queryDateEnd, roomId);
            IEnumerable<exhibitionRoom> rooms = this._ExhibitionRoomService.GetAllWithoutIsDelete();
            return View(Data2QueryByRoomViewModel(dispatchList,rooms,queryDateStart,queryDateEnd));
        }

        private ListQueryInspectionByRoomViewModel Data2QueryByRoomViewModel(IEnumerable<roomInspectionDispatchDetail> dispatchList, IEnumerable<exhibitionRoom> rooms, string startDate, string endDate)
        {
            ListQueryInspectionByRoomViewModel viewModel = new ListQueryInspectionByRoomViewModel();
            viewModel.roomInspectionDispatch = dispatchList;
            List<exhibitionRoomList> room = new List<exhibitionRoomList>();
            foreach (var item in rooms)
            {
                exhibitionRoomList x = new exhibitionRoomList();
                x.roomId = item.roomId;
                x.roomName = item.roomName;
                x.inspectionUserId = item.inspectionUserId;
                room.Add(x);
            }
            viewModel.room = room;
            viewModel.startDate = startDate;
            viewModel.endDate = endDate;
            return viewModel;
        }

        [HttpGet]
        public ActionResult QueryInspectionByItem()
        {
            IEnumerable<exhibitionItem> items = this._ExhibitionItemService.GetAllWithoutIsDelete();
            return View(Data2QueryByItemViewModel(null, items, null, null));
        }

        [HttpPost]
        public ActionResult QueryInspectionByItem(string queryDateStart, string queryDateEnd, List<string> itemId)
        {
            IEnumerable<itemInspectionDispatchDetail> dispatchList = this._ItemInspectionDispatchService.GetAllByItemCondition(queryDateStart,queryDateEnd,itemId);
            IEnumerable<exhibitionItem> items = this._ExhibitionItemService.GetAllWithoutIsDelete();
            return View(Data2QueryByItemViewModel(dispatchList,items,queryDateStart,queryDateEnd));
        }

        private ListQueryInspectionByItemViewModel Data2QueryByItemViewModel(IEnumerable<itemInspectionDispatchDetail> dispatchList, IEnumerable<exhibitionItem> items, string startDate, string endDate)
        {
            ListQueryInspectionByItemViewModel viewModel = new ListQueryInspectionByItemViewModel();
            viewModel.itemInspectionDispatch = dispatchList;
            List<exhibitionItemList> item = new List<exhibitionItemList>();
            foreach (var i in items)
            {
                exhibitionItemList x = new exhibitionItemList();
                x.itemId = i.itemId;
                x.itemName = i.itemName;
                x.inspectionUserId = "";
                item.Add(x);
            }
            viewModel.item = item;
            viewModel.startDate = startDate;
            viewModel.endDate = endDate;
            return viewModel;
        }


        [HttpGet]
        public ActionResult QueryInspectionByUserId()
        {
            IEnumerable<user> users = this._UserService.GetAll();
            return View(Data2QueryByUserIdViewModel(null,null,users,null,null));
        }

        [HttpPost]
        public ActionResult QueryInspectionByUserId(string queryDateStart, string queryDateEnd, List<string> userId)
        {
            IEnumerable<roomInspectionDispatchDetail> roomDispatchList = this._RoomInspectionDispatchService.GetAllByUserCondition(queryDateStart, queryDateEnd, userId);
            IEnumerable<itemInspectionDispatchDetail> itemDispatchList = this._ItemInspectionDispatchService.GetAllByUserCondition(queryDateStart, queryDateEnd, userId);
            IEnumerable<user> users = this._UserService.GetAll();
            return View(Data2QueryByUserIdViewModel(itemDispatchList,roomDispatchList,users,queryDateStart,queryDateEnd));
        }

        private ListQueryInspectionByUserIdViewModel Data2QueryByUserIdViewModel(IEnumerable<itemInspectionDispatchDetail> itemDispatchList, IEnumerable<roomInspectionDispatchDetail> roomDispatchList, IEnumerable<user>users , string startDate, string endDate)
        {
            ListQueryInspectionByUserIdViewModel viewModel = new ListQueryInspectionByUserIdViewModel();
            viewModel.itemInspectionDispatch = itemDispatchList;
            viewModel.roomInspectionDispatch = roomDispatchList;
            viewModel.users = users;
            viewModel.startDate = startDate;
            viewModel.endDate = endDate;
            return viewModel;
        }

        [HttpGet]
        public ActionResult QueryInspectionByDate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QueryInspectionByDate(string dispatchDate)
        {
            System.DateTime date = Convert.ToDateTime(dispatchDate);
            IEnumerable<itemInspectionDispatchDetail> itemDispatchList = this._ItemInspectionDispatchService.GetAllByDate(date);
            IEnumerable<roomInspectionDispatchDetail> roomDispatchList = this._RoomInspectionDispatchService.GetAllByDate(date);
            return View(Data2QueryByDateViewModel(itemDispatchList, roomDispatchList , dispatchDate));
        }

        private ListQueryInspectionByDateViewModel Data2QueryByDateViewModel(IEnumerable<itemInspectionDispatchDetail> itemDispatchList, IEnumerable<roomInspectionDispatchDetail> roomDispatchList, string date)
        {
            ListQueryInspectionByDateViewModel viewModel = new ListQueryInspectionByDateViewModel();
            viewModel.itemInspectionDispatch = itemDispatchList;
            viewModel.roomInspectionDispatch = roomDispatchList;
            viewModel.checkDate = date;
            return viewModel;
        }
    }
}