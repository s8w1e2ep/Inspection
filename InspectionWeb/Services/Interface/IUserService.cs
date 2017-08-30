using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using System.Collections.Generic;

namespace InspectionWeb.Services.Interface
{
    public interface IUserService
    {
        IResult Create(user instance);
        IResult Create(string account, string password);
        IResult Update(user instance);
<<<<<<< HEAD
        IResult Update(user instance, string key, object value);
=======
        IResult Update(user instance, string propertyName, object value);
>>>>>>> abnormal
        IResult Delete(string userId);
        IResult changePassword(string userId, string oldPassword, string newPassword);

        user Login(string account, string password);
        bool IsExists(string userId);
<<<<<<< HEAD
        bool IsRepeat(string userCode);
=======
>>>>>>> abnormal
        user GetByID(string userId);
        user GetByAccount(string account);
        IEnumerable<user> GetAll();
    }
}