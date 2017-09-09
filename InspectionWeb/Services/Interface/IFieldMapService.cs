using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
namespace InspectionWeb.Services.Interface
{
    public interface IFieldMapService
    {
        IResult Create(fieldMap instance);

        IResult Create(string fieldName);

        IResult Update(fieldMap instance);

        IResult Delete(string fieldId);

        fieldMap GetById(string fieldId);

        IEnumerable<fieldMap> GetAll();

        bool IsRepeat(string fieldName);

    }
}
