using MyShop.Ultilities;
using MyShop.ViewModels;
using MyShop.Views.Popups;
using MyShop.Views.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int Number { get; set; }
        public bool IsChanged { get; set; } = false;
        public Settings()
        {
            InitializeComponent();
            DataContext = this;
            Number = AppConfigUltility.CurrentPageSize;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            string language = AppConfigUltility.CurrentLanguage;
            string theme = AppConfigUltility.CurrentTheme;
            
            
            if (language.Equals("VI"))
            {
                this.LanguageComboBox.SelectedIndex = 0;
            }
            else if (language.Equals("EN"))
            {
                this.LanguageComboBox.SelectedIndex = 1;
            }

            if (theme.Equals("Light"))
            {
                this.ThemeCombobox.SelectedIndex = 0;
            }
            else if (theme.Equals("Dark"))
            {
                this.ThemeCombobox.SelectedIndex = 1;
            }
            IsChanged = false;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //save page size
            AppConfigUltility.CurrentPageSize = Number;
            AppConfigUltility.SetValue(AppConfigUltility.PageSize, Number.ToString());

            //save language
            int selectedLanguageIndex = LanguageComboBox.SelectedIndex;
            string language = selectedLanguageIndex == 0 ? "VI" : "EN";
            AppConfigUltility.CurrentLanguage = language;
            AppConfigUltility.SetValue(AppConfigUltility.Language, language);


            //save theme
            int selectedThemeIndex = ThemeCombobox.SelectedIndex;
            string theme = selectedThemeIndex == 0 ? "Light" : "Dark";
            IsChanged = false;
            AppConfigUltility.CurrentTheme = theme;
            AppConfigUltility.SetValue(AppConfigUltility.Theme, theme);
            AppConfigUltility.ApplyChange();
        }

        private void Change(object sender, TextChangedEventArgs e)
        {
            IsChanged = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsChanged = true;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var tiltle = "Logout?";
            var msg = $"Do you want to logout?";
            if (AppConfigUltility.CurrentLanguage.Equals("VI"))
            {
                tiltle = "Đăng xuất?";
                msg = $"Bạn muốn đăng xuất?";
            }
            var popup = new ConfirmPopup(tiltle, msg);
            if (popup.ShowDialog() == true)
            {
                AppConfigUltility.Logout();
                var screen = new LoginWindow();
                screen.Show();
                var parentWindow = this.Parent as Window;

                if (parentWindow != null)
                {
                    parentWindow.Close();
                }
            }
        }
    }
}
