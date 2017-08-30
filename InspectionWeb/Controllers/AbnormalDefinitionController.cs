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
    public class AbnormalDefinitionController : Controller
    {
        private IAbnormalDefinitionService _AbnormalDefinitionService;

        public class updateAbnormalDefinitionJson
        {
            public string name { get; set; }
            public string pk { get; set; }
            public string value { get; set; }

        }

        public AbnormalDefinitionController(IAbnormalDefinitionService abnormalDefinitionService)
        {
            this._AbnormalDefinitionService = abnormalDefinitionService;
        }

        public ActionResult ListAbnormalDefinition()
        {
            var TotalViewModel = new List<ListAbnormalDefinitionViewModel>();
            var abnormals = this._AbnormalDefinitionService.GetAll().ToList();

            foreach (var abnormal in abnormals)
            {
                ListAbnormalDefinitionViewModel abnormalViewModel = this.AbnormalDefinition2ViewModel(abnormal);
                TotalViewModel.Add(abnormalViewModel);
            }
            return View(TotalViewModel);
        }

        [HttpPost]
        public ActionResult AddAbnormalDefinition(string abnormalCode, string abnormalName)
        {
            if (!string.IsNullOrEmpty(abnormalCode) && !string.IsNullOrEmpty(abnormalName) && ModelState.IsValid)
            {

                IResult result = _AbnormalDefinitionService.Create(abnormalCode, abnormalName);

                if (result.Success == false)
                {
                    AddAbnormalDefinitionViewModel ad = new AddAbnormalDefinitionViewModel();
                    ad.abnormalDefinitionCode = abnormalCode;
                    ad.abnormalDefinitionName = abnormalName;
                    ad.ErrorMsg = result.ErrorMsg;

                    return View("AddAbnormalDefinition", ad);
                }

                return RedirectToAction("EditAbnormalDefinition", new { abnormalId = _AbnormalDefinitionService.GetId(abnormalCode) });
            }
            else
            {
                AddAbnormalDefinitionViewModel adEmpty = new AddAbnormalDefinitionViewModel();
                adEmpty.abnormalDefinitionName = abnormalName;
                adEmpty.ErrorMsg = "";

                return View("AddAbnormalDefinition", adEmpty);
            }

        }

        [HttpGet]
        public ActionResult AddAbnormalDefinition()
        {
            AddAbnormalDefinitionViewModel vm = new AddAbnormalDefinitionViewModel();

            return View(vm);
        }

        [HttpPost]
        public ActionResult UpdateAbnormalDefinition(updateAbnormalDefinitionJson abnormalDefinitionJson)
        {
            var abnormalId = abnormalDefinitionJson.pk;
            var abnormalDefinition = this._AbnormalDefinitionService.GetById(abnormalId);
            if (abnormalDefinition != null && ModelState.IsValid)
            {
                IResult result = this._AbnormalDefinitionService.Update(abnormalDefinition, abnormalDefinitionJson.name, abnormalDefinitionJson.value);
                abnormalDefinition = this._AbnormalDefinitionService.GetById(abnormalId);
                if (result.Success)
                {
                    string lastUpdateTime = abnormalDefinition.lastUpdateTime.ToString();
                    return Json(new { result = 1, abnormalId = abnormalId, lastUpdateTime = lastUpdateTime });
                }
                else
                {
                    return Json(new { result = 0, abnormalId = abnormalId });
                }
            }
            else
            {
                return Json(new { result = 0, abnormalId = abnormalId });
                //return RedirectToAction("EditAbnormalDefinition", new { abnormalId = abnormalId });
            }
        }

        public ActionResult EditAbnormalDefinition(string abnormalId)
        {
            if (string.IsNullOrEmpty(abnormalId))
            {
                return RedirectToAction("ListAbnormalDefintion");

            }
            else
            {
                var abnormal = _AbnormalDefinitionService.GetById(abnormalId);
                ListAbnormalDefinitionViewModel viewModel = new ListAbnormalDefinitionViewModel();
                viewModel = AbnormalDefinition2ViewModel(abnormal);

                return View(viewModel);
            }

        }

        public ActionResult DeleteAbnormalDefinition(string abnormalId)
        {
            if (string.IsNullOrEmpty(abnormalId))
            {
                return RedirectToAction("ListAbnormalDefinition");
            }

            var abnormalDefinition = this._AbnormalDefinitionService.GetById(abnormalId);

            if (abnormalDefinition == null)
            {
                return RedirectToAction("ListAbnormalDefinition");
            }

            try
            {
                this._AbnormalDefinitionService.Delete(abnormalId);
            }
            catch (Exception)
            {
                return RedirectToAction("ListAbnormalDefinition");
            }

            return RedirectToAction("ListAbnormalDefinition");
        }

        private ListAbnormalDefinitionViewModel AbnormalDefinition2ViewModel(abnormalDefinition instance)
        {
            ListAbnormalDefinitionViewModel viewModel = new ListAbnormalDefinitionViewModel();

            viewModel.abnormalDefinitionId = instance.abnormalId;
            viewModel.abnormalDefinitionCode = instance.abnormalCode;
            viewModel.abnormalDefinitionName = instance.abnormalName;
            viewModel.abnormalDefinitionDescription = instance.description;
            viewModel.isDelete = instance.isDelete;
            viewModel.createTime = instance.createTime;
            viewModel.lastUpdateTime = instance.lastUpdateTime;

            return viewModel;
        }
    }
}