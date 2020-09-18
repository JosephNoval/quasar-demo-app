using DEMO.Core.DAL.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        long AddOrUpdate(Item model);
        string Delete(long Id);
    }
}
