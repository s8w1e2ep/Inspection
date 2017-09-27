using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;
using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models.Misc;
using InspectionWeb.Services.Interface;

namespace InspectionWeb.Services
{
    public class RoomActiveRecordService : IRoomActiveRecordService
    {
        private IRepository<roomActiveRecord> _repository;
        public RoomActiveRecordService(IRepository<roomActiveRecord> repository)
        {
            _repository = repository;
        }

        public IResult Create(roomActiveRecord instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                instance.createTime = DateTime.Now;
                IdGenerator idg = new IdGenerator();
                string recordId = idg.GetID("roomActiveRecord");
                instance.activityId = recordId;

                instance.isDelete = 0;


                this._repository.Create(instance);
                result.Success = true;
                result.Message = recordId;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(roomActiveRecord instance)
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

        public roomActiveRecord GetById(string id)
        {
            return this._repository.Get(x => x.activityId == id && x.isDelete == 0);
        }

        public IEnumerable<roomActiveRecord> GetAll()
        {
            return this._repository.GetAll().Where(x => x.isDelete == 0);
        }

        public IEnumerable<roomActiveRecord> GetEvery()
        {
            return this._repository.GetAll();
        }
    }
}