using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;

namespace InspectionWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        private IUserService _userService;
        private IUserGroupService _userGroupService;

        public AuthenticationController(IUserService service, IUserGroupService service2)
        {
            this._userService = service;
            this._userGroupService = service2;
        }

        // GET: Authentication/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: Authentication/AuthenticationFail
        [AllowAnonymous]
        public ActionResult AuthenticationFail()
        {
            return View();
        }

        // GET: Authentication/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
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
                
                // 使用 MVC 內建登入並利用自訂權限 [AuthorizeUser] 功能
                LoginProcess(user, false);

                // 取得自訂的 userData
                /*if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    FormsIdentity id = (FormsIdentity)User.Identity;
                    FormsAuthenticationTicket ticket = id.Ticket;
                    var userData = ticket.UserData.Split(',');
                }*/

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Authentication/Logout
        [AllowAnonymous]
        public ActionResult Logout()
        {
            // 使用者登出
            FormsAuthentication.SignOut();

            // 清除所有的 session
            Session.RemoveAll();

            // 建立一個同名的 Cookie 來覆蓋原本的 Cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // 建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            // 清除 Session 所有 key 值，並呼叫 Session_End
            Session.Abandon();

            return RedirectToAction("Login");
        }

        private void LoginProcess(user loginUser, bool isRemember)
        {
            var now = DateTime.Now;
            string roles = "Normal";

            // 依照 group 設定權限
            userGroup group = this._userGroupService.GetByID(loginUser.groupId);

            if (group.superUserOnly == 1)
            {
                roles += ",Super";
            }
            if (group.systemManagement == 1)
            {
                roles += ",Manager";
            }
            if (group.userManagement == 1)
            {
                roles += ",User";
            }
            if (group.dispatchManagement == 1)
            {
                roles += ",Dispatch";
            }

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,                          // version
                loginUser.userId,           // 使用者ID
                now,                        // 核發日期
                now.AddMinutes(10),         // 到期時間 10 分鐘 
                isRemember,                 // 永續性
                roles,                      // 使用者定義的資料
                FormsAuthentication.FormsCookiePath
            );

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
    }
}