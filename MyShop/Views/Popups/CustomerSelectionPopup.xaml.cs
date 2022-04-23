using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using MyShop.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyShop.Views.Popups
{
    /// <summary>
    /// Interaction logic for CustomerSelectionPopup.xaml
    /// </summary>
    public partial class CustomerSelectionPopup : Window
    {
        public Customer selectedCustomer;
        public CustomerSelectionPopup()
        {
            InitializeComponent();
        }
        // Start: Button Close | Restore | Minimize 
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var selectItem = this.myTable.SelectedItem;
            if (selectItem != null)
            {
                selectedCustomer = selectItem as Customer;
                DialogResult = true;
            }
                
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectMyShopContext context = DbUltility.Instance.Db;
            var list = context.Customers.Where(customer => customer.Active != 0).ToList();
            this.myTable.ItemsSource = list;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProjectMyShopContext context = DbUltility.Instance.Db;
            var Keyword = txtBox.Text;
            string sql = "SELECT * FROM Customer";
            //filter by keyword
            if (Keyword != "")
            {
                sql += $" Where Customer.customer_name LIKE N'%{Keyword}%'";
                var query = context.Customers.FromSqlRaw(sql)
                    .Include(customer => customer.Gender)
                    .Where(customer => customer.Active != 0);
                var list = query.ToList();
                this.myTable.ItemsSource = list;
            }
            else
            {
                var list = context.Customers.Where(customer => customer.Active != 0).ToList();
                this.myTable.ItemsSource = list;
            }
            

        }
        // End: Button Close | Restore | Minimize
    }
}
