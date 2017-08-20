using System;
using System.Web.Http;
using InspectionWeb.WebSupport;
using InspectionWeb.WebSupport.WebApi;
using System.Diagnostics;
using InspectionWeb.Models;
using InspectionWeb.Services.Interface;
using System.Collections.Generic;

namespace InspectionWeb.Controllers
{
    public class ServiceController : ApiController
    {
        private IAbnormalDefinitionService abnormalDefinitionService;
        private IAbnormalRecordService abnormalRecordService;
        private IExhibitionItemService exhibitionItemService;
        private IReportDeviceService reportDeviceService;
        private IReportSourceService reportSourceService;
        private ISoftwareVersionService softwareVersionService;

        public ServiceController(IAbnormalDefinitionService abnormalDefinitionService, IExhibitionItemService exhibitionItemService, 
            IAbnormalRecordService abnormalRecordService, IReportDeviceService reportDeviceService, IReportSourceService reportSourceService,
            ISoftwareVersionService softwareVersionService)
        {
            this.abnormalDefinitionService = abnormalDefinitionService;
            this.abnormalRecordService = abnormalRecordService;
            this.exhibitionItemService = exhibitionItemService;
            this.reportDeviceService = reportDeviceService;
            this.reportSourceService = reportSourceService;
            this.softwareVersionService = softwareVersionService;
        }

        /*
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        */

        // GET api/AbnormalDefinitionList
        [HttpGet]
        public IHttpActionResult AbnormalDefinitionList()
        {
            var result = -1;
            try
            {
                var abnormalDefinitionResponseData = new List<AbnormalDefinitionResponse>();
                var abnormalDefinitionList = this.abnormalDefinitionService.GetAll();
                foreach (abnormalDefinition item in abnormalDefinitionList)
                {
                    AbnormalDefinitionResponse abnormalDefinitionResponse = new AbnormalDefinitionResponse();
                    abnormalDefinitionResponse.code = item.abnormalCode;
                    abnormalDefinitionResponse.name = item.abnormalName;
                    abnormalDefinitionResponseData.Add(abnormalDefinitionResponse);
                }

                result = 0;
                var data = new { result = result, data = abnormalDefinitionResponseData };
                return Json(data);
            }
            catch (Exception e)
            {
            }
            return Json(new { result = result });
        }

        // GET api/AbnormalSourceList
        [HttpGet]
        public IHttpActionResult AbnormalSourceList()
        {
            var result = -1;
            try
            {
                var reportSourceResponseData = new List<ReportSourceResponse>();
                var reportSourceList = this.reportSourceService.GetAll();
                foreach (reportSource item in reportSourceList)
                {
                    ReportSourceResponse reportSourceResponse = new ReportSourceResponse();
                    reportSourceResponse.code = item.sourceCode;
                    reportSourceResponse.name = item.sourceName;
                    reportSourceResponseData.Add(reportSourceResponse);
                }

                result = 0;
                var data = new { result = result, data = reportSourceResponseData };
                return Json(data);
            }
            catch (Exception e)
            {
            }
            return Json(new { result = result });
        }

        // POST api/service/AbnormalAutoReport
        /**
         * support
         * Content-Type: application/x-www-form-urlencoded;charset=utf-8
         * Content-Type: application/json;charset=utf-8
         */
        [HttpPost]
        public IHttpActionResult AbnormalAutoReport([FromBody]AutoReportPost autoReportPost)
        {
            var result = -1;
            try
            {
                if (autoReportPost != null)
                {
                    if (!String.IsNullOrEmpty(autoReportPost.deviceCode))
                    {
                        // get data by device code
                        var reportDevice = this.reportDeviceService.GetByDeviceCode(autoReportPost.deviceCode);
                        // get data by abnormal code
                        var abnormalDefinition = this.abnormalDefinitionService.GetByAbnormalCode(autoReportPost.abnormalCode);
                        if (reportDevice != null && abnormalDefinition != null)
                        {
                            var abnormalRecord = new abnormalRecord();
                            abnormalRecord.recordId = IdGenerator.GetId("abnormalRecord");
                            abnormalRecord.itemId = reportDevice.itemId;
                            abnormalRecord.sourceId = reportDevice.sourceId;
                            abnormalRecord.abnormalId = abnormalDefinition.abnormalId;
                            abnormalRecord.deviceId = reportDevice.deviceId;
                            abnormalRecord.isClose = 0;
                            // get current time
                            DateTime now = DateTime.Now;
                            abnormalRecord.happenedTime = now;
                            abnormalRecord.createTime = now;
                            abnormalRecord.lastUpdateTime = now;

                            this.abnormalRecordService.Create(abnormalRecord);

                            result = 0;
                            var data = new { result = result, data = abnormalRecord.recordId };
                            return Json(data);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return Json(new { result = result });
        }

        // POST api/AbnormalUserReport
        [HttpPost]
        public IHttpActionResult AbnormalUserReport([FromBody]UserReportPost userReportPost)
        {
            var result = -1;
            try
            {
                if (userReportPost != null)
                {
                    if (!String.IsNullOrEmpty(userReportPost.itemCode))
                    {
                        // get item data 
                        var exhibitionItem = this.exhibitionItemService.GetByItemCode(userReportPost.itemCode);
                        if (exhibitionItem != null)
                        {
                            var abnormalRecord = new abnormalRecord();
                            abnormalRecord.recordId = IdGenerator.GetId("abnormalRecord");
                            // exhibition item
                            abnormalRecord.itemId = exhibitionItem.itemId;
                            // unable to determine abnormal situation
                            var reportSource = this.reportSourceService.GetBySourceCode("06"); //  06 is app
                            abnormalRecord.sourceId = reportSource.sourceId;
                            abnormalRecord.abnormalId = "";

                            var reportDevice = this.reportDeviceService.GetByExhibitionItemId(exhibitionItem.itemId);
                            if (exhibitionItem != null)
                            {
                                abnormalRecord.deviceId = reportDevice.deviceId;
                            }
                            abnormalRecord.description = String.Format("使用者 app 回報：{0}, \n\r說明：{1}", userReportPost.email + userReportPost.desc);
                            abnormalRecord.isClose = 0;
                            // get current time
                            DateTime now = DateTime.Now;
                            abnormalRecord.happenedTime = now;
                            abnormalRecord.createTime = now;
                            abnormalRecord.lastUpdateTime = now;

                            this.abnormalRecordService.Create(abnormalRecord);

                            result = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return Json(new { result = result });
        }

        // GET api/AbnormalStatus
        [HttpGet]
        public IHttpActionResult AbnormalStatus([FromUri]string recordId)
        {
            var result = -1;
            try
            {
                if (!String.IsNullOrEmpty(recordId))
                {
                    var abnormalRecord = this.abnormalRecordService.GetById(recordId);
                    if (abnormalRecord.isClose == 1)
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception e)
            {
            }
            return Json(new { result = result });
        }

        // GET api/CheckSoftwareVersion
        [HttpGet]
        public IHttpActionResult CheckSoftwareVersion([FromUri]string softwareCode)
        {
            var result = -1;
            try
            {
                if (!String.IsNullOrEmpty(softwareCode))
                {
                    var softwareVersion = this.softwareVersionService.GetBySoftwareCode(softwareCode);
                    if (softwareVersion != null)
                    {
                        result = 0;
                        var data = new { result = result, version = softwareVersion.version };
                        return Json(data);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return Json(new { result = result });
        }

        // POST api/DevicePeriodReport
        [HttpPost]
        public void DevicePeriodReport([FromBody]string deviceCode)
        {

        }
    }
}
