using DEMO.Core.DAL.Entities.Common;
using DEMO.Core.DAL.Entities.Master;
using DEMO.Core.DAL.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.DAL.Entities.Transaction
{
    public class OrderDetails : BaseEntity
    {
        public long OrderId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
        
    }
}
