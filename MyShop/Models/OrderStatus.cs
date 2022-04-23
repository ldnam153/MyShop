using System;
using System.Collections.Generic;

#nullable disable

namespace MyShop.Models
{
    public partial class OrderStatus
    {
        public int OrderStatusId { get; set; }
        public int OrderId { get; set; }
        public int IsCompleted { get; set; }
        public DateTime Time { get; set; }

        public virtual Order Order { get; set; }
    }
}
