using System;
using System.Collections.Generic;
using System.Linq;
using InspectionWeb.Models.DbContextFactory;
using InspectionWeb.Models.Interface;
using System.Data.Entity;
using System.Linq.Expressions;

namespace InspectionWeb.Models.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext _context
        {
            get;
            set;
        }

        //public GenericRepository()
        //    : this(new everest_trackingEntities())
        //{

        //}

        public GenericRepository(IDbContextFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this._context = factory.GetDbContext();
        }

        //public GenericRepository(ObjectContext context)
        //{
        //    if (context == null)
        //    {
        //        throw new ArgumentNullException("context");
        //    }

        //    this._context = new DbContext(context, true);
        //}

        public void Create(TEntity instance)
        {
            if(instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                this._context.Set<TEntity>().Add(instance);
                this.SaveChanges();
            }

        }

        public void Update(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                this._context.Entry(instance).State = EntityState.Modified;
                this.SaveChanges();
                
            }
        }

        //public void Update(TEntity instance, Expression<Func<TEntity, object>>[] updateProperties)
        //{
        //    if (instance == null)
        //    {
        //        throw new ArgumentNullException("instance");
        //    }
        //    else if (updateProperties == null)
        //    {
        //        throw new ArgumentNullException("updateProperties");
        //    }
        //    else
        //    {
        //        foreach(var property in updateProperties)
        //        {
        //            this._context.Entry(instance).Property(property).IsModified = true;
        //        }

        //        this.SaveChanges();
        //    }
        //}

        public void Update(TEntity instance, string propertyName, object value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            else if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                this._context.Entry(instance).Property(propertyName).CurrentValue = value;
                this._context.Entry(instance).Property(propertyName).IsModified = true;
                this.SaveChanges();
            }
        }

        public void Update(TEntity instance, Dictionary<string, object> DicPropertyValue)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else if (DicPropertyValue == null)
            {
                throw new ArgumentNullException("DicPropertyValue");
            }
            else
            {
                foreach (var item in DicPropertyValue)
                {
                    this._context.Entry(instance).Property(item.Key).CurrentValue = item.Value;
                    this._context.Entry(instance).Property(item.Key).IsModified = true;
                }

                this.SaveChanges();
            }
        }


        public void Delete(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                this._context.Entry(instance).State = EntityState.Deleted;
                this.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return this._context.Set<TEntity>().FirstOrDefault(predicate);
            } catch(Exception e) {
                return null;
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            return this._context.Set<TEntity>().AsQueryable();
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool Disposing)
        {
            if(Disposing)
            {
                if(this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }

            }
        }
    }
}