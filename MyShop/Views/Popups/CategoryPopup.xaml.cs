using MyShop.Models;
using MyShop.Ultilities;
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

namespace MyShop.Views.Popups
{
    /// <summary>
    /// Interaction logic for CategoryPopup.xaml
    /// </summary>
    public partial class CategoryPopup : Window
    {
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        ProjectMyShopContext context = DbUltility.Instance.Db;
        public CategoryPopup(ObservableCollection<Category> OldCategories)
        {
            InitializeComponent();
            Categories = OldCategories;
            DataContext = this;
            myTable.ItemsSource = Categories;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new CategoryEnterPopup(new Category() { Active = 1});
            if (screen.ShowDialog() == true)
            {
                context.Categories.Add(screen.newCategory);
                context.SaveChanges();
                Categories.Add(screen.newCategory);
            }
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            Category c = (Category)((Button)e.Source).DataContext;
            var index = Categories.IndexOf(c);
            var screen = new CategoryEnterPopup(c);
            if (screen.ShowDialog() == true)
            {
                var result = context.Categories.Where(c => c.CategoryId == screen.newCategory.CategoryId).FirstOrDefault();
                if (result != null)
                {
                    result.CategoryName = screen.newCategory.CategoryName;
                    context.SaveChanges();
                    Categories[index] = screen.newCategory;
                }
                
            }

        }
        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Category c = (Category)((Button)e.Source).DataContext;
            var index = Categories.IndexOf(c);
            var screen = new ConfirmPopup("Delete?", $"Do you want to delete category {c.CategoryName}?");
            if (screen.ShowDialog() == true)
            {
                var result = context.Categories.Where(c => c.CategoryId == c.CategoryId).FirstOrDefault();
                if (result != null)
                {
                    result.Active = 0;
                    context.SaveChanges();
                    Categories.Remove(c);
                }
                
            }

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

        }

        private void DialogHost_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {

        }
    }
}
