using InspectionWeb.Models;
using InspectionWeb.Models.Interface;
using InspectionWeb.Models.Misc;
using InspectionWeb.Services.Interface;
using InspectionWeb.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InspectionWeb.Services
{
    public class MaintenanceWorkService : IMaintenanceWorkService
    {
        private IRepository<abnormalRecord> _repository;

        public MaintenanceWorkService(IRepository<abnormalRecord> repository)
        {
            _repository = repository;
        }

        public IResult Create(abnormalRecord instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Create(instance);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }
    }
}
