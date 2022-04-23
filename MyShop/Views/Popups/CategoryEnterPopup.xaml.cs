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
    /// Interaction logic for CategoryEnterPopup.xaml
    /// </summary>
    public partial class CategoryEnterPopup : Window
    {
        public Category newCategory;
        public CategoryEnterPopup(Category oldCategory)
        {
            InitializeComponent();
            newCategory = new Category
            {
                CategoryId = oldCategory.CategoryId,
                CategoryName = oldCategory.CategoryName
            };
            DataContext = newCategory;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            ProjectMyShopContext context = DbUltility.Instance.Db;

            DialogResult = true;
        }
    }
}
