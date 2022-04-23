using MyShop.Ultilities;
using MyShop.ViewModels;
using MyShop.Views.Popups;
using MyShop.Views.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop.Views.Pages
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : Page
    {
        CustomersViewModel viewModel;
        public Customers()
        {
            InitializeComponent();
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new NewCustomerWindow();
            if (screen.ShowDialog() == true)
            {
                var newCustomer = screen.newCustomer;
                viewModel.AddCustomer(newCustomer);
            }
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = this.myTable.SelectedIndex;
            if (index >= 0)
            {
                var oldCustomer = viewModel.Customers[index];
                var screen = new EditCustomerWindow(oldCustomer);
                if (screen.ShowDialog() == true)
                {
                    var newCustomer = screen.newCustomer;
                    viewModel.EditCustomer(index, newCustomer);

                }
            }
            
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = this.myTable.SelectedIndex;
            if (index >= 0)
            {
                var customer = viewModel.Customers[index];
                var tiltle = "Delete?";
                var msg = $"Do you want to delete Customer {customer.CustomerName}?";
                if (AppConfigUltility.CurrentLanguage.Equals("VI"))
                {
                    tiltle = "Xóa?";
                    msg = $"Bạn muốn xóa khách hàng {customer.CustomerName}?";
                }
               var popup = new ConfirmPopup(tiltle, msg);
                if (popup.ShowDialog() == true)
                {
                    viewModel.DeleteCustomer(index);
                }
                 
                
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = new CustomersViewModel();
            DataContext = viewModel;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Next();
        }

        private void Pre_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pre();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Search();
        }
    }
}
