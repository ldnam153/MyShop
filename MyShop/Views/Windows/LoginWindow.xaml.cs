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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public LoginWindow()
        {
            InitializeComponent();
            var connString = AppConfigUltility.GetConnectionStringFromLocal();
            if (AppConfigUltility.CheckConnection(connString))
            {
                AppConfigUltility.ConnectionString = connString;
                var screen = new MainWindow();
                screen.Show();
                this.Close();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
            
        }
        bool NotEmptyCheck()
        {
            if(this.UsernameTextBox.Text.Length == 0 || this.PasswordTextBox.Password.Length == 0)
            {
                return false;
            }
            return true;
        }
        bool Authen()
        {
            Password = this.PasswordTextBox.Password;
            return AppConfigUltility.Login(Username, Password);
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string title = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Notification" : "Thông báo";
            string msg = "";
            if (!NotEmptyCheck())
            {
                msg = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Please fill in all required fields" : "Vui lòng điền hết các trường cần thiết";
                var popup = new NotificationPopup(title, msg);
                popup.ShowDialog();
                return;
            }
                
           
            if (!Authen())
            {
                msg = AppConfigUltility.CurrentLanguage.Equals("EN") ? "Wrong username or password" : "Sai tài khoản hoặc mật khẩu";
                var popup = new NotificationPopup(title, msg);
                popup.ShowDialog();
                return;
            }
            var screen = new MainWindow();
            screen.Show();
            this.Close();
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
    }
}
