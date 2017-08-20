using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.DbContextFactory
{
    public class DbContextFactory : IDbContextFactory
    {
        private string _connectionString = string.Empty;

        public DbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        private DbContext _dbContext;
        private DbContext dbContext
        {
            get 
            {
                if(_dbContext == null)
                {
                    Type T = typeof(DbContext);
                    this._dbContext = (DbContext)Activator.CreateInstance(T, this._connectionString);
                }
                return _dbContext;
            }
        }

        public DbContext GetDbContext()
        {
            return this.dbContext;
        }
    }
}