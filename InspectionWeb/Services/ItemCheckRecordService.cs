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
                result.Message = "找不到台車資料";
            }

            try
            {
                var instance = this.GetById(checkId);
                this._repository.Update(instance, "isDelete", 1);
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
            return this._repository.GetAll().Any(x => x.checkId == checkId);
        }

        public itemCheckRecord GetById(string checkId)
        {
            return this._repository.Get(x => x.checkId == checkId);
        }

        public IEnumerable<itemCheckRecord> GetAll()
        {
            return this._repository.GetAll().OrderBy(roomCheckRecord => roomCheckRecord.createTime);
        }
    }
}