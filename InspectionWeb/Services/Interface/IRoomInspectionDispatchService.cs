using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IRoomInspectionDispatchService
    {
        IResult Create(roomInspectionDispatch instance);

        IResult Update(roomInspectionDispatch instance);

        IResult Delete(string dispatchId);

        roomInspectionDispatch GetById(string dispatchId);

        IEnumerable<roomInspectionDispatch> GetAll();
    }
}