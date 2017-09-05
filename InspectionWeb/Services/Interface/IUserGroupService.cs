using System.Collections.Generic;
using InspectionWeb.Models;
using InspectionWeb.Services.Misc;

namespace InspectionWeb.Services.Interface
{
    public interface IUserGroupService
    {
        IResult Create(userGroup instance);
        IResult Create(string groupName);
        IResult Update(userGroup instance);
        IResult Update(userGroup instance, string key, object value);
        IResult Delete(string groupId);

        bool IsExists(string groupId);
        bool IsRepeat(string groupName);
        userGroup GetByID(string groupId);
        IEnumerable<userGroup> GetAll();
    }
}