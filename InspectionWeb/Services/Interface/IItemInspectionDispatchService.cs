using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IItemInspectionDispatchService
    {
        IResult Create(itemInspectionDispatch instance);

        IResult Update(itemInspectionDispatch instance);

        IResult Delete(string dispatchId);

        itemInspectionDispatch GetById(string dispatchId);

        IEnumerable<itemInspectionDispatch> GetAll();
    }
}