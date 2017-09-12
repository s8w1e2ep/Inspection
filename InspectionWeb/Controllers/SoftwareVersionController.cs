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

       

       
    }
}