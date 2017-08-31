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

        public IResult Create(string abnormalCode, string abnormalName)
        {
            if (string.IsNullOrEmpty(abnormalCode))
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            abnormalDefinition abnormal = new abnormalDefinition();

            try
            {
                if (IsRepeat(abnormalCode))
                {
                    result.ErrorMsg = "異常編號重複";
                    return result;
                }
                DateTime now = DateTime.Now;
                IdGenerator idGen = new IdGenerator();

                abnormal.abnormalId = idGen.GetID("abnormalDefinition");
                abnormal.abnormalCode = abnormalCode;
                abnormal.abnormalName = abnormalName;
                abnormal.description = "";
                abnormal.isDelete = 0;
                abnormal.createTime = now;
                abnormal.lastUpdateTime = now;

                this._repository.Create(abnormal);
                result.ErrorMsg = abnormal.abnormalId;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;

                if (((System.Data.SqlClient.SqlException)((ex.InnerException).InnerException)).Number == 2627)
                {
                    result.ErrorMsg = ex.ToString();
                    //result.ErrorMsg = "其他未知錯誤";
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
                abnormalDefinition instance = this.GetById(abnormalId);
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

        public bool IsExists(string abnormalId)
        {
            return this._repository.GetAll().Any(x => x.abnormalId == abnormalId);
        }

        public bool IsRepeat(string abnormalCode)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.abnormalCode == abnormalCode);
        }

        public abnormalDefinition GetById(string abnormalId)
        {
            return this._repository.Get(x => x.abnormalId == abnormalId);
        }

        public abnormalDefinition GetByAbnormalCode(string abnormalCode)
        {
            return this._repository.Get(x => x.abnormalCode == abnormalCode && x.isDelete == 0);
        }

        public IEnumerable<abnormalDefinition> GetAll()
        {
            return this._repository.GetAll().Where(x => x.isDelete == 0).OrderBy(x => x.createTime);
        }
    }
}