using MyShop.Models;
using MyShop.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ViewModels
{
    public class ViewDetailOrderViewModel : ViewModelBase
    {
        public Order Order { get; set; }
        ProjectMyShopContext context = DbUltility.Instance.Db;
        public DateTime OrderTime { get; set; }
        public DateTime CompletedTime { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsUnCompleted { get; set; }
        public bool IsCanceled { get; set; }
        public ViewDetailOrderViewModel(Order oldOrder)
        {
            Order = oldOrder;
            List<OrderStatus> statuses = context.OrderStatuses.Where(os => os.OrderId == Order.OrderId).ToList();
            foreach (var os in statuses)
            {
                if (os.IsCompleted == 0)
                {
                    OrderTime = os.Time;
                }
                else
                    CompletedTime = os.Time;
            }
            IsUnCompleted = Order.Complete == 0;
            IsCompleted = Order.Complete == 1;
            IsCanceled = Order.Complete == 2;
        }
        public void CompleteOrder()
        {
            Order.OrderTime = DateTime.Now;
            Order.Complete = 1;
            OrderStatus CompleteOrderStatus = new OrderStatus
            {
                Order = Order,
                Time = Order.OrderTime,
                IsCompleted = Order.Complete
            };
            context.OrderStatuses.Add(CompleteOrderStatus);
            var result = context.Orders.SingleOrDefault(o => o.OrderId == Order.OrderId);
            if (result != null)
            {
                result.OrderTime = Order.OrderTime;
                result.Complete = 1;
                context.SaveChanges();
            }
            CompletedTime = Order.OrderTime;
            IsUnCompleted = Order.Complete == 0;
            IsCompleted = Order.Complete == 1;
            IsCanceled = Order.Complete == 2;
        }
        public void CancelOrder()
        {
            Order.OrderTime = DateTime.Now;
            Order.Complete = 2;
            OrderStatus CompleteOrderStatus = new OrderStatus
            {
                Order = Order,
                Time = Order.OrderTime,
                IsCompleted = Order.Complete
            };
            context.OrderStatuses.Add(CompleteOrderStatus);

            // reverse quanity of products
            var ProductsInOrder = Order.OrderedProducts;
            foreach (var p in ProductsInOrder)
            {
                var pr = context.Products.SingleOrDefault(pro => pro.ProductId == p.ProductId);
                if (pr != null)
                {
                    pr.ProductQuantityInStock += p.Amount;
                }
            }


            var result = context.Orders.SingleOrDefault(o => o.OrderId == Order.OrderId);
            if (result != null)
            {
                result.OrderTime = Order.OrderTime;
                result.Complete = Order.Complete;
                context.SaveChanges();
            }
            CompletedTime = Order.OrderTime;
            IsUnCompleted = Order.Complete == 0;
            IsCompleted = Order.Complete == 1;
            IsCanceled = Order.Complete == 2;
        }
    }
}
