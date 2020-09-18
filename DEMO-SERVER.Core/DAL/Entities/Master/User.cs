using DEMO.Core.DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.DAL.Entities.Master
{
    public class User : BaseEntity
    {
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] Image { get; set; }
        public string Roles { get; set; }
        //public Boolean IsActive { get; set; }

        //public enum RolesType
        //{
        //    Admin,
        //    IsApprover,
        //    User
        //}
    }
}
