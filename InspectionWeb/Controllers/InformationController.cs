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

        // GET: /Field/AddField
        public ActionResult AddField()
        {
            return View();
        }

        // GET: /Field/EditField
        public ActionResult EditField()
        {
            return View();
        }
        
    }
}