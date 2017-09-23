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
    public class SolutionService : ISolutionService
    {
        private IRepository<quickSolution> _repository;

        public SolutionService(IRepository<quickSolution> repository)
        {
            _repository = repository;
        }

        public IResult Create(quickSolution instance)
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

        public IResult Create(string name, string description)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            quickSolution newSolution = new quickSolution();

            if (IsRepeat(name))
            {
                result.ErrorMsg = "該名稱已使用過";
            }
            else
            {
                try
                {
                    IdGenerator idg = new IdGenerator();
                    string solutionId = idg.GetID("quickSolution");
                    DateTime nowTime = DateTime.Now;

                    newSolution.solutionId = solutionId;
                    newSolution.name = name;
                    newSolution.description = description;
                    newSolution.isDelete = 0;
                    newSolution.createTime = nowTime;
                    newSolution.lastUpdateTime = nowTime;

                    this._repository.Create(newSolution);
                    result.Message = solutionId;
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

        //public IResult Update(quickSolution instance)
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

        public IResult Update(quickSolution instance, string key, object value)
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

        //private bool JsonValue2Short(object value, out short transValue)
        //{

        //    if (value == null)   //介面未選擇Yes 會傳回null,所以寫入0(No)
        //    {
        //        transValue = 0;
        //        return true;
        //    }
        //    else if (Int16.TryParse((string)value, out transValue))
        //    {
        //        return true;
        //    }
        //    else //轉換異常寫入預設值
        //    {
        //        return false;
        //    }
        //}

        //public IResult Delete(string userId)
        //{
        //    IResult result = new Result(false);

        //    if (!IsExists(userId))
        //    {
        //        result.ErrorMsg = "找不到使用者資料";
        //    }

        //    try
        //    {
        //        var instance = this.GetByID(userId);
        //        this._repository.Delete(instance);
        //        result.Success = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        result.Exception = ex;
        //    }

        //    return result;
        //}



        //public bool IsExists(string userId)
        //{
        //    return this._repository.GetAll().Any(x => x.isDelete == 0 && x.solutionId == userId);
        //}

        public quickSolution GetByID(string solutionID)
        {
            return _repository.Get(x => x.solutionId == solutionID);
        }

       
        //取出未被刪除的資料
        public IEnumerable<quickSolution> GetAll()
        {
            return _repository.GetAll().Where(x => x.isDelete == 0);
        }

      


        public bool IsRepeat(string name)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.name == name);
        }
    }
}
