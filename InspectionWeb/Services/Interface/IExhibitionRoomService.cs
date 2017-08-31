using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IExhibitionRoomService
    {
        /// <summary>
        /// add new abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Create(exhibitionItem instance);

        /// <summary>
        /// update 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Update(exhibitionItem instance);

        /// <summary>
        /// update a property of abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IResult Update(exhibitionItem instance, string propertyName, object value);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        IResult Delete(string itemId);

        /// <summary>
        /// check exists of abnormal record
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        bool IsExists(string itemId);

        /// <summary>
        /// get report device by id
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        exhibitionRoom GetById(string itemId);

        /// <summary>
        /// get report device by id
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        exhibitionRoom GetByItemCode(string itemCode);

        /// <summary>
        /// get all report device
        /// </summary>
        /// <returns></returns>
        IEnumerable<exhibitionRoom> GetAll();

    }
}