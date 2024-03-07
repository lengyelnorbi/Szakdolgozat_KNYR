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
        private CreateChartsViewModel _viewModel;
        DolgozoRepository _dolgozoRepository = new DolgozoRepository();
        KoltsegvetesRepository _koltsegvetesRepository = new KoltsegvetesRepository();
        KotelezettsegKovetelesRepository kotelezettsegKovetelesRepository = new KotelezettsegKovetelesRepository();
        GazdalkodoSzervezetRepository _gazdalkodoSzervezetRepository = new GazdalkodoSzervezetRepository();

        public CreateChartsView()
        {
            InitializeComponent();

            _viewModel = new CreateChartsViewModel();
            DataContext = _viewModel;

            _viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(CreateChartsViewModel.DataChanged))
                {
                    UpdateDataGrid();
                }
            };
        }

        private void UpdateDataGrid()
        {
            if (DataContext is CreateChartsViewModel viewModel)
            {
                switch (viewModel.SelectedDataSourceName)
                {
                    case "Dolgozok":
                        dataGrid.ItemsSource = _dolgozoRepository.GetDolgozok();
                        break;
                    case "Gazdasági Szervezetek":
                        dataGrid.ItemsSource = _gazdalkodoSzervezetRepository.GetGazdalkodoSzervezetek();
                        break;
                    case "Bevétel":
                        dataGrid.ItemsSource = _koltsegvetesRepository.GetKoltsegvetesek();
                        break;
                    case "Kiadás":
                        dataGrid.ItemsSource = _koltsegvetesRepository.GetKoltsegvetesek();
                        break;
                    default:
                        dataGrid.ItemsSource = _dolgozoRepository.GetDolgozok();
                        break;
                }
            }
        }
    }
}
