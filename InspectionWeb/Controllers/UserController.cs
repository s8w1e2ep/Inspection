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