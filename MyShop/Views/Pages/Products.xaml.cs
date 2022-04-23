using Aspose.Cells;
using Microsoft.Win32;
using MyShop.Models;
using MyShop.Ultilities;
using MyShop.ViewModels;
using MyShop.Views.Popups;
using MyShop.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MyShop.Views.Pages
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : Page
    {
        ProductsViewModel viewModel;
        
        public Products()
        {
            InitializeComponent();
            
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddProductWindow();
            if (screen.ShowDialog() == true)
            {
                var newProduct = screen.newProduct;
                viewModel.AddProduct(newProduct);
            } 
        }
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = this.myTable.SelectedIndex;
            if (index >= 0)
            {
                var oldProduct = viewModel.Products[index];
                var screen = new EditProductWindow(oldProduct);
                if (screen.ShowDialog() == true)
                {
                    var newProduct = screen.newProduct;
                    viewModel.EditProduct(index, newProduct);

                }
            }
           
        }
        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = this.myTable.SelectedIndex;
            if (index >= 0)
            {
                var product = viewModel.Products[index];
                
                var tiltle = "Delete?";
                var msg = $"Do you want to delete {product.ProductName}?";
                if (AppConfigUltility.CurrentLanguage.Equals("VI"))
                {
                    tiltle = "Xóa?";
                    msg = $"Bạn muốn xóa sản phẩm {product.ProductName}?";
                }
                var popup = new ConfirmPopup(tiltle, msg);
                if (popup.ShowDialog() == true)
                {
                    viewModel.DeleteProduct(index);
                }
                
            }
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = new ProductsViewModel();
            DataContext = viewModel;
            myTable.ItemsSource = viewModel.Products;
            this.CategoryCombobox.ItemsSource = viewModel.Categories;
            CategoryCombobox.SelectedIndex = 0;
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e)
        {

        }

        private void PopupBox_Closed(object sender, RoutedEventArgs e)
        {

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Search();
        }

        private void CategoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectCategory(CategoryCombobox.SelectedIndex);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Next();
        }

        private void Pre_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pre();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Đọc lên từ tập tin excel được chọn
            var screen = new OpenFileDialog();
            screen.Title = "Select a file";
            screen.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            List<Category> categories = new List<Category>();
            List<Product> products = new List<Product>();
            if (screen.ShowDialog() == true)
            {
                string filename = screen.FileName;

                var workbook = new Workbook(filename);

                var tabs = workbook.Worksheets;
                // In ra các tab để d ebug
                foreach (var tab in tabs)
                {
                    Category cat = new Category()
                    {
                        CategoryName = tab.Name,
                        Active = 1
                    };

                    // Bắt đầu từ ô B3
                    var column = 'C';
                    var row = 4;

                    var cell = tab.Cells[$"{column}{row}"];

                    while (cell.Value != null)
                    {
                        string name = cell.StringValue;
                        int price = tab.Cells[$"D{row}"].IntValue;
                        
                        string description = tab.Cells[$"E{row}"].StringValue;
                        int quantity = tab.Cells[$"F{row}"].IntValue;
                        string imagePath = tab.Cells[$"G{row}"].StringValue;
                        
                        var p = new Product()
                        {
                            ProductName = name,
                            ProductPrice = price,
                            ProductImage = imagePath,
                            ProductDescription = description,
                            ProductQuantityInStock = quantity,
                            CreateAt = DateTime.Now,
                            Active = 1,
                            Category = cat,
                        };
                        products.Add(p);
                        row++;
                        cell = tab.Cells[$"{column}{row}"];
                    }
                    categories.Add(cat);
                }
                viewModel.ImportData(categories, products);
            }
        }

        private void CategoryEdit_Click(object sender, RoutedEventArgs e)
        {
            var screen = new CategoryPopup(viewModel.Categories);
            screen.ShowDialog();
        }

        private void FilterByPrice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveFilter();
        }

        private void Filter_Opened(object sender, RoutedEventArgs e)
        {
            viewModel.OpenFilter();
        }
    }
}
