using InspectionWeb.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    public class HomeController : Controller
    {
        private IAbnormalRecordService abnormalRecordService;

        public HomeController(IAbnormalRecordService abnormalRecordService)
        {
            this.abnormalRecordService = abnormalRecordService;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
