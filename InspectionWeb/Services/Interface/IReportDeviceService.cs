using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IReportDeviceService
    {
        /// <summary>
        /// add new abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Create(reportDevice instance);

        /// <summary>
        /// update 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Update(reportDevice instance);

        /// <summary>
        /// update a property of abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IResult Update(reportDevice instance, string propertyName, object value);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        IResult Delete(string deviceId);

        /// <summary>
        /// check exists of abnormal record
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        bool IsExists(string deviceId);

        /// <summary>
        /// get report device by id
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        reportDevice GetById(string deviceId);

        /// <summary>
        /// get report device by id
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        reportDevice GetByDeviceCode(string deviceCode);

        /// <summary>
        /// get report device by id
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        reportDevice GetByExhibitionItemId(string itemId);

        /// <summary>
        /// get all report device
        /// </summary>
        /// <returns></returns>
        IEnumerable<reportDevice> GetAll();

    }
}