using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface ISoftwareVersionService
    {
        /// <summary>
        /// add new abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Create(softwareVersion instance);

        /// <summary>
        /// update 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Update(softwareVersion instance);

        /// <summary>
        /// update a property of abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IResult Update(softwareVersion instance, string propertyName, object value);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="softwareId"></param>
        /// <returns></returns>
        IResult Delete(string softwareId);

        /// <summary>
        /// check exists of abnormal record
        /// </summary>
        /// <param name="softwareId"></param>
        /// <returns></returns>
        bool IsExists(string softwareId);

        /// <summary>
        /// get software version
        /// </summary>
        /// <param name="softwareId"></param>
        /// <returns></returns>
        softwareVersion GetById(string softwareId);

        softwareVersion GetBySoftwareCode(string code);

        /// <summary>
        /// get all abnormal definition data
        /// </summary>
        /// <returns></returns>
        IEnumerable<softwareVersion> GetAll();
    }
}