using InspectionWeb.Models.Interface;
using InspectionWeb.Models.Misc;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;

namespace InspectionWeb.Services
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

        public IResult Create(string itemId, string sourceId, string abnormalId, string reporter)
        {
            if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(sourceId) 
                || string.IsNullOrEmpty(abnormalId) || string.IsNullOrEmpty(reporter) )
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            abnormalRecord newRecord = new abnormalRecord();

            if (IsRepeat(itemId))
            {
                result.ErrorMsg = "該展項已申請過, 請至檢修頁面查看";
            }
            else
            {
                try
                {
                    IdGenerator idg = new IdGenerator();
                    string recordId = idg.GetID("abnormalRecord");
                    DateTime nowTime = DateTime.Now;

                    newRecord.recordId = recordId;
                    newRecord.itemId = itemId;
                    newRecord.sourceId = sourceId;
                    newRecord.deviceId = reporter;      // 6000通報時deviceid = 通報者
                    newRecord.abnormalId = abnormalId;

                    newRecord.isDelete = 0;
                    newRecord.createTime = nowTime;
                    newRecord.lastUpdateTime = nowTime;

                    this._repository.Create(newRecord);

                    result.Message = recordId;
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                    result.ErrorMsg = ex.ToString();
                }
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

        public bool IsRepeat(string itemId)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.isClose == 0 && x.itemId == itemId);
        }

    }
}