using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;
using InspectionWeb.Models.Interface;
using InspectionWeb.Models.Misc;

namespace InspectionWeb.Services
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

        public IResult Create(System.DateTime date, IEnumerable<exhibitionItem> items)
        {
            IResult result = new Result(false);
            GetAll().Where(x => x.isDelete == 0);
            for (int i = 0; i < items.Count(); i++)
            {
                itemInspectionDispatch itemDispatch = new itemInspectionDispatch();
                try
                {
                    DateTime now = DateTime.Now;
                    IdGenerator idGen = new IdGenerator();

                    itemDispatch.dispatchId = idGen.GetID("itemInspectionDispatch");
                    itemDispatch.checkDate = date;
                    itemDispatch.itemId = items.ElementAt(i).roomId;
                    itemDispatch.inspectorId1 = "";// items.ElementAt(i).inspectionUserId;
                    itemDispatch.inspectorId2 = "";// items.ElementAt(i).inspectionUserId;
                    itemDispatch.setupId = "";
                    itemDispatch.isDelete = 0;
                    itemDispatch.createTime = now;
                    itemDispatch.lastUpdateTime = now;

                    this._repository.Create(itemDispatch);
                    result.ErrorMsg = "create success";
                    result.Success = true;
                }
                catch (DbEntityValidationException ex)
                {
                    result.Exception = ex;
                    result.ErrorMsg = ex.ToString();
                    System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                    result.ErrorMsg = ex.ToString();
                    System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
                }
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
                result.Message = "找不到體驗項目派工的資料";
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

        public bool IsExists(System.DateTime date)
        {
            return this._repository.GetAll().Any(x => x.checkDate == date);
        }

        public itemInspectionDispatch GetById(string dispatchId)
        {
            return this._repository.Get(x => x.dispatchId == dispatchId);
        }

        public bool checkItemInsert()
        {
            string sqlString = "SELECT COUNT(DISTINCT(itemId)) "
                            + "FROM itemInspectionDispatch;";

            string sqlString2 = "SELECT COUNT(DISTINCT(itemId)) "
                            + "FROM exhibitionItem;";

            using (inspectionEntities db = new inspectionEntities())
            {
                var existNum = (int)db.Database.SqlQuery<int>(sqlString).First();
                var roomNum = db.Database.SqlQuery<int>(sqlString2).First();
                return (existNum - existNum == 0) ? false : true;
            }
        }

        public IEnumerable<itemInspectionDispatch> GetAll()
        {
            return this._repository.GetAll().OrderBy(itemInspectionDispatch => itemInspectionDispatch.createTime);
        }

        public IEnumerable<itemInspectionDispatchDetail> GetAllByDate(System.DateTime date)
        {
            System.Diagnostics.Debug.WriteLine(date.Date.ToString());
            string sqlString = "IF OBJECT_ID('temp','U') IS NOT NULL DROP TABLE temp;"
                    + "SELECT IID.dispatchId, I.itemId, I.itemName, IID.inspectorId1,  U1.userCode AS inspectorCode1, U1.userName AS inspectorName1, IID.inspectorId2 "
                    + "INTO temp "
                    + "FROM exhibitionItem I, "
                    + "itemInspectionDispatch IID LEFT OUTER JOIN[user] U1 on IID.inspectorId1 = U1.userId "
                    + "WHERE IID.itemId = I.itemId "
                    + "AND IID.checkDate = '" + date.ToString("d") + "' "
                    + "SELECT temp.*, U2.userCode AS inspectorCode2, U2.userName AS inspectorName2 "
                    + "FROM temp LEFT OUTER JOIN[user] U2 on temp.inspectorId2 = U2.userId "
                    + "ORDER BY temp.itemId;";

            using (inspectionEntities db = new inspectionEntities())
            {
                var detialDate = db.Database.SqlQuery<itemInspectionDispatchDetail>(sqlString).ToList();

                return detialDate;
            }
        }
    }
}