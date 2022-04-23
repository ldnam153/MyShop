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
    public class DashBoardViewModel:ViewModelBase
    {
        ProjectMyShopContext context;
        public int TotalProducts { get; set; } = 0;
        public int TotalOrdersThisWeek { get; set; } = 0;
        public int TotalOrdersThisMonth { get; set; } = 0;
        public string CurrentDate { get => DateTime.Now.ToShortDateString(); }
        public ObservableCollection<Product> SmallQuantityProducts { get; set; } = new ObservableCollection<Product>();
        public DashBoardViewModel()
        {
            context = DbUltility.Instance.Db;
            TotalProducts = CountAllProducts();
            TotalOrdersThisWeek = CountOrdersThisWeek();
            TotalOrdersThisMonth = CountOrdersThisMonth();
            SmallQuantityProducts = new ObservableCollection<Product>(
                context.Products.Where(p=>p.ProductQuantityInStock <= 5 && p.Active != 0).OrderBy(p=> p.ProductQuantityInStock).Take(4).ToList());


        }
        private int CountAllProducts()
        {
            return context.Products.Where(product => product.Active != 0).ToList().Count; 
        }
        private int CountOrdersThisWeek()
        {
            DateTime now = DateTime.Now.Date;
            int numDays = (int)now.DayOfWeek ; // get number of previous days in this week
            DateTime preDate = now.AddDays(-numDays);
            List<Order> list = GetOrdersByDate(preDate, now);
            return list.Count;
        }
        private int CountOrdersThisMonth()
        {
            DateTime now = DateTime.Now.Date;
            int numDays = now.Day - 1;// get number of previous days in this week
            DateTime preDate = now.AddDays(-numDays);
            List<Order> list = GetOrdersByDate(preDate, now);
            return list.Count;
        }
        public List<Order> GetOrdersByDate(DateTime PreDate, DateTime LastDate)
        {
            var query = context.Orders.Where(order => order.Active != 0 && order.Complete != 2 );
            var lastDate = LastDate.AddDays(1).AddSeconds(-1); // 23:59:59
            query = query.Where(order => order.OrderTime >= PreDate && order.OrderTime <= lastDate);
            return query.ToList();
        }
    }
}
