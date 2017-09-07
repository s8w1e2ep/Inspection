using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Models;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, Dispatch = true)]
    public class ReportJobController : Controller
    {
        // GET: /ReportJob/AddExhibitionItem
        public ActionResult AddExhibitionItem()
        {
            return View();
        }

        // GET: /ReportJob/AddExperience
        public ActionResult AddExperience()
        {
            return View();
        }

        
        public ActionResult AddOther()
        {
            return View();
        }

        // GET: /ReportJob/EditExhibitionItem
        public ActionResult EditExhibitionItem()
        {
            return View();
        }

        // GET: /ReportJob/EditExperience
        public ActionResult EditExperience()
        {
            return View();
        }

        // GET: /ReportJob/EditOther
        public ActionResult EditOther()
        {
            return View();
        }

        // GET: /ReportJob/Query
        public ActionResult Query()
        {
            return View();
        }

        // GET: /ReportJob/DetailedData
        public ActionResult DetailedData()
        {
            return View();
        }
    }
}