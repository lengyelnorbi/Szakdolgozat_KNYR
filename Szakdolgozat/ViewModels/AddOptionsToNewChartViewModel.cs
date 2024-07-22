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
                return Mediator.NotifyDataRequest();
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
                AddNewRowSeries();
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
                OnPropertyChanged(nameof(DataSourceOptions));
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
                AddNewRowSeries();
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
            AddNewRowSeries();
            setDataSourceForCB();
        }

        public void setDataSourceForCB()
        {
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
                    break;
                }
                else if(item is BevetelKiadas bevetelKiadas)
                {
                    DataSourceOptions = new ObservableCollection<string>
                    {
                        "ID",
                        "Összeg",
                        "Pénznem",
                        "Bevétel kód",
                        "Kiadás kód",
                        "Teljesítési dátum",
                        "Kötelezettség ID",
                        "Követelés ID",
                        "Partner ID"
                    };
                    break;
                }
                else if (item is GazdalkodoSzervezet gazdalkodoSzervezet)
                {
                    DataSourceOptions = new ObservableCollection<string>
                    {
                        "ID",
                        "Név",
                        "Pénznem",
                        "Kapcsolattartó",
                        "Email",
                        "Telefonszám"
                    };
                    break;
                }
            }
        }

        public AddOptionsToNewChartViewModel()
        {
            AddNewRowSeries();
            setDataSourceForCB();
        }
        private SeriesCollection  _rowSeriesCollection = new SeriesCollection();
        public SeriesCollection RowSeriesCollection
        {
            get { return _rowSeriesCollection; }
            set { _rowSeriesCollection = value; }
        }
        public string[] RowSeriesLabels { get; set; }
        public Func<double, string> RowSeriesFormatter { get; set; }

        public void AddNewRowSeries()
        {

            RowSeriesCollection.Add(new RowSeries
            {
                Title = "AH1",
                Values = new ChartValues<double> ()
            });

            RowSeriesCollection.Add(new RowSeries
            {
                Title = "AH2",
                Values = new ChartValues<double> {12, 31, 21, 44, 55 }
            });
            RowSeriesLabels = new[] { "O1", "O2", "O3", "O4" };
            RowSeriesFormatter = value => value.ToString("N");
        }

        public void Teszt()
        {
            foreach (var item in DataForCharts)
            {
                MessageBox.Show(item.ToString());
            }
        }
    }
}
