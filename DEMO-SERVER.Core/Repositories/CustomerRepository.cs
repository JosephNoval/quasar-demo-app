using DEMO.Core.DAL.Entities.Master;
using DEMO_SERVER.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.Repositories
{
    internal class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext context) : base(context)
        {
        }

        public long AddOrUpdate(Customer model)
        {
            try
            {
                var oldModel = Get(x => x.ID == model.ID).FirstOrDefault();

                using (var trx = db.Database.BeginTransaction())
                {
                    if (oldModel != null)
                    {
                        //update values
                        model.UpdatedOn = DateTime.Now;
                        //update values
                        db.Entry<Customer>(oldModel).CurrentValues.SetValues(model);
                        db.SaveChanges();
                        
                    }
                    else
                    {
                        //insert new model
                        db.Set<Customer>().Add(model);
                        db.SaveChanges();

                    }

                    trx.Commit();

                    return model.ID;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Delete(long Id)
        {
            try
            {
                var model = Get(x => x.ID == Id).FirstOrDefault();

                if (model == null)
                {
                    throw new Exception($"Cannot find Customer with ID = {Id}");
                }
                using (var trx = db.Database.BeginTransaction())
                {
                    model.UpdatedOn = DateTime.Now;
                    model.RecordStatus = DEMO.Core.DAL.Entities.Common.BaseEntity.RecordStatusType.Deleted;

                    db.Set<Customer>().Attach(model);
                    db.Entry<Customer>(model).State = EntityState.Modified;
                    db.SaveChanges();

                    trx.Commit();
                }
                return "Success";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(Customer model)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Customer> Get(Expression<Func<Customer, bool>> condition)
        {
            return db.Set<Customer>()
                    .Where(condition)
                    .ToList()
                    .AsQueryable();
        }

        public IQueryable<Customer> GetAll()
        {
            return db.Set<Customer>()
                    //.Where(x => x.RecordStatus != DEMO.Core.DAL.Entities.Common.BaseEntity.RecordStatusType.Deleted)
                    .ToList().AsQueryable();
        }

        void IRepository<Customer>.AddOrUpdate(Customer model)
        {
            throw new NotImplementedException();
        }
    }
}
