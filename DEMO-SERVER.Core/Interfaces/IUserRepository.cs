using DEMO_SERVER.Core.DAL.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUser(string username, string password);
        
        long AddOrUpdateUser(User model);
        string Delete(long Id);
    }
}
