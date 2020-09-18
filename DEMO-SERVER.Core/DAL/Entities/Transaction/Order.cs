using DEMO.Core.DAL.Entities.Common;
using DEMO.Core.DAL.Entities.Master;
using DEMO_SERVER.Core.DAL.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DEMO.Core.DAL.Entities.Transaction
{
    public class Order : BaseEntity
    {
        public string OrderNo { get; set; }
        public long CustomerId { get; set; }
        public PaymentType PaymentMethod { get; set; }
        public double Total { get; set; }

        public List<OrderDetails> Details { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public enum PaymentType
        {
            Cash = 0,
            Credit = 1
        }
    }
}
