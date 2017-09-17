using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IOtherAbnormalRecordService
    {
        /// <summary>
        /// add new abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Create(otherAbnormalRecord instance);

        /// <summary>
        /// add new abnormal record
        /// </summary>
        /// <param name="name", "sourceId", "abnormalId"></param>
        /// <returns></returns>
        IResult Create(string name, string sourceId, string abnormalId, string reporter);

        ///// <summary>
        ///// add new abnormal record
        ///// </summary>
        ///// <param name="itemId", "sourceId", "abnormalId", "reporter"></param>
        ///// <returns></returns>
        //Result Create(string itemId, string sourceId, string abnormalId, string reporter);

        ///// <summary>
        ///// update 
        ///// </summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //IResult Update(otherAbnormalRecord instance);

        /// <summary>
        /// update a property of abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IResult Update(otherAbnormalRecord instance, string propertyName, object value);

        ///// <summary>
        ///// 刪除資料
        ///// </summary>
        ///// <param name="recordId"></param>
        ///// <returns></returns>
        //IResult Delete(string recordId);

        ///// <summary>
        ///// check exists of abnormal record
        ///// </summary>
        ///// <param name="recordId"></param>
        ///// <returns></returns>
        //bool IsExists(string recordId);

        bool IsRepeat(string name);

        /// <summary>
        /// get abnormal record
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        otherAbnormalRecord GetById(string recordId);

        /// <summary>
        /// get all abnormal record
        /// </summary>
        /// <returns></returns>
        IEnumerable<otherAbnormalRecord> GetAll();

       // IEnumerable<otherAbnormalRecord> GetUnhandledRecords();
    }
}
