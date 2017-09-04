using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface INoCheckDateService
    {

        IResult Create(System.DateTime date, string description, short type);

        IResult Create(noCheckDate instance);

        IResult Update(noCheckDate instance);

        IResult Update(noCheckDate instance, string propertyName, object value);

        IResult Delete(string abnormalId);

        bool IsExists(string abnormalId);

        noCheckDate GetById(string abnormal);

        IEnumerable<noCheckDate> GetAll();
    }
}