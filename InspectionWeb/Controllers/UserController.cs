using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Models.ViewModel;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;

namespace InspectionWeb.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService service)
        {
            this._userService = service;
        }

        // GET: /User/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            //Session["authenticated"] = false;
            return View();
        }

        // POST: /User/Login/account/password
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string account, string password)
        {
            user user = this._userService.Login(account, password);

            if (user != null)
            {
                Session["authenticated"] = true;
                Session["account"] = user.userCode;
                Session["userId"] = user.userId;
                //TODO: 依照 groupId 設定權限

                //使用 MVC 內建登入並利用自訂權限 [AuthorizeUser] 功能
                LoginProcess(user, false);

                //取得權限寫法
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity id = (FormsIdentity)User.Identity;
                    FormsAuthenticationTicket ticket = id.Ticket;
                    var userData = ticket.UserData.Split(',');
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        private void LoginProcess(user loginUser, bool isRemember)
        {
            var now = DateTime.Now;
            string roles = "Normal";
            // TODO: 依照 groupId 設定角色名稱，Ex: roles = roles + ",Manager";

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,                          // version
                loginUser.userId,           // 使用者ID
                DateTime.Now,               // 核發日期
                DateTime.Now.AddMinutes(10),// 到期時間 10 分鐘 
                isRemember,                 // 永續性
                roles,                      // 使用者定義的資料
                FormsAuthentication.FormsCookiePath
            );

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }

        // GET: /User/Logout
        [AllowAnonymous]
        public ActionResult Logout()
        {
            //使用者登出
            FormsAuthentication.SignOut();

            //清除所有的 session
            Session.RemoveAll();

            //建立一個同名的 Cookie 來覆蓋原本的 Cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            //建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            //Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login");
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
                UserDetailViewModel vm = this.User2ViewModel(user);

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
                    vm.Account = account;
                    vm.Password = password;
                    vm.ErrorMsg = result.ErrorMsg;

                    return View("Add", vm);
                }

                return RedirectToAction("List");
            }
            else
            {
                UserAddViewModel vm = new UserAddViewModel();
                vm.Account = account;
                vm.Password = password;
                vm.ErrorMsg = "帳號或密碼空白";

                return View("Add", vm);
            }
        }

        // POST: /User/UpdateUser
        [HttpPost]
        public ActionResult UpdateUser(UserJson userJson)
        {
            var id = userJson.key;
            var user = this._userService.GetByID(id);

            if (user != null && ModelState.IsValid)
            {
                this._userService.Update(user, userJson.name, userJson.value);
                var updatedUser = this._userService.GetByID(id);
                return View("Edit", this.User2ViewModel(updatedUser));
            }
            else
            {
                return RedirectToAction("List");
            }
        }

        // DELETE: /User/deleteUser
        public ActionResult DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("List");
            }

            var user = _userService.GetByID(userId);

            if (user == null)
            {
                return RedirectToAction("List");
            }

            try
            {
                _userService.Delete(userId);
            }
            catch (Exception)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("List");
        }

        // GET: /User/AddGroup
        public ActionResult AddGroup()
        {
            return View();
        }

        // GET: /User/ListGroup
        public ActionResult ListGroup()
        {
            return View();
        }

        // GET: /User/EditGroup
        public ActionResult EditGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addGroup(string name)
        {
            return RedirectToAction("ListGroup");
        }

        private UserDetailViewModel User2ViewModel(user instance)
        {
            UserDetailViewModel vm = new UserDetailViewModel();

            vm.userId = instance.userId;
            vm.userCode = instance.userCode;
            vm.groupId = instance.groupId;
            vm.email = instance.email;
            vm.password = instance.password;
            vm.name = instance.userName;
            vm.agent = instance.agent;
            vm.picture = instance.agent;
            vm.active = instance.active;
            vm.isDelete = (short)instance.isDelete;
            vm.createTime = instance.createTime;
            vm.lastUpdateTime = instance.lastUpdateTime;

            return vm;
        }

        public class UserJson
        {
            public string name { get; set; }
            public string key { get; set; }
            public string value { get; set; }

        }
    }
}