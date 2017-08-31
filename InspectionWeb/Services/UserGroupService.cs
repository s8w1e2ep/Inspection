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
    public class UserGroupService : IUserGroupService
    {
        private IRepository<userGroup> _repository;

        public UserGroupService(IRepository<userGroup> repository)
        {
            _repository = repository;
        }

        public IResult Create(userGroup instance)
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

        public IResult Create(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            userGroup newGroup = new userGroup();

            if (IsRepeat(groupName))
            {
                result.ErrorMsg = "群組名稱已被使用,請重新申請";
            }
            else
            {
                try
                {
                    IdGenerator idg = new IdGenerator();
                    string groupId = idg.GetID("group");
                    DateTime nowTime = DateTime.Now;

                    newGroup.groupId = groupId;
                    newGroup.groupName = groupName;
                    newGroup.isDelete = 0;
                    newGroup.createTime = nowTime;
                    newGroup.lastUpdateTime = nowTime;

                    this._repository.Create(newGroup);
                    result.Success = true;
                    result.Message = groupId;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                    result.ErrorMsg = ex.ToString();
                }
            }

            return result;
        }

        public IResult Delete(string groupId)
        {
            IResult result = new Result(false);

            if (!IsExists(groupId))
            {
                result.ErrorMsg = "找不到使用者資料";
            }

            try
            {
                var instance = this.GetByID(groupId);
                this._repository.Delete(instance);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public IEnumerable<userGroup> GetAll()
        {
            return _repository.GetAll().Where(x => x.isDelete == 0);
        }

        public userGroup GetByID(string groupId)
        {
            return _repository.Get(x => x.isDelete == 0 && x.groupId == groupId);
        }

        public bool IsExists(string groupId)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.groupId == groupId);
        }

        public bool IsRepeat(string groupName)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.groupName == groupName);
        }

        public IResult Update(userGroup instance)
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

        public IResult Update(userGroup instance, string key, object value)
        {
            Dictionary<string, object> DicUpdateUser = new Dictionary<string, object>();

            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                string[] KB = { "active", "isDelete", "superUserOnly", "systemManagement", "userManagement", "dispatchManagement", "normalUser" };
                if (KB.Any(s => key.Contains(s)))
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