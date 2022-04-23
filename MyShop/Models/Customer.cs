using System;
using System.Collections.Generic;

#nullable disable

namespace MyShop.Models
{
    public partial class Customer: ICloneable
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public int GenderId { get; set; }
        public int Active { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
