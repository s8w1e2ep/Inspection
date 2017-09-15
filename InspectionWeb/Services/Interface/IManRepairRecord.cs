using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using System.Collections.Generic;
namespace InspectionWeb.Services.Interface
{
    public interface IManRepairRecord
    {
        IResult Create(manRepairRecord instance);
        IResult Create(string account, string password);
        IResult Update(manRepairRecord instance);
        IResult Update(manRepairRecord instance, string key, object value);
        IResult Delete(string userId);
       

        user Login(string account, string password);
        bool IsExists(string userId);
       
        user GetByID(string userId);
        IEnumerable<manRepairRecord> GetAll();
    }
}
