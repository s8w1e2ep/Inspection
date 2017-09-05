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

        public ActionResult ListField()
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