﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    public class SystemController : Controller
    {
        // GET: System
        public ActionResult Index()
        {
            return View();
        }

        // GET: System/AddSoftware
        public ActionResult AddSoftware()
        {
            return View();
        }

        // GET: System/EditSoftware
        public ActionResult EditSoftware()
        {
            return View();
        }

        // GET: System/ListSoftware
        public ActionResult ListSoftware()
        {
            return View();
        }
    }
}