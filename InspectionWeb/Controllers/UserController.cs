﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System.IO;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, User = true)]
    public class UserController : Controller
    {
        private IUserService _userService;
        private IUserGroupService _userGroupService;
        private IExhibitionRoomService _exhibitionRoomService;

        public UserController(IUserService service, IUserGroupService service2, IExhibitionRoomService service3)
        {
            this._userService = service;
            this._userGroupService = service2;
            this._exhibitionRoomService = service3;
        }

        // GET: /User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: /User/Add
        public ActionResult Add()
        {
            UserAddViewModel vm = new UserAddViewModel();
            return View(vm);
        }

        // GET: /User/List
        public ActionResult List()
        {
            var vms = new List<UserDetailViewModel>();
            var users = this._userService.GetAll().ToList();

            foreach (var user in users)
            {
                vms.Add(this.User2ViewModel(user));
            }

            return View(vms);
        }

        // GET: /User/Edit/id
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("List");
            }
            else
            {
                var user = this._userService.GetByID(id);
                if (user == null)
                {
                    return RedirectToAction("List");
                }
                UserEditViewModel vm = this.User2EditViewModel(user);

                return View(vm);
            }
        }

        // GET: /User/EditPassword
        public ActionResult EditPassword(string userId, string originalPassword, string newPassword, string repeatPassword)
        {
            IResult result = new Result(false);

            if (newPassword.Equals(repeatPassword))
            {
                result = this._userService.changePassword(userId, originalPassword, repeatPassword);
            }
            return Json(result);
        }

        // POST: /User/AddUser
        [HttpPost]
        public ActionResult AddUser(string account, string password, string repeatPassword)
        {

            if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password) && ModelState.IsValid)
            {

                IResult result = this._userService.Create(account, password);

                if (result.Success == false)
                {
                    UserAddViewModel vm = new UserAddViewModel();
                    vm.account = account;
                    vm.password = password;
                    vm.errorMsg = result.ErrorMsg;

                    return View("Add", vm);
                }

                return RedirectToAction("Edit", new { id = result.Message });
            }
            else
            {
                UserAddViewModel vm = new UserAddViewModel();
                vm.account = account;
                vm.password = password;
                vm.errorMsg = "帳號或密碼空白";

                return View("Add", vm);
            }
        }

        // POST: /User/UpdateUser
        [HttpPost]
        public ActionResult UpdateUser(UserJson userJson)
        {
            var id = userJson.pk;
            var user = this._userService.GetByID(id);
            if (user != null && ModelState.IsValid)
            {
                IResult result = this._userService.Update(user, userJson.name, userJson.value);

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

        // DELETE: /User/DeleteUser
        public ActionResult DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, msg = "使用者ID為空" });
            }

            var user = _userService.GetByID(userId);

            if (user == null)
            {
                return Json(new { success = false, msg = "無此使用者" });
            }

            try
            {
                if (this._userService.GetAll().Any(x => x.agent == userId))
                {
                    return Json(new { success = false, msg = "無法刪除，此使用者為其他使用者的代理人" });
                }
                else
                {
                    this._userService.Update(user, "isDelete", "1");
                    return Json(new { success = true, msg = "刪除成功" });
                }    
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "刪除錯誤" });
            }
        }

        // GET: /User/AddGroup
        public ActionResult AddGroup()
        {
            GroupAddViewModel vm = new GroupAddViewModel();
            return View(vm);
        }

        // GET: /User/ListGroup
        public ActionResult ListGroup()
        {
            var vms = new List<GroupDetailViewModel>();
            var groups = this._userGroupService.GetAll().ToList();

            foreach (var group in groups)
            {
                vms.Add(this.Group2ViewModel(group));
            }

            return View(vms);
        }

        // GET: /User/EditGroup/id
        public ActionResult EditGroup(string groupId)
        {
            if (string.IsNullOrEmpty(groupId))
            {
                return RedirectToAction("List");
            }
            else
            {
                var group = this._userGroupService.GetByID(groupId);
                if (group == null)
                {
                    return RedirectToAction("List");
                }
                GroupDetailViewModel vm = this.Group2ViewModel(group);

                return View(vm);
            }
        }

        // POST: /User/AddUserGroup
        [HttpPost]
        public ActionResult AddUserGroup(string groupName)
        {
            if (!string.IsNullOrEmpty(groupName) && ModelState.IsValid)
            {
                IResult result = this._userGroupService.Create(groupName);

                if (result.Success == false)
                {
                    GroupAddViewModel vm = new GroupAddViewModel();
                    vm.GroupName = groupName;
                    vm.ErrorMsg = result.ErrorMsg;

                    return View("AddGroup", vm);
                }

                return RedirectToAction("EditGroup", new { groupid = result.Message });
            }
            else
            {
                GroupAddViewModel vm = new GroupAddViewModel();
                vm.GroupName = groupName;
                vm.ErrorMsg = groupName;// "帳號或密碼空白";

                return View("AddGroup", vm);
            }
        }

        // POST: /User/UpdateUserGroup
        [HttpPost]
        public ActionResult UpdateUserGroup(UserJson userJson)
        {
            var id = userJson.pk;
            var group = this._userGroupService.GetByID(id);
            if (group != null && ModelState.IsValid)
            {
                IResult result = this._userGroupService.Update(group, userJson.name, userJson.value);

                if (result.Success)
                {
                    return Json(result);
                }
                else
                {
                    return RedirectToAction("EditGroup");
                }
            }
            else
            {
                return RedirectToAction("ListGroup");
            }
        }

        // POST: /User/UploadImg
        [HttpPost]
        public ActionResult UpdateImg(HttpPostedFileBase upload, string userId, string type)
        {
            if (upload.ContentLength > 0)
            {
                try
                {
                    var fileName = userId;

                    if (type == "jpeg")
                    {
                        fileName += ".jpg";
                    }
                    else
                    {
                        fileName += ".png";
                    }

                    var path = Server.MapPath("~/media/user/");

                    if (!Directory.Exists(path))
                    {                    
                        Directory.CreateDirectory(path);
                    }
                    System.Diagnostics.Debug.WriteLine("GG: \n\n" + path);
                    path = Path.Combine(path, fileName);
                    upload.SaveAs(path);

                    user instance = _userService.GetByID(userId);

                    var result = _userService.Update(instance, "picture", fileName);
 
                    if (result.Success)
                    {
                        result.Message = fileName;
                        return Json(result);
                    }  
                }
                catch(Exception e)
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

        // DELETE: /User/DeleteGroup
        public ActionResult DeleteGroup(string groupId)
        {
            if (string.IsNullOrEmpty(groupId))
            {
                return Json(new { success = false, msg = "群組ID為空" });
            }

            var group = _userGroupService.GetByID(groupId);

            if (group == null)
            {
                return Json(new { success = false, msg = "無此群組" });
            }

            try
            {
                if (this._userService.GetAll().Any(x => x.groupId == groupId))
                {
                    return Json(new { success = false, msg = "請先刪除群組內的使用者" });
                }
                else
                {
                    this._userGroupService.Update(group, "isDelete", "1");
                    return Json(new { success = true, msg = "刪除成功" });
                }
                
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "刪除錯誤" });
            }
        }

        private UserDetailViewModel User2ViewModel(user instance)
        {
            UserDetailViewModel vm = new UserDetailViewModel();

            vm.userId = instance.userId;
            vm.userCode = instance.userCode;
            
            if (this._userGroupService.IsExists(instance.groupId))
            {
                vm.group = (this._userGroupService.GetByID(instance.groupId)).groupName;
            }
            else
            {
                vm.group = null;
            }
            vm.email = instance.email;
            vm.tel = instance.tel;
            vm.password = instance.password;
            vm.name = instance.userName;
            if (this._userService.IsExists(instance.agent))
            {
                vm.agent = (this._userService.GetByID(instance.agent)).userCode;
            }
            else
            {
                vm.agent = null;
            }
            vm.picture = instance.picture;
            vm.active = instance.active;
            vm.isDelete = (short)instance.isDelete;
            vm.createTime = instance.createTime;
            vm.lastUpdateTime = instance.lastUpdateTime;

            return vm;
        }

        private UserEditViewModel User2EditViewModel(user instance)
        {
            UserEditViewModel vm = new UserEditViewModel();
            var groups = this._userGroupService.GetAll().Where(x => x.isDelete == 0 && x.superUserOnly == 0).ToList();
            var agents = this._userService.GetAll().Where(x => x.isDelete == 0 && x.userId != instance.userId).ToList();
            var rooms = this._exhibitionRoomService.GetAll().Where(x => x.inspectionUserId == instance.userId);

            vm.user = User2ViewModel(instance);

            foreach (var group in groups)
            {
                vm.groups.Add(Group2ViewModel(group));
            }
            foreach (var agent in agents)
            {
                vm.agents.Add(User2ViewModel(agent));
            }
            foreach (var room in rooms)
            {
                vm.rooms.Add(Room2ViewModel(room));
            }

            return vm;
        }

        private ExhibitionRoomListViewModel Room2ViewModel(exhibitionRoom instance)
        {
            ExhibitionRoomListViewModel vm = new ExhibitionRoomListViewModel();

            vm.roomId = instance.roomId;
            vm.roomName = instance.roomName;
            vm.createTime = instance.createTime;
            vm.lastUpdateTime = instance.lastUpdateTime;

            return vm;
        }

        private GroupDetailViewModel Group2ViewModel(userGroup instance)
        {
            GroupDetailViewModel vm = new GroupDetailViewModel();

            vm.groupId = instance.groupId;
            vm.groupName = instance.groupName;
            vm.superUser = instance.superUserOnly;
            vm.system = instance.systemManagement;
            vm.user = instance.userManagement;
            vm.dispatch = instance.dispatchManagement;
            vm.normal = instance.normalUser;
            vm.isDelete = (short)instance.isDelete;
            vm.createTime = instance.createTime;
            vm.lastUpdateTime = instance.lastUpdateTime;

            return vm;
        }

        public class UserJson
        {
            public string name { get; set; }
            public string pk { get; set; }
            public string value { get; set; }

        }
    }
}
