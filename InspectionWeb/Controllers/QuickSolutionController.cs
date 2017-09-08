using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System.Web.Mvc;



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

            ViewBag.createTime = data.createTime;
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

        public class SoluJson
        {
            public string Name { get; set; }
            public string Pk { get; set; }
            public string Value { get; set; }

        }

    }
}