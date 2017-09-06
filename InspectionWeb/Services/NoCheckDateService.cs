﻿using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using InspectionWeb.Models.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InspectionWeb.Services
{
    public class NoCheckDateService : INoCheckDateService
    {
        private IRepository<noCheckDate> _repository;

        public NoCheckDateService(IRepository<noCheckDate> repository)
        {
            _repository = repository;
        }

        public IResult Create(System.DateTime date, string description, bool am, bool pm)
        {
            IResult result = new Result(false);
            noCheckDate noCheckDate = new noCheckDate();

            try
            {
                DateTime now = DateTime.Now;
                IdGenerator idGen = new IdGenerator();
                noCheckDate.id = idGen.GetID("noCheckDate");
                noCheckDate.noCheckDate1 = date;
                noCheckDate.am = Convert.ToByte(am);
                noCheckDate.pm = Convert.ToByte(pm);
                noCheckDate.description = description;
                noCheckDate.setupUserId = "";
                noCheckDate.isDelete = Convert.ToByte(0);
                noCheckDate.createTime = now;
                noCheckDate.lastUpdateTime = now;

                this._repository.Create(noCheckDate);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;

                if (((System.Data.SqlClient.SqlException)((ex.InnerException).InnerException)).Number == 2627)
                {
                    result.ErrorMsg = ex.ToString();
                }
            }

            return result;
        }

        public IResult Create(noCheckDate instance)
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

        public IResult Update(noCheckDate instance)
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

        public IResult Update(noCheckDate instance, string propertyName, object value)
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

        public IResult Delete(string Id)
        {
            IResult result = new Result(false);

            if (!IsExists(Id))
            {
                result.Message = "找不到異常定義資料";
            }

            try
            {
                noCheckDate instance = this.GetById(Id);
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

        public bool IsExists(string Id)
        {
            return this._repository.GetAll().Any(x => x.id == Id);
        }

        public int IsExists(System.DateTime date)
        {
            if( this._repository.GetAll().Any(x => x.noCheckDate1 == date && x.isDelete == 0 && x.am == Convert.ToByte(1) && x.pm == Convert.ToByte(1)))
            {
                return 1;
            }else if (this._repository.GetAll().Any(x => x.noCheckDate1 == date && x.isDelete == 0 && x.am == Convert.ToByte(1) && x.pm == Convert.ToByte(0)))
            {
                return 2;
            }
            else if (this._repository.GetAll().Any(x => x.noCheckDate1 == date && x.isDelete == 0 && x.am == Convert.ToByte(0) && x.pm == Convert.ToByte(1)))
            {
                return 3;
            }
            return 0;

        }

        public noCheckDate GetById(string Id)
        {
            return this._repository.Get(x => x.id == Id);
        }

        public IEnumerable<noCheckDate> GetAllWithTimeInterval(System.DateTime start, System.DateTime end)
        {
            return this._repository.GetAll().Where(x => x.noCheckDate1 <= end && x.noCheckDate1 >= start).OrderBy(x => x.noCheckDate1);
        }

        public IEnumerable<noCheckDate> GetAll()
        {
            return this._repository.GetAll().Where(x => x.isDelete == 0).OrderBy(x => x.noCheckDate1);
        }
    }
}