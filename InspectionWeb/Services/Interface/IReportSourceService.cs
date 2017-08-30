using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IReportSourceService
    {

        IResult Create(reportSource instance);

        IResult Create(string sourceCode, string sourceName);

        IResult Update(reportSource instance);

        IResult Update(reportSource instance, string propertyName, object value);

        IResult Delete(string sourceId);

        bool IsExists(string sourceId);

        string GetId(string sourceCode);

        reportSource GetById(string sourceId);

        reportSource GetBySourceCode(string sourceCode);

        reportSource GetBySourceName(string name);

        IEnumerable<reportSource> GetAll();
    }
}