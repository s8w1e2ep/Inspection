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
                    string groupId = idg.GetUserGroupNewID();
                    DateTime nowTime = DateTime.Now;

                    newGroup.groupId = groupId;
                    newGroup.groupName = groupName;
                    newGroup.isDelete = 0;
                    newGroup.createTime = nowTime;
                    newGroup.lastUpdateTime = nowTime;

                    this._repository.Create(newGroup);
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
            return _repository.GetAll();
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
            throw new NotImplementedException();
        }

        public IResult Update(userGroup instance, string key, object value)
        {
            throw new NotImplementedException();
        }
    }
}