using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    public class StatisticController : Controller
    {
        public StatisticController()
        {
        }

        // GET: /Statistic/Exhibition
        public ActionResult Exhibition()
        {
            return View();
        }

        // GET: /Statistic/ExhibitionItem
        public ActionResult ExhibitionItem()
        {
            return View();
        }

        // GET: /Statistic/Facility
        public ActionResult Facility()
        {
            return View();
        }

        // GET: /Statistic/RankItem
        public ActionResult RankItem()
        {
            return View();
        }

        // GET: /Statistic/RankRecord
        public ActionResult RankReport()
        {
            return View();
        }


    }
}