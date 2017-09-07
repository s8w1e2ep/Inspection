using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, User = true)]
    public class QuickSolutionController : Controller
    {
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
    }
}