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

        public IEnumerable<itemInspectionDispatchDetail> GetAllByDate(System.DateTime date)
        {
            System.Diagnostics.Debug.WriteLine(date.Date.ToString());
            string sqlString = "IF OBJECT_ID('temp','U') IS NOT NULL DROP TABLE temp;"
                                + "SELECT IID.dispatchId, I.itemId, I.itemName, IID.inspectorId1, "
                                + "U1.userCode AS inspectorCode1, U1.userName AS inspectorName1, IID.inspectorId2 "
                                + "INTO temp "
                                + "FROM exhibitionItem I, itemInspectionDispatch IID LEFT OUTER JOIN[user] U1 on IID.inspectorId1 = U1.userId "
                                + "WHERE IID.itemId = I.itemId "
                                + "AND IID.checkDate = '" + date.ToString("d") + "' "
                                + "AND IID.isDelete = 0 "
                                + "SELECT temp.*, U2.userCode AS inspectorCode2, U2.userName AS inspectorName2 "
                                + "FROM temp LEFT OUTER JOIN[user] U2 on temp.inspectorId2 = U2.userId "
                                + "ORDER BY temp.dispatchId;";

            using (inspectionEntities db = new inspectionEntities())
            {
                List<itemInspectionDispatchDetail> allData = db.Database.SqlQuery<itemInspectionDispatchDetail>(sqlString).ToList();
                List<itemInspectionDispatchDetail> filterData = new List<itemInspectionDispatchDetail>();
                foreach (var data in allData )
                {
                    if(!string.IsNullOrEmpty(data.inspectorId1) || !string.IsNullOrEmpty(data.inspectorId2))
                    {
                        filterData.Add(data);
                    }
                }
                return filterData;
            }
        }

    }
}