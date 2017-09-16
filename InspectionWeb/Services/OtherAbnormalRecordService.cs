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
    public class OtherAbnormalRecordService : IOtherAbnormalRecordService
    {
        private IRepository<otherAbnormalRecord> _repository;

        public OtherAbnormalRecordService(IRepository<otherAbnormalRecord> repository)
        {
            _repository = repository;
        }

        public IResult Create(otherAbnormalRecord instance)
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

        public IResult Create(string name, string sourceId, string abnormalId, string reporter)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(sourceId)
                || string.IsNullOrEmpty(abnormalId) || string.IsNullOrEmpty(abnormalId))
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            otherAbnormalRecord newRecord = new otherAbnormalRecord();

            if (IsRepeat(name))
            {
                result.ErrorMsg = "該設施已申請過, 可至檢修頁面查看";
            }
            else
            {
                try
                {
                    IdGenerator idg = new IdGenerator();
                    string recordId = idg.GetID("OtherAboRecord");
                    DateTime nowTime = DateTime.Now;
                    
                    newRecord.recordId = recordId;
                    newRecord.name = name;
                    newRecord.sourceId = sourceId;
                    newRecord.deviceId = reporter;      // 6000通報時deviceid = 通報者
                    newRecord.abnormalId = abnormalId;

                    newRecord.isClose = 0;          
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

        //public IResult Create(string name, string sourceId, string abnormalId, string reporter)
        //{
        //    if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(sourceId)
        //        || string.IsNullOrEmpty(abnormalId) || string.IsNullOrEmpty(reporter))
        //    {
        //        throw new ArgumentNullException();
        //    }

        //    IResult result = new Result(false);
        //    otherAbnormalRecord newRecord = new abnormalRecord();

        //    if (IsRepeat(itemId))
        //    {
        //        result.ErrorMsg = "該展項已申請過, 可至檢修頁面查看";
        //    }
        //    else
        //    {
        //        try
        //        {
        //            IdGenerator idg = new IdGenerator();
        //            string recordId = idg.GetID("abnormalRecord");
        //            DateTime nowTime = DateTime.Now;

        //            newRecord.recordId = recordId;
        //            newRecord.itemId = itemId;
        //            newRecord.sourceId = sourceId;
        //            newRecord.deviceId = reporter;      // 6000通報時deviceid = 通報者
        //            newRecord.abnormalId = abnormalId;

        //            newRecord.isClose = 1;          //暫時 isrepeat也要記得家回去isclose()
        //            newRecord.isDelete = 0;
        //            newRecord.createTime = nowTime;
        //            newRecord.lastUpdateTime = nowTime;

        //            this._repository.Create(newRecord);

        //            result.Message = recordId;
        //            result.Success = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            result.Exception = ex;
        //            result.ErrorMsg = ex.ToString();
        //        }
        //    }
        //    return result;
        //}

        //public IResult Update(abnormalRecord instance)
        //{
        //    if (instance == null)
        //    {
        //        throw new ArgumentNullException();
        //    }

        //    IResult result = new Result(false);

        //    try
        //    {
        //        this._repository.Update(instance);
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Exception = ex;
        //    }

        //    return result;
        //}

        public IResult Update(otherAbnormalRecord instance, string propertyName, object value)
        {
            Dictionary<string, object> DicUpdate = new Dictionary<string, object>();

            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                if (propertyName == "isClose" || propertyName == "isDelete")
                {
                    int iValue;
                    bool ret = JsonValue2Int(value, out iValue);

                    if (ret == true)
                    {
                        value = Convert.ToByte(iValue);
                    }
                    else //轉換異常寫入預設值
                    {
                        value = Convert.ToByte(1); ;
                    }
                }
                else if (propertyName == "happenedTime" || propertyName == "fixDate")
                {
                    value = Convert.ToDateTime(value);
                }
                else if (propertyName == "fixMethod")
                {
                    value = Convert.ToInt16(value);
                }

                DateTime now = DateTime.Now;
                string lastUpdateTime = now.ToString("yyyy-MM-dd HH:mm:ss");

                DicUpdate.Add(propertyName, value);
                DicUpdate.Add("lastUpdateTime", now);

                _repository.Update(instance, DicUpdate);
                result.Success = true;
                result.lastUpdateTime = lastUpdateTime;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        private bool JsonValue2Int(object value, out int transValue)
        {

            if (value == null)   //介面未選擇Yes 會傳回null,所以寫入0(No)
            {
                transValue = 0;
                return true;

            }
            else if (Int32.TryParse((string)value, out transValue))
            {
                return true;
            }
            else //轉換異常
            {
                return false;
            }
        }
        //public IResult Delete(string recordId)
        //{
        //    IResult result = new Result(false);

        //    if (!IsExists(recordId))
        //    {
        //        result.Message = "找不到台車資料";
        //    }

        //    try
        //    {
        //        var instance = this.GetById(recordId);
        //        this._repository.Update(instance, "isDelete", 1);
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Exception = ex;
        //    }
        //    return result;
        //}

        //public bool IsExists(string recordId)
        //{
        //    return this._repository.GetAll().Any(x => x.recordId == recordId);
        //}

        public otherAbnormalRecord GetById(string recordId)
        {
            return this._repository.Get(x => x.recordId == recordId);
        }

        public IEnumerable<otherAbnormalRecord> GetAll()
        {
            return this._repository.GetAll().OrderBy(otherAbnormalRecord => otherAbnormalRecord.createTime);
        }

        //public IEnumerable<abnormalRecord> GetUnhandledRecords()
        //{
        //    return this._repository.GetAll().Where(abnormalRecord => abnormalRecord.isClose == 0);
        //}

        public bool IsRepeat(string name)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.name == name);
        }
    }
}
