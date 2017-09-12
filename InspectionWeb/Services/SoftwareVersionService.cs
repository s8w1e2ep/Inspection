using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;
using InspectionWeb.Models.Misc;

namespace InspectionWeb.Services
{
    public class SoftwareVersionService : ISoftwareVersionService
    {
        private IRepository<softwareVersion> _repository;

        public SoftwareVersionService(IRepository<softwareVersion> repository)
        {
            _repository = repository;
        }

        public IResult Create(softwareVersion instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                instance.lastUpdateTime = DateTime.Now;
                instance.createTime = DateTime.Now;
                IdGenerator idg = new IdGenerator();
                string softwareId = idg.GetID("softwareVersion");
                instance.softwareId = softwareId;
                instance.isDelete = 0;


                this._repository.Create(instance);
                result.Success = true;
                result.Message = softwareId;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.ErrorMsg = "新增軟體版本失敗"; 
            }
            return result;
        }

        public IResult Update(softwareVersion instance)
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

        public IResult Update(softwareVersion instance, string propertyName, object value)
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

        public bool IsExists(string softwareId)
        {
            return this._repository.GetAll().Any(x => x.softwareId == softwareId);
        }

        public softwareVersion GetById(string softwareId)
        {
            return this._repository.Get(x => x.softwareId == softwareId);
        }

        public softwareVersion GetBySoftwareCode(string softwareCode)
        {
            return this._repository.Get(x => x.softwareCode == softwareCode);
        }

        public IEnumerable<softwareVersion> GetAll()
        {
            return this._repository.GetAll().OrderBy(abnormalDefinition => abnormalDefinition.createTime);
        }
    }
}