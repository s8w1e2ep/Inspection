using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IRoomInspectionDispatchService
    {
        IResult Create(roomInspectionDispatch instance);

        IResult Create(System.DateTime date, IEnumerable<exhibitionRoom> rooms);

        IResult Update(roomInspectionDispatch instance);

        IResult Update(roomInspectionDispatch instance, string propertyName, object value);

        IResult Delete(string dispatchId);

        bool IsExists(System.DateTime date);

        bool IsExists(string dispatchId);

        bool checkRoomInsert(System.DateTime data);

        roomInspectionDispatch GetById(string dispatchId);

        IEnumerable<roomInspectionDispatch> GetAll();

        IEnumerable<roomInspectionDispatchDetail> GetAllByDate(System.DateTime date);

        IEnumerable<roomInspectionDispatchDetail> GetAllByRoomCondition(string startDate, string endDate, List<string> roomId);

        IEnumerable<roomInspectionDispatchDetail> GetAllByUserCondition(string startDate, string endDate, List<string> userId);
    }
}