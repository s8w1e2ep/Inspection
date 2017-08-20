using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IReportSourceService
    {
        /// <summary>
        /// add new abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Create(reportSource instance);

        /// <summary>
        /// update 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Update(reportSource instance);

        /// <summary>
        /// update a property of abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IResult Update(reportSource instance, string propertyName, object value);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        IResult Delete(string sourceId);

        /// <summary>
        /// check exists of abnormal record
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        bool IsExists(string sourceId);

        /// <summary>
        /// get abnormal definition data
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        reportSource GetById(string sourceId);

        /// <summary>
        /// get abnormal definition data by abnormal code
        /// </summary>
        /// <param name="abnormalCode"></param>
        /// <returns></returns>
        reportSource GetBySourceCode(string abnormalCode);

        reportSource GetBySourceName(string name);

        /// <summary>
        /// get all abnormal definition data
        /// </summary>
        /// <returns></returns>
        IEnumerable<reportSource> GetAll();
    }
}