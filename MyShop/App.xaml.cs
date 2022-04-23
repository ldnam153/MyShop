using MyShop.Ultilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AppConfigUltility.LoadConfig();
            AppConfigUltility.ApplyChange();
        }
        
    }
}
