using DEMO.Core.DAL.Entities;
using DEMO_SERVER.Core.DAL.Entities;
using DEMO_SERVER.Core.DAL.Entities.Master;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.Migrations
{
    internal class Configurations : DbMigrationsConfiguration<MyDataContext>
    {
        public Configurations()
        {
            AutomaticMigrationsEnabled = false;
        }
        protected override void Seed(MyDataContext context)
        {
            List<User> user = context.Users.ToList();
            if (user.Count() == 0)
            {
                //This method will be called after migrating to the latest version.
                context.Users.AddOrUpdate(x => x.UserName, new User { UserName = "admin", Password = "admin123", CreatedBy = "DB Seed", CreatedOn = DateTime.Now, Roles = "Administrator", DisplayName = "Programmer", RecordStatus = DEMO.Core.DAL.Entities.Common.BaseEntity.RecordStatusType.Active });

            }
        }
    }
}
