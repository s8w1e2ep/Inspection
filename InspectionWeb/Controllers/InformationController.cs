using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
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
            vm.fieldId = result.Message;
            vm.FieldName = fieldName;
            vm.ErrorMsg = result.ErrorMsg;

            if (result.Success == false)
            {
                return View("AddField",vm);
            }

             return RedirectToAction("EditField", new { id = vm.fieldId});

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

            vm.fieldId = fieldId;
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
        [ValidateAntiForgeryToken]
        public ActionResult EditField(FieldAddViewModel vm)
        {
            fieldMap field = this._fieldMapService.GetById(vm.fieldId);
            field.fieldName = vm.FieldName;
            field.description = vm.Description;
            field.mapFileName = vm.MapFileName;
            field.photo = vm.Photo;
            field.version = vm.Version;

            IResult result = this._fieldMapService.Update(field);
            if(result.Success == false)
            {
                return RedirectToAction("EditField", vm.fieldId);
            }

            return RedirectToAction("ListField");
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
            fieldMap field = this._fieldMapService.GetById(fieldId);
            field.isDelete = 1;
            this._fieldMapService.Update(field);

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