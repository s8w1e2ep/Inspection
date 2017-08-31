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
    public class ReportSourceService : IReportSourceService
    {
        private IRepository<reportSource> _repository;

        public ReportSourceService(IRepository<reportSource> repository)
        {
            _repository = repository;
        }

        public IResult Create(string sourceCode, string sourceName)
        {
            if (string.IsNullOrEmpty(sourceCode))
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            reportSource source = new reportSource();

            try
            {
                if (IsRepeat(sourceCode))
                {
                    result.ErrorMsg = "故障來源編號重複";
                    return result;
                }
                DateTime now = DateTime.Now;
                IdGenerator idGen = new IdGenerator();

                source.sourceId = idGen.GetID("reportSource");
                source.sourceCode = sourceCode;
                source.sourceName = sourceName;
                source.isDelete = 0;
                source.description = "";
                source.createTime = now;
                source.lastUpdateTime = now;

                this._repository.Create(source);
                result.ErrorMsg = source.sourceId;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;

                if (((System.Data.SqlClient.SqlException)((ex.InnerException).InnerException)).Number == 2627)
                {
                    result.ErrorMsg = ex.ToString();
                    result.ErrorMsg = "來源編號或是名稱為空白";
                }
            }

            return result;
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
                DicUpdate.Add(propertyName, value);
                DicUpdate.Add("lastUpdateTime", DateTime.Now);
                _repository.Update(instance, DicUpdate);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Delete(string sourceId)
        {
            IResult result = new Result(false);

            if (!IsExists(sourceId))
            {
                result.Message = "找不到故障來源資料";
            }
            try
            {
                reportSource instance = this.GetById(sourceId);
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

        public bool IsRepeat(string sourceCode)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.sourceCode == sourceCode);
        }

        public bool IsExists(string sourceId)
        {
            return this._repository.GetAll().Any(x => x.sourceId == sourceId && x.isDelete == 0);
        }

        public reportSource GetById(string sourceId)
        {
            return this._repository.Get(x => x.sourceId == sourceId);
        }

        public reportSource GetBySourceCode(string sourceCode)
        {
            return this._repository.Get(x => x.sourceCode == sourceCode && x.isDelete == 0);
        }

        public IEnumerable<reportSource> GetAll()
        {
            return this._repository.GetAll().Where(x => x.isDelete ==0 ).OrderBy(x => x.createTime);
        }
    }
}