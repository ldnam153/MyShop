using MyShop.Models;
using MyShop.Ultilities;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;

namespace MyShop.ViewModels
{
    public class CustomersViewModel: ViewModelBase
    {
        public string Keyword { get; set; } = "";
        public int CurrentPage { get; set; } = 1;
        public bool IsNotEmpty { get; set; } = false;
        public int TotalPage { get; set; } = 1;
        ProjectMyShopContext myShopEntitiesDbContext;
        public int PageSize { get; set; } = 3;
        public CustomersViewModel()
        {
            myShopEntitiesDbContext = DbUltility.Instance.Db;
            LoadItems();

        }
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();
        public void AddCustomer(Customer customer)
        {
            try
            {
                customer.Active = 1;
                myShopEntitiesDbContext.Customers.Add(customer);
                myShopEntitiesDbContext.SaveChanges();
                Customers.Add(customer);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Database Error!");
            }
            
        }
        public void EditCustomer(int index, Customer customer)
        {
            var result = myShopEntitiesDbContext.Customers.SingleOrDefault(c => c.CustomerId == customer.CustomerId);
            if (result != null)
            {
                try
                {
                    PropertiesCopy.CopyProperties(customer, result);
                    myShopEntitiesDbContext.SaveChanges();
                    Customers[index] = customer.Clone() as Customer;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error!");
                }
            }
           
    
        }
        public void DeleteCustomer(int index)
        {
            var result = myShopEntitiesDbContext.Customers.SingleOrDefault(c => c.CustomerId == Customers[index].CustomerId);
            if (result != null)
            {
                try
                {
                    result.Active = 0;
                    myShopEntitiesDbContext.SaveChanges();
                    Customers.RemoveAt(index);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error!");
                }
            }
            
        }
        public void LoadItems()
        {

            string sql = "SELECT * FROM Customer";
            //filter by keyword
            if (Keyword != "")
            {
                sql += $" Where Customer.customer_name LIKE N'%{Keyword}%'";
            }
            var query = myShopEntitiesDbContext.Customers.FromSqlRaw(sql)
                    .Include(customer => customer.Gender)
                    .Where(customer => customer.Active != 0);
            
            int PageSize = AppConfigUltility.CurrentPageSize;
            if (query != null && query.Any())
            {
                TotalPage = query.Count() / PageSize +
                    (query.Count() % PageSize == 0 ? 0 : 1);
            }
            else TotalPage = 0;
            var items = query.Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToArray();
            Customers.Clear();
            foreach (var item in items)
            {
                Customers.Add(item);
            }
            IsNotEmpty = Customers.Count != 0;
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
        public void Search()
        {
            CurrentPage = 1;
            LoadItems();
        }
    }
}
