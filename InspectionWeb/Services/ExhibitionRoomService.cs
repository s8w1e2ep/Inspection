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

        public IResult Update(exhibitionRoom instance)
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
            return this._repository.Get(x => x.roomId == roomId);
        }


        public IEnumerable<exhibitionRoom> GetAll()
        {
            return this._repository.GetAll().Where(x => x.isDelete == 0).OrderBy(x => x.createTime);
        }

        public IEnumerable<exhibitionRoomList> GetAllIdAndName()
        {
            string sqlString = " SELECT R.roomId, R.roomName, R.inspectionUserId"
                    + "FROM exhibitionRoom R"
                    + "WHERE R.isDelete = 0"
                    + "ORDER BY R.createTime";

            using (inspectionEntities db = new inspectionEntities())
            {
                var roomList = db.Database.SqlQuery<exhibitionRoomList>(sqlString).ToList();
                return roomList;
            }

        }
    }
}