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

        public IResult Create(string itemId, string inspectorId, string date, string dispatchId, int status, int type)
        {
            IResult result = new Result(false);
            try
            {
                DateTime now = DateTime.Now;
                IdGenerator idGen = new IdGenerator();
                itemCheckRecord record = new itemCheckRecord();
                record.checkId = idGen.GetID("itemCheck");
                record.dispatchId = dispatchId;
                record.checkDate = Convert.ToDateTime(date);
                record.itemId = itemId;
                record.status = Convert.ToInt16(status);
                record.inspectorId = inspectorId;
                record.checkTimeType = Convert.ToInt16(type);
                record.isDelete = Convert.ToByte(0);
                record.createTime = now;
                record.lastUpdateTime = now;

                this._repository.Create(record);
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
            return result;
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

        public IEnumerable<itemCheckRecord> GetAllByDateRange(string startDate, string endDate, string itemId)
        {

            string sqlString = "SELECT * FROM itemCheckRecord WHERE itemId = '" + itemId
                             + "' AND CAST(checkDate AS date) >= '" + startDate
                             + "' AND CAST(checkDate AS date) <= '" + endDate
                             + "' ORDER BY checkDate ASC";

            using (inspectionEntities db = new inspectionEntities())
            {
                var itemCheckRecordList = db.Database.SqlQuery<itemCheckRecord>(sqlString).ToList();
                return itemCheckRecordList;
            }
        }
    }
}