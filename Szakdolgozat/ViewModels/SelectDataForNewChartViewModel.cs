using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.ViewModels
{
    public class SelectDataForNewChartViewModel : ViewModelBase
    {

        private string _selectedDataSourceName;
        public string SelectedDataSourceName
        {
            get { return _selectedDataSourceName; }
            set
            {
                _selectedDataSourceName = value;
                OnPropertyChanged(nameof(SelectedDataSourceName));
                OnPropertyChanged(nameof(DataChanged)); // Notify about a generic data change
            }
        }

        private ObservableCollection<object> _selectedRows;
        public ObservableCollection<object> SelectedRows
        {
            get { return _selectedRows; }
            set
            {
                _selectedRows = value;
                OnPropertyChanged(nameof(SelectedRows));
            }
        }

        public ObservableCollection<string> DataSourceOptions { get; } = new ObservableCollection<string>
    {
        "Dolgozok",
        "Bevétel",
        "Kiadás",
        "Gazdasági Szervezetek"
    };

        // Additional property to indicate a generic data change
        public bool DataChanged { get; set; }
    }
}
