﻿using InspectionWeb.Models.Interface;
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
                result.ErrorMsg = "該項目已申請過, 可至查詢頁面查看";
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
                    newRecord.deviceId = reporter;      
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
                if (propertyName == "isClose" || propertyName == "isDelete" )
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
                else if(propertyName == "happenedTime" || propertyName == "fixDate")
                {
                    value = Convert.ToDateTime(value);
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
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.itemId == itemId && x.isClose == 0);
        }

    }
}