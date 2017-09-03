using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IItemInspectionDispatchService
    {
        IResult Create(itemInspectionDispatch instance);

        IResult Create(System.DateTime date, IEnumerable<exhibitionItem> items);

        IResult Update(itemInspectionDispatch instance);

        IResult Update(itemInspectionDispatch instance, string propertyName, object value);

        IResult Delete(string dispatchId);

        bool IsExists(System.DateTime date);

        bool IsExists(string dispatchId);

        bool checkItemInsert();

        itemInspectionDispatch GetById(string dispatchId);

        IEnumerable<itemInspectionDispatch> GetAll();

        IEnumerable<itemInspectionDispatchDetail> GetAllByDate(System.DateTime date);
    }
}