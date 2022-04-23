using System;
using System.Collections.Generic;

#nullable disable

namespace MyShop.Models
{
    public partial class Product: ICloneable
    {
        public Product()
        {
            OrderedProducts = new HashSet<OrderedProduct>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryId { get; set; }
        public int ProductQuantityInStock { get; set; }
        public DateTime CreateAt { get; set; }
        public int Active { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
