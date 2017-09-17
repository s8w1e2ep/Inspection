using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using System.Collections.Generic;

namespace InspectionWeb.Services.Interface
{
    public interface IMaintenanceWorkService
    {
        IResult Create(abnormalRecord instance);
    }
}
