﻿using InspectionWeb.Models.Interface;
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