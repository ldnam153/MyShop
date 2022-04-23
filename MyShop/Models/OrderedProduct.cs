using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace MyShop.Models
{
    public partial class OrderedProduct: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int OrderedProductId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int ProductUnitPrice { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
