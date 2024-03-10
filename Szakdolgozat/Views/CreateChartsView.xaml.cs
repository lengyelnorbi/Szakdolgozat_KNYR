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
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;
using Szakdolgozat.ViewModels;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for CreateChartsView.xaml
    /// </summary>
    public partial class CreateChartsView : Window
    {
        public CreateChartsView()
        {
            InitializeComponent();

            DataContext = new CreateChartsViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (DataContext is SelectDataForNewChartViewModel viewModel)
            //{
            //    foreach(var row in viewModel.SelectedRows)
            //    {
            //        if(row is Dolgozo dolgozo)
            //        {

            //        }
            //        else if(row is BevetelKiadas bevetelKiadas)
            //        {

            //        }
            //        else if(row is GazdalkodoSzervezet gazdalkodoSzervezet)
            //        {

            //        }
            //    }
            //}
            goBackButton.Visibility = Visibility.Visible;
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            goBackButton.Visibility = Visibility.Hidden;
        }
    }
}
