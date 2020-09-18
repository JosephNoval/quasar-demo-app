using DEMO.Core.DAL.Entities.Transaction;
using DEMO_SERVER.Core.DAL.Entities.Transaction;
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
    internal class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext context) : base(context)
        {
        }

        public void AddOrUpdate(Order model)
        {
            throw new NotImplementedException();
        }

        public void Delete(Order model)
        {
            throw new NotImplementedException();
        }

        public string Delete(long Id)
        {
            try
            {
                var model = Get(x => x.ID == Id).FirstOrDefault();

                if (model == null)
                {
                    throw new Exception($"Cannot find Order with ID = {Id}");
                }
                using (var trx = db.Database.BeginTransaction())
                {
                    model.UpdatedOn = DateTime.Now;
                    model.RecordStatus = DEMO.Core.DAL.Entities.Common.BaseEntity.RecordStatusType.Deleted;
                    model.Details.ForEach(x => { x.RecordStatus = DEMO.Core.DAL.Entities.Common.BaseEntity.RecordStatusType.Deleted; x.UpdatedOn = DateTime.Now; });

                    db.Set<Order>().Attach(model);
                    db.Entry<Order>(model).State = EntityState.Modified;
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

        public IQueryable<Order> Get(Expression<Func<Order, bool>> condition)
        {
            return db.Set<Order>()
                    .Include($"{nameof(Order.Customer)}")
                    .Include($"{nameof(Order.Details)}.{nameof(OrderDetails.Item)}")
                    .Where(condition)
                    .ToList()
                    .AsQueryable();
        }

        public IQueryable<Order> GetAll()
        {
            return db.Set<Order>()
                    .Include($"{nameof(Order.Customer)}")
                    .Include($"{nameof(Order.Details)}.{nameof(OrderDetails.Item)}")
                    //.Where(x => x.RecordStatus != DEMO.Core.DAL.Entities.Common.BaseEntity.RecordStatusType.Deleted)
                    .ToList().AsQueryable();
        }

        long IOrderRepository.AddOrUpdate(Order model)
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
                        db.Entry<Order>(oldModel).CurrentValues.SetValues(model);
                        db.SaveChanges();

                        //delete old details
                        db.Set<OrderDetails>().RemoveRange(oldModel.Details);
                        db.SaveChanges();

                        //add new details
                        model.Details.ForEach(x => { x.OrderId = model.ID; x.UpdatedOn = DateTime.Now; });
                        db.Set<OrderDetails>().AddRange(model.Details);
                        db.SaveChanges();
                    }
                    else
                    {
                        //insert new model
                        db.Set<Order>().Add(model);
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
    }
}
