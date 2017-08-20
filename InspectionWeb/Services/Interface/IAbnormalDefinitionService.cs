using System.Collections.Generic;
using InspectionWeb.Services.Misc;
using InspectionWeb.Models;

namespace InspectionWeb.Services.Interface
{
    public interface IAbnormalDefinitionService
    {
        /// <summary>
        /// add new abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Create(abnormalDefinition instance);

        /// <summary>
        /// update 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        IResult Update(abnormalDefinition instance);

        /// <summary>
        /// update a property of abnormal record
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IResult Update(abnormalDefinition instance, string propertyName, object value);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="abnormalId"></param>
        /// <returns></returns>
        IResult Delete(string abnormalId);

        /// <summary>
        /// check exists of abnormal record
        /// </summary>
        /// <param name="abnormalId"></param>
        /// <returns></returns>
        bool IsExists(string abnormalId);

        /// <summary>
        /// get abnormal definition data
        /// </summary>
        /// <param name="abnormalId"></param>
        /// <returns></returns>
        abnormalDefinition GetById(string abnormalId);

        /// <summary>
        /// get abnormal definition data by abnormal code
        /// </summary>
        /// <param name="abnormalCode"></param>
        /// <returns></returns>
        abnormalDefinition GetByAbnormalCode(string abnormalCode);

        /// <summary>
        /// get all abnormal definition data
        /// </summary>
        /// <returns></returns>
        IEnumerable<abnormalDefinition> GetAll();
    }
}