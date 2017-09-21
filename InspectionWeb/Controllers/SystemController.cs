using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models.ViewModel;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, User = true)]
    public class SystemController : Controller
    {
        private ISystemArgService _systemArgService;
        public SystemController(ISystemArgService systemArgService)
        {
            this._systemArgService = systemArgService;
        }


        //GET /System/AddSystemArg
        public ActionResult AddSystemArg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSystemArg(FormCollection fc)
        {
            systemSettings setting = new systemSettings();
            setting.keyName = fc["keyName"];
            setting.description = fc["description"];
            setting.value = fc["value"];

            
            IResult result = _systemArgService.Create(setting);
            string id = result.Message;
            if (result.Success == false)
            {
                return View("AddSystemArg");
            }

            return RedirectToAction("EditSystemArg", new { id = id });
        }

        //GET /System/EditSystemArg
        public ActionResult EditSystemArg(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                RedirectToAction("AddSystemArg");
            }
            systemSettings setting = this._systemArgService.GetById(id);
            SystemArgAddViewModel vm = new SystemArgAddViewModel();

            if(setting == null)
            {
                return RedirectToAction("ListSystemArg");
            }
            vm.Id = id;
            vm.KeyName = setting.keyName;
            vm.Description = setting.description;
            vm.Value = setting.value;

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditSystemArg(FormCollection fc)
        {
            string id = fc["pk"];
            systemSettings setting = this._systemArgService.GetById(id);

            if (setting != null && ModelState.IsValid)
            {
                setting.GetType().GetProperty(fc["name"]).SetValue(setting, fc["value"]);
                IResult result = this._systemArgService.Update(setting);
               
            }
            else
            {
                return RedirectToAction("EditSystemArg");
            }
            return RedirectToAction("ListSystemArg");
        }

        //GET /System/ListSystemArg
        public ActionResult ListSystemArg()
        {
            List<SystemArgAddViewModel> vms = new List<SystemArgAddViewModel>();
            List<systemSettings> allSystemSetting = this._systemArgService.GetAll().ToList();

            foreach(var setting in allSystemSetting)
            {
                SystemArgAddViewModel vm = new SystemArgAddViewModel();
                vm.Id = setting.id;
                vm.KeyName = setting.keyName;
                vm.Value = setting.keyName;
                vms.Add(vm);
            }

            return View(vms);
        }

        //GET /System/DeleteSystemArg
        public ActionResult DeleteSystemArg(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("ListSystemArg");
            }

            systemSettings setting = this._systemArgService.GetById(id);
            if (setting == null)
            {
                return RedirectToAction("ListSystemArg");
            }

            setting.isDelete = 1;

            try
            {
                this._systemArgService.Update(setting);
            }
            catch (Exception)
            {
                return RedirectToAction("ListSystemArg");
            }

            return RedirectToAction("ListSystemArg");
        }
    }
}