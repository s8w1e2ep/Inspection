using System.Collections.Generic;
using InspectionWeb.Models;
using InspectionWeb.Services.Misc;

namespace InspectionWeb.Services.Interface
{
    public interface ICompanyService
    {
        IResult Create(company instance);
        IResult Create(string companyName);
        IResult Update(company instance);
        IResult Update(company instance, string key, object value);
        IResult Delete(string companyId);

        bool IsExists(string companyId);
        bool IsRepeat(string companyName);
        company GetByID(string companyId);
        IEnumerable<company> GetAll();
    }
}
