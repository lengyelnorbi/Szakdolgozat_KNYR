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
using System.Windows.Documents;

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
                OnPropertyChanged(nameof(DataForCharts));
            }
        }

        private string _selectedDataSourceName;
        public string SelectedDataSourceName
        {
            get { return _selectedDataSourceName; }
            set
            {
                _selectedDataSourceName = value;
                OnPropertyChanged(nameof(SelectedDataSourceName));
            }
        }
        private ObservableCollection<string> _dataSourceOptions;
        public ObservableCollection<string> DataSourceOptions
        {
            get
            {
                return _dataSourceOptions;
            }
            set 
            {
                _dataSourceOptions = value;
            }
        }
        private string _addtitle;
        public string AddTitle
        {
            get 
            { 
                return _addtitle; 
            } 
            set
            {
                _addtitle = value;
                OnPropertyChanged(nameof(AddTitle));
            }
        }
        private List _values;
        public List Values
        {
            get
            {
                return _values;
            }
            set
            {
                Values = value;
                OnPropertyChanged(nameof(Values));
            }
        }
        public AddOptionsToNewChartViewModel(ObservableCollection<object> objects)
        {
            DataForCharts = objects;
            TestDataRowSeries();
            foreach (var item in DataForCharts)
            {
                if (item is Dolgozo dolgozo)
                {
                    DataSourceOptions = new ObservableCollection<string>
                    {
                        "ID",
                        "Vezetéknév",
                        "Keresztnév",
                        "Email",
                        "Telefonszám"
                    };
                }
                else
                {
                    DataSourceOptions = new ObservableCollection<string>
                    {
                        "ID"
                    };
                }
            }
        }
        public AddOptionsToNewChartViewModel()
        {
            TestDataRowSeries();
        }
        private SeriesCollection  _rowSeriesCollection = new SeriesCollection();
        public SeriesCollection RowSeriesCollection
        {
            get { return _rowSeriesCollection; }
            set { _rowSeriesCollection = value; }
        }
        public string[] TestRowSeriesLabelsForShow { get; set; }
        public Func<double, string> TestRowSeriesFormatterForShow { get; set; }
        public void TestDataRowSeries()
        {
            RowSeriesCollection.Add(new RowSeries
            {
                Title = AddTitle,
                Values = new ChartValues<double> { 11, 56, 42 }
            });


            TestRowSeriesLabelsForShow = new[] { "Maria", "Susan", "Charles", "Frida" };
            TestRowSeriesFormatterForShow = value => value.ToString("N");
        }

        public void AddNewRowSeries()
        {

            RowSeriesCollection.Add(new RowSeries
            {
                Title = AddTitle,
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            TestRowSeriesLabelsForShow = new[] { "Maria", "Susan", "Charles", "Frida" };
            TestRowSeriesFormatterForShow = value => value.ToString("N");
        }
    }
}
