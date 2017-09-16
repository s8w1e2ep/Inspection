using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface INoCheckDateService
    {

        IResult Create(System.DateTime date, string description, bool am, bool pm, string setupId);

        IResult Create(noCheckDate instance);

        IResult Update(noCheckDate instance);

        IResult Update(noCheckDate instance, string propertyName, object value);

        IResult Delete(string id);

        bool IsExists(string id);

        int IsExists(System.DateTime date);

        noCheckDate GetById(string id);

        IEnumerable<noCheckDate> GetAllWithTimeInterval (System.DateTime start,System.DateTime end);

        IEnumerable<noCheckDate> GetAll();
    }
}