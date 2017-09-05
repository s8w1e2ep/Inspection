using InspectionWeb.Models;
using InspectionWeb.Models.Interface;
using InspectionWeb.Models.Misc;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace InspectionWeb.Services
{
    public class FieldMapService : IFieldMapService
    {
        private IRepository<fieldMap> _repository;
        public FieldMapService(IRepository<fieldMap> repository)
        {
            _repository = repository;
        }
        public IResult Create(fieldMap instance)
        {
            if(instance == null)
            {
                
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Create(instance);
                result.Success = true;
            }catch(Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        public IResult Create(string fieldName)
        {
            IResult result = new Result(false);
            fieldMap newFieldMap = new fieldMap();

            if (IsRepeat(fieldName))
            {
                result.ErrorMsg = "場域名稱已重複,請重新命名";
            }
            else
            {
                try
                {
                    IdGenerator idg = new IdGenerator();
                    string fieldId = idg.GetID("fieldMap");

                    DateTime nowTime = DateTime.Now;

                    newFieldMap.fieldId = fieldId;
                    newFieldMap.fieldName = fieldName;
                    newFieldMap.createTime = nowTime;
                    newFieldMap.isDelete = 0;
                    newFieldMap.lastUpdateTime = nowTime;

                    this._repository.Create(newFieldMap);

                    // send id to message
                    result.Message = fieldId;
                    result.Success = true;
                }
                catch(Exception ex)
                {
                    result.Success = false;
                    result.ErrorMsg = "新增場域失敗";
                    result.Exception = ex;
                }
            }
            return result;
        }

        public IResult Delete(string fieldId)
        {
            throw new NotImplementedException();
        }

        public IResult Update(fieldMap instance)
        {

            if(instance == null)
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
            catch(Exception ex)
            {
           
                result.Exception = ex;
                result.ErrorMsg = "更改場域失敗";
            }
            return result;
        }

        public fieldMap GetById(string fieldId)
        {
            return _repository.Get(x => x.isDelete == 0  && ( x.fieldId == fieldId));
        }

        public IEnumerable<fieldMap> GetAll()
        {
            return this._repository.GetAll().Where(x => x.isDelete == 0);
        }

       

        public bool IsRepeat(string fieldName)
        {
            return this._repository.GetAll().Any(x => x.isDelete == 0 && x.fieldName == fieldName);
        }
    }
}