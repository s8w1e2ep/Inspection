using InspectionWeb.Services.Misc;
using InspectionWeb.Models;
using System.Collections.Generic;

namespace InspectionWeb.Services.Interface
{
    public interface ISolutionService
    {
        IResult Create(quickSolution instance);
        IResult Create(string description);
        //IResult Update(quickSolution instance);
        IResult Update(quickSolution instance, string key, object value);
        //IResult Delete(string userId);



        //bool IsExists(string userId);

        quickSolution GetByID(string solutionID);
       

        bool IsRepeat(string description);
       
    }
}
