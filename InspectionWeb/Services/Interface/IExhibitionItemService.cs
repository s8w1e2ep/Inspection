using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IExhibitionItemService
    {

        IResult Create(exhibitionItem instance);

        IResult Update(exhibitionItem instance);

        IResult Update(exhibitionItem instance, string propertyName, object value);

        IResult Delete(string itemId);

        exhibitionItem GetById(string itemId);

        IEnumerable<exhibitionItem> GetAll();

        IEnumerable<exhibitionItemList> GetAllIdAndName();

    }
}