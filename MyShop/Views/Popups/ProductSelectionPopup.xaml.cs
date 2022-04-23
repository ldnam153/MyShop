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
    /// Interaction logic for ProductSelectionPopup.xaml
    /// </summary>
    public partial class ProductSelectionPopup : Window
    {
        public Product selectedProduct;
        public List<Product> AllProducts;
        public ProductSelectionPopup()
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
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectMyShopContext context = DbUltility.Instance.Db;
            AllProducts = context.Products.Where(p => p.Active != 0).ToList();
            this.myTable.ItemsSource = AllProducts;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var selectItem = this.myTable.SelectedItem;
            if (selectItem != null)
            {
                selectedProduct = selectItem as Product;
                DialogResult = true;
            }
        }
        // End: Button Close | Restore | Minimize
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProjectMyShopContext context = DbUltility.Instance.Db;
            var keyword = txtBox.Text;
            if (keyword.Length != 0)
            {
                string sql = "SELECT * FROM Product";
                //filter by keyword
                if (keyword != "")
                {
                    sql += $" Where Product.product_name LIKE N'%{keyword}%'";
                }
                var query = context.Products.FromSqlRaw(sql)
                        .Include(product => product.Category)
                        .Where(product => product.Active != 0);
                AllProducts = query.ToList();

            }
            else
            {

                AllProducts = context.Products.Where(p => p.Active != 0).ToList();
            }
            this.myTable.ItemsSource = AllProducts;

        }
    }
}
