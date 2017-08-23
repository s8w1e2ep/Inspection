using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    public class UserController : Controller
    {
        public UserController()
        {
        }

        // GET: /User/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            //Session["authenticated"] = false;
            return View();
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult Login(string email, string password)
        //{
        //    user user = this._userService.Login(email, password);

        //    if (user != null)
        //    {
        //        Session["authenticated"] = true;
        //        Session["email"] = user.email;
        //        Session["userId"] = user.userId;
        //        if (user.useProjectManagementTool == 1)
        //        {
        //            Session["useProjectManagementTool"] = "1";
        //        }
        //        else
        //        {
        //            Session["useProjectManagementTool"] = "0";
        //        }
        //        if (user.useCustomTool == 1)
        //        {
        //            Session["useCustomTool"] = "1";
        //        }
        //        else
        //        {
        //            Session["useCustomTool"] = "0";
        //        }


        //        LoginProcess(user, false);  //使用MVC內建登入並利用自訂權限[AuthorizeUser]功能


        //        if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == true)    //取得權限寫法
        //        {
        //            FormsIdentity id = (FormsIdentity)User.Identity;
        //            FormsAuthenticationTicket ticket = id.Ticket;

        //            var userData = ticket.UserData.Split(',');
        //        }

        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View();
        //}

        //[AllowAnonymous]
        //public ActionResult Logout()
        //{

        //    //使用者登出
        //    FormsAuthentication.SignOut();

        //    //清除所有的 session
        //    Session.RemoveAll();

        //    //建立一個同名的 Cookie 來覆蓋原本的 Cookie
        //    HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
        //    cookie1.Expires = DateTime.Now.AddYears(-1);
        //    Response.Cookies.Add(cookie1);

        //    //建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
        //    HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
        //    cookie2.Expires = DateTime.Now.AddYears(-1);
        //    Response.Cookies.Add(cookie2);

        //    //Session.Clear();
        //    Session.Abandon();

        //    return RedirectToAction("Login");
        //}

        // GET: /User/Add
        public ActionResult Add()
        {
            return View();
        }

        // GET: /User/List
        public ActionResult List()
        {
            return View();
        }

        // GET: /User/Edit
        public ActionResult Edit()
        {
            return View();
        }

        // GET: /User/EditPassword
        public ActionResult EditPassword()
        {
            return View();
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
        public ActionResult addUser(string email, string password, string repeatPassword)
        {
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult addGroup(string name)
        {
            return RedirectToAction("ListGroup");
        }
    }
}