using InspectionWeb.Models;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
