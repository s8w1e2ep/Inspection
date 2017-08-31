using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IExhibitionRoomService
    {

        IResult Create(exhibitionRoom instance);

        IResult Update(exhibitionRoom instance);

        IResult Update(exhibitionRoom instance, string propertyName, object value);

        IResult Delete(string roomId);

        exhibitionRoom GetById(string roomId);

        IEnumerable<exhibitionRoom> GetAll();

        IEnumerable<exhibitionRoomList> GetAllIdAndName();
    }
        
}