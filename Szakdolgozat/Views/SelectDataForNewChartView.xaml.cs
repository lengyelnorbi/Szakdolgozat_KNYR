﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
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
using Szakdolgozat.ViewModels;
using Szakdolgozat.Repositories;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for SelectDataForNewChartView.xaml
    /// </summary>
    public partial class SelectDataForNewChartView : UserControl
    {
        private SelectDataForNewChartViewModel _viewModel;
        DolgozoRepository _dolgozoRepository = new DolgozoRepository();
        KoltsegvetesRepository _koltsegvetesRepository = new KoltsegvetesRepository();
        KotelezettsegKovetelesRepository kotelezettsegKovetelesRepository = new KotelezettsegKovetelesRepository();
        GazdalkodoSzervezetRepository _gazdalkodoSzervezetRepository = new GazdalkodoSzervezetRepository();
        public SelectDataForNewChartView()
        {
            InitializeComponent();

            _viewModel = new SelectDataForNewChartViewModel();
            DataContext = _viewModel;

            _viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(SelectDataForNewChartViewModel.DataChanged))
                {
                    UpdateDataGrid();
                }
            };
        }

        private void UpdateDataGrid()
        {
            if (DataContext is SelectDataForNewChartViewModel viewModel)
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
