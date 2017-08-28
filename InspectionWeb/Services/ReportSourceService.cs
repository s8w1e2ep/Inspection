using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;

namespace InspectionWeb.Services
{
    public class ReportSourceService : IReportSourceService
    {
        private IRepository<reportSource> _repository;

        public ReportSourceService(IRepository<reportSource> repository)
        {
            _repository = repository;
        }

        public IResult Create(reportSource instance)
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

        public IResult Update(reportSource instance)
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

        public IResult Update(reportSource instance, string propertyName, object value)
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

        public bool IsExists(string sourceId)
        {
            return this._repository.GetAll().Any(x => x.sourceId == sourceId);
        }

        public reportSource GetById(string sourceId)
        {
            return this._repository.Get(x => x.sourceId == sourceId);
        }

        public reportSource GetBySourceCode(string sourceCode)
        {
            return this._repository.Get(x => x.sourceCode == sourceCode);
        }

        public reportSource GetBySourceName(string name)
        {
            return this._repository.Get(x => x.sourceName == name);
        }

        public IEnumerable<reportSource> GetAll()
        {
            return this._repository.GetAll().OrderBy(abnormalDefinition => abnormalDefinition.createTime);
        }
    }
}