using MyShop.Models;
using MyShop.Ultilities;
using MyShop.ViewModels;
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
    /// Interaction logic for ViewDetailOrderWindow.xaml
    /// </summary>
    public partial class ViewDetailOrderWindow : Window
    {
        ViewDetailOrderViewModel viewModel;
        public ViewDetailOrderWindow(Order oldOrder)
        {
            InitializeComponent();
            viewModel = new ViewDetailOrderViewModel(oldOrder);
            
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
            this.DataContext = viewModel;
            this.myTable.ItemsSource = viewModel.Order.OrderedProducts;
            
        }

        private void CompleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var check = MessageBox.Show($"Complete Order {viewModel.Order.OrderId}?", "Complete?", MessageBoxButton.YesNo);
            if (check == MessageBoxResult.Yes)
            {
                viewModel.CompleteOrder();
            }
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            var check = MessageBox.Show($"Cancel Order {viewModel.Order.OrderId}?", "Cancel?", MessageBoxButton.YesNo);
            if (check == MessageBoxResult.Yes)
            {
                viewModel.CancelOrder();
            }
        }
    }
}
