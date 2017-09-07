using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IItemCheckRecordService
    {

        IResult Create(itemCheckRecord instance);

        IResult Update(itemCheckRecord instance);

        IResult Delete(string dispatchId);

        itemCheckRecord GetById(string dispatchId);

        IEnumerable<itemCheckRecord> GetAll();

        IEnumerable<itemInspectionDispatchDetail> GetAllByDate(System.DateTime date);
    }
}