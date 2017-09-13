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
        private IItemCheckRecordService _itemCheckService;
        private IAbnormalRecordService _abnormalService;
        private IOtherAbnormalRecordService _otherService;
        private IReportSourceService _reportService;
        private IAbnormalDefinitionService _defineService;

        public StatisticController(IExhibitionRoomService service, IExhibitionItemService service2, 
                                   IRoomCheckRecordService service3, IItemCheckRecordService service4,
                                   IAbnormalRecordService service5, IOtherAbnormalRecordService service6,
                                   IReportSourceService service7, IAbnormalDefinitionService service8)
        {
            this._roomService = service;
            this._itemService = service2;
            this._roomCheckService = service3;
            this._itemCheckService = service4;
            this._abnormalService = service5;
            this._otherService = service6;
            this._reportService = service7;
            this._defineService = service8;
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

        // GET: /Statistic/QueryRoom
        public ActionResult QueryRoom(string roomId, string startDate, string endDate, string type)
        {
            // 選出符合日期的展示廳巡檢紀錄
            var roomRecords = this._roomCheckService.GetAllByDateRange(startDate, endDate, roomId);
            List<RecordJson> records = new List<RecordJson>();

            foreach (var record in roomRecords)
            {
                // 取得展示廳紀錄內當天所有展項
                var items = this._itemService.GetAll().Where(x => x.roomId == roomId && x.lastUpdateTime <= record.checkDate).ToList();
                // 取得展項數量
                int num = items.Count;
                int normal = items.Count;
                // ∀ item, check it if abnormal
                if (type == "man")
                {
                    foreach (var item in items)
                    {
                        if (this._abnormalService.GetAll().Any(x => x.itemId == item.itemId &&
                                                               x.happenedTime == record.checkDate &&
                                                               x.sourceId == "01" || x.sourceId == "02"))
                        {
                            normal--;
                        }
                    }
                }
                else if (type == "auto")
                {
                    foreach (var item in items)
                    {
                        if (this._abnormalService.GetAll().Any(x => x.itemId == item.itemId &&
                                                               x.happenedTime == record.checkDate &&
                                                               x.sourceId == "00" || x.sourceId == "03" ||
                                                               x.sourceId == "04" || x.sourceId == "05" ||
                                                               x.sourceId == "06"))
                        {
                            normal--;
                        }
                    }
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                
                RecordJson re = new RecordJson();
                re.dispatchDate = record.checkDate.ToString();
                re.total = num;
                re.normal = normal;
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
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(records, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: /Statistic/QueryItem
        public ActionResult QueryItem(string roomId, string itemId, string startDate, string endDate, string type)
        {
            // 選出符合日期的展示廳巡檢紀錄
            var roomRecords = this._roomCheckService.GetAllByDateRange(startDate, endDate, roomId);
            List<RecordJson> records = new List<RecordJson>();
            // 取得展項
            var item = this._itemService.GetById(itemId);
            //確認異常數量
            int normal = 1;

            foreach (var record in roomRecords)
            {
                // Check item if abnormal
                if (type == "man")
                {
                    if (this._abnormalService.GetAll().Any(x => x.itemId == item.itemId &&
                                                           x.happenedTime == record.checkDate &&
                                                           x.sourceId == "01" || x.sourceId == "02"))
                    {
                        normal--;
                    }
                }
                else if (type == "auto")
                {
                    if (this._abnormalService.GetAll().Any(x => x.itemId == item.itemId &&
                                                               x.happenedTime == record.checkDate &&
                                                               x.sourceId == "00" || x.sourceId == "03" ||
                                                               x.sourceId == "04" || x.sourceId == "05" ||
                                                               x.sourceId == "06"))
                    {
                        normal--;
                    }
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

                RecordJson re = new RecordJson();
                re.dispatchDate = record.checkDate.ToString();
                re.total = 1;
                re.normal = normal;
                re.prob = (int)((float)re.normal / re.total * 100);

                records.Add(re);
            }

            if (records.Count == 0)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(records, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: /Statistic/QueryFacility
        public ActionResult QueryFacility(string facilityId, string startDate, string endDate, string type)
        {
            // 選出符合日期的體驗設施巡檢紀錄
            var itemRecords = this._itemCheckService.GetAllByDateRange(startDate, endDate, facilityId);
            List<RecordJson> records = new List<RecordJson>();
            // 取得展項
            var item = this._itemService.GetById(facilityId);
            //確認異常數量
            int normal = 1;

            foreach (var record in itemRecords)
            {
                // Check item if abnormal
                if(type == "man")
                {
                    if (this._abnormalService.GetAll().Any(x => x.itemId == item.itemId &&
                                                            x.happenedTime == record.checkDate &&
                                                            x.sourceId == "01" || x.sourceId == "02"))
                    {
                        normal--;
                    }
                }
                else if(type == "auto")
                {
                    if (this._abnormalService.GetAll().Any(x => x.itemId == item.itemId &&
                                                                x.happenedTime == record.checkDate &&
                                                                x.sourceId == "00" || x.sourceId == "03" ||
                                                                x.sourceId == "04" || x.sourceId == "05" ||
                                                                x.sourceId == "06"))
                    {
                        normal--;
                    }
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                
                RecordJson re = new RecordJson();
                re.dispatchDate = record.checkDate.ToString();
                re.total = 1;
                re.normal = normal;
                re.prob = (int)((float)re.normal / re.total * 100);

                records.Add(re);
            }

            if (records.Count == 0)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(records, JsonRequestBehavior.AllowGet);
            }  
        }

        // GET: /Statistic/QueryRankItem
        public ActionResult QueryRankItem(string startDate, string endDate)
        {
            List<RankItemJson> ranks = new List<RankItemJson>();
            // 取出全部展項
            var items = this._itemService.GetAll().Where(x => x.roomId != null).ToList();
            int sum = 0;

            foreach(var data in items)
            {
                RankItemJson rank = new RankItemJson();
                rank.itemId = data.itemId;
                rank.itemName = data.itemName == null ? "" : data.itemName;
                
                if(this._roomService.GetById(data.roomId) == null)
                {
                    rank.roomName = "";
                }
                else
                {
                    rank.roomName = this._roomService.GetById(data.roomId).roomName == null ? "" : this._roomService.GetById(data.roomId).roomName;
                }
                rank.manNum = this._abnormalService.GetAll().Where(x => x.itemId == data.itemId && x.happenedTime >= Convert.ToDateTime(startDate) && x.happenedTime <= Convert.ToDateTime(endDate) && x.sourceId == "01" || x.sourceId == "02").ToList().Count;
                rank.autoNum = this._abnormalService.GetAll().Where(x => x.itemId == data.itemId && x.happenedTime >= Convert.ToDateTime(startDate) && x.happenedTime <= Convert.ToDateTime(endDate) && x.sourceId == "00" || x.sourceId == "03" || x.sourceId == "04" || 
                                                                    x.sourceId == "05" || x.sourceId == "06" ).ToList().Count;
                sum += rank.manNum + rank.autoNum;
                ranks.Add(rank);
            }

            if (ranks.Count == 0)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = ranks, total = sum }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: /Statistic/QueryRankReport
        public ActionResult QueryRankReport(string startDate, string endDate)
        {
            List<RankReportJson> ranks = new List<RankReportJson>();
            // 取出每個通報來源
            var sources = this._reportService.GetAll();
            int sum = 0;

            foreach(var source in sources)
            {
                RankReportJson rank = new RankReportJson();
                rank.sourceId = source.sourceId;
                rank.sourceName = source.sourceName;
                rank.abNum = this._abnormalService.GetAll().Where(x => x.sourceId == source.sourceId).ToList().Count +
                             this._otherService.GetAll().Where(x => x.sourceId == source.sourceId).ToList().Count;
                sum += rank.abNum;

                ranks.Add(rank);
            }

            if (ranks.Count == 0)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = ranks, total = sum }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: /Statistic/QueryRankReport
        public ActionResult RankItemList(string itemId, string startDate, string endDate)
        {
            ViewBag.item = this._itemService.GetById(itemId);
            var abnormals = this._abnormalService.GetAll().Where(x => x.itemId == itemId && x.happenedTime >= Convert.ToDateTime(startDate) && x.happenedTime <= Convert.ToDateTime(endDate)).ToList();
            List<ItemListJson> lists = new List<ItemListJson>();

            foreach(var abnormal in abnormals)
            {
                ItemListJson ele = new ItemListJson();
                ele.abName = this._defineService.GetById(abnormal.abnormalId).abnormalName;
                ele.desc = abnormal.description;
                ele.sourceName = this._reportService.GetById(abnormal.sourceId).sourceName;
                ele.isClose = abnormal.isClose == 1 ? "已結案" : "未結案";
                ele.happen = abnormal.happenedTime;
                lists.Add(ele);
            }
            ViewBag.lists = lists;

            return View();
        }

        // GET: /Statistic/RankReportList
        public ActionResult RankReportList(string sourceId, string startDate, string endDate)
        {
            ViewBag.source = this._reportService.GetById(sourceId);
            var abnormals = this._abnormalService.GetAll().Where(x => x.sourceId == sourceId).ToList();
            var others = this._otherService.GetAll().Where(x => x.sourceId == sourceId).ToList();
            List<ItemListJson> lists = new List<ItemListJson>();

            foreach (var abnormal in abnormals)
            {
                ItemListJson ele = new ItemListJson();
                if(this._defineService.GetById(abnormal.abnormalId) == null)
                {
                    ele.abName = "找不到異常定義名稱";
                }
                else
                {
                    ele.abName = this._defineService.GetById(abnormal.abnormalId).abnormalName == null ? "" : this._defineService.GetById(abnormal.abnormalId).abnormalName;
                }
                
                ele.desc = abnormal.description;
                ele.sourceName = this._itemService.GetById(abnormal.itemId).itemName;
                ele.isClose = abnormal.isClose == 1 ? "已結案" : "未結案";
                ele.happen = abnormal.happenedTime;
                lists.Add(ele);
            }
            foreach (var abnormal in others)
            {
                ItemListJson ele = new ItemListJson();
                if(this._defineService.GetById(abnormal.abnormalId) == null)
                {
                    ele.abName = "";
                }
                else
                {
                    ele.abName = this._defineService.GetById(abnormal.abnormalId).abnormalName == null ? "" : this._defineService.GetById(abnormal.abnormalId).abnormalName;
                }
                
                ele.desc = abnormal.description;
                ele.sourceName = abnormal.name;
                ele.isClose = abnormal.isClose == 1 ? "已結案" : "未結案";
                ele.happen = abnormal.happenedTime;
                lists.Add(ele);
            }
            ViewBag.lists = lists;

            return View();
        }

        public class RecordJson
        {
            public string dispatchDate { get; set; }
            public int total { get; set; }
            public int normal { get; set; }
            public int prob { get; set; }
        }

        public class RankItemJson
        {
            public string itemId { get; set; }
            public string itemName { get; set; }
            public string roomName { get; set; }
            public int manNum { get; set; }
            public int autoNum { get; set; }
        }

        public class RankReportJson
        {
            public string sourceId { get; set; }
            public string sourceName { get; set; }
            public int abNum { get; set; }
        }

        public class ItemListJson
        {
            public string abName { set; get; }
            public string desc { set; get; }
            public string sourceName { set; get; }
            public string isClose { set; get; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
            public Nullable<DateTime> happen { set; get; }
        }

    }
}