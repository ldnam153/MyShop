using MyShop.Ultilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable disable

namespace MyShop.Models
{
    public partial class Order: ICloneable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Order()
        {
            OrderedProducts = new HashSet<OrderedProduct>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int OrderTotalProduct { get; set; }
        public int OrderTotalPrice { get; set; }
        public DateTime OrderTime { get; set; }
        public int Complete { get; set; }
        public int Active { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderStatus> OrderStatuses { get; set; }
        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
        public string ProductsToString {
            get{
                var context = DbUltility.Instance.Db;
                string s = "";
                foreach(var p in OrderedProducts)
                {
                    var result = context.Products.SingleOrDefault(product => product.ProductId == p.ProductId);
                    if (result != null)
                    {
                        s += result.ProductName + ", ";
                    }
                        
                }
                return s; 
            }
             
        }
        
    }
}
