using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Szakdolgozat.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Szakdolgozat.Repositories;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Principal;
using System.Threading;
using Szakdolgozat.Views;
using LiveCharts;
using System;
using LiveCharts.Wpf;

namespace Szakdolgozat.ViewModels
{
    class AddOptionsToNewChartViewModel : ViewModelBase
    {
        private ObservableCollection<object> charts;
        public ObservableCollection<object> DataForCharts
        {
            get
            {
                if (charts == null)
                    return null;
                return charts;
            }
            set
            {
                charts = value;
            }
        }

        public AddOptionsToNewChartViewModel(ObservableCollection<object> objects)
        {
            DataForCharts = objects;
            TestDataRowSeries();
        }
        public AddOptionsToNewChartViewModel()
        {
            TestDataRowSeries();
        }
        public SeriesCollection TestRowSeriesForShow { get; set; }
        public string[] TestRowSeriesLabelsForShow { get; set; }
        public Func<double, string> TestRowSeriesFormatterForShow { get; set; }
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
    }
}
