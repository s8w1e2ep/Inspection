using InspectionWeb.Models;
using InspectionWeb.Services.Misc;
using System.Collections.Generic;

namespace InspectionWeb.Services.Interface
{
    public interface ISystemArgService
    {
        IResult Create(systemSettings instance);
        IResult Update(systemSettings instance);
        systemSettings GetById(string id);
        IEnumerable<systemSettings> GetAll();
    }
}
