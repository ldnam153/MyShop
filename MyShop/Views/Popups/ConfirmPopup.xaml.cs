﻿using System;
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

namespace MyShop.Views.Popups
{
    /// <summary>
    /// Interaction logic for ConfirmPopup.xaml
    /// </summary>
    public partial class ConfirmPopup : Window
    {
        public string TitleText { get; set; } = "";
        public string Message { get; set;} = "";
        public ConfirmPopup(string title, string message)
        {
            InitializeComponent();
            TitleText = title;
            Message = message;
            DataContext = this;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
