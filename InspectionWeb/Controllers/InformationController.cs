using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, User = true)]
    public class InformationController : Controller
    {
        private IFieldMapService _fieldMapService;
        private IExhibitionRoomService _exhibitionRoomService;
        private IUserService _userService;
        private IExhibitionItemService _exhibitionItemService;

        public InformationController(IFieldMapService fieldMapService,
                                     IExhibitionRoomService exhibitionRoomService,
                                     IUserService userService,
                                     IExhibitionItemService exhibitionItemService)
        {
            this._fieldMapService = fieldMapService;
            this._exhibitionRoomService = exhibitionRoomService;
            this._userService = userService;
            this._exhibitionItemService = exhibitionItemService;

            //TODO: add company service
        }

        // GET: Field
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Information/AddField
        public ActionResult AddField()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddField(FormCollection fc)
        {
            string fieldName = fc["fieldName"];
            IResult result = this._fieldMapService.Create(fieldName);
            FieldAddViewModel vm = new FieldAddViewModel();
            vm.FieldId = result.Message;
            vm.FieldName = fieldName;
            vm.ErrorMsg = result.ErrorMsg;

            if (result.Success == false)
            {
                return View("AddField",vm);
            }

             return RedirectToAction("EditField", new { id = vm.FieldId});

        }

        // GET: /Information/EditField/fieldId
        public ActionResult EditField(string id)
        {
            string fieldId = id;
            fieldMap field = this._fieldMapService.GetById(fieldId);
            FieldAddViewModel vm = new FieldAddViewModel();

            if(field == null)
            {
                return RedirectToAction("ListField");
            }

            vm.FieldId = fieldId;
            vm.FieldName = field.fieldName;
            vm.Description = field.description;
            vm.MapFileName = field.mapFileName;
            vm.Photo = field.photo;
            vm.Version = field.version;
            vm.CreateTime = field.createTime.Value;
            vm.LastUpdateTime = field.lastUpdateTime.Value;


            return View(vm);
        }

        [HttpPost]
        public ActionResult EditField(FormCollection fc)
        {
            string fieldId = fc["pk"];
            fieldMap field = this._fieldMapService.GetById(fieldId);
            if(field != null && ModelState.IsValid)
            {
                field.GetType().GetProperty(fc["name"]).SetValue(field, fc["value"], null);
                IResult result = this._fieldMapService.Update(field);
                if (result.Success)
                {
                    return Json(new { lastUpdateTime=field.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    return RedirectToAction("EditField");
                }
                
            }
            else{
                    return RedirectToAction("ListField");
            }
        }

        [HttpPost]
        public ActionResult UpdateFieldPhoto(HttpPostedFileBase upload, string fieldId)
        {
            if(upload.ContentLength > 0)
            {
                string fileName = fieldId;
                fileName = fileName + Path.GetExtension(upload.FileName);
                string savePath = System.IO.Path.Combine(Server.MapPath("~/media/field"), fileName);
                upload.SaveAs(savePath);

                fieldMap field = _fieldMapService.GetById(fieldId);
                field.photo = fileName;
                IResult result = _fieldMapService.Update(field);
                if (result.Success)
                {
                    return Json(new { lastUpdateTime = field.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                      photoName = fileName});
                }
            }
            return Json(null);
        }


        public ActionResult ListField()
        {
            List<FieldListViewModel> vms = new List<FieldListViewModel>();
            List <fieldMap> allFieldMaps = this._fieldMapService.GetAll().ToList();

            foreach(var field in allFieldMaps)
            {
                FieldListViewModel vm = new FieldListViewModel();
                vm.FieldId = field.fieldId;
                vm.FieldName = field.fieldName;
                vm.Version = field.version;
                vm.CreateTime = field.createTime.Value;
                vm.LastUpdateTime = field.lastUpdateTime.Value;
                vms.Add(vm);
                   
            }
            return View(vms);
        }

        public ActionResult DeleteField(string fieldId)
        {
            if (string.IsNullOrEmpty(fieldId))
            {
                return RedirectToAction("ListField");
            }


            fieldMap field = this._fieldMapService.GetById(fieldId);
            if(field == null)
            {
                return RedirectToAction("ListField");
            }

            field.isDelete = 1;
            try
            {
                this._fieldMapService.Update(field);
            }
            catch (Exception)
            {
                return RedirectToAction("ListField");
            }

            return RedirectToAction("ListField");
        }


        //GET:　/Information/AddExhibition
        public ActionResult AddExhibition()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddExhibition(FormCollection fc)
        {
            string roomName = fc["roomName"];
            IResult result = this._exhibitionRoomService.Create(roomName);
            ExhibitionRoomAddViewModel vm = new ExhibitionRoomAddViewModel();
            vm.RoomId = result.Message;
            vm.RoomName = roomName;
            vm.ErrorMsg = result.ErrorMsg;

            if (result.Success == false) {
                return View("AddExhibition", vm);
            }
            return RedirectToAction("EditExhibition", new { id = vm.RoomId });
        }

        //GET: /Information/EditExhibition
        public ActionResult EditExhibition(string id)
        {
            string roomId = id;
            exhibitionRoom room = this._exhibitionRoomService.GetById(roomId);
            ExhibitionRoomAddViewModel vm = new ExhibitionRoomAddViewModel();

            if (room == null)
            {
                return RedirectToAction("ListExhibition");
            }

            // setting viewModel {{{
            vm.RoomId = roomId;
            vm.RoomName = room.roomName;
            vm.Description = room.description;
            vm.Floor = room.floor;
            vm.Picture = room.picture;
            vm.Active = (int) room.active;
            vm.FieldId = room.fieldId;
            vm.Inspector = this._userService.GetByID(room.inspectionUserId);
            //TODO company field 

            vm.X = room.x;
            vm.Y = room.y;
            vm.Width = room.width;
            vm.Height = room.height;
            vm.CreateTime = room.createTime;
            vm.LastUpdateTime = room.lastUpdateTime;

            vm.ExhibitionItems = this._exhibitionItemService.GetAll().Where(x => x.roomId == roomId).ToList();
            vm.Fields = this._fieldMapService.GetAll().ToList();
            vm.Inspectors = this._userService.GetAll().ToList();
            // }}}

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditExhibition(FormCollection fc) {
            string roomId = fc["pk"];
            exhibitionRoom room = this._exhibitionRoomService.GetById(roomId);
            if (room != null && ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("name" + fc["name"] + ",value" + fc["value"]);
                // 處理active的int16型態,其餘皆為字串
                if (fc["name"] == "active")
                {
                    room.GetType().GetProperty(fc["name"]).SetValue(room, Convert.ToInt16(fc["value"]), null);
                }
                else
                {
                    room.GetType().GetProperty(fc["name"]).SetValue(room, fc["value"], null);
                }

                IResult result = this._exhibitionRoomService.Update(room);
                if (result.Success)
                {
                    return Json(new { lastUpdateTime = room.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    return RedirectToAction("EditExhibition");
                }

            }
            else
            {
                return RedirectToAction("ListExhibition");
            }
            return View();
        }

        [HttpPost]
        public ActionResult UpdateExhibitionPhoto(HttpPostedFileBase upload, string roomId)
        {
            if (upload.ContentLength > 0)
            {
                string fileName = roomId;
                fileName = fileName + Path.GetExtension(upload.FileName);
                string savePath = System.IO.Path.Combine(Server.MapPath("~/media/exhibition"), fileName);
                upload.SaveAs(savePath);

                exhibitionRoom room = _exhibitionRoomService.GetById(roomId);
                room.picture = fileName;
                IResult result = _exhibitionRoomService.Update(room);
                if (result.Success)
                {
                    return Json(new
                    {
                        lastUpdateTime = room.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        photoName = fileName
                    });
                }
            }
            return Json(null);
        }


        //GET: /Information/ListExhibition
        public ActionResult ListExhibition()
        {
            List<ExhibitionRoomListViewModel> vms = new List<ExhibitionRoomListViewModel>();
            List<exhibitionRoom> allExhibitionRoom = this._exhibitionRoomService.GetAll().ToList();

            foreach (var room in allExhibitionRoom)
            {
                ExhibitionRoomListViewModel vm = new ExhibitionRoomListViewModel();
                vm.roomId = room.roomId;
                vm.roomName = room.roomName;
                vm.floor = room.floor;
                user inspector = this._userService.GetByID(room.inspectionUserId);
                if(inspector == null)
                {
                    vm.InspectorName = string.Empty;
                }

                vms.Add(vm);

            }
            return View(vms);
        }

        //GET: /Information/EditExhibitItem
        public ActionResult EditExhibitItem()
        {
            return View();
        }

        public ActionResult DeleteExhibitionRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                return RedirectToAction("ListExhibition");
            }

            exhibitionRoom room = this._exhibitionRoomService.GetById(roomId);
            if(room == null)
            {
                return RedirectToAction("ListExhibition");
            }

            room.isDelete = 1;

            try
            {
                this._exhibitionRoomService.Update(room);
            }
            catch (Exception)
            {
                return RedirectToAction("ListExhibition");
            }

            return RedirectToAction("ListExhibition");
        }


        public ActionResult AddExhibitItem(string id)
        {

            string roomId = id;
            exhibitionItem item = new exhibitionItem();
            item.roomId = roomId;
            item.itemType = 0; // 0為一般展項, 1為體驗設施
            item.fieldId = _exhibitionRoomService.GetById(roomId).fieldId;
            IResult result = this._exhibitionItemService.Create(item);
            string itemId = result.Message;

            if (result.Success == false)
            {
                return RedirectToAction("EditExhibition", new { id = roomId });
            }


            return RedirectToAction("EditExhibitItem", new { id = itemId });

        }
        //GET: /Information/AddDevice
        public ActionResult AddDevice()
        {
            return View();
        }

        //GET: /Information/EditDevice
        public ActionResult EditDevice()
        {
            return View();
        }

        //GET: /Information/ListDevice
        public ActionResult ListDevice()
        {
            return View();
        }

        //GET: /Information/AddNotifyDevice
        public ActionResult AddNotifyDevice()
        {
            return View();
        }

        //GET: /Information/EditNotifyDevice
        public ActionResult EditNotifyDevice()
        {
            return View();
        }

        //GET: /Information/ListNotifyDevice
        public ActionResult ListNotifyDevice()
        {
            return View();
        }
    }
}