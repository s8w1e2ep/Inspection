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
    public class MailService : IMailService
    {
        private IRepository<systemSettings> _repository;

        public MailService(IRepository<systemSettings> repository)
        {
            _repository = repository;
        }


        public IResult Create(systemSettings instance)
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

        public IResult Create(string keyname, string value)
        {
            if (string.IsNullOrEmpty(keyname) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            systemSettings newSystemSettings = new systemSettings();

            if (IsRepeat(keyname))
            {
                result.ErrorMsg = "此系統參數已有,請重新申請";
            }
            else
            {
                try
                {
                    IdGenerator idg = new IdGenerator();
                    Encrypt encoder = new Encrypt();
                    string id = idg.GetID("id");
                    DateTime nowTime = DateTime.Now;

                    newSystemSettings.id = id;
                    newSystemSettings.keyName = keyname;
                    newSystemSettings.description = "系統寄信";
                    newSystemSettings.value = "09:10:00";
                    newSystemSettings.isDelete = 0;
                    newSystemSettings.createTime = nowTime;
                    newSystemSettings.lastUpdateTime = nowTime;

                    this._repository.Create(newSystemSettings);
                    result.Message = id;
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

        public IResult Update(systemSettings instance)
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

        public IResult Update(systemSettings instance, string key, object value)
        {
            Dictionary<string, object> DicUpdate = new Dictionary<string, object>();

            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                DicUpdate.Add(key, value);
                DateTime now = DateTime.Now;
                DicUpdate.Add("lastUpdateTime", now);
                this._repository.Update(instance, DicUpdate);
                result.Success = true;
                result.lastUpdateTime = now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public IResult Delete(string id)
        {
            IResult result = new Result(false);

            if (!IsExists(id))
            {
                result.ErrorMsg = "找不到系統參數資料";
            }

            try
            {
                var instance = this.GetByID(id);
                this._repository.Delete(instance);
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public systemSettings GetByID(string id)
        {
            return _repository.Get(x => x.id == id);
        }

        public IEnumerable<systemSettings> GetAll()
        {
            return _repository.GetAll().Where(x => x.isDelete == 0);
        }

        public bool IsRepeat(string keyName)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.keyName == keyName);
        }

        public bool IsExists(string keyName)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.keyName == keyName);
        }
    }
}