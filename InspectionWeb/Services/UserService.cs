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
    public class UserService : IUserService
    {
        private IRepository<user> _repository;

        public UserService(IRepository<user> repository)
        {
            _repository = repository;
        }

        public IResult Create(user instance)
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

        public IResult Create(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            user newUser = new user();

            try
            {
                IdGenerator idg = new IdGenerator();
                Encrypt encoder = new Encrypt();
                string userId = idg.GetUserNewID();
                string encodePassword = encoder.EncryptSHA(password);
                DateTime nowTime = DateTime.Now;

                newUser.userId = userId;
                newUser.userCode = account;
                newUser.password = encodePassword;
                newUser.active = 0;
                newUser.isDelete = 0;
                newUser.createTime = nowTime;
                newUser.lastUpdateTime = nowTime;

                this._repository.Create(newUser);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.ErrorMsg = ex.ToString();

                // 要先判斷是否 isDelete 再判斷重複
                //if (((System.Data.SqlClient.SqlException)((ex.InnerException).InnerException)).Number == 2627)
                //{
                //    result.ErrorMsg = "Email 已被使用,請重新申請";
                //    //result.ErrorMsg = "";
                //}
            }

            return result;
        }

        public IResult Update(user instance)
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

        public IResult Update(user instance, string key, object value)
        {
            Dictionary<string, object> DicUpdateUser = new Dictionary<string, object>();

            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                if (key == "active")
                {
                    short sValue;
                    bool ret = JsonValue2Short(value, out sValue);

                    if (ret == true)
                    {
                        value = sValue;
                    }
                    else //轉換異常寫入預設值
                    {
                        value = 1;
                    }
                }

                DicUpdateUser.Add(key, value);
                DicUpdateUser.Add("lastUpdateTime", DateTime.Now);

                this._repository.Update(instance, DicUpdateUser);
                result.Success = true;
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

        public IResult changePassword(string userId, string oldPassword, string newPassword)
        {
            IResult result = new Result(false);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentNullException();
            }

            var user = this.GetByID(userId);

            if (user != null)
            {
                Encrypt encoder = new Encrypt();
                string encodePassword = encoder.EncryptSHA(oldPassword);

                if (encodePassword == user.password)
                {
                    this._repository.Update(user, "password", encoder.EncryptSHA(newPassword));
                    result.Success = true;
                }
            }

            return result;
        }

        public bool IsExists(string userId)
        {
            return this._repository.GetAll().Any(x => x.userId == userId);
        }

        public user GetByID(string userId)
        {
            return _repository.Get(x => x.userId == userId);
        }

        public user GetByAccount(string account)
        {
            return _repository.Get(x => x.userCode == account);
        }

        public IEnumerable<user> GetAll()
        {
            return _repository.GetAll();
        }

        public user Login(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException();
            }

            var user = this.GetByAccount(account);

            if (user != null)
            {
                Encrypt encoder = new Encrypt();
                string encodePassword = encoder.EncryptSHA(password);

                if (encodePassword == user.password)
                {
                    return user;
                }
            }
            return null;
        }
    }
}