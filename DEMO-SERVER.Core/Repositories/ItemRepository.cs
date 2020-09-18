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
    internal class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(DbContext context) : base(context)
        {
        }

        public long AddOrUpdate(Item model)
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
                        db.Entry<Item>(oldModel).CurrentValues.SetValues(model);
                        db.SaveChanges();

                    }
                    else
                    {
                        //insert new model
                        db.Set<Item>().Add(model);
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
                    throw new Exception($"Cannot find Item with ID = {Id}");
                }
                using (var trx = db.Database.BeginTransaction())
                {
                    model.UpdatedOn = DateTime.Now;
                    model.RecordStatus = DEMO.Core.DAL.Entities.Common.BaseEntity.RecordStatusType.Deleted;

                    db.Set<Item>().Attach(model);
                    db.Entry<Item>(model).State = EntityState.Modified;
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

        public void Delete(Item model)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Item> Get(Expression<Func<Item, bool>> condition)
        {
            return db.Set<Item>()
                    .Where(condition)
                    .ToList()
                    .AsQueryable();
        }

        public IQueryable<Item> GetAll()
        {
            return db.Set<Item>()
                    //.Where(x => x.RecordStatus != DEMO.Core.DAL.Entities.Common.BaseEntity.RecordStatusType.Deleted)
                    .ToList().AsQueryable();
        }

        void IRepository<Item>.AddOrUpdate(Item model)
        {
            throw new NotImplementedException();
        }
    }
}
