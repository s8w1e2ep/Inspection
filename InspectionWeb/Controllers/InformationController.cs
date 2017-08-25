using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    public class InformationController : Controller
    {
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

        // GET: /Information/EditField
        public ActionResult EditField()
        {
            return View();
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
    }
}