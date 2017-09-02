using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;
using InspectionWeb.Models.Interface;
using InspectionWeb.Models.Misc;

namespace InspectionWeb.Services
{
    public class RoomInspectionDispatchService : IRoomInspectionDispatchService
    {
        private IRepository<roomInspectionDispatch> _repository;

        public RoomInspectionDispatchService(IRepository<roomInspectionDispatch> repository)
        {
            this._repository = repository;
        }

        public IResult Create(roomInspectionDispatch instance)
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

        public IResult Create(System.DateTime date, IEnumerable<exhibitionRoom> rooms)
        {
            IResult result = new Result(false);
            GetAll().Where(x => x.isDelete == 0);
            for (int i = 0; i < rooms.Count(); i++)
            {
                roomInspectionDispatch roomDispatch = new roomInspectionDispatch();
                try
                {
                    DateTime now = DateTime.Now;
                    IdGenerator idGen = new IdGenerator();

                    roomDispatch.dispatchId = idGen.GetID("roomInspectionDispatch");
                    roomDispatch.checkDate = date;
                    roomDispatch.roomId = rooms.ElementAt(i).roomId;
                    roomDispatch.inspectorId1 = rooms.ElementAt(i).inspectionUserId;
                    roomDispatch.inspectorId2 = rooms.ElementAt(i).inspectionUserId;
                    roomDispatch.setupUserId = "";
                    roomDispatch.isDelete = 0;
                    roomDispatch.createTime = now;
                    roomDispatch.lastUpdateTime = now;

                    this._repository.Create(roomDispatch);
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                    if (((System.Data.SqlClient.SqlException)((ex.InnerException).InnerException)).Number == 2627)
                    {
                        result.ErrorMsg = ex.ToString();
                        //result.ErrorMsg = "";
                    }
                    result.ErrorMsg = "error";
                }
            }
            return result;
        }

        public IResult Update(roomInspectionDispatch instance)
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

        public IResult Update(roomInspectionDispatch instance, string propertyName, object value)
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

        public bool IsExists(System.DateTime date)
        {
            return this._repository.GetAll().Any(x => x.checkDate == date);
        }

        public bool IsExists(string dispatchId)
        {
            return this._repository.GetAll().Any(x => x.dispatchId == dispatchId);
        }

        public roomInspectionDispatch GetById(string dispatchId)
        {
            return this._repository.Get(x => x.dispatchId == dispatchId);
        }

        public bool checkRoomInsert()
        {
            string sqlString = "SELECT COUNT(DISTINCT(roomId)) "
                            + "FROM roomInspectionDispatch;";

            string sqlString2 = "SELECT COUNT(DISTINCT(roomId)) "
                            + "FROM exhibitionRoom;";

            using (inspectionEntities db = new inspectionEntities())
            {
                var existNum = (int)db.Database.SqlQuery<int>(sqlString).First();
                var roomNum = db.Database.SqlQuery<int>(sqlString2).First();
                return (existNum - existNum == 0) ? false : true;
            }
        }

        public IEnumerable<roomInspectionDispatch> GetAll()
        {
            return this._repository.GetAll().OrderBy(roomInspectionDispatch => roomInspectionDispatch.createTime);
        }

        public IEnumerable<roomInspectionDispatchDetail> GetAllByDate(System.DateTime date)
        {
            System.Diagnostics.Debug.WriteLine(date.Date.ToString());
            string sqlString = "IF OBJECT_ID('temp','U') IS NOT NULL DROP TABLE temp;"
                    + "SELECT RID.dispatchId, R.roomId, R.roomName, RID.inspectorId1,  U1.userCode AS inspectorCode1, U1.userName AS inspectorName1, RID.inspectorId2 "
                    + "INTO temp "
                    + "FROM exhibitionRoom R, "
                    + "roomInspectionDispatch RID LEFT OUTER JOIN[user] U1 on RID.inspectorId1 = U1.userId "
                    + "WHERE RID.roomId = R.roomId "
                    + "AND RID.checkDate = '" + date.ToString("d") +"' "
                    + "SELECT temp.*, U2.userCode AS inspectorCode2, U2.userName AS inspectorName2 "
                    + "FROM temp LEFT OUTER JOIN[user] U2 on temp.inspectorId2 = U2.userId "
                    + "ORDER BY temp.roomId;";

            using (inspectionEntities db = new inspectionEntities())
            {
                var detialDate = db.Database.SqlQuery<roomInspectionDispatchDetail>(sqlString).ToList();

                return detialDate;
            }
        }

    }
}