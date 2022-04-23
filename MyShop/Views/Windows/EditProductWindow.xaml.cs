using Microsoft.Win32;
using MyShop.Models;
using MyShop.Ultilities;
using MyShop.Views.Popups;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MyShop.Views.Windows
{
    /// <summary>
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public Product newProduct;
        BindingList<Category> Categories;
        public Category SelectedItem { get => Categories[CategoriesCombobox.SelectedIndex]; }
        public EditProductWindow(Product oldProduct)
        {
            InitializeComponent();
            newProduct = oldProduct.Clone() as Product;
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

        // End: Button Close | Restore | Minimize
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectMyShopContext dbContext = DbUltility.Instance.Db;
            var list = dbContext.Categories.ToList();
            Categories = new BindingList<Category>(list);
            CategoriesCombobox.ItemsSource = Categories;
            this.DataContext = newProduct;
            CategoriesCombobox.SelectedItem = newProduct.Category as Category;
        }
        private string sourceFile = "";
        bool NotEmptyCheck()
        {
            if (newProduct.ProductName.Length == 0 || newProduct.Category == null)
            {
                return false;
            }
            return true;
        }
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            string title = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Notification" : "Thông báo";
            if (!NotEmptyCheck())
            {
                var msg = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Please fill in all required fields" : "Vui lòng điền hết các trường cần thiết";
                var popup = new NotificationPopup(title, msg);
                popup.ShowDialog();
                return;
            }
            else
            {
                if (!sourceFile.Equals(""))
                    FileUltility.CopyImage(sourceFile, newProduct.ProductImage);
                DialogResult = true;
            }
            
        }

        private void ImagePicker_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                // op.FileName
                sourceFile = op.FileName;
                newProduct.ProductImage = FileUltility.CreateNewImagePath(sourceFile);
                this.Preview.Source = new BitmapImage(new Uri(sourceFile));

            }
        }

        private void CategoriesCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = CategoriesCombobox.SelectedIndex;
            if (index >= 0)
            {
                newProduct.Category = Categories[index];
                newProduct.CategoryId = newProduct.Category.CategoryId;
            }

        }
    }
}
