using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;

namespace InspectionWeb.Services
{
    public class ExhibitionItemService : IExhibitionItemService
    {
        private IRepository<exhibitionItem> _repository;

        public ExhibitionItemService(IRepository<exhibitionItem> repository)
        {
            _repository = repository;
        }

        public IResult Create(exhibitionItem instance)
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

        public IResult Update(exhibitionItem instance)
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

        public IResult Update(exhibitionItem instance, string propertyName, object value)
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

        public IResult Delete(string itemId)
        {
            IResult result = new Result(false);

            if (!IsExists(itemId))
            {
                result.Message = "找不到台車資料";
            }

            try
            {
                var instance = this.GetById(itemId);
                this._repository.Update(instance, "isDelete", 1);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public bool IsExists(string itemId)
        {
            return this._repository.GetAll().Any(x => x.itemId == itemId);
        }

        public exhibitionItem GetById(string itemId)
        {
            return this._repository.Get(x => x.itemId == itemId);
        }

        public exhibitionItem GetByItemCode(string itemCode)
        {
            return this._repository.Get(x => x.itemCode == itemCode);
        }

        public IEnumerable<exhibitionItem> GetAll()
        {
            return this._repository.GetAll().OrderBy(exhibitionItem => exhibitionItem.createTime);
        }
    }
}