using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using MyShop.Ultilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop.ViewModels
{
    
    public class StatisticsViewModel: ViewModelBase
    {
        public ProjectMyShopContext context;
        public Product SelectedProduct { get; set; }
        public int WeekRevenue { get; set; }
        public DateTime Now { get; set; } = DateTime.Now.Date;
        public int MonthRevenue { get; set; }
        public string ThisMonth { get => GetMonth(Now.Month); }
        public int YearRevenue { get; set; }
        public string ThisYear { get => Now.Year.ToString(); }

        public bool DateFilterCheck { get; set; } = false;
        public DateTime PreDate { get; set; } = DateTime.Now.Date;
        public DateTime LastDate { get; set; } = DateTime.Now.Date;
        int ViewType = 0;
        public StatisticsViewModel()
        {
            context = DbUltility.Instance.Db;
            WeekRevenue = GetWeekRevenue();
            MonthRevenue = GetMonthRevenue();
            YearRevenue = GetYearRevenue();
            ReLoad();
            CreatePieCharts();
        }
        public SeriesCollection piechartData1 { get; set; } = new SeriesCollection();
        public SeriesCollection piechartData2 { get; set; } = new SeriesCollection();
        public SeriesCollection piechartData3 { get; set; } = new SeriesCollection();
        void CreatePieCharts()
        {
            Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            //piechart 1
            var weekOrders = GetWeekOrder();
            var listProductWeek = GetSetOfProduct(weekOrders);
            List<KeyValuePair<Product,int>> listAmountProductWeek = new List<KeyValuePair<Product, int>>();
            foreach (var p in listProductWeek)
            {
                listAmountProductWeek.Add(new KeyValuePair<Product, int>(p, CountNumProductInOrders(weekOrders, p.ProductId)));
            }


            var monthOrders = GetMonthOrder();
            var listProductMonth = GetSetOfProduct(monthOrders);
            List<KeyValuePair<Product, int>> listAmountProductMonth = new List<KeyValuePair<Product, int>>();
            foreach (var p in listProductMonth)
            {
                listAmountProductMonth.Add(new KeyValuePair<Product, int>(p, CountNumProductInOrders(monthOrders, p.ProductId)));
            }

            var yearOrders = GetYearOrder();
            var listProductYear = GetSetOfProduct(yearOrders);
            List<KeyValuePair<Product, int>> listAmountProductYear = new List<KeyValuePair<Product, int>>();
            foreach (var p in listProductYear)
            {
                listAmountProductYear.Add(new KeyValuePair<Product, int>(p, CountNumProductInOrders(yearOrders, p.ProductId)));
            }
            

            piechartData1 = GetSeriesCollection(listAmountProductWeek, labelPoint);
            piechartData2 = GetSeriesCollection(listAmountProductMonth, labelPoint);
            piechartData3 = GetSeriesCollection(listAmountProductYear, labelPoint);

        }
        SeriesCollection GetSeriesCollection(List<KeyValuePair<Product, int>> list, Func<ChartPoint, string> labelPoint)
        {
            Sort(list);
            SeriesCollection res = new SeriesCollection();
            var l2 = list.Take(4);
            foreach(var i in l2)
            {
                res.Add(GetPiePart(labelPoint, i.Key.ProductName, i.Value));
            }
            return res;
        }
        void Sort(List<KeyValuePair<Product, int>> list)
        {
            for(var i = 0; i < list.Count -1; i++)
            {
                for(var j = i+ 1; j < list.Count; j++)
                {
                    if(list[i].Value < list[j].Value)
                    {
                        var tmp = list[i];
                        list[i] = list[j];
                        list[j] = tmp;
                    }
                }
            }
        }
        List<Product> GetSetOfProduct(List<Order> list)
        {
            List<Product> result = new List<Product>();
            foreach (var o in list)
            {
                foreach (var op in o.OrderedProducts)
                    if (!CheckExistInList(result, op.ProductId))
                        result.Add(context.Products.Where(p=> p.ProductId == op.ProductId).FirstOrDefault());
            }
            return result;
        }
        bool CheckExistInList(List<Product> list, int ProductId)
        {
            foreach(var item in list)
            {
                if (item.ProductId == ProductId)
                    return true;
            }
            return false;
        }
        PieSeries GetPiePart(Func<ChartPoint, string> labelPoint, string title, double value)
        {
            return new PieSeries
            {
                Title = title,
                Values = new ChartValues<double> { value },
                DataLabels = true,
                LabelPoint = labelPoint
            };
        }
        public SeriesCollection Revenues { get; set; } = new SeriesCollection();
        public SeriesCollection Sales { get; set; } = new SeriesCollection();
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public void ReLoad()
        {
            switch (ViewType)
            {
                case 0: Last7WeeksStat(); break;
                case 1: Last7MonthsStat(); break;
                case 2: Last7YearsStat(); break;
                case 3: FromDateToDateStat(); break;
            }
            
        }
        void createRevenueChart(List<int> revenues, List<string> labels, List<int> sales)
        {
            Revenues.Clear();
            Revenues.Add(new ColumnSeries
            {
                Title = "Revenue",
                Values = new ChartValues<int>(revenues),
            });

            Sales.Clear();
            Sales.Add(new ColumnSeries
            {
                Title = "Sales",
                Values = new ChartValues<int>(sales),
            });
            Labels = labels.ToArray();
            Formatter = value => priceFormatter(value);
        }
        string priceFormatter(double value)
        {
            int number = (int)value;
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            return String.Format(info, "{0:c}", number);
        }
        public void SetPreDate(DateTime preDate)
        {
            PreDate = preDate;
            ReLoad();

        }
        public void SetLastDate(DateTime lastDate)
        {
            LastDate = lastDate;
            ReLoad();
        }
        public void SelectViewType(int index)
        {
            ViewType = index;
            DateFilterCheck = index == 3;
            ReLoad();
        }
        public void SelecteProduct(Product pro)
        {
            SelectedProduct = pro;
            ReLoad();
        }
        public void Last7WeeksStat()
        {
            DateTime now = DateTime.Now.Date;
             // get number of previous days in this week
            
            List<DateTime> list = new List<DateTime>();
            DateTime tmp = now;
            list.Add(now);
            int numDays = (int)tmp.DayOfWeek;
            tmp = tmp.AddDays(-numDays);
            list.Add(tmp);

            for (var i = 0; i < 6; i++)
            {
                tmp = tmp.AddDays(-7);
                list.Add(tmp);
            }
            List<int> revenues = new List<int>();
            List<int> sales = new List<int>();
            List<string> labels =  new List<string>();
            int count = 0;
            for (var i = 0; i < list.Count - 1; i++)
            {
                DateTime lastDate = list[i];
                DateTime preDate = list[i + 1];
                var orders = GetOrdersByDate(preDate, lastDate);
                int revenue = GetTotalRevenue(orders);
                int sale = CountNumProductInOrders(orders);
                sales.Insert(0, sale);
                revenues.Insert(0, revenue); 
                labels.Insert(0, "Last " + (count++).ToString() + " weeks");
            }
            labels[labels.Count - 1] = "This Week";
            createRevenueChart(revenues, labels, sales);
        }
        public void Last7MonthsStat()
        {
            DateTime now = DateTime.Now.Date;
            // get number of previous days in this week

            List<DateTime> list = new List<DateTime>();
            DateTime tmp = now;
            list.Add(now);
            tmp = tmp.AddDays(-now.Day - 1);
            list.Add(tmp);

            for (var i = 0; i < 6; i++)
            {
                tmp = tmp.AddMonths(-1);
                list.Add(tmp);
            }
            List<int> revenues = new List<int>();
            List<int> sales = new List<int>();
            List<string> labels = new List<string>();
            for (var i = 0; i < list.Count - 1; i++)
            {
                DateTime lastDate = list[i];
                DateTime preDate = list[i + 1];
                var orders = GetOrdersByDate(preDate, lastDate);
                int revenue = GetTotalRevenue(orders);
                int sale = CountNumProductInOrders(orders);
                sales.Insert(0, sale);
                revenues.Insert(0, revenue);
                labels.Insert(0, GetMonth(lastDate.Month) + ", " + lastDate.Year.ToString());
            }
            createRevenueChart(revenues, labels, sales);
        }
        public void Last7YearsStat()
        {
            
            DateTime now = DateTime.Now.Date;
            // get number of previous days in this week

            List<DateTime> list = new List<DateTime>();
            DateTime tmp = now;
            list.Add(now);
            int numMonth = now.Month - 1;
            tmp = now.AddMonths(-numMonth);
            int numDays = tmp.Day - 1;
            tmp = tmp.AddDays(-numDays);
            list.Add(tmp);

            for (var i = 0; i < 6; i++)
            {
                tmp = tmp.AddYears(-1);
                list.Add(tmp);
            }
            List<int> revenues = new List<int>();
            List<int> sales = new List<int>();
            List<string> labels = new List<string>();
            for (var i = 0; i < list.Count - 1; i++)
            {
                DateTime lastDate = list[i];
                DateTime preDate = list[i + 1];
                var orders = GetOrdersByDate(preDate, lastDate);
                int revenue = GetTotalRevenue(orders);
                int sale = CountNumProductInOrders(orders);
                sales.Insert(0, sale);
                revenues.Insert(0, revenue);
                labels.Insert(0, lastDate.Year.ToString());
            }
            createRevenueChart(revenues, labels, sales);
        }
        public void FromDateToDateStat()
        {
            List<int> revenues = new List<int>();
            List<int> sales = new List<int>();
            List<string> labels = new List<string>();
            if ((LastDate - PreDate).TotalDays <= 7)
            {
                int num = (int)(LastDate - PreDate).TotalDays + 1;
                 
                for(int i = 0; i < num; i++)
                {
                    DateTime date = LastDate.AddDays(-i);
                    var orders = GetOrdersByDate(date, date);
                    int revenue = GetTotalRevenue(orders);
                    revenues.Insert(0, revenue);
                    labels.Insert(0, date.ToShortDateString());
                }
            }
            else
            {
                int numDays = (int)(LastDate - PreDate).TotalDays;
                int eps = numDays / 7;
                List<DateTime> list = new List<DateTime>();
                list.Add(LastDate);
                DateTime tmp = LastDate;
                for (var i = 0; i < 7; i++)
                {
                    tmp = tmp.AddDays(-eps);
                    list.Add(tmp);
                }

                for (var i = 0; i < list.Count - 1; i++)
                {
                    DateTime lastDate = list[i];
                    DateTime preDate = list[i + 1];
                    var orders = GetOrdersByDate(preDate, lastDate);
                    int revenue = GetTotalRevenue(orders);
                    int sale = CountNumProductInOrders(orders);
                    sales.Insert(0, sale);
                    revenues.Insert(0, revenue);
                    labels.Insert(0, preDate.ToShortDateString() + "-" + lastDate.ToShortDateString());
                }

            }
            createRevenueChart(revenues, labels, sales);
        }



        private string GetMonth(int number)
        {
            switch (number)
            {
                case 1: return "January";
                case 2: return "February";
                case 3: return "March";
                case 4: return "April";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "August";
                case 9: return "Septemper";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";
            }
            return "";
        }
        private int GetWeekRevenue()
        {
            return GetTotalRevenue(GetWeekOrder());
        }
        List<Order> GetWeekOrder()
        {
            DateTime now = DateTime.Now.Date;
            int numDays = (int)now.DayOfWeek + 1; // get number of previous days in this week
            DateTime preDate = now.AddDays(-numDays);
            return GetOrdersByDate(preDate, now);
        }
        private int GetMonthRevenue()
        {
            
            return GetTotalRevenue(GetMonthOrder());
        }
        List<Order> GetMonthOrder()
        {
            DateTime now = DateTime.Now.Date;
            int numDays = now.Day - 1;// get number of previous days in this week
            DateTime preDate = now.AddDays(-numDays);
            return GetOrdersByDate(preDate, now);
        }
        private int GetYearRevenue()
        {
            return GetTotalRevenue(GetYearOrder());
        }
        List<Order> GetYearOrder()
        {
            DateTime now = DateTime.Now.Date;
            int numMonth = now.Month - 1;
            DateTime preDate = now.AddMonths(-numMonth);
            int numDays = preDate.Day - 1;
            preDate = preDate.AddDays(-numDays);
            return GetOrdersByDate(preDate, now);
        }
        public List<Order> GetOrdersByDate(DateTime PreDate, DateTime LastDate)
        {
            var query = context.Orders
                        .Include(o => o.OrderedProducts)
                         .Where(order => order.Active != 0 && order.Complete == 1);
            var lastDate = LastDate.AddDays(1).AddSeconds(-1); // 23:59:59
            query = query.Where(order => order.OrderTime >= PreDate && order.OrderTime <= lastDate);
            return query.ToList();
        }
        public int GetTotalRevenue(List<Order> orders)
        {
            int sum = 0;
            foreach(var o in orders)
            {
                sum += o.OrderTotalPrice;
            }
            return sum;
        }
        public int CountNumProductInOrders(List<Order> orders)
        {
            if (SelectedProduct == null)
                return 0;
            int sum = 0;
            foreach (var o in orders)
            {
                var orderProucts = context.OrderedProducts.Where(op => op.OrderId == o.OrderId).ToList();
                foreach (var op in orderProucts)
                    if (op.ProductId == SelectedProduct.ProductId)
                        sum += op.Amount;
            }
            return sum;
        }
        public int CountNumProductInOrders(List<Order> orders, int ProductId)
        {
       
            int sum = 0;
            foreach (var o in orders)
            {
                var orderProucts = context.OrderedProducts.Where(op => op.OrderId == o.OrderId).ToList();
                foreach (var op in orderProucts)
                    if (op.ProductId == ProductId)
                        sum += op.Amount;
            }
            return sum;
        }
    }
}
