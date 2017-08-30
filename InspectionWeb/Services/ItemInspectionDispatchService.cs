using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;

namespace InspectionWeb.Service
{
    public class ItemInspectionDispatchService : IItemInspectionDispatchService
    {
        private IRepository<itemInspectionDispatch> _repository;

        public ItemInspectionDispatchService(IRepository<itemInspectionDispatch> repository)
        {
            _repository = repository;
        }

        public IResult Create(itemInspectionDispatch instance)
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

        public IResult Update(itemInspectionDispatch instance)
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

        public IResult Update(itemInspectionDispatch instance, string propertyName, object value)
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

        public IResult Delete(string abnormalId)
        {
            IResult result = new Result(false);

            if (!IsExists(abnormalId))
            {
                result.Message = "找不到台車資料";
            }

            try
            {
                var instance = this.GetById(abnormalId);
                this._repository.Update(instance, "isDelete", 1);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public bool IsExists(string dispatchId)
        {
            return this._repository.GetAll().Any(x => x.dispatchId == dispatchId);
        }

        public itemInspectionDispatch GetById(string dispatchId)
        {
            return this._repository.Get(x => x.dispatchId == dispatchId);
        }

        public IEnumerable<itemInspectionDispatch> GetAll()
        {
            return this._repository.GetAll().OrderBy(itemInspectionDispatch => itemInspectionDispatch.createTime);
        }
    }
}