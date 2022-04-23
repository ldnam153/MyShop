using MyShop.Models;
using MyShop.Ultilities;
using MyShop.ViewModels;
using MyShop.Views.Popups;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyShop.Views.Windows
{
    /// <summary>
    /// Interaction logic for NewOrderWindow.xaml
    /// </summary>
    public partial class NewOrderWindow : Window
    {
        NewOrderViewModel viewModel;
        public Order newOrder;
        public NewOrderWindow()
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
            viewModel = new NewOrderViewModel();
            this.DataContext = viewModel;
            ProjectMyShopContext context = DbUltility.Instance.Db;
            var listGender = context.Genders.ToList();
            this.GenderCombobox.ItemsSource = listGender;
            this.myTable.ItemsSource = viewModel.ProductsInOrder;
        }


        private void newcustomer_Click(object sender, RoutedEventArgs e)
        {
            bool? isNewCustomer = newcustomer.IsChecked;
            viewModel.SelectOldCustomer = isNewCustomer == true ? false : true;
            viewModel.Order.Customer = new Customer() { Active = 1};
        }

        private void SelectCustomer_Click(object sender, RoutedEventArgs e)
        {
            var screen = new CustomerSelectionPopup();
            if (screen.ShowDialog() == true)
            {
                var customer = screen.selectedCustomer;
                viewModel.SelectCustomer(customer);
               
            }
        }

        private void addRowButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new ProductSelectionPopup();
            if (screen.ShowDialog() == true)
            {
                var product = screen.selectedProduct;
                viewModel.AddProduct(product);
                
            }
        }

        private void Descrease_Click(object sender, RoutedEventArgs e)
        {
            OrderedProduct p = (OrderedProduct)((Button)e.Source).DataContext;
            viewModel.DescreaseAmountOrderedProduct(p);
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            OrderedProduct p = (OrderedProduct)((Button)e.Source).DataContext;
                viewModel.IncreaseAmountOrderedProduct(p);
        }
        bool NotEmptyProductsCheck()
        {
            return (viewModel.ProductsInOrder.Count != 0);
        }
        bool NotEmptyCheck()
        {
            Customer newCustomer = viewModel.Order.Customer;
            if (newCustomer.CustomerName.Length == 0 || newCustomer.CustomerPhone.Length == 0 || newCustomer.CustomerAddress.Length == 0)
            {
                return false;
            }
            return true;
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
            if (!NotEmptyProductsCheck())
            {
                var msg = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Order must contain at least 1 product" : "Đơn hàng phải có ít nhất 1 sản phẩm";
                var popup = new NotificationPopup(title, msg);
                popup.ShowDialog();
                return;
            }
            viewModel.SaveOrder();
            newOrder = viewModel.Order;
            DialogResult = true;
        }

        private void PayNow_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Order.Complete = 1;
        }

        private void PreOrder_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Order.Complete = 0;
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            var oldCustomer = viewModel.Order.Customer;
            var screen = new EditCustomerWindow(oldCustomer);
            if (screen.ShowDialog() == true)
            {
                var newCustomer = screen.newCustomer;
                viewModel.EditCustomer(newCustomer);

            }
        }


        // End: Button Close | Restore | Minimize
    }
}
