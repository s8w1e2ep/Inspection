using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models;
using InspectionWeb.Models.Interface;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models.Misc;
using InspectionWeb.Services.Interface;

namespace InspectionWeb.Services.Interface
{
    public interface IRoomActiveRecordService
    {
        IResult Create(roomActiveRecord instance);
        IResult Update(roomActiveRecord instance);
        roomActiveRecord GetById(string id);
        IEnumerable<roomActiveRecord> GetAll();
    }
}