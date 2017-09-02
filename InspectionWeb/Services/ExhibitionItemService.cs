using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using InspectionWeb.Models.Misc;
using System;
using System.Collections.Generic;
using System.Linq;


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
                DicUpdate.Add(propertyName, value);
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

        public IResult Delete(string roomId)
        {
            IResult result = new Result(false);

            try
            {
                exhibitionItem instance = this.GetById(roomId);
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
            return this._repository.GetAll().Where(x => x.isDelete == 0).OrderBy(x => x.createTime);
        }

        public IEnumerable<exhibitionItemList> GetAllIdAndName()
        {
            string sqlString = " SELECT R.itemId, R.itemName, R.inspectionUserId "
                    + "FROM exhibitionItem R "
                    + "WHERE R.isDelete = 0 "
                    + "ORDER BY R.createTime; ";

            using (inspectionEntities db = new inspectionEntities())
            {
                var itemList = db.Database.SqlQuery<exhibitionItemList>(sqlString).ToList();
                return itemList;
            }

        }
    }
}