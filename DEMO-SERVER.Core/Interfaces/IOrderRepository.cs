using DEMO.Core.DAL.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        long AddOrUpdate(Order model);
        string Delete(long Id);
    }
}
