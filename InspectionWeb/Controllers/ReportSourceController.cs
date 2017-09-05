using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using InspectionWeb.Models.Misc;
using InspectionWeb.Models.ViewModel;
using System.Web.Hosting;
using System.IO;

namespace InspectionWeb.Controllers
{
    public class ReportSourceController : Controller
    {
        private IReportSourceService _ReportSourceService;

        public class updateReportSourceJson
        {
            public string name { get; set; }
            public string pk { get; set; }
            public string value { get; set; }
        }

        public ReportSourceController(IReportSourceService reportSourceService)
        {
            this._ReportSourceService = reportSourceService;
        }

        public ActionResult ListReportSource()
        {
            var TotalViewModel = new List<ReportSourceViewModel>();
            var sources = this._ReportSourceService.GetAll().ToList();

            foreach (var source in sources)
            {
                ReportSourceViewModel reportSourceViewModel = this.ReportSource2ViewModel(source);
                TotalViewModel.Add(reportSourceViewModel);
            }
            return View(TotalViewModel);

        }

        [HttpPost]
        public ActionResult AddReportSource(string sourceCode, string sourceName)
        {
            if (!string.IsNullOrEmpty(sourceCode) && !string.IsNullOrEmpty(sourceName) && ModelState.IsValid)
            {

                IResult result = _ReportSourceService.Create(sourceCode, sourceName);

                if (result.Success == false)
                {
                    ReportSourceViewModel rs = new ReportSourceViewModel();
                    rs.sourceCode = sourceCode;
                    rs.sourceName = sourceName;
                    rs.ErrorMsg = result.ErrorMsg;

                    return View("AddReportSource", rs);
                }

                return RedirectToAction("EditReportSource", new { sourceId = _ReportSourceService.GetId(sourceCode) });
            }
            else
            {
                ReportSourceViewModel rsEmpty = new ReportSourceViewModel();
                rsEmpty.sourceCode = sourceCode;
                rsEmpty.sourceName = sourceName;
                rsEmpty.ErrorMsg = "";

                return View("AddReportSource", rsEmpty);
            }

        }

        [HttpGet]
        public ActionResult AddReportSource()
        {
            ReportSourceViewModel vm = new ReportSourceViewModel();

            return View(vm);
        }

        public ActionResult EditReportSource(string sourceId)
        {
            if (string.IsNullOrEmpty(sourceId))
            {
                return RedirectToAction("ListReportSource");
            }
            else
            {
                var reportSource = _ReportSourceService.GetById(sourceId);
                ReportSourceViewModel viewModel = ReportSource2ViewModel(reportSource);
                return View(viewModel);
            }

        }

        [HttpPost]
        public ActionResult UpdateReportSource(updateReportSourceJson reportSourceJson)
        {
            var sourceId = reportSourceJson.pk;
            var reportSource = this._ReportSourceService.GetById(sourceId);
            if (reportSource != null && ModelState.IsValid)
            {
                IResult result = this._ReportSourceService.Update(reportSource, reportSourceJson.name, reportSourceJson.value);
                reportSource = this._ReportSourceService.GetById(sourceId);
                if (result.Success)
                {
                    string lastUpdateTime = reportSource.lastUpdateTime.ToString();
                    return Json(new { result = 1, sourceId = sourceId, lastUpdateTime = lastUpdateTime });
                }
                else
                {
                    return Json(new { result = 0, sourceId = sourceId });
                }
            }
            else
            {
                return Json(new { result = 0, sourceId = sourceId });
                //return RedirectToAction("ListReportSource");
            }
        }

        private ReportSourceViewModel ReportSource2ViewModel(reportSource instance)
        {
            ReportSourceViewModel viewModel = new ReportSourceViewModel();

            viewModel.sourceId = instance.sourceId;
            viewModel.sourceCode = instance.sourceCode;
            viewModel.sourceName = instance.sourceName;
            viewModel.description = instance.description;
            viewModel.isDelete = instance.isDelete;
            viewModel.createTime = instance.createTime;
            viewModel.lastUpdateTime = instance.lastUpdateTime;

            return viewModel;
        }

        public ActionResult DeleteReportSource(string sourceId)
        {
            if (string.IsNullOrEmpty(sourceId))
            {
                return RedirectToAction("ListReportSource");
            }

            var reportSource = this._ReportSourceService.GetById(sourceId);

            if (reportSource == null)
            {
                return RedirectToAction("ListReportSource");
            }

            try
            {
                this._ReportSourceService.Delete(sourceId);
            }
            catch (Exception)
            {
                return RedirectToAction("ListReportSource");
            }

            return RedirectToAction("ListReportSource");
        }

    }
}