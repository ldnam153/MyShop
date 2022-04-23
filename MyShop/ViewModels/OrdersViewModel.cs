using MyShop.Models;
using MyShop.Ultilities;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;

namespace MyShop.ViewModels
{
    public class OrdersViewModel:ViewModelBase
    {
        ProjectMyShopContext myShopEntitiesDbContext;
        public bool IsNotEmpty { get; set; } = false;
        public int CurrentPage { get; set; } = 1;
        public int TotalPage { get; set; } = 1;
        public DateTime PreDate { get; set; } = DateTime.Now;
        public DateTime LastDate { get; set; } = DateTime.Now; 
        public bool FilterCheck { get; set; } = false;
        public bool InProgressFilterCheck { get; set; } = false;
        public OrdersViewModel()
        {
            myShopEntitiesDbContext = DbUltility.Instance.Db;
            LoadItems();
        }
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        public void LoadItems()
        {
            var query = myShopEntitiesDbContext.Orders
                         .Include(order => order.Customer)
                         .Include(order => order.OrderedProducts)
                         .Where(order => order.Active != 0);
            if (FilterCheck)
            {
                var lastDate = LastDate.AddDays(1).AddSeconds(-1); // 23:59:59
                query = query.Where(order => order.OrderTime >= PreDate && order.OrderTime <= lastDate);
            }
            if (InProgressFilterCheck)
            {
                query = query.Where(order => order.Complete == 0);
            }
            int PageSize = AppConfigUltility.CurrentPageSize;
            if (query != null && query.Any())
            {
                TotalPage = query.Count() / PageSize +
                    (query.Count() % PageSize == 0 ? 0 : 1);
            }
            else TotalPage = 0;
            var items = query.OrderByDescending(o => o.OrderTime)
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToArray();
            Orders.Clear();
            foreach (var item in items)
            {
                Orders.Add(item);
            }
            IsNotEmpty = Orders.Count != 0;

        }
        public void AddOrder(Order order)
        {
            try
            {
                //order.Active = 1;
                //myShopEntitiesDbContext.Orders.Add(order);
                //myShopEntitiesDbContext.SaveChanges();
                Orders.Add(order);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error!");
            }
            
        }

        public void UpdateOrder(int index, Order order)
        {
            var result = myShopEntitiesDbContext.Orders.SingleOrDefault(o => o.OrderId == order.OrderId);
            if (result != null)
            {
                try
                {
                    PropertiesCopy.CopyProperties(order, result);
                    myShopEntitiesDbContext.SaveChanges();
                    Orders[index] = order.Clone() as Order;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error!");
                }
            }
        }
        public void DeleteOrder(int index)
        {
            var result = myShopEntitiesDbContext.Orders.SingleOrDefault(o => o.OrderId == Orders[index].OrderId);
            if (result != null)
            {
                try
                {
                    result.Active = 0;
                    Orders.RemoveAt(index);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error!");
                }
            }
        }
        public void Next()
        {
            
            if (CurrentPage < TotalPage)
            {
                CurrentPage++;
                LoadItems();
            }

        }
        public void Pre()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadItems();
            }
        }
        public void CheckFilterByDate()
        {
            FilterCheck = !FilterCheck;
            CurrentPage = 1;
            LoadItems();
        }
        public void CheckInProgressFilter()
        {
            InProgressFilterCheck = !InProgressFilterCheck;
            CurrentPage = 1;
            LoadItems();
        }
        public void SetPreDate(DateTime preDate)
        {
            PreDate = preDate;
            CurrentPage = 1;
            LoadItems();

        }
        public void SetLastDate(DateTime lastDate)
        {
            LastDate = lastDate;
            CurrentPage = 1;
            LoadItems();

        }

    }
}
