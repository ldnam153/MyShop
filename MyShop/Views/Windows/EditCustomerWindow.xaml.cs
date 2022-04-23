using MyShop.Models;
using MyShop.Ultilities;
using MyShop.Views.Popups;
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

namespace MyShop.Views.Windows
{
    /// <summary>
    /// Interaction logic for EditCustomerWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow : Window
    {
        public Customer newCustomer;
        public EditCustomerWindow(Customer oldCustomer)
        {
            InitializeComponent();
            newCustomer = oldCustomer.Clone() as Customer;
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

        bool NotEmptyCheck()
        {
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
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectMyShopContext dbContext = DbUltility.Instance.Db;
            var list = dbContext.Genders.ToList();
            GenderCombobox.ItemsSource = list;
            this.DataContext = newCustomer;
            GenderCombobox.SelectedItem = newCustomer.Gender as Gender;
        }
        // End: Button Close | Restore | Minimize
    }
}
