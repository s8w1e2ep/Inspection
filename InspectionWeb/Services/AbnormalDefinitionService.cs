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
    public class AbnormalDefinitionService : IAbnormalDefinitionService
    {
        private IRepository<abnormalDefinition> _repository;

        public AbnormalDefinitionService(IRepository<abnormalDefinition> repository)
        {
            _repository = repository;
        }

        public IResult Create(string abnormaCode, string abnormalName)
        {
            if (string.IsNullOrEmpty(abnormaCode))
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            abnormalDefinition abnormal = new abnormalDefinition();

            try
            {
                DateTime now = DateTime.Now;
                IdGenerator idGen = new IdGenerator();

                abnormal.abnormalId = idGen.GetAbnormalDefinitionNewID();
                abnormal.abnormalCode = abnormaCode;
                abnormal.abnormalName = abnormalName;
                abnormal.description = "";
                abnormal.createTime = now;
                abnormal.lastUpdateTime = now;

                this._repository.Create(abnormal);
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
            }

            return result;
        }

        public IResult Create(abnormalDefinition instance)
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

        public IResult Update(abnormalDefinition instance)
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

        public IResult Update(abnormalDefinition instance, string propertyName, object value)
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

        public IResult Delete(string abnormalId)
        {
            IResult result = new Result(false);

            if (!IsExists(abnormalId))
            {
                result.Message = "找不到異常定義資料";
            }

            try
            {
                var instance = this.GetById(abnormalId);
                this._repository.Delete(instance);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public string GetId(string abnormalCode)
        {
            return this._repository.Get(x => x.abnormalCode == abnormalCode).abnormalId;
        }

        public bool IsExists(string abnormalId)
        {
            return this._repository.GetAll().Any(x => x.abnormalId == abnormalId);
        }

        public abnormalDefinition GetById(string abnormalId)
        {
            return this._repository.Get(x => x.abnormalId == abnormalId);
        }

        public abnormalDefinition GetByAbnormalCode(string abnormalCode)
        {
            return this._repository.Get(x => x.abnormalCode == abnormalCode);
        }

        public IEnumerable<abnormalDefinition> GetAll()
        {
            return this._repository.GetAll();
        }
    }
}