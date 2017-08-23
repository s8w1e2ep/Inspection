using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    public class FieldController : Controller
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
    }
}