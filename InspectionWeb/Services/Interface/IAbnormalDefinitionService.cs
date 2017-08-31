using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IAbnormalDefinitionService
    {

        IResult Create(string abnormalCode, string abnormalName);

        IResult Create(abnormalDefinition instance);

        IResult Update(abnormalDefinition instance);

        IResult Update(abnormalDefinition instance, string propertyName, object value);

        IResult Delete(string abnormalId);

        bool IsExists(string abnormalId);

        bool InRepeat(string abnormalCode);

        string GetId(string abnormalCode);

        abnormalDefinition GetByAbnormalCode(string abnormalCode);

        abnormalDefinition GetById(string abnormal);

        IEnumerable<abnormalDefinition> GetAll();
    }
}