﻿using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IRoomCheckRecordService
    {

        IResult Create(roomCheckRecord instance);

        IResult Create(string roomId, string inspectorId, string date, int status, int type);

        roomCheckRecord GetById(string dispatchId);

        IEnumerable<roomCheckRecord> GetAll(System.DateTime date); 

        IEnumerable<roomInspectionDispatchDetail> GetAllByDate(System.DateTime date);

    }
}