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

        public InformationController(IFieldMapService fieldMapService)
        {
            _fieldMapService = fieldMapService;
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

        //GET: /Information/EditExhibition
        public ActionResult EditExhibition()
        {
            return View();
        }


        //GET: /Information/ListExhibition
        public ActionResult ListExhibition()
        {
            return View();
        }

        //GET: /Information/EditExhibitItem
        public ActionResult EditExhibitItem()
        {
            return View();
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