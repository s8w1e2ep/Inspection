using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;

namespace InspectionWeb.Service
{
    public class AbnormalRecordService : IAbnormalRecordService
    {
        private IRepository<abnormalRecord> _repository;

        public AbnormalRecordService(IRepository<abnormalRecord> repository)
        {
            _repository = repository;
        }

        public IResult Create(abnormalRecord instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                // create a new record
                this._repository.Create(instance);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(abnormalRecord instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Update(instance);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public IResult Update(abnormalRecord instance, string propertyName, object value)
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

        public IResult Delete(string recordId)
        {
            IResult result = new Result(false);

            if (!IsExists(recordId))
            {
                result.Message = "找不到台車資料";
            }

            try
            {
                var instance = this.GetById(recordId);
                this._repository.Update(instance, "isDelete", 1);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public bool IsExists(string recordId)
        {
            return this._repository.GetAll().Any(x => x.recordId == recordId);
        }

        public abnormalRecord GetById(string recordId)
        {
            return this._repository.Get(x => x.recordId == recordId);
        }

        public IEnumerable<abnormalRecord> GetAll()
        {
            return this._repository.GetAll().OrderBy(abnormalRecord => abnormalRecord.createTime);
        }

        public IEnumerable<abnormalRecord> GetUnhandledRecords()
        {
            return this._repository.GetAll().Where(abnormalRecord => abnormalRecord.isClose == 0);
        }
    }
}