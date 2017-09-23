using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using System.Collections.Generic;

namespace InspectionWeb.Services.Interface
{
    public interface IMailService
    {
        IResult Create(systemSettings instance);
        IResult Create(string keyname, string value);
        IResult Update(systemSettings instance);
        IResult Update(systemSettings instance, string key, object value);
        IResult Delete(string userId);

        systemSettings GetByID(string userId);
        IEnumerable<systemSettings> GetAll();
    }
}