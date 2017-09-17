using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using System.Collections.Generic;
namespace InspectionWeb.Services.Interface
{
    public interface IManRepairRecordService
    {
        IResult Create(manRepairRecord instance);
        IResult Create(string account, string password);
        IResult Update(manRepairRecord instance);
        IResult Update(manRepairRecord instance, string key, object value);
        IResult Delete(string userId);
       

       
        bool IsExists(string userId);

        manRepairRecord GetByID(string recordId);
        IEnumerable<manRepairRecord> GetAll();
    }
}
