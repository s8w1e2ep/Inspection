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

            if(IsRepeat(account))
            {
                result.ErrorMsg = "帳號已被使用,請重新申請";
            }
            else
            {
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
                }
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
                System.Diagnostics.Debug.WriteLine("P5 :\n\n" + ex.ToString());
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
                    this._repository.Update(user, "lastUpdateTime", DateTime.Now);
                    result.Success = true;
                }
            }

            return result;
        }

        public bool IsExists(string userId)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.userId == userId);
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

        public bool IsRepeat(string userCode)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.userCode == userCode);
        }
    }
}