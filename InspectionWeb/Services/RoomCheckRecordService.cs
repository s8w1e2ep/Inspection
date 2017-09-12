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
    public class RoomCheckRecordService : IRoomCheckRecordService
    {
        private IRepository<roomCheckRecord> _repository;

        public RoomCheckRecordService(IRepository<roomCheckRecord> repository)
        {
            _repository = repository;
        }

        public IResult Create(roomCheckRecord instance)
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

        public IResult Create(string roomId, string inspectorId, string date, int status, int type)
        {
            IResult result = new Result(false);
            try
            {
                DateTime now = DateTime.Now;
                IdGenerator idGen = new IdGenerator();
                roomCheckRecord record = new roomCheckRecord();
                record.checkId = idGen.GetID("itemCheck");
                record.checkDate = Convert.ToDateTime(date);
                record.roomId = roomId;
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

        public roomCheckRecord GetById(string checkId)
        {
            return this._repository.Get(x => x.checkId == checkId);
        }

        public IEnumerable<roomCheckRecord> GetAll(System.DateTime date)
        {
            return this._repository.GetAll().OrderBy(roomCheckRecord => roomCheckRecord.createTime == date);
        }

        public IEnumerable<roomInspectionDispatchDetail> GetAllByDate(System.DateTime date)
        {
            System.Diagnostics.Debug.WriteLine(date.Date.ToString());
            string sqlString = "IF OBJECT_ID('temp','U') IS NOT NULL DROP TABLE temp;"
                    + "SELECT RID.dispatchId, R.roomId, R.roomName, RID.checkDate, RID.inspectorId1, "
                    + "U1.userCode AS inspectorCode1, U1.userName AS inspectorName1, RID.inspectorId2 "
                    + "INTO temp "
                    + "FROM exhibitionRoom R, "
                    + "roomInspectionDispatch RID LEFT OUTER JOIN[user] U1 on RID.inspectorId1 = U1.userId "
                    + "WHERE RID.roomId = R.roomId "
                    + "AND RID.checkDate = '" + date.ToString("d") + "' "
                    + "AND RID.isDelete = 0 "
                    + "SELECT temp.*, U2.userCode AS inspectorCode2, U2.userName AS inspectorName2 "
                    + "FROM temp LEFT OUTER JOIN[user] U2 on temp.inspectorId2 = U2.userId "
                    + "ORDER BY temp.dispatchId;";

            using (inspectionEntities db = new inspectionEntities())
            {
                List<roomInspectionDispatchDetail> allData = db.Database.SqlQuery<roomInspectionDispatchDetail>(sqlString).ToList();
                List<roomInspectionDispatchDetail> filterData = new List<roomInspectionDispatchDetail>();
                foreach (var data in allData)
                {
                    if (!string.IsNullOrEmpty(data.inspectorId1) || !string.IsNullOrEmpty(data.inspectorId2))
                    {
                        filterData.Add(data);
                    }
                }
                return filterData;
            }
        }

        public IEnumerable<roomCheckRecord> GetAllByDateRange(string startDate, string endDate, string roomId)
        {

            string sqlString = "SELECT * FROM roomCheckRecord WHERE roomId = '" + roomId 
                             + "' AND CAST(checkDate AS date) >= '" + startDate
                             + "' AND CAST(checkDate AS date) <= '" + endDate
                             + "' ORDER BY checkDate ASC";

            System.Diagnostics.Debug.WriteLine("GGG0: \n\n" + sqlString);
            using (inspectionEntities db = new inspectionEntities())
            {
                var roomCheckRecordList = db.Database.SqlQuery<roomCheckRecord>(sqlString).ToList();
                return roomCheckRecordList;
            }
        }
    }
}