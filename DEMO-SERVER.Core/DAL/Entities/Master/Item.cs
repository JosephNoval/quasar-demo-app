using DEMO.Core.DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DEMO.Core.DAL.Entities.Master
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
