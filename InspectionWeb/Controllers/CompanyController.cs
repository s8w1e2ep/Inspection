using System;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Models;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System.Linq;
using System.IO;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, User = true)]
    public class CompanyController : Controller
    {
        private ICompanyService _companyService;
        private IExhibitionItemService _itemService;

        public CompanyController(ICompanyService service, IExhibitionItemService service2)
        {
            this._companyService = service;
            this._itemService = service2;
        }

        // GET: Company/Add
        public ActionResult Add()
        {
            GroupAddViewModel vm = new GroupAddViewModel();
            return View(vm);
        }

        // GET: Company/AddCompany
        [HttpPost]
        public ActionResult AddCompany(string companyName)
        {
            if (!string.IsNullOrEmpty(companyName) && ModelState.IsValid)
            {
                IResult result = this._companyService.Create(companyName);

                if (result.Success == false)
                {
                    GroupAddViewModel vm = new GroupAddViewModel();
                    vm.GroupName = companyName;
                    vm.ErrorMsg = result.ErrorMsg;

                    return View("Add", vm);
                }

                return RedirectToAction("Edit", new { companyId = result.Message });
            }
            else
            {
                GroupAddViewModel vm = new GroupAddViewModel();
                vm.GroupName = companyName;
                vm.ErrorMsg = "帳號或密碼空白";

                return View("Add", vm);
            }
        }

        // GET: Company/Edit/companyId
        public ActionResult Edit(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("List");
            }
            else
            {
                var company = this._companyService.GetByID(companyId);
                if (company == null)
                {
                    return RedirectToAction("List");
                }
                ViewBag.company = company;

                return View();
            }
        }

        // GET: Company/List
        public ActionResult List()
        {
            ViewBag.companys = this._companyService.GetAll();
            return View();
        }

        // GET: Company/UpdateCompany
        [HttpPost]
        public ActionResult UpdateCompany(CompanyJson userJson)
        {
            var id = userJson.pk;
            var company = this._companyService.GetByID(id);
            if (company != null && ModelState.IsValid)
            {
                IResult result = this._companyService.Update(company, userJson.name, userJson.value);

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

        // DELETE: Company/DeleteCompany
        public ActionResult DeleteCompany(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
            {
                return Json(new { success = false, msg = "廠商ID為空" });
            }

            var company = _companyService.GetByID(companyId);

            if (company == null)
            {
                return Json(new { success = false, msg = "無此公司" });
            }

            try
            {
                if(this._itemService.GetAll().Any(x => x.companyId == companyId))
                {
                    return Json(new { success = false, msg = "請先刪除此廠商的展項"});
                }
                else
                {
                    this._companyService.Update(company, "isDelete", "1");
                    return Json(new { success = true, msg = "刪除成功" });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "刪除錯誤" });
            }    
        }

        // POST: /Company/UploadImg
        [HttpPost]
        public ActionResult UpdateImg(HttpPostedFileBase upload, string companyId, string type)
        {
            if (upload.ContentLength > 0)
            {
                try
                {
                    var fileName = companyId;

                    if (type == "jpeg")
                    {
                        fileName += ".jpg";
                    }
                    else
                    {
                        fileName += ".png";
                    }

                    var path = Server.MapPath("~/media/company/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, fileName);
                    upload.SaveAs(path);

                    company instance = _companyService.GetByID(companyId);

                    var result = _companyService.Update(instance, "logo", fileName);

                    if (result.Success)
                    {
                        result.Message = fileName;
                        return Json(result);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("UploadImg ex:\n" + e.ToString());
                }
                return Json(null);
            }
            else
            {
                return Json(null);
            }
        }

        public class CompanyJson
        {
            public string name { get; set; }
            public string pk { get; set; }
            public string value { get; set; }

        }
    }
}