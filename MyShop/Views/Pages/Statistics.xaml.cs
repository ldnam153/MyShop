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
using LiveCharts;
using LiveCharts.Wpf;
using MyShop.ViewModels;
using MyShop.Views.Popups;

namespace MyShop.Views.Pages
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Page
    {
        StatisticsViewModel viewModel;
        public Statistics()
        {
            InitializeComponent();
            viewModel = new StatisticsViewModel();
            this.DataContext = viewModel;
            this.RevenueChart.Series = viewModel.Revenues;
            this.SalesChart.Series = viewModel.Sales;
            this.PieWeek.Series = viewModel.piechartData1;
            this.PieMonth.Series = viewModel.piechartData2;
            this.PieYear.Series = viewModel.piechartData3;
            ViewType.SelectedIndex = 0;
        }


        private void preDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? date = this.preDate.SelectedDate;
            if (date != null && viewModel.DateFilterCheck)
                viewModel.SetPreDate((DateTime)date);
        }

        private void lastDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? date = this.lastDate.SelectedDate;
            if (date != null && viewModel.DateFilterCheck)
                viewModel.SetLastDate((DateTime)date);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ViewType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectViewType(ViewType.SelectedIndex);
        }

        private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SelectProduct_Click(object sender, RoutedEventArgs e)
        {
            var screen = new ProductSelectionPopup();
            if (screen.ShowDialog() == true)
            {
                var product = screen.selectedProduct;
                viewModel.SelecteProduct(product);

            }
        }
    }
}
