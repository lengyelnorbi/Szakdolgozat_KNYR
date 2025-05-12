using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;
using Szakdolgozat.Views;

namespace Szakdolgozat.ViewModels
{
    public class LetezoDiagrammokViewModel : ViewModelBase
    {
        private ObservableCollection<Diagramm> _diagramms;
        public ObservableCollection<Diagramm> Diagrams
        {
            get { return _diagramms; }
            set
            {
                _diagramms = value;
                OnPropertyChanged(nameof(Diagrams));
            }
        }

        private ObservableCollection<Diagramm> _filteredDiagramms;
        public ObservableCollection<Diagramm> FilteredDiagrams
        {
            get { return _filteredDiagramms; }
            set
            {
                _filteredDiagramms = value;
                OnPropertyChanged(nameof(FilteredDiagrams));
            }
        }

        private bool _isOnlyOwnDiagramsVisible = false;
        public bool IsOnlyOwnDiagramsVisible
        {
            get { return _isOnlyOwnDiagramsVisible; }
            set
            {
                _isOnlyOwnDiagramsVisible = value;
                OnPropertyChanged(nameof(IsOnlyOwnDiagramsVisible));
                if(value)
                {
                    FilterDiagrams();
                }
                else
                {
                    FilteredDiagrams = new ObservableCollection<Diagramm>(Diagrams.ToList());
                }
            }
        }

        public ICommand OpenDiagramCommand { get; }
        public ICommand DeleteDiagramCommand { get; }
        private DiagrammRepository _diagrammRepository { get; set; } = new DiagrammRepository();

        public LetezoDiagrammokViewModel()
        {
            OpenDiagramCommand = new ViewModelCommand(ExecuteOpenDiagramCommand);
            DeleteDiagramCommand = new ViewModelCommand(ExecuteDeleteDiagramCommand);

            Diagrams = new ObservableCollection<Diagramm>(_diagrammRepository.GetAllDiagramms());
            foreach(var diagramm in Diagrams)
            {
                var values = GetSeriesValues(diagramm.DataChartValues);
                diagramm.PreviewChart = new SeriesCollection();
                diagramm.PreviewPieChart = new SeriesCollection();
                var groupBySettings = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(diagramm.GroupBySettings);
                foreach (var value in values)
                {
                    switch (diagramm.ChartType)
                    {
                        case "DoughnutSeries":
                            diagramm.PreviewPieChart.Add(new LiveCharts.Wpf.PieSeries
                            {
                                Values = value.Values,
                                Title = value.Name,
                                Name = value.Name,
                                DataLabels = false,
                                Fill = value.Fill,
                            });
                            diagramm.InnerRadius = groupBySettings["InnerRadius"] != null ? (double)groupBySettings["InnerRadius"] : 0.0;
                            break;
                        case "RowSeries":
                            diagramm.PreviewChart.Add(new RowSeries
                            {
                                Values = value.Values,
                                Title = value.Name,
                                Name = value.Name,
                                DataLabels = false,
                                Fill = value.Fill,
                            });
                            break;
                        case "StackedColumnSeries":
                            diagramm.PreviewChart.Add(new LiveCharts.Wpf.StackedColumnSeries
                            {
                                Values = value.Values,
                                Title = value.Name,
                                Name = value.Name,
                                DataLabels = false,
                                Fill = value.Fill,
                            });
                            break;
                        case "BasicColumnSeries":
                            diagramm.PreviewChart.Add(new LiveCharts.Wpf.ColumnSeries
                            {
                                Values = value.Values,
                                Title = value.Name,
                                Name = value.Name,
                                DataLabels = false,
                                Fill = value.Fill,
                            });
                            break;
                        case "LineSeries":
                            diagramm.PreviewChart.Add(new LiveCharts.Wpf.LineSeries
                            {
                                Values = value.Values,
                                Title = value.Name,
                                DataLabels = false,
                                PointGeometry = null // Hide points for cleaner preview
                            });
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine($"Unsupported chart type: {diagramm.ChartType}");
                            break;
                    }
                }
            }
            FilteredDiagrams = new ObservableCollection<Diagramm>(
                Diagrams.Select(d => new Diagramm(d.ID, d.Name, d.Description, d.ChartType, d.DataSource, d.DataChartValues, d.FilterSettings, d.GroupBySettings, d.SeriesGroupBySelection, d.SelectedItemsIDs, d.DataStatistic, d.CreatedDate,
                d.CreatedByUserID, d.CreatorName, d.PreviewChart, d.PreviewPieChart, d.InnerRadius)).ToList()
            );
        }

        public void FilterDiagrams()
        {
            FilteredDiagrams.Clear();
            foreach (var diagram in Diagrams)
            {
                if(diagram.CreatedByUserID == Mediator.NotifyGetUserID())
                {
                    FilteredDiagrams.Add(diagram);
                }
            }
        }
        private List<SeriesItem> GetSeriesValues(string values)
        {
            var result = new List<SeriesItem>();

            if (string.IsNullOrWhiteSpace(values))
                return result;

            try
            {
                // Deserialize the JSON into a list of SeriesItem objects
                var seriesItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SeriesItem>>(values);

                if (seriesItems != null)
                {
                    foreach (var item in seriesItems)
                    {
                        // Convert each SeriesItem into a SeriesData object
                        result.Add(new SeriesItem
                        {
                            Name = item.Name,
                            Values = new ChartValues<double>(item.Values)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                System.Diagnostics.Debug.WriteLine($"Error deserializing DataChartValues: {ex.Message}");
            }

            return result;
        }

        private void ExecuteOpenDiagramCommand(object parameter)
        {
            if (parameter is Diagramm diagram)
            {
                // Open the CreateChartsView with pre-filled data
                var createChartsView = new CreateChartsView(diagram.ChartType);
                if (createChartsView.DataContext is CreateChartsViewModel viewModel)
                {
                    viewModel.LoadDiagram(diagram);
                    createChartsView.Show();
                    viewModel.UpdateSearch(viewModel.SearchQuery);
                }
            }
        }

        private void ExecuteDeleteDiagramCommand(object parameter)
        {
            if (parameter is Diagramm diagram)
            {
                // Ask for confirmation in Hungarian
                var result = System.Windows.MessageBox.Show(
                    $"Biztosan törölni szeretné a(z) \"{diagram.Name}\" diagramot?",
                    "Törlés megerősítése",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    _diagrammRepository.DeleteDiagramm(diagram.ID);
                    // Refresh the diagrams list
                    Diagrams = new ObservableCollection<Diagramm>(_diagrammRepository.GetAllDiagramms());
                }
            }
        }
    }
}
