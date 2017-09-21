using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IItemCheckRecordService
    {

        IResult Create(itemCheckRecord instance);

        IResult Create(string itemId, string inspectorId, string date, int status, int type);

        itemCheckRecord GetById(string checkId);

        IEnumerable<itemCheckRecord> GetAll();

        IEnumerable<itemInspectionDispatchDetail> GetAllByDate(System.DateTime date);

        IEnumerable<itemCheckRecord> GetAllByDateRange(string startDate, string endDate, string itemId);
    }
}