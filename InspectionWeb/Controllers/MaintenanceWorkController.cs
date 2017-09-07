using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Models;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, Dispatch = true)]
    public class MaintenanceWorkController : Controller
    {
        // GET: MaintenanceWork
        public ActionResult Index()
        {
            return View();
        }
        // GET: /ReportJob/PendingList
        public ActionResult PendingList()
        {
            return View();
        }
        
        // GET: /ReportJob/ExtendedWork
        public ActionResult ExtendedWork()
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

        // GET: /ReportJob/WriteDetailedData
        public ActionResult WriteDetailedData()
        {
            return View();
        }
        
    }
}