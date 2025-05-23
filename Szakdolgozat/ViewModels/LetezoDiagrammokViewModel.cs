using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;
using Szakdolgozat.Views;

namespace Szakdolgozat.ViewModels
{
    public class LetezoDiagrammokViewModel : ViewModelBase
    {
        private ObservableCollection<Diagram> _diagramms;
        public ObservableCollection<Diagram> Diagrams
        {
            get { return _diagramms; }
            set
            {
                _diagramms = value;
                OnPropertyChanged(nameof(Diagrams));
            }
        }

        private ObservableCollection<Diagram> _filteredDiagramms;
        public ObservableCollection<Diagram> FilteredDiagrams
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
                    FilteredDiagrams = new ObservableCollection<Diagram>(Diagrams.ToList());
                }
            }
        }

        public ICommand OpenDiagramCommand { get; }
        public ICommand DeleteDiagramCommand { get; }
        private DiagramRepository _diagrammRepository { get; set; } = new DiagramRepository();

        public LetezoDiagrammokViewModel()
        {
            OpenDiagramCommand = new ViewModelCommand(ExecuteOpenDiagramCommand);
            DeleteDiagramCommand = new ViewModelCommand(ExecuteDeleteDiagramCommand);

            RefreshDiagram(null);
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
                    try
                    {
                        foreach (var series in seriesItems)
                        {
                            result.Add(new SeriesItem
                            {
                                Name = series.Name,
                                Values = new ChartValues<double>(series.Values),
                                Fill = series.Fill,
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error restoring group by selections: {ex.Message}");
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
            if (parameter is Diagram diagram)
            {
                // Open the CreateChartsView with pre-filled data
                var createChartsView = new CreateChartsView(diagram.ChartType);
                if (createChartsView.DataContext is CreateChartsViewModel viewModel)
                {
                    viewModel.LoadDiagram(diagram);
                    createChartsView.Show();
                    viewModel.UpdateSearch(viewModel.SearchQuery);
                    Mediator.DiagramModified += RefreshDiagram;
                }
            }
        }

        private void ExecuteDeleteDiagramCommand(object parameter)
        {
            if (parameter is Diagram diagram)
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
                    RefreshDiagram(null);
                }
            }
        }

        private void RefreshDiagram(Diagram diagram)
        {
            Diagrams = new ObservableCollection<Diagram>(_diagrammRepository.GetAllDiagramms());
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredMaganSzemelyek = MaganSzemelyek, in this case when clearing the FilteredMaganSzemelyek in later times, it will affect the MaganSzemelyek collection too
            foreach (var diagramm in Diagrams)
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
                                PointGeometry = null, // Hide points for cleaner preview
                                PointForeground = value.Fill,
                                Stroke = value.Fill,
                            });
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine($"Unsupported chart type: {diagramm.ChartType}");
                            break;
                    }
                }
            }
            FilteredDiagrams = new ObservableCollection<Diagram>(
              Diagrams.Select(d => new Diagram(d.ID, d.Name, d.Description, d.ChartType, d.DataSource, d.DataChartValues, d.FilterSettings, d.GroupBySettings, d.SeriesGroupBySelection, d.SelectedItemsIDs, d.DataStatistic, d.CreatedDate,
              d.CreatedByUserID, d.CreatorName, d.PreviewChart, d.PreviewPieChart, d.InnerRadius)).ToList()
          );
        }
    }
}
