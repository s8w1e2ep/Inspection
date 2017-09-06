using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;

namespace InspectionWeb.Services
{
    public class ItemCheckRecordService : IItemCheckRecordService
    {
        private IRepository<itemCheckRecord> _repository;

        public ItemCheckRecordService(IRepository<itemCheckRecord> repository)
        {
            this._repository = repository;
        }

        public IResult Create(itemCheckRecord instance)
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

        public IResult Create(System.DateTime date)
        {
            IResult result = new Result(false);
            bool hasRecord = IsExists(date);
            //先找是否已經建立
            if (hasRecord)
            {
                return result;
            }
            else
            {
                return result;
            }
        }

        public IResult Update(itemCheckRecord instance)
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

        public IResult Update(itemCheckRecord instance, string propertyName, object value)
        {
            Dictionary<string, object> DicUpdate = new Dictionary<string, object>();

            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {

                DateTime now = DateTime.Now;
                string lastUpdateTime = now.ToString("yyyy/MM/dd HH:mm:ss");

                DicUpdate.Add(propertyName, (string)value);
                DicUpdate.Add("lastUpdateTime", lastUpdateTime);

                _repository.Update(instance, DicUpdate);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public IResult Delete(string checkId)
        {
            IResult result = new Result(false);

            if (!IsExists(checkId))
            {
                result.Message = "找不到紀錄資料";
            }

            try
            {
                var instance = this.GetById(checkId);
                Dictionary<string, object> DicUpdate = new Dictionary<string, object>();
                DicUpdate.Add("isDelete", Convert.ToByte(1));
                DicUpdate.Add("lastUpdateTime", DateTime.Now);
                this._repository.Update(instance, DicUpdate);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public bool IsExists(string checkId)
        {
            return this._repository.GetAll().Any(x => x.checkId == checkId && x.isDelete == 0);
        }

        public bool IsExists(System.DateTime date)
        {
            return this._repository.GetAll().Any(x => x.checkDate == date);
        }

        public itemCheckRecord GetById(string checkId)
        {
            return this._repository.Get(x => x.checkId == checkId);
        }

        public IEnumerable<itemCheckRecord> GetAll()
        {
            return this._repository.GetAll().Where(x => x.isDelete == 0).OrderBy(roomCheckRecord => roomCheckRecord.createTime);
        }
    }
}