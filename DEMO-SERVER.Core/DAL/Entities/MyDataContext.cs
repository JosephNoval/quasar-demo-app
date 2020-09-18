using DEMO.Core.DAL.Entities.Master;
using DEMO.Core.DAL.Entities.Transaction;
using DEMO_SERVER.Core.DAL.Entities.Master;
using DEMO_SERVER.Core.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.DAL.Entities
{
    public class MyDataContext : DbContext
    {
        public MyDataContext() : base("name=MyDataContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyDataContext, Configurations>());
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
    }
}
