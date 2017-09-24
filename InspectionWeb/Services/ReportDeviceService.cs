using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;
using InspectionWeb.Models.Misc;

namespace InspectionWeb.Services
{
    public class ReportDeviceService : IReportDeviceService
    {
        private IRepository<reportDevice> _repository;

        public ReportDeviceService(IRepository<reportDevice> repository)
        {
            _repository = repository;
        }

        public IResult Create(reportDevice instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                IdGenerator idg = new IdGenerator();
                instance.deviceId = idg.GetID("reportDevice");

                instance.createTime = DateTime.Now;
                instance.lastUpdateTime = instance.createTime;
                instance.x = 30;
                instance.y = 30;
                instance.isDelete = 0;


                this._repository.Create(instance);
                result.Success = true;
                result.Message = instance.deviceId;

            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(reportDevice instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                instance.lastUpdateTime = DateTime.Now;
                this._repository.Update(instance);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public IResult Update(reportDevice instance, string propertyName, object value)
        {
            Dictionary<string, object> DicUpdate = new Dictionary<string, object>();

            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {

                DateTime now = DateTime.Now;
                string lastUpdateTime = now.ToString("yyyy/MM/dd HH:mm:ss");

                DicUpdate.Add(propertyName, (string)value);
                DicUpdate.Add("lastUpdateTime", lastUpdateTime);

                _repository.Update(instance, DicUpdate);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public IResult Delete(string deviceId)
        {
            IResult result = new Result(false);

            if (!IsExists(deviceId))
            {
                result.Message = "找不到台車資料";
            }

            try
            {
                var instance = this.GetById(deviceId);
                this._repository.Update(instance, "isDelete", 1);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public bool IsExists(string deviceId)
        {
            return this._repository.GetAll().Any(x => x.deviceId == deviceId);
        }

        public reportDevice GetById(string deviceId)
        {
            return this._repository.Get(x => x.deviceId == deviceId);
        }

        public reportDevice GetByDeviceCode(string deviceCode)
        {
            return this._repository.Get(x => x.deviceCode == deviceCode);
        }

        public reportDevice GetByExhibitionItemId(string itemId)
        {
            return this._repository.Get(x => x.itemId == itemId);
        }

        public IEnumerable<reportDevice> GetAll()
        {
            return this._repository.GetAll().OrderBy(reportDevice => reportDevice.createTime).Where(x=> x.isDelete == 0);
        }
    }
}