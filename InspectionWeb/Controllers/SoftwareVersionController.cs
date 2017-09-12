using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    public class SoftwareVersionController : Controller
    {

        private ISoftwareVersionService _softwareVersionService;
        public SoftwareVersionController(ISoftwareVersionService softwareVersionService)
        {
            this._softwareVersionService = softwareVersionService;
        }

        public ActionResult AddSoftware()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSoftware(FormCollection fc)
        {
            softwareVersion software = new softwareVersion();

            software.softwareName = fc["softwareName"];
            software.softwareCode = fc["softwareCode"];
            IResult result = _softwareVersionService.Create(software);
            string softwareId = result.Message;
            if (result.Success == false)
            {
                return View("AddSoftware");
            }

            return RedirectToAction("EditSoftware", new { id = softwareId });
        }

        public ActionResult EditSoftware(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                RedirectToAction("AddSoftware");
            }
            string softwareId = id;
            softwareVersion software = this._softwareVersionService.GetById(softwareId);
            SoftwareVersionAddViewModel vm = new SoftwareVersionAddViewModel();

            if (software == null)
            {
                return RedirectToAction("ListSoftwareVersion");
            }

            vm.SoftwareId = softwareId;
            vm.SoftwareName = software.softwareName;
            vm.SoftwareCode = software.softwareCode;
            vm.Description = software.description;
            vm.FileName = software.fileName;
            vm.Version = software.version;
            vm.CreateTime = software.createTime;
            vm.LastUpdateTime = software.lastUpdateTime;

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditSoftware(FormCollection fc)
        {
            string softwareId = fc["pk"];
            softwareVersion software = this._softwareVersionService.GetById(softwareId);

            if (software != null && ModelState.IsValid)
            {
                software.GetType().GetProperty(fc["name"]).SetValue(software, fc["value"]);
                IResult result = this._softwareVersionService.Update(software);
                if (result.Success)
                {
                    return Json(new { lastUpdateTime = software.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
                }
            }
            else
            {
                return RedirectToAction("EditSoftware");
            }
            return RedirectToAction("ListField");
        }

        public ActionResult ListSoftware()
        {
            List<SoftwareVersionListViewModel> vms = new List<SoftwareVersionListViewModel>();
            List<softwareVersion> allSoftwares = this._softwareVersionService.GetAll().ToList();

            foreach (var software in allSoftwares)
            {
                SoftwareVersionListViewModel vm = new SoftwareVersionListViewModel();
                vm.SoftwareId = software.softwareId;
                vm.SoftwareCode = software.softwareCode;
                vm.SoftwareName = software.softwareName;
                vm.Version = software.version;
                vm.CreateTime = software.createTime;
                vm.LastUpdateTime = software.lastUpdateTime;
                vms.Add(vm);

            }
            return View(vms);
        }

        public ActionResult DeleteSoftware(string softwareId)
        {
            if (string.IsNullOrEmpty(softwareId))
            {
                return RedirectToAction("ListSoftware");
            }

            softwareVersion software = this._softwareVersionService.GetById(softwareId);
            if (software == null)
            {
                return RedirectToAction("ListSoftware");
            }

            software.isDelete = 1;

            try
            {
                this._softwareVersionService.Update(software);
            }
            catch (Exception)
            {
                return RedirectToAction("ListSoftware");
            }

            return RedirectToAction("ListSoftware");
        }

        [HttpPost]
        public ActionResult UpdateFile(HttpPostedFileBase upload, string softwareId )
        {
            if (upload.ContentLength > 0)
            {
                string fileName = softwareId;
                fileName = fileName + Path.GetExtension(upload.FileName);

                string savePath = System.IO.Path.Combine(Server.MapPath("~/media/software"), fileName);
                upload.SaveAs(savePath);

                softwareVersion software = _softwareVersionService.GetById(softwareId);
                software.fileName = fileName;
                IResult result = _softwareVersionService.Update(software);
                if (result.Success)
                {
                    return Json(new
                    {
                        lastUpdateTime = software.lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        fileName = fileName
                    });
                }
            }
            return Json(null);
        }
    }
}