using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Szakdolgozat.Views;
using System.Windows;
using System.Diagnostics;
using LiveCharts.Defaults;
using LiveCharts.Wpf.Charts.Base;
using System.Windows.Media;

namespace Szakdolgozat.ViewModels
{
    class UjDiagrammokViewModel : ViewModelBase
    {
        public UjDiagrammokViewModel() 
        {
            CreateChartCommand = new ViewModelCommand(ExecuteCreateChartCommand, CanExecuteCreateChartCommand);

            TestDataRowSeries();
            TestDataPieSeries();
            TestDataStackedColumnSeries();
            TestDataBasicColumnSeries();
            TestDataLineSeries();
        }

        public ICommand CreateChartCommand { get; }

        private bool CanExecuteCreateChartCommand(object obj)
        {
            return true;
        }
        private void ExecuteCreateChartCommand(object obj)
        {
            CreateChartsView createCharts = new CreateChartsView(obj.ToString());
            createCharts.Show();
        }

        public SeriesCollection TestRowSeriesForShow { get; set; }
        public string[] TestRowSeriesLabelsForShow { get; set; }
        public Func<double, string> TestRowSeriesFormatterForShow { get; set; }

        public SeriesCollection TestLineSeriesForShow { get; set; }
        public string[] TestLineSeriesLabelsForShow { get; set; }
        public Func<double, string> TestLineSeriesYFormatterForShow { get; set; }

        public SeriesCollection TestBasicColumnSeriesForShow { get; set; }
        public string[] TestBasicColumnLabelsForShow { get; set; }
        public Func<double, string> TestBasicColumnFormatterForShow { get; set; }

        public SeriesCollection TestStackedColumnSeriesForShow { get; set; }
        public string[] TestStackedColumnSeriesLabelsForShow { get; set; }
        public Func<double, string> TestStackedColumnSeriesFormatterForShow { get; set; }

        public SeriesCollection TestDoghnutSeriesForShow { get; set; }
        public Func<ChartPoint, string> TestPieRowSeriesLableForShow { get; set; }
        public void TestDataRowSeries()
        {
            TestRowSeriesForShow = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            TestRowSeriesForShow.Add(new RowSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            TestRowSeriesForShow[1].Values.Add(48d);

            TestRowSeriesLabelsForShow = new[] { "Maria", "Susan", "Charles", "Frida" };
            TestRowSeriesFormatterForShow = value => value.ToString("N");
        }

        public void TestDataLineSeries()
        {
            TestLineSeriesForShow = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            TestLineSeriesLabelsForShow = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            TestLineSeriesYFormatterForShow = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            TestLineSeriesForShow.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            TestLineSeriesForShow[3].Values.Add(5d);
        }

        public void TestDataBasicColumnSeries()
        {
            TestBasicColumnSeriesForShow = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            TestBasicColumnSeriesForShow.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            TestBasicColumnSeriesForShow[1].Values.Add(48d);

            TestBasicColumnLabelsForShow = new[] { "Maria", "Susan", "Charles", "Frida" };
            TestBasicColumnFormatterForShow = value => value.ToString("N");
        }
        public void TestDataPieSeries()
        {
            TestDoghnutSeriesForShow = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Chrome",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Mozilla",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Opera",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Explorer",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                    DataLabels = true
                }
            };
        }
        public void TestDataStackedColumnSeries()
        {
            TestStackedColumnSeriesForShow = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Values = new ChartValues<double> {4, 5, 6, 8},
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    DataLabels = true
                },
                new StackedColumnSeries
                {
                    Values = new ChartValues<double> {2, 5, 6, 7},
                    StackMode = StackMode.Values,
                    DataLabels = true
                }
            };
            TestStackedColumnSeriesForShow.Add(new StackedColumnSeries
            {
                Values = new ChartValues<double> { 6, 2, 7 },
                StackMode = StackMode.Values
            });

            //adding values also updates and animates
            TestStackedColumnSeriesForShow[2].Values.Add(4d);

            TestStackedColumnSeriesLabelsForShow = new[] { "Chrome", "Mozilla", "Opera", "IE" };
            TestStackedColumnSeriesFormatterForShow = value => value + " Mill";
        }
    }
}
