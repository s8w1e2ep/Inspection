using System;
using System.Web;
using System.Web.Mvc;
using InspectionWeb.Models;
using System.Collections.Generic;
using InspectionWeb.Services.Interface;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Normal = true)]
    public class StatisticController : Controller
    {
        private IExhibitionRoomService _roomService;
        private IExhibitionItemService _itemService;
        private IRoomCheckRecordService _roomCheckService;
        private IAbnormalRecordService _abnormalService;

        public StatisticController(IExhibitionRoomService service, IExhibitionItemService service2, IRoomCheckRecordService service3, IAbnormalRecordService service4)
        {
            this._roomService = service;
            this._itemService = service2;
            this._roomCheckService = service3;
            this._abnormalService = service4;
        }

        // GET: /Statistic/Exhibition
        public ActionResult Exhibition()
        {
            ViewBag.rooms = this._roomService.GetAll();

            return View();
        }

        // GET: /Statistic/ExhibitionItem
        public ActionResult ExhibitionItem()
        {
            ViewBag.rooms = this._roomService.GetAll();
            return View();
        }

        // GET: /Statistic/Items
        public ActionResult Items(string roomId)
        {
            var items = this._itemService.GetAll().Where(x => x.roomId == roomId && x.itemType == 0);
            return Json(items.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: /Statistic/Facility
        public ActionResult Facility()
        {
            ViewBag.facilities = this._itemService.GetAll().Where(x => x.itemType == 1);
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

        // GET: /Statistic/QueryDispatch
        public ActionResult QueryDispatch(string roomId, string startDate, string endDate, string type)
        {
            System.Diagnostics.Debug.WriteLine("TE0: \n\n" + roomId + ", " + type + ", " + startDate + ", " + endDate);

            if (type == "man" || type == "auto")
            {             
                // 選出符合日期的展示廳巡檢紀錄
                var roomRecords = this._roomCheckService.GetAllByDateRange(startDate, endDate, roomId);
                List<RecordJson> records = new List<RecordJson>();

                // 選出展示廳所有展項
                foreach (var record in roomRecords)
                {
                    // 取得展示廳紀錄內當天展項
                    var items = this._itemService.GetAll().Where(x => x.roomId == roomId && x.lastUpdateTime <= record.checkDate).ToList();
                    // 取得展項數量
                    int num = items.Count;
                    //確認異常數量
                    int abNum = 0;
                    foreach (var item in items)
                    {
                        if (this._abnormalService.GetAll().Any(x => x.itemId == item.itemId &&
                                                               x.happenedTime == record.checkDate &&
                                                               x.fixDate > record.checkDate))
                        {
                            abNum++;
                        }
                    }
                    RecordJson re = new RecordJson();
                    re.dispatchDate = record.checkDate;
                    re.total = items.Count;
                    re.normal = re.total - abNum;
                    if (re.total == 0)
                    {
                        re.prob = 0;
                    }
                    else
                    {
                        re.prob = (int)((float)re.normal / re.total * 100);
                    }
                    records.Add(re);
                }

                if (records.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("TE4: \n\n");
                    return Json("[]", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("TE5 " + records.Count);
                    return Json(records, JsonRequestBehavior.AllowGet);
                }
            }
            else if (type == "auto")
            {
                return Json("[]");
            }

            return Json("[]", JsonRequestBehavior.AllowGet);
        }

        public class RecordJson
        {
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
            public Nullable<DateTime> dispatchDate { get; set; }
            public int total { get; set; }
            public int normal { get; set; }
            public int prob { get; set; }
        }

    }
}