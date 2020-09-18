using DEMO.Core.DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DEMO.Core.DAL.Entities.Master
{
    public class Customer : BaseEntity
    {
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
    }
}
