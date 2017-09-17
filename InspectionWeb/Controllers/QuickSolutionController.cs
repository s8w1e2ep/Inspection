using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;




namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, User = true)]
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

       

        public ActionResult List()
        {
            var vms = new List<SolutionViewModel>();
            var solutions = this._solutionService.GetAll().ToList();
            foreach (var data in solutions)
            {
                vms.Add(this.solution2ViewModel(data));
            }
            return View(vms);
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

                return RedirectToAction("Edit", new { id = result.Message });
            }
            else
            {
                ViewBag.ErrorMsg = "方法說明空白";
                return View("Add");
            }
            
        }


        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("List");
            }
            var data = this._solutionService.GetByID(id);
            ViewBag.description = data.description;

            ViewBag.createTime = data.createTime?.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.solutionId = data.solutionId;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateSolution(SoluJson solutionJ)
        {
            var id = solutionJ.Pk;
            var SoluData = this._solutionService.GetByID(id);
            if (SoluData != null && ModelState.IsValid)
            {
                IResult result = this._solutionService.Update(SoluData, solutionJ.Name, solutionJ.Value);

                if (result.Success)
                {
                    return Json(result);
                }
                else
                {
                    return RedirectToAction("Edit");
                }
            }
            else
            {
                return RedirectToAction("List");
            }
        }

        public ActionResult DeleteSolution(string solutionId)
        {
            if (string.IsNullOrEmpty(solutionId))
            {
                return RedirectToAction("List");
            }

            var solution = _solutionService.GetByID(solutionId);

            if (solution == null)
            {
                return RedirectToAction("List");
            }

            try
            {
                this._solutionService.Update(solution, "isDelete", "1");
            }
            catch (Exception)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("List");
        }

        private SolutionViewModel solution2ViewModel(quickSolution instance)
        {
            SolutionViewModel vm = new SolutionViewModel();

            vm.solutionId = instance.solutionId;
            vm.description = instance.description;
            vm.lastUpdateTime = instance.lastUpdateTime;

            return vm;
        }

        public class SoluJson
        {
            public string Name { get; set; }
            public string Pk { get; set; }
            public string Value { get; set; }

        }

    }
}