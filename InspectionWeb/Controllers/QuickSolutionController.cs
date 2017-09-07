using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;


namespace InspectionWeb.Controllers
{
    public class QuickSolutionController : Controller
    {
        private ISolutionService _solutionService;
        public QuickSolutionController(ISolutionService service)
        {
            this._solutionService = service;
        }

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


        //function
        [HttpPost]
        public ActionResult AddSolution(string description)
        {
            if (!string.IsNullOrEmpty(description) && ModelState.IsValid)
            {
                IResult result = this._solutionService.Create(description);

                if (result.Success == false)
                {
                    ViewBag.ErrorMsg = result.ErrorMsg;

                    return View("Add");
                }

                return RedirectToAction("Edit");
                //return RedirectToAction("Edit", new { id = result.Message });
            }
            else
            {
                ViewBag.ErrorMsg = "方法說明空白";
                return View("Add");
            }
            
        }
    }
}