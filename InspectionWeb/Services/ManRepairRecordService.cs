using InspectionWeb.Models;
using InspectionWeb.Models.Interface;
using InspectionWeb.Models.Misc;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InspectionWeb.Services
{
    public class ManRepairRecordService : IManRepairRecordService
    {
        private IRepository<manRepairRecord> _repository;

        public ManRepairRecordService(IRepository<manRepairRecord> repository)
        {
            _repository = repository;
        }

        public IResult Create(manRepairRecord instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Create(instance);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public IResult Create(string recordId, string filluserId)
        {
            if (string.IsNullOrEmpty(recordId) || string.IsNullOrEmpty(filluserId))
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            manRepairRecord newRecord = new manRepairRecord();
            try
            {
                    
                DateTime nowTime = DateTime.Now;

                newRecord.recordId = recordId;
                newRecord.fillUserId = filluserId;

                newRecord.isDelete = 0;
                newRecord.createTime = nowTime;
                newRecord.lastUpdateTime = nowTime;

                this._repository.Create(newRecord);
                  
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.ErrorMsg = ex.ToString();
            }
            
            return result;
        }

        public IResult Update(manRepairRecord instance)
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

        public IResult Update(manRepairRecord instance, string key, object value)
        {
            Dictionary<string, object> DicUpdateUser = new Dictionary<string, object>();

            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                if (key == "isDelete")
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
                else if(key == "expectDate")
                {
                    value = Convert.ToDateTime(value);
                }

                DicUpdateUser.Add(key, value);
                DateTime now = DateTime.Now;
                DicUpdateUser.Add("lastUpdateTime", now);
                this._repository.Update(instance, DicUpdateUser);
                result.Success = true;
                result.lastUpdateTime = now.ToString("yyyy-MM-dd HH:mm:ss");
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

        private bool JsonValue2Short(object value, out short transValue)
        {

            if (value == null)   //介面未選擇Yes 會傳回null,所以寫入0(No)
            {
                transValue = 0;
                return true;
            }
            else if (Int16.TryParse((string)value, out transValue))
            {
                return true;
            }
            else //轉換異常寫入預設值
            {
                return false;
            }
        }

        public IResult Delete(string userId)
        {
            IResult result = new Result(false);

            if (!IsExists(userId))
            {
                result.ErrorMsg = "找不到使用者資料";
            }

            try
            {
                var instance = this.GetByID(userId);
                this._repository.Delete(instance);
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
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.recordId == recordId);
        }

        public manRepairRecord GetByID(string recordId)
        {
            return _repository.Get(x => x.recordId == recordId);
        }


        public IEnumerable<manRepairRecord> GetAll()
        {
            return _repository.GetAll().Where(x => x.isDelete == 0);
        }

    }
}
