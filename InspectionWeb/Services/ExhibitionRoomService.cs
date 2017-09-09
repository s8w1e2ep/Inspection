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
    public class ExhibitionRoomService : IExhibitionRoomService
    {
        private IRepository<exhibitionRoom> _repository;

        public ExhibitionRoomService(IRepository<exhibitionRoom> repository)
        {
            _repository = repository;
        }

        public IResult Create(exhibitionRoom instance)
        {
            throw new NotImplementedException();
        }

        public IResult Create(string account, string password)
        {
            throw new NotImplementedException();
        }

        public IResult Delete(string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<exhibitionRoom> GetAll()
        {
            return _repository.GetAll().Where(x => x.isDelete == 0 && x.active == 1);
        }

        public user GetByAccount(string account)
        {
            throw new NotImplementedException();
        }

        public user GetByID(string userId)
        {
            throw new NotImplementedException();
        }

        public bool IsExists(string userId)
        {
            throw new NotImplementedException();
        }

        public bool IsRepeat(string userCode)
        {
            throw new NotImplementedException();
        }

        public IResult Update(exhibitionRoom instance)
        {
            throw new NotImplementedException();
        }

        public IResult Update(exhibitionRoom instance, string key, object value)
        {
            throw new NotImplementedException();
        }
    }
}