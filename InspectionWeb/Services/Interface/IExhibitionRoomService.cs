using System;
using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IExhibitionRoomService
    {
        IResult Create(exhibitionRoom instance);
        IResult Create(string account, string password);
        IResult Update(exhibitionRoom instance);
        IResult Update(exhibitionRoom instance, string key, object value);
        IResult Delete(string userId);

        bool IsExists(string userId);
        bool IsRepeat(string userCode);
        user GetByID(string userId);
        user GetByAccount(string account);
        IEnumerable<exhibitionRoom> GetAll();
    }
}
