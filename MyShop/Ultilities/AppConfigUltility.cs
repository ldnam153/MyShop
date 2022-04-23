using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Linq;
using System.Security.Cryptography;
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
using Microsoft.Data.SqlClient;

namespace MyShop.Ultilities
{
    public class AppConfigUltility
    {
        public static string Server = "Server";
        public static string Database = "Database";
        public static string Username = "Username";
        public static string Password = "Password";
        public static string Entropy = "Entropy";
        public static string Theme = "Theme";
        public static string Page = "Page";
        public static string Language = "Language";
        public static string PageSize = "PageSize";

        public static string ConnectionString = "";
        public static string CurrentLanguage = "";
        public static string CurrentTheme = "Dark";
        public static int CurrentPage = 0;
        public static int CurrentPageSize = 3;
        public static string GetValue(string key)
        {
            string? value = ConfigurationManager
                .AppSettings[key];
            return value;
        }

        public static void SetValue(string key, string value)
        {
            var configFile = ConfigurationManager
            .OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings[key].Value = value;

            configFile.Save(ConfigurationSaveMode.Minimal);
        }
        public static bool CheckConnection(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
        public static bool Login(string username, string password)
        {
            string? server = AppConfigUltility.GetValue(AppConfigUltility.Server);
            string? database = AppConfigUltility.GetValue(AppConfigUltility.Database);
            var connectionString = $"Data Source={server};Initial Catalog={database};User Id={username};Password={password};";
            bool IsTrue = CheckConnection(connectionString);
            if (IsTrue)
            {
                SavePassword(password);
                ConnectionString = connectionString;
                AppConfigUltility.SetValue(AppConfigUltility.Username, username);
                
                return true;
            }
            return false;
        }
        public static void Logout()
        {
            AppConfigUltility.SetValue(AppConfigUltility.Username, "");
            AppConfigUltility.SetValue(AppConfigUltility.Password, "");
            AppConfigUltility.SetValue(AppConfigUltility.Entropy, "");
        }
        public static string? GetConnectionStringFromLocal()
        {
            string? result = "";
            
            string? server = AppConfigUltility.GetValue(AppConfigUltility.Server);
            string? database = AppConfigUltility.GetValue(AppConfigUltility.Database);
            string? username = AppConfigUltility.GetValue(AppConfigUltility.Username);
            string? password = GetPassword();
            result = $"Data Source={server};Initial Catalog={database};User Id={username};Password={password};";
            return result;
        }
        public static void LoadConfig()
        {
            CurrentLanguage = AppConfigUltility.GetValue(AppConfigUltility.Language);
            CurrentTheme = AppConfigUltility.GetValue(AppConfigUltility.Theme);
            CurrentPage = Int32.Parse(AppConfigUltility.GetValue(AppConfigUltility.Page)); 
            CurrentPageSize = Int32.Parse(AppConfigUltility.GetValue(AppConfigUltility.PageSize));
        }
        public static string GetPassword()
        {
            try
            {
                var cypherText = AppConfigUltility.GetValue(AppConfigUltility.Password);
                var cypherTextInBytes = Convert.FromBase64String(cypherText!);

                var entropyText = AppConfigUltility.GetValue(AppConfigUltility.Entropy);
                var entropyTextInBytes = Convert.FromBase64String(entropyText);

                var passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
                    entropyTextInBytes, DataProtectionScope.CurrentUser);

                var password = Encoding.UTF8.GetString(passwordInBytes);
                return password;
            }
            catch(Exception ex)
            {

            }
            return "";
        }
        public static void SavePassword(string password)
        {

            var passwordInBytes = Encoding.UTF8.GetBytes(password);

            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            var entropyBase64 = Convert.ToBase64String(entropy);

            var cypherText = ProtectedData.Protect(passwordInBytes, entropy,
                DataProtectionScope.CurrentUser);
            var cypherTextBase64 = Convert.ToBase64String(cypherText);

            AppConfigUltility.SetValue(AppConfigUltility.Password, cypherTextBase64);
            AppConfigUltility.SetValue(AppConfigUltility.Entropy, entropyBase64);
        }
        public static void ApplyChange()
        {
            string language = AppConfigUltility.CurrentLanguage;
            string theme = AppConfigUltility.CurrentTheme;
            if (language.Equals("VI"))
            {
                Application.Current.Resources.MergedDictionaries[0].Source = new Uri("./Resources/Languages/Vietnamese.xaml", UriKind.RelativeOrAbsolute);
            }
            else if (language.Equals("EN"))
            {
               
                Application.Current.Resources.MergedDictionaries[0].Source = new Uri("./Resources/Languages/English.xaml", UriKind.RelativeOrAbsolute);
            }

            if (theme.Equals("Light"))
            {
                Application.Current.Resources.MergedDictionaries[1].Source = new Uri("./Resources/Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute);
                Application.Current.Resources.MergedDictionaries[2].Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml", UriKind.RelativeOrAbsolute);

            }
            else if (theme.Equals("Dark"))
            {
                Application.Current.Resources.MergedDictionaries[1].Source = new Uri("./Resources/Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute);
                Application.Current.Resources.MergedDictionaries[2].Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml", UriKind.RelativeOrAbsolute);
            }

        }
    }
}
