using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using MyShop.Ultilities;
using System.Collections.Generic;
using MyShop.Views.Controls;

namespace MyShop.Views.Windows
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var t = DbUltility.Instance.Db;
        }
        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        // Start: MenuLeft PopupButton //
     
        private void btnHome_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnDashboard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnDashboard;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Dashboard";
            }
        }

        private void btnDashboard_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnProducts_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnProducts;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Products" : "Sản phẩm";
            }
        }

        private void btnProducts_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        
        private void btnProductStock_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnOrderList_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnOrderList;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Orders" : "Đơn hàng" ;
            }
        }

        private void btnOrderList_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnStatistics_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = this.btnStatistics;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Statistics" : "Thống kê";
            }
        }

        private void btnStatistics_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnCustomer_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = this.btnCustomers;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Customers" : "Khách hàng";
            }
        }

        private void btnCustomer_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

       

        private void btnSecurity_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }
        private void btnSetting_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnSetting;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Setting" : "Cấu hình";
            }
        }

        private void btnSetting_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }
        // End: MenuLeft PopupButton //

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


        static Uri dashBoardUri = new System.Uri("Views/Pages/Dashboard.xaml", UriKind.RelativeOrAbsolute);
        static Uri productsUri = new System.Uri("Views/Pages/Products.xaml", UriKind.RelativeOrAbsolute);
        static Uri ordersUri = new System.Uri("Views/Pages/Orders.xaml", UriKind.RelativeOrAbsolute);
        static Uri statisticsUri = new System.Uri("Views/Pages/Statistics.xaml", UriKind.RelativeOrAbsolute);
        static Uri customersUri = new System.Uri("Views/Pages/Customers.xaml", UriKind.RelativeOrAbsolute);
        static Uri settingUri = new System.Uri("Views/Pages/Settings.xaml", UriKind.RelativeOrAbsolute);
        List<Uri> Uris = new List<Uri>()
        {
            dashBoardUri,
            productsUri,
            ordersUri,
            statisticsUri,
            customersUri,
            settingUri
        };
        List<string> TitlesEN = new List<string>()
        {
            "Dashboard",
            "Products",
            "Orders",
            "Statistics",
            "Customers",
            "Setting"
        };
        List<string> TitlesVI = new List<string>()
        {
            "Dashboard",
            "Sản phẩm",
            "Đơn hàng",
            "Thống kê",
            "Khách hàng",
            "Cấu hình"
        };
        List<MenuItem> MenuItems = new List<MenuItem>();
        void MoveMenu(int i)
        {
            fContainer.Navigate(Uris[i]);
            this.Title.Text = AppConfigUltility.CurrentLanguage.Equals("EN") ?  TitlesEN[i] : TitlesVI[i];
            MenuItems[i].IsSelected = true;
            AppConfigUltility.CurrentPage = i;
            AppConfigUltility.SetValue(AppConfigUltility.Page, i.ToString());
        }
        private void home_Loaded(object sender, RoutedEventArgs e)
        {
            MenuItems = new List<MenuItem>()
            {
                menuItem1,
                menuItem2,
                menuItem3,
                menuItem4,
                menuItem5,
                menuItem6
            };
            MoveMenu(AppConfigUltility.CurrentPage);
        }
        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MoveMenu(0);
        }
        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            MoveMenu(1);
        }

        private void btnOrderList_Click(object sender, RoutedEventArgs e)
        {
            MoveMenu(2);
        }


        private void btnStatistics_Click(object sender, RoutedEventArgs e)
        {
            MoveMenu(3);
        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            MoveMenu(4);
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            MoveMenu(5);
        }
    }
}
