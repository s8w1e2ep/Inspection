using System;
using System.Linq;
using System.Collections.Generic;
using InspectionWeb.Models;
using InspectionWeb.Models.Misc;
using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;

namespace InspectionWeb.Services
{
    public class CompanyService : ICompanyService
    {
        private IRepository<company> _repository;

        public CompanyService(IRepository<company> repository)
        {
            _repository = repository;
        }

        public IResult Create(company instance)
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

        public IResult Create(string companyName)
        {
            IResult result = new Result(false);
            company com = new company();

            if (IsRepeat(companyName))
            {
                result.ErrorMsg = "公司名稱已被使用,請重新申請";
            }
            else
            {
                try
                {
                    IdGenerator idg = new IdGenerator();
                    string companyId = idg.GetID("company");
                    DateTime nowTime = DateTime.Now;

                    com.companyId = companyId;
                    com.companyName = companyName;
                    com.isDelete = 0;
                    com.createTime = nowTime;
                    com.lastUpdateTime = nowTime;

                    this._repository.Create(com);
                    result.Success = true;
                    result.Message = companyId;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                    result.ErrorMsg = ex.ToString();
                }
            }

            return result;
        }

        public IResult Delete(string companyId)
        {
            IResult result = new Result(false);

            if (!IsExists(companyId))
            {
                result.ErrorMsg = "找不到公司資料";
            }

            try
            {
                var instance = this.GetByID(companyId);
                this._repository.Delete(instance);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public IEnumerable<company> GetAll()
        {
            return _repository.GetAll().Where(x => x.isDelete == 0);
        }

        public company GetByID(string companyId)
        {
            return _repository.Get(x => x.isDelete == 0 && x.companyId == companyId);
        }

        public bool IsExists(string companyId)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.companyId == companyId);
        }

        public bool IsRepeat(string companyName)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.companyName == companyName);
        }

        public IResult Update(company instance)
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

        public IResult Update(company instance, string key, object value)
        {
            Dictionary<string, object> DicUpdateUser = new Dictionary<string, object>();

            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                if (key.Contains("isDelete"))
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
    }
}
