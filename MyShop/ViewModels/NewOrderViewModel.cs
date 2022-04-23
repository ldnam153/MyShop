using MyShop.Models;
using MyShop.Ultilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ViewModels
{
    public class NewOrderViewModel: ViewModelBase
    {
        ProjectMyShopContext myShopEntitiesDbContext;
        public ObservableCollection<OrderedProduct> ProductsInOrder { get; set; } = new ObservableCollection<OrderedProduct>();
        public Order Order { get; set; }
        public bool SelectOldCustomer { get; set; } = true;
        public NewOrderViewModel()
        {
            myShopEntitiesDbContext = DbUltility.Instance.Db;
            Order = new Order()
            {
                Active = 1,
                Complete = 1
            };
            Order.Customer = new Customer();
            //var tmp = myShopEntitiesDbContext.Orders
            //            .Include(product => product.Category)
            //            .ToArray();
            //Orders = new ObservableCollection<Product>(tmp);

        }
        
        public void SelectCustomer(Customer customer)
        {
            Order.Customer = customer;
            Order.CustomerId = customer.CustomerId;
        }
        public void EditCustomer(Customer newCustomer)
        {
            var result = myShopEntitiesDbContext.Customers.Where(c => c.CustomerId == newCustomer.CustomerId).FirstOrDefault();
            if(result != null)
            {
                PropertiesCopy.CopyProperties(newCustomer, result);
                myShopEntitiesDbContext.SaveChanges();
                Order.Customer = newCustomer;
                Order.CustomerId = newCustomer.CustomerId;
            }
        }
        public void AddProduct(Product product)
        {
            var productInOrder = new OrderedProduct
            {
                Amount = 1,
                Order = this.Order,
                Product = product,
                ProductUnitPrice = product.ProductPrice
            };
            ProductsInOrder.Add(productInOrder);
            UpdateTotal();
        }
        public void IncreaseAmountOrderedProduct(OrderedProduct productInOrder)
        {
            if (productInOrder.Product.ProductQuantityInStock > productInOrder.Amount)
            {
                productInOrder.Amount++;
            }
            UpdateTotal();
        }
        public void DescreaseAmountOrderedProduct(OrderedProduct productInOrder)
        {
            if (productInOrder.Amount > 1)
            {
                productInOrder.Amount--;
            }
            else
            {
                ProductsInOrder.Remove(productInOrder);
            }

            UpdateTotal();
        }
        private void UpdateTotal()
        {
            int count = 0;
            int sum = 0;
            foreach(var productInOrder in ProductsInOrder)
            {
                count += productInOrder.Amount;
                sum += productInOrder.Amount * productInOrder.ProductUnitPrice;
            }
            Order.OrderTotalPrice = sum;
            Order.OrderTotalProduct = count;
        }
        public void SaveOrder()
        {
            
            Order.OrderTime = DateTime.Now;
            myShopEntitiesDbContext.Orders.Add(Order);
            myShopEntitiesDbContext.OrderedProducts.AddRange(ProductsInOrder);
            foreach(var p in ProductsInOrder)
            {
                var result = myShopEntitiesDbContext.Products.SingleOrDefault(pro => pro.ProductId == p.ProductId);
                if(result != null)
                {
                    result.ProductQuantityInStock -= p.Amount;
                }
            }
            OrderStatus newOrderStatus = new OrderStatus
            {
                Order = Order,
                Time = Order.OrderTime,
                IsCompleted = 0
            };
            myShopEntitiesDbContext.OrderStatuses.Add(newOrderStatus);
            if (Order.Complete == 1)
            {
                OrderStatus CompleteOrderStatus = new OrderStatus
                {
                    Order = Order,
                    Time = Order.OrderTime,
                    IsCompleted = 1
                };
                myShopEntitiesDbContext.OrderStatuses.Add(CompleteOrderStatus);
            }
            myShopEntitiesDbContext.SaveChanges();
        }
    }
}
