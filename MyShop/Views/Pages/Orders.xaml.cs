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
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        OrdersViewModel viewModel;
        public Orders()
        {
            InitializeComponent();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new NewOrderWindow();
            if (screen.ShowDialog() == true)
            {
                var newOrder = screen.newOrder;
                viewModel.AddOrder(newOrder);
            }
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = this.myTable.SelectedIndex;
            if (index >= 0)
            {
                var oldOrder = viewModel.Orders[index];
                var screen = new ViewDetailOrderWindow(oldOrder);
                screen.Show();
            }
            
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = this.myTable.SelectedIndex;
            if (index >= 0)
            {
                var order = viewModel.Orders[index];
                var tiltle = "Delete?";
                var msg = $"Do you want to delete Order {order.OrderId}?";
                if (AppConfigUltility.CurrentLanguage.Equals("VI"))
                {
                    tiltle = "Xóa?";
                    msg = $"Bạn muốn xóa đơn hàng {order.OrderId}?";
                }
                var popup = new ConfirmPopup(tiltle, msg);
                if (popup.ShowDialog() == true)
                {
                    viewModel.DeleteOrder(index);
                }
               
                
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = new OrdersViewModel();
            this.DataContext = viewModel;
            this.myTable.ItemsSource = viewModel.Orders;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Next();
        }

        private void Pre_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pre();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            viewModel.CheckFilterByDate();
        }


        private void preDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? date = this.preDate.SelectedDate;
            if (date != null && viewModel.FilterCheck )
                viewModel.SetPreDate((DateTime)date);
        }

        private void lastDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? date = this.lastDate.SelectedDate;
            if (date != null && viewModel.FilterCheck)
                viewModel.SetLastDate((DateTime)date);
        }

        private void InProgressFilter_Click(object sender, RoutedEventArgs e)
        {
            viewModel.CheckInProgressFilter();
        }
    }
}
