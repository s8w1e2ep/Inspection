using System;
using System.Collections.Generic;

namespace InspectionWeb.Services.Misc
{
    public interface IResult
    {
        Guid ID { get; }
        bool Success { get; set; }
        string Message { get; set; }
        string lastUpdateTime { get; set; }
        Exception Exception { get; set; }

        string ErrorMsg { get; set; }
        List<IResult> InnerResults { get; }

    }
}
