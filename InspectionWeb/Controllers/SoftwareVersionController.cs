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
    public class SoftwareVersionController : Controller
    {

        private ISoftwareVersionService _softwareVersionService;
        public SoftwareVersionController(ISoftwareVersionService softwareVersionService)
        {
            this._softwareVersionService = softwareVersionService;
        }

       
        public ActionResult AddSoftware()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSoftware(FormCollection fc)
        {
            softwareVersion software = new softwareVersion();

            software.softwareName = fc["softwareName"];
            software.softwareCode = fc["softwareCode"];
            IResult result = _softwareVersionService.Create(software);
            string softwareId = result.Message;
            if(result.Success == false)
            {
                return View("AddSoftware");
            }

            return RedirectToAction("EditSoftwareVersion", new { id = softwareId });
        }


       
    }
}