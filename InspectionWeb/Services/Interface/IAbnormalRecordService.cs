using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IAbnormalRecordService
    {
        /// <summary>
        /// add new abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Create(abnormalRecord instance);

        /// <summary>
        /// add new abnormal record
        /// </summary>
        /// <param name="itemId", "sourceId", "abnormalId", "reporter"></param>
        /// <returns></returns>
        IResult Create(string itemId, string sourceId, string abnormalId, string reporter);

        /// <summary>
        /// update 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Update(abnormalRecord instance);

        /// <summary>
        /// update a property of abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IResult Update(abnormalRecord instance, string propertyName, object value);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        IResult Delete(string recordId);

        /// <summary>
        /// check exists of abnormal record
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        bool IsExists(string recordId);

        /// <summary>
        /// get abnormal record
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        abnormalRecord GetById(string recordId);

        /// <summary>
        /// get all abnormal record
        /// </summary>
        /// <returns></returns>
        IEnumerable<abnormalRecord> GetAll();

        IEnumerable<abnormalRecord> GetUnhandledRecords();
    }
}