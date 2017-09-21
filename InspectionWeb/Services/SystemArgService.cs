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
    public class SystemArgService : ISystemArgService
    {
        private IRepository<systemSettings> _repository;
        public SystemArgService(IRepository<systemSettings> repository)
        {
            _repository = repository;
        }
        public IResult Create(systemSettings instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                instance.createTime = DateTime.Now;
                instance.lastUpdateTime = instance.createTime;
                IdGenerator idg = new IdGenerator();
                string id = idg.GetID("");
                instance.id = id;
                instance.isDelete = 0;


                this._repository.Create(instance);
                result.Success = true;
                result.Message = id;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.ErrorMsg = "新增系統參數失敗";
            }
            return result;
        }

        public IResult Update(systemSettings instance)
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

        public systemSettings GetById(string id)
        {
            return this._repository.Get(x => x.id == id && x.isDelete == 0);
        }

        public IEnumerable<systemSettings> GetAll()
        {
            return this._repository.GetAll().Where(x => x.isDelete == 0);
        }
    }
}