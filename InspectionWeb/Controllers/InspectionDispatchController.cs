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

        public InspectionDispatchController(IRoomInspectionDispatchService roomInpectionDispatchService, 
                                            IItemInspectionDispatchService itemInspectionDispatchService, 
                                            IExhibitionRoomService exhibitionRoomService, 
                                            IExhibitionItemService exhibitionItemService,
                                            IUserService userService)
        {
            this._RoomInspectionDispatchService = roomInpectionDispatchService;
            this._ItemInspectionDispatchService = itemInspectionDispatchService;
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
            //System.Diagnostics.Debug.WriteLine("########date: " + date);
            bool isExist = this._RoomInspectionDispatchService.IsExists(date);
            bool roomNumCheck = this._RoomInspectionDispatchService.checkRoomInsert();
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

                    List<userListForInspectionViweModel> userList = getUserListForInspection();
                    return View(data2ViewModel(roomDispatchDetail, userList, null));
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
                        return View(data2ViewModel(null, null, result));
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
                        List<userListForInspectionViweModel> userList = getUserListForInspection();
                        return View(data2ViewModel(roomDispatchDetail, userList, result));
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
                    return View(data2ViewModel(null, null, result));
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
                    List<userListForInspectionViweModel> userList = getUserListForInspection();
                    return View(data2ViewModel(roomDispatchDetail, userList, result));
                }
            }
        }

        public ActionResult updateRoomInspectionDispatch(inspectionDispatchJson dispatchJson)
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
                    return Json(new { result = 1, roomDispatchId = roomDispatchId, lastUpdateTime = lastUpdateTime });
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

        private ListRoomInspectionDispatchViewModel data2ViewModel(List<roomInspectionDispatchDetail> dispatchList, List<userListForInspectionViweModel> userList, IResult result)
        {
            ListRoomInspectionDispatchViewModel viewModel = new ListRoomInspectionDispatchViewModel();
            viewModel.roomInspectionDispatch = dispatchList;
            viewModel.userList = userList;
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

        private List<userListForInspectionViweModel> getUserListForInspection()
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
        public ActionResult ListItemInspectionDispatch(inpectionDateJson inpectionTime)
        {
            return View();
        }

        public ActionResult updateItemInspectionDispatch(inspectionDispatchJson dispatchJson)
        {
            var itemDispatchId = dispatchJson.pk;
            var itemDispatch = this._RoomInspectionDispatchService.GetById(itemDispatchId);
            if (itemDispatch != null && ModelState.IsValid)
            {
                IResult result = this._RoomInspectionDispatchService.Update(itemDispatch, dispatchJson.name, dispatchJson.value);
                itemDispatch = this._RoomInspectionDispatchService.GetById(itemDispatchId);
                if (result.Success)
                {
                    string lastUpdateTime = itemDispatch.lastUpdateTime.ToString();
                    return Json(new { result = 1, itemDispatchId = itemDispatchId, lastUpdateTime = lastUpdateTime });
                }
                else
                {
                    return Json(new { result = 0, itemDispatchId = itemDispatchId });
                }
            }
            else
            {
                return Json(new { result = 0, itemDispatchId = itemDispatchId });
                //return RedirectToAction("ListReportSource");
            }
        }

        public ActionResult ListNonInspectionDispatchDate()
        {
            return View();
        }

        public ActionResult AddNonInspectionDispatchDate()
        {
            return View();
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