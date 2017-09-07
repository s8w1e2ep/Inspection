using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;

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

        public IResult Update(roomCheckRecord instance)
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

        public IResult Update(roomCheckRecord instance, string propertyName, object value)
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

        public roomCheckRecord GetById(string checkId)
        {
            return this._repository.Get(x => x.checkId == checkId);
        }

        public IEnumerable<roomCheckRecord> GetAll(System.DateTime date)
        {
            return this._repository.GetAll().OrderBy(roomCheckRecord => roomCheckRecord.createTime == date);
        }

        public IEnumerable<roomCheckRecord> GetAll()
        {
            return this._repository.GetAll().Where(x => x.isDelete == 0).OrderBy(roomCheckRecord => roomCheckRecord.createTime);
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
    }
}