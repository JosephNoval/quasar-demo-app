using DEMO.Core.DAL.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        long AddOrUpdate(Customer model);
        string Delete(long Id);
    }
}
