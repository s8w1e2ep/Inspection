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

        public IResult Create(System.DateTime date, IEnumerable<exhibitionRoom> rooms, string setupId)
        {
            IResult result = new Result(false);
            for (int i = 0; i < rooms.Count(); i++)
            {
                roomInspectionDispatch roomDispatch = new roomInspectionDispatch();
                try
                {
                    DateTime now = DateTime.Now;
                    IdGenerator idGen = new IdGenerator();

                    roomDispatch.dispatchId = idGen.GetID("roomDispatch");
                    roomDispatch.checkDate = date;
                    roomDispatch.roomId = rooms.ElementAt(i).roomId;
                    roomDispatch.inspectorId1 = rooms.ElementAt(i).inspectionUserId;
                    roomDispatch.inspectorId2 = rooms.ElementAt(i).inspectionUserId;
                    roomDispatch.setupUserId = setupId;
                    roomDispatch.isDelete = Convert.ToByte(0);
                    roomDispatch.createTime = now;
                    roomDispatch.lastUpdateTime = now;

                    this._repository.Create(roomDispatch);
                    //result.ErrorMsg = "create success";
                    result.Success = true;
                }
                catch (DbEntityValidationException ex)
                {
                    result.Exception = ex;
                    result.ErrorMsg = ex.ToString();
                    System.Diagnostics.Debug.WriteLine(result.ErrorMsg);
                    foreach(var ve in ex.EntityValidationErrors)
                    {
                        foreach(var vee in ve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("Type: " + vee.PropertyName + " error: " + vee.ErrorMessage);
                        }
                    }
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
                DicUpdate.Add(propertyName, (string)value);
                DicUpdate.Add("lastUpdateTime", DateTime.Now);
                _repository.Update(instance, DicUpdate);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Delete(string dispatchId)
        {
            IResult result = new Result(false);

            if (!IsExists(dispatchId))
            {
                result.Message = "找不到派工資料";
            }

            try
            {
                var instance = this.GetById(dispatchId);
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

        public bool checkRoomInsert(System.DateTime date)
        {
            string sqlString = "SELECT COUNT(exhibitionRoom.roomId) AS num " +
                                "FROM exhibitionRoom " +
                                "WHERE active = 1 AND isDelete = 0 " +
                                "AND NOT EXISTS( " +
                                "SELECT roomInspectionDispatch.roomId " +
                                "FROM roomInspectionDispatch " +
                                "WHERE roomInspectionDispatch.roomId = exhibitionRoom.roomId " +
                                "AND roomInspectionDispatch.checkDate = '" + date.ToString("d") + "')";

            using (inspectionEntities db = new inspectionEntities())
            {
                var num = (int)db.Database.SqlQuery<int>(sqlString).First();
                return (num == 0) ? false : true;
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
                var detailDate = db.Database.SqlQuery<roomInspectionDispatchDetail>(sqlString).ToList();

                return detailDate;
            }
        }

        public IEnumerable<roomInspectionDispatchDetail> GetAllByRoomCondition(string startDate, string endDate, List<string>roomId )
        {
            string sqlString = "IF OBJECT_ID('temp','U') IS NOT NULL DROP TABLE temp;"
                    + "SELECT RID.dispatchId, R.roomId, R.roomName, RID.checkDate, RID.inspectorId1, "
                    + "U1.userCode AS inspectorCode1, U1.userName AS inspectorName1, RID.inspectorId2 "
                    + "INTO temp "
                    + "FROM exhibitionRoom R, "
                    + "roomInspectionDispatch RID LEFT OUTER JOIN[user] U1 on RID.inspectorId1 = U1.userId "
                    + "WHERE RID.roomId = R.roomId "
                    + "AND RID.isDelete = 0 "
                    + "AND RID.checkDate >= '" + startDate  + "' "
                    + "AND RID.checkDate <= '" + endDate + "' "
                    + "AND ( ";
            foreach (var i in roomId)
            {
                sqlString += "R.roomId = '";
                sqlString += i;
                sqlString += "' OR ";
            }
            sqlString = sqlString.Substring(0, sqlString.Length - 4);

            sqlString += ") ";
            sqlString += "SELECT temp.*, U2.userCode AS inspectorCode2, U2.userName AS inspectorName2 ";
            sqlString += "FROM temp LEFT OUTER JOIN[user] U2 on temp.inspectorId2 = U2.userId ";
            sqlString += "ORDER BY temp.dispatchId; ";
            using (inspectionEntities db = new inspectionEntities())
            {
                var detailDate = db.Database.SqlQuery<roomInspectionDispatchDetail>(sqlString).ToList();

                return detailDate;
            }

        }

        public IEnumerable<roomInspectionDispatchDetail> GetAllByUserCondition(string startDate, string endDate, List<string>userId)
        {
            string sqlString = "SELECT RID.dispatchId, R.roomId, R.roomName, RID.checkDate, "
                            + "RID.inspectorId1, UU1.userCode AS inspectorCode1, UU1.userName AS inspectorName1, "
                            + "RID.inspectorId2, UU2.userCode AS inspectorCode2, UU2.userName AS inspectorName2 "
                            + "FROM roomInspectionDispatch AS RID, [user] AS UU1, [user] AS UU2, exhibitionRoom AS R "
                            + "WHERE RID.roomId = R.roomId "
                            + "AND RID.checkDate >= '" + startDate + "' "
                            + "AND RID.checkDate <= '" + endDate + "' "
                            + "AND UU1.userId = RID.inspectorId1 "
                            + "AND UU2.userId = RID.inspectorId2 "
                            + "AND RID.isDelete = 0 "
                            + "AND (";
            foreach(var i in userId)
            {
                sqlString += "RID.inspectorId1 = '";
                sqlString += i;
                sqlString += "' OR RID.inspectorId2 = '";
                sqlString += i;
                sqlString += "' OR ";
            }
            sqlString = sqlString.Substring(0, sqlString.Length - 4);
            sqlString += ") ORDER BY RID.dispatchId";

            using (inspectionEntities db = new inspectionEntities())
            {
                var detailDate = db.Database.SqlQuery<roomInspectionDispatchDetail>(sqlString).ToList();
                return detailDate;
            }
        }
    }
}