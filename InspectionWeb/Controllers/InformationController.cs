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
        private IReportDeviceService _reportDeviceService;
        private IReportSourceService _reportSourceService;
        private ICompanyService _companyService;
        private IRoomActiveRecordService _roomActiveRecordService;
        public InformationController(IFieldMapService fieldMapService,
                                     IExhibitionRoomService exhibitionRoomService,
                                     IUserService userService,
                                     IExhibitionItemService exhibitionItemService,
                                     IReportDeviceService reportDeviceService,
                                     IReportSourceService reportSourceService,
                                     ICompanyService companyService,
                                     IRoomActiveRecordService roomActiveRecordService)

        {
            this._fieldMapService = fieldMapService;
            this._exhibitionRoomService = exhibitionRoomService;
            this._userService = userService;
            this._exhibitionItemService = exhibitionItemService;
            this._reportDeviceService = reportDeviceService;
            this._reportSourceService = reportSourceService;
            this._companyService = companyService;
            this._roomActiveRecordService = roomActiveRecordService;

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
                return View("AddField", vm);
            }

            return RedirectToAction("EditField", new { id = vm.FieldId });

        }

        // GET: /Information/EditField/fieldId
        public ActionResult EditField(string id)
        {
            string fieldId = id;
            fieldMap field = this._fieldMapService.GetById(fieldId);
            FieldAddViewModel vm = new FieldAddViewModel();

            if (field == null)
            {
                return RedirectToAction("ListField");
            }

            vm.FieldId = fieldId;
            vm.FieldName = field.fieldName;
            vm.Description = field.description;
            vm.Floor = field.floor;
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
            if (field != null && ModelState.IsValid)
            {
                field.GetType().GetProperty(fc["name"]).SetValue(field, fc["value"], null);
                IResult result = this._fieldMapService.Update(field);
                if (result.Success)
                {
                    return Json(new { lastUpdateTime = field.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    return RedirectToAction("EditField");
                }

            }
            else
            {
                return RedirectToAction("ListField");
            }
        }

        [HttpPost]
        public ActionResult UpdateFieldPhoto(HttpPostedFileBase upload, string fieldId)
        {
            if (upload.ContentLength > 0)
            {
                string fileName = fieldId;
                fileName = fileName + Path.GetExtension(upload.FileName);
                string folder = Server.MapPath("~/media/field");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string savePath = System.IO.Path.Combine(folder, fileName);
                upload.SaveAs(savePath);

                fieldMap field = _fieldMapService.GetById(fieldId);
                field.photo = fileName;
                IResult result = _fieldMapService.Update(field);
                if (result.Success)
                {
                    return Json(new
                    {
                        lastUpdateTime = field.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        photoName = fileName
                    });
                }
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult UpdateFieldMap(HttpPostedFileBase upload, string fieldId)
        {
            if (upload.ContentLength > 0)
            {
                try
                {
                    //var fileName = System.IO.Path.GetFileName(upload.FileName);
                    var fileName = fieldId + ".svg";
                    string folder = Server.MapPath("~/media/map");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    var path = System.IO.Path.Combine(folder, fileName);
                    upload.SaveAs(path);

                    fieldMap field = _fieldMapService.GetById(fieldId);
                    field.mapFileName = fileName;
                    IResult result = _fieldMapService.Update(field);

                    if (result.Success) return Json(new
                    {
                        lastUpdateTime = field.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        mapName = fileName
                    });
                }
                catch (Exception ex)
                { }
                return Json(null);
            }

            else
            {
                return Json(null);
            }
        }

        public ActionResult ListField()
        {
            List<FieldListViewModel> vms = new List<FieldListViewModel>();
            List<fieldMap> allFieldMaps = this._fieldMapService.GetAll().ToList();

            foreach (var field in allFieldMaps)
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
            if (field == null)
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

            if (result.Success == false)
            {
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
            vm.Active = (int)room.active;
            vm.FieldId = room.fieldId;
            vm.Inspector = this._userService.GetByID(room.inspectionUserId);
            vm.Company = _companyService.GetByID(room.companyId);
            vm.MapFileName = "";
            if (!string.IsNullOrEmpty(room.fieldId))
            {
                fieldMap field = this._fieldMapService.GetById(room.fieldId);
                vm.MapFileName = field.mapFileName;
            }
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
            vm.Companys = this._companyService.GetAll().ToList();
            vm.RoomActiveRecords = this._roomActiveRecordService.GetAll().Where(x => x.roomId == roomId).ToList();
            if(_roomActiveRecordService.GetEvery().Where(x=> x.roomId == roomId).Count() == 0)
            {
                roomActiveRecord newRecord = new roomActiveRecord();
                newRecord.active = 1;
                newRecord.roomId = roomId;
                IResult result = this._roomActiveRecordService.Create(newRecord);
                if (result.Success)
                {
                    vm.RoomActiveRecords.Add(newRecord);
                }
            }
            // }}}

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditExhibition(FormCollection fc)
        {
            string roomId = fc["pk"];
            bool activeStateChange = false;
            string recordId = "";
            exhibitionRoom room = this._exhibitionRoomService.GetById(roomId);
            if (room != null && ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("name" + fc["name"] + ",value" + fc["value"]);
                // 處理active的int16型態,其餘皆為字串
                if (fc["name"] == "active")
                {
                    room.GetType().GetProperty(fc["name"]).SetValue(room, Convert.ToInt16(fc["value"]), null);
                    roomActiveRecord record = new roomActiveRecord();
                    record.roomId = roomId;
                    record.active = Convert.ToInt16(fc["value"]);
                    IResult resu = _roomActiveRecordService.Create(record);
                    if (resu.Success)
                    {
                        recordId = resu.Message;
                    }
                    activeStateChange = true;
                    

                }
                else
                {
                    room.GetType().GetProperty(fc["name"]).SetValue(room, fc["value"], null);
                }

                IResult result = this._exhibitionRoomService.Update(room);
                if (result.Success)
                {
                    fieldMap field = this._fieldMapService.GetById(room.fieldId);
                    if (activeStateChange == true)
                    {

                        return Json(new
                        {
                            lastUpdateTime = room.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            recordId = recordId,
                            recordCreateTiime = _roomActiveRecordService.GetById(recordId).createTime

                        });
                    }

                    if (field == null)
                    {
                        return Json(new
                        {
                            lastUpdateTime = room.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            mapFileName = ""
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            lastUpdateTime = room.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            mapFileName = field.mapFileName
                        });
                    }

                    
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
        }

        [HttpPost]
        public ActionResult UpdateExhibitionPhoto(HttpPostedFileBase upload, string roomId)
        {
            if (upload.ContentLength > 0)
            {
                string fileName = roomId;
                fileName = fileName + Path.GetExtension(upload.FileName);
                string folder = Server.MapPath("~/media/exhibition");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string savePath = System.IO.Path.Combine(folder, fileName);
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

        [HttpPost]
        public ActionResult SaveRoomSvgChangeToServer(FormCollection fc)
        {
            string roomId = fc["roomId"];
            exhibitionRoom room = _exhibitionRoomService.GetById(roomId);
            if (room == null)
            {
                return Json(null);
            }
            room.x = Convert.ToInt32(fc["x"]);
            room.y = Convert.ToInt32(fc["y"]);
            room.width = Convert.ToInt32(fc["width"]);
            room.height = Convert.ToInt32(fc["height"]);
            IResult result = _exhibitionRoomService.Update(room);
            if (result.Success)
            {
                return Json(new { lastUpdateTime = room.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
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
                fieldMap field = _fieldMapService.GetById(room.fieldId);
                user inspector = _userService.GetByID(room.inspectionUserId);
                if (field != null)
                {
                    vm.fieldName = field.fieldName;
                }

                if (inspector != null)
                {
                    vm.InspectorName = inspector.userName;
                }

                vms.Add(vm);

            }
            return View(vms);
        }

        public ActionResult DeleteExhibitionRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                return RedirectToAction("ListExhibition");
            }

            exhibitionRoom room = this._exhibitionRoomService.GetById(roomId);
            if (room == null)
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

        //GET: /Information/ListExhibition
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

        //GET: /Information/EditExhibitItem/itemId
        public ActionResult EditExhibitItem(string id)
        {
            string itemId = id;
            exhibitionItem item = _exhibitionItemService.GetById(itemId);
            ExhibitionItemEditViewModel vm = new ExhibitionItemEditViewModel();

            if (item != null && ModelState.IsValid)
            {
                vm.ItemId = item.itemId;
                vm.RoomId = item.roomId;
                vm.FieldId = item.fieldId;

                vm.ItemCode = item.itemCode;
                vm.ItemName = item.itemName;
                vm.ItemType = item.itemType;
                vm.Description = item.description;
                vm.Picture = item.picture;
                vm.Company = _companyService.GetByID(item.companyId);
                vm.X = item.x;
                vm.Y = item.y;
                vm.Active = item.active;
                vm.IsLock = item.isLock;
                vm.PeriodReportTime = item.periodReportTime;
                vm.CreateTime = item.createTime;
                vm.LastUpdateTime = item.lastUpdateTime;
                vm.Companys = _companyService.GetAll().ToList();
            }
            return View(vm);

        }

        [HttpPost]
        public ActionResult EditExhibitItem(FormCollection fc)
        {
            string itemId = fc["pk"];
            exhibitionItem item = _exhibitionItemService.GetById(itemId);
            if (item != null && ModelState.IsValid)
            {
                switch (fc["name"])
                {
                    case "itemCode":
                        item.itemCode = fc["value"];
                        break;
                    case "itemName":
                        item.itemName = fc["value"];
                        break;
                    case "description":
                        item.description = fc["value"];
                        break;
                    case "companyId":
                        item.companyId = fc["value"];
                        break;
                    case "active":
                        item.active = Convert.ToInt16(fc["value"]);
                        break;
                    case "isLock":
                        item.isLock = Convert.ToByte(fc["value"]);
                        break;
                    case "periodReportTime":
                        item.periodReportTime = Convert.ToInt32(fc["value"]);
                        break;
                    default:
                        break;
                }

                IResult result = this._exhibitionItemService.Update(item);
                if (result.Success)
                {
                    return Json(new { lastUpdateTime = item.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    return RedirectToAction("EditExhibitionItem");
                }

            }
            else
            {
                return RedirectToAction("EditExhibition");
            }
        }
        public ActionResult DeleteExhibitionItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return RedirectToAction("EditExhibition");
            }


            exhibitionItem item = this._exhibitionItemService.GetById(itemId);
            if (item == null)
            {
                return RedirectToAction("EditExhibition");
            }

            item.isDelete = 1;
            try
            {
                this._exhibitionItemService.Update(item);
            }
            catch (Exception)
            {
                return RedirectToAction("EditExhibition");
            }

            return RedirectToAction("EditExhibition");
        }

        [HttpPost]
        public ActionResult UpdateExhibitionItemPhoto(HttpPostedFileBase upload, string itemId)
        {
            if (upload.ContentLength > 0)
            {
                string fileName = itemId;
                fileName = fileName + Path.GetExtension(upload.FileName);
                string folder = Server.MapPath("~/media/exhibitionItem");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string savePath = System.IO.Path.Combine(folder, fileName);
                upload.SaveAs(savePath);

                exhibitionItem item = _exhibitionItemService.GetById(itemId);
                item.picture = fileName;
                IResult result = _exhibitionItemService.Update(item);
                if (result.Success)
                {
                    return Json(new
                    {
                        lastUpdateTime = item.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        photoName = fileName
                    });
                }
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult SaveItemSvgChangeToServer(FormCollection fc)
        {
            string itemId = fc["itemId"];
            exhibitionItem item = _exhibitionItemService.GetById(itemId);
            if (item == null)
            {
                return Json(null);
            }
            item.x = Convert.ToInt32(fc["x"]);
            item.y = Convert.ToInt32(fc["y"]);
            IResult result = _exhibitionItemService.Update(item);
            if (result.Success)
            {
                return Json(new { lastUpdateTime = item.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
            }

            return Json(null);
        }

        [HttpPost]
        public ActionResult AddRoomActiveRecord(FormCollection fc)
        {
            exhibitionRoom room = _exhibitionRoomService.GetById(fc["pk"]);
            room.active = Convert.ToInt16(fc["value"]);
            _exhibitionRoomService.Update(room);

            roomActiveRecord record = new roomActiveRecord();
            record.roomId = fc["pk"];
            record.active = Convert.ToInt16(fc["value"]);
            IResult result = _roomActiveRecordService.Create(record);
            if(result.Success == false)
            {
                return RedirectToAction("EditExhibition", new { id = record.roomId });

            }
            RoomActiveRecordAddViewModel vm = new RoomActiveRecordAddViewModel();
            vm.Active = record.active;
            vm.ActivityId = result.Message;
            vm.Description = record.description;
            vm.CreateTime = record.createTime;
            return Json(new { id = record.roomId });

        }
        public ActionResult EditRoomActiveRecord(string id)
        {

            string recordId = id;
            roomActiveRecord record = _roomActiveRecordService.GetById(recordId);
            string roomName = _exhibitionRoomService.GetById(record.roomId).roomName;

            RoomActiveRecordAddViewModel vm = new RoomActiveRecordAddViewModel();

            if (record != null && ModelState.IsValid)
            {
                vm.ActivityId = recordId;
                vm.Description = record.description;
                vm.Active = record.active;
                vm.RoomName = roomName;
                vm.CreateTime = record.createTime;
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditRoomActiveRecord()
        {

            return Json(null);
        }

        public ActionResult DeleteRecord(string recordId)
        {
            if (string.IsNullOrEmpty(recordId))
            {
                return RedirectToAction("EditExhibition");
            }


            roomActiveRecord record = this._roomActiveRecordService.GetById(recordId);
            if (record == null)
            {
                return RedirectToAction("EditExhibition");
            }

            record.isDelete = 1;
            try
            {
                this._roomActiveRecordService.Update(record);
            }
            catch (Exception)
            {
                return RedirectToAction("EditExhibition");
            }

            return RedirectToAction("EditExhibition");
        }
        //GET: /Information/AddDevice
        public ActionResult AddDevice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDevice(FormCollection fc)
        {
            exhibitionItem item = new exhibitionItem();
            item.itemName = fc["itemName"];
            item.itemCode = fc["itemCode"];
            item.itemType = 1;
            item.roomId = "experience";
            IResult result = _exhibitionItemService.Create(item);
            string itemId = result.Message;
            if (result.Success == false)
            {
                return View("AddDevice");
            }

            return RedirectToAction("EditDevice", new { id = itemId });
        }

        //GET: /Information/EditDevice
        public ActionResult EditDevice(string id)
        {
            string itemId = id;
            if (string.IsNullOrEmpty(itemId))
            {
                RedirectToAction("AddDevice");
            }

            exhibitionItem item = _exhibitionItemService.GetById(itemId);
            DeviceEditViewModel vm = new DeviceEditViewModel();

            if (item == null)
            {
                return RedirectToAction("ListDevice");
            }

            vm.ItemId = item.itemId;
            vm.ItemName = item.itemName;
            vm.IsLock = item.isLock;
            vm.Active = item.active;
            vm.ItemCode = item.itemCode;
            vm.Inspector = _userService.GetByID(item.inspectionUserId);
            vm.CreateTime = item.createTime;
            vm.LastUpdateTime = item.lastUpdateTime;
            vm.X = item.x;
            vm.Y = item.y;
            vm.FieldId = item.fieldId;
            vm.Inspectors = _userService.GetAll().ToList();
            vm.Fields = this._fieldMapService.GetAll().ToList();

            return View(vm);

        }

        [HttpPost]
        public ActionResult EditDevice(FormCollection fc)
        {
            string deviceId = fc["pk"];
            exhibitionItem device = _exhibitionItemService.GetById(deviceId);
            fieldMap field = null;
            if (device != null && ModelState.IsValid)
            {
                switch (fc["name"])
                {
                    case "itemCode":
                        device.itemCode = fc["value"];
                        break;
                    case "itemName":
                        device.itemName = fc["value"];
                        break;
                    case "inspectionUserId":
                        device.inspectionUserId = fc["value"];
                        break;
                    case "active":
                        device.active = Convert.ToInt16(fc["value"]);
                        break;
                    case "isLock":
                        device.isLock = Convert.ToByte(fc["value"]);
                        break;
                    case "fieldId":
                        device.fieldId = fc["value"];
                        field = _fieldMapService.GetById(device.fieldId);
                        break;

                    default:
                        break;
                }

                IResult result = _exhibitionItemService.Update(device);
                if (result.Success)
                {

                    if (field != null)
                    {
                        return Json(new { mapFileName = field.mapFileName, lastUpdateTime = device.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
                    }
                    else {
                        return Json(new {  lastUpdateTime = device.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
                    }
                }
            }
            else
            {
                return RedirectToAction("EditDevice");
            }

            return RedirectToAction("ListDevice");
        }

        //GET: /Information/ListDevice
        public ActionResult ListDevice()
        {
            List<DeviceListViewModel> vms = new List<DeviceListViewModel>();
            List<exhibitionItem> allDevices = _exhibitionItemService.GetAll().Where(x => x.itemType == 1).ToList();
            foreach (var device in allDevices)
            {
                DeviceListViewModel vm = new DeviceListViewModel();
                vm.ItemId = device.itemId;
                vm.ItemName = device.itemName;
                switch (device.active)
                {
                    case 0:
                        vm.Active = "停用";
                        break;
                    case 1:
                        vm.Active = "啟用";
                        break;
                    case 2:
                        vm.Active = "維護中";
                        break;
                    default:
                        vm.Active = "未設定";
                        break;
                }
                vm.CreateTime = device.createTime;
                vm.LastUpdateTime = device.lastUpdateTime;
                vms.Add(vm);
            }

            return View(vms);
        }

        public ActionResult DeleteDevice(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return RedirectToAction("ListDevice");
            }

            exhibitionItem device = this._exhibitionItemService.GetById(deviceId);
            if (device == null)
            {
                return RedirectToAction("ListDevice");
            }

            device.isDelete = 1;

            try
            {
                this._exhibitionItemService.Update(device);
            }
            catch (Exception)
            {
                return RedirectToAction("ListDevice");
            }

            return RedirectToAction("ListDevice");
        }

        [HttpPost]
        public ActionResult SaveDeviceSvgChangeToServer(FormCollection fc)
        {
            string deviceId = fc["deviceId"];
            exhibitionItem item = _exhibitionItemService.GetById(deviceId);
            if (item == null)
            {
                return Json(null);
            }
            item.x = Convert.ToInt32(fc["x"]);
            item.y = Convert.ToInt32(fc["y"]);
            IResult result = _exhibitionItemService.Update(item);
            if (result.Success)
            {
                return Json(new { lastUpdateTime = item.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
            }

            return Json(null);
        }
        //GET: /Information/AddNotifyDevice
        public ActionResult AddNotifyDevice()
        {
            NotifyDeviceAddViewModel vm = new NotifyDeviceAddViewModel();
            vm.sources = _reportSourceService.GetAll().ToList();
            return View(vm);
        }

        [HttpPost]
        public ActionResult AddNotifyDevice(FormCollection fc)
        {
            reportDevice report = new reportDevice();
            report.name = fc["name"];
            report.sourceId = fc["sourceId"];
            report.deviceCode = fc["deviceCode"];


            IResult result = _reportDeviceService.Create(report);
            ReportDeviceEditViewModel vm = new ReportDeviceEditViewModel();
            vm.DeviceId = result.Message;
            vm.Source = _reportSourceService.GetById(report.sourceId);
            vm.DeviceCode = report.deviceCode;
            vm.Name = report.name;

            if (result.Success == false)
            {
                return View("AddNotifyDevice", vm);
            }
            return RedirectToAction("EditNotifyDevice", new { id = vm.DeviceId });
        }



        //GET: /Information/EditNotifyDevice
        public ActionResult EditNotifyDevice(string id)
        {
            string deviceId = id;
            if (string.IsNullOrEmpty(deviceId))
            {
                RedirectToAction("AddNotifyDevice");
            }

            reportDevice device = _reportDeviceService.GetById(deviceId);
            ReportDeviceEditViewModel vm = new ReportDeviceEditViewModel();

            if (device == null)
            {
                return RedirectToAction("ListNotifyDevice");
            }

            vm.Name = device.name;
            vm.DeviceId = device.deviceId;
            vm.DeviceCode = device.deviceCode;
            vm.Description = device.description;
            vm.Source = _reportSourceService.GetById(device.sourceId);
            vm.Photo = device.photo;
            vm.Item = _exhibitionItemService.GetById(device.itemId);
            vm.X = device.x;
            vm.Y = device.y;
            vm.CreateTime = device.createTime;
            vm.LastUpdateTime = device.lastUpdateTime;
            vm.MapFileName = _fieldMapService.GetById(device.fieldId)?.mapFileName;
            vm.ReportSources = _reportSourceService.GetAll().ToList();
            vm.Field = _fieldMapService.GetById(device.fieldId);
            vm.Rooms = _exhibitionRoomService.GetAll().ToList();
            vm.Items = _exhibitionItemService.GetAll().ToList();
            vm.Fields = _fieldMapService.GetAll().ToList();




            return View(vm);

        }

        [HttpPost]
        public ActionResult EditNotifyDevice(FormCollection fc)
        {
            string deviceId = fc["pk"];
            reportDevice device = _reportDeviceService.GetById(deviceId);
            fieldMap field = null;
            System.Diagnostics.Debug.WriteLine("YOYOYOYOYO" + fc["name"]);
            if (device != null && ModelState.IsValid)
            {
                switch (fc["name"])
                {
                    case "name":
                        device.name = fc["value"];
                        break;
                    case "sourceId":
                        device.sourceId = fc["value"];
                        _reportDeviceService.Update(device);
                        break;
                    case "deviceCode":
                        device.deviceCode = fc["value"];
                        break;
                    case "description":
                        device.description = fc["value"];
                        break;
                    case "itemId":
                        device.itemId = fc["value"];
                        _reportDeviceService.Update(device);
                        break;
                    case "roomId":
                        string roomId = fc["value"];
                        List<exhibitionItem> items = _exhibitionItemService.GetAll().Where(x => x.roomId == roomId).ToList();

                        return Json(new { items = items });

                        break;
                    case "fieldId":
                        device.fieldId = fc["value"];
                        field = _fieldMapService.GetById(device.fieldId);
                        List<exhibitionRoom> rooms = _exhibitionRoomService.GetAll().ToList().Where(x => x.fieldId == field.fieldId).ToList();
                        _reportDeviceService.Update(device);
                        return Json(new
                        {
                            lastUpdateTime = device.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            mapFileName = field?.mapFileName,
                            rooms = rooms
                        });
                        break;
                    default:

                        break;
                }

            }
                return RedirectToAction("ListNotifyDevice");
        }

        //GET: /Information/ListNotifyDevice
        public ActionResult ListNotifyDevice()
        {
            List<ReportDeviceListViewModel> vms = new List<ReportDeviceListViewModel>();
            List<reportDevice> reportDevices = _reportDeviceService.GetAll().ToList();
            foreach (var device in reportDevices)
            {
                ReportDeviceListViewModel vm = new ReportDeviceListViewModel();
                vm.DeviceId = device.deviceId;
                vm.DeviceCode = device.deviceCode;
                vm.DeviceName = device.name;
                vm.FieldName = _fieldMapService.GetById(device.fieldId)?.fieldName;
                vm.ItemName = _exhibitionItemService.GetById(device.itemId)?.itemName;
                vm.CreateTime = device.createTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                vm.LastUpdateTime = device.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                vms.Add(vm);
            }

            return View(vms);
        }

        [HttpPost]
        public ActionResult UpdateReportDevicePhoto(HttpPostedFileBase upload, string deviceId)
        {
            if (upload.ContentLength > 0)
            {
                string fileName = deviceId;
                fileName = fileName + Path.GetExtension(upload.FileName);
                string folder = Server.MapPath("~/media/reportDevice");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string savePath = System.IO.Path.Combine(folder, fileName);
                upload.SaveAs(savePath);

                reportDevice device = _reportDeviceService.GetById(deviceId);
                device.photo = fileName;
                IResult result = _reportDeviceService.Update(device);
                if (result.Success)
                {
                    return Json(new
                    {
                        lastUpdateTime = device.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        photoName = fileName
                    });
                }
            }
            return Json(null);

        }

        public ActionResult DeleteReportDevice(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return RedirectToAction("ListNotifyDevice");
            }

            reportDevice device = this._reportDeviceService.GetById(deviceId);
            if (device == null)
            {
                return RedirectToAction("ListNotifyDevice");
            }

            device.isDelete = 1;

            try
            {
                this._reportDeviceService.Update(device);
            }
            catch (Exception)
            {
                return RedirectToAction("ListNotifyDevice");
            }

            return RedirectToAction("ListNotifyDevice");
        }

        [HttpPost]
        public ActionResult SaveReportDeviceSvgChangeToServer(FormCollection fc)
        {
            string deviceId = fc["deviceId"];
            reportDevice device = _reportDeviceService.GetById(deviceId);
            if (device == null)
            {
                return Json(null);
            }
            device.x = Convert.ToInt32(fc["x"]);
            device.y = Convert.ToInt32(fc["y"]);
            IResult result = _reportDeviceService.Update(device);
            if (result.Success)
            {
                return Json(new { lastUpdateTime = device.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
            }

            return Json(null);
        }
    }
}