using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    public class ReportJobController : Controller
    {
        public ActionResult AddExhibitionItem()
        {
            ViewBag.Message = "Hello ";
            ViewBag.NumTimes = 5;

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

        // GET: /ReportJob/Query
        public ActionResult Query()
        {
            return View();
        }

        // GET: /ReportJob/DetailedData
        public ActionResult ItemDetailedData()
        {
            return View();
        }

        //function
        [HttpPost]
        public ActionResult F_AddExhibitionItem()
        {
            return View();
        }

    }
}