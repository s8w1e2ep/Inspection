using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IRoomCheckRecordService
    {

        IResult Create(roomCheckRecord instance);

        IResult Update(roomCheckRecord instance);

        IResult Delete(string dispatchId);

        roomCheckRecord GetById(string dispatchId);

        IEnumerable<roomCheckRecord> GetAll(System.DateTime date); 

        IEnumerable<roomCheckRecord> GetAll();

    }
}