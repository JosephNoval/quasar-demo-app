using DEMO.Core.DAL.Entities;
using DEMO.Core.DAL.Entities.Common;
using DEMO_SERVER.Core.DAL.Entities;
using DEMO_SERVER.Core.Interfaces;
using DEMO_SERVER.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.Factories
{
    public class RepositoryFactory
    {
        protected DbContext db = null;
        public RepositoryFactory()
        {
            db = new MyDataContext();
        }
        public RepositoryFactory(DbContext context)
        {
            db = context;
        }

        public IUserRepository CreateUserRepository()
        {
            return new UserRepository(db);
        }

        public ICustomerRepository CreateCustomerRepository()
        {
            return new CustomerRepository(db);
        }

        public IItemRepository CreateItemRepository()
        {
            return new ItemRepository(db);
        }

        public IOrderRepository CreateOrderRepository()
        {
            return new OrderRepository(db);
        }

        public IRepository<T> CreateRepository<T>() where T : BaseEntity
        {
            return new BaseRepository<T>();
        }
        internal GenericRepository<T> CreateGenericRepository<T>() where T : BaseEntity
        {
            return new GenericRepository<T>(db);
        }
    }
}
