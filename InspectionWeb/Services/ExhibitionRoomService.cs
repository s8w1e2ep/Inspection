using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using InspectionWeb.Models.Misc;
using InspectionWeb.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InspectionWeb.Services
{
    public class ExhibitionRoomService : IExhibitionRoomService
    {
        private IRepository<exhibitionRoom> _repository;

        public ExhibitionRoomService(IRepository<exhibitionRoom> repository)
        {
            _repository = repository;
        }


        public IResult Create(exhibitionRoom instance)
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

        public IResult Create(string exhibitRoomName)
        {
            IResult result = new Result(false);
            exhibitionRoom newExhibitionRoom = new exhibitionRoom();
            if (IsRepeat(exhibitRoomName))
            {
                result.ErrorMsg = "展示廳名稱已重複,請重新命名";
            }
            else
            {
                try
                {
                    IdGenerator idg = new IdGenerator();
                    string roomId = idg.GetID("exhibitionRoom");

                    DateTime nowTime = DateTime.Now;

                    newExhibitionRoom.roomId = roomId;
                    newExhibitionRoom.roomName = exhibitRoomName;
                    newExhibitionRoom.createTime = nowTime;

                    newExhibitionRoom.x = 0;
                    newExhibitionRoom.y = 0;
                    newExhibitionRoom.width = 0;
                    newExhibitionRoom.height = 0;

                    newExhibitionRoom.active = 0;
                    newExhibitionRoom.isDelete = 0;
                    newExhibitionRoom.lastUpdateTime = nowTime;

                    this._repository.Create(newExhibitionRoom);

                    // send id to message
                    result.Message = roomId;
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMsg = "新增展示廳失敗";
                    result.Exception = ex;
                }
            }
            return result;
        }

        public IResult Update(exhibitionRoom instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                instance.lastUpdateTime = DateTime.Now;
                this._repository.Update(instance);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public IResult Update(exhibitionRoom instance, string propertyName, object value)
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
                exhibitionRoom instance = this.GetById(roomId);
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

        public exhibitionRoom GetById(string roomId)
        {
            return this._repository.Get(x => x.isDelete == 0 && x.roomId == roomId);
        }


        public IEnumerable<exhibitionRoom> GetAllWithoutIsDelete()
        {
            return this._repository.GetAll().OrderBy(x => x.createTime);
        }

        public IEnumerable<exhibitionRoom> GetAll()
        {
            return this._repository.GetAll().Where(x => x.isDelete == 0).OrderBy(x => x.createTime);
        }

        public IEnumerable<exhibitionRoomList> GetAllIdAndName()
        {
            string sqlString = " SELECT R.roomId, R.roomName, R.inspectionUserId "
                    + "FROM exhibitionRoom R "
                    + "WHERE R.isDelete = 0 "
                    + "ORDER BY R.createTime ";

            using (inspectionEntities db = new inspectionEntities())
            {
                var roomList = db.Database.SqlQuery<exhibitionRoomList>(sqlString).ToList();
                return roomList;
            }

        }

        public IEnumerable<exhibitionRoom> GetUndispatchRoom(System.DateTime date)
        {
            string sqlString = "SELECT exhibitionRoom.* " +
                                "FROM exhibitionRoom " +
                                "WHERE active = 1 AND isDelete = 0 " +
                                "AND NOT EXISTS( " +
                                "SELECT roomInspectionDispatch.roomId " +
                                "FROM roomInspectionDispatch " +
                                "WHERE roomInspectionDispatch.roomId = exhibitionRoom.roomId " +
                                "AND roomInspectionDispatch.checkDate = '"+ date.ToString("d") + "')";

            using (inspectionEntities db = new inspectionEntities())
            {
                var roomList = db.Database.SqlQuery<exhibitionRoom>(sqlString).ToList();
                return roomList;
            }
        }

        public bool IsRepeat(string exhibitRoomName)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.roomName == exhibitRoomName);
        }
    }
}