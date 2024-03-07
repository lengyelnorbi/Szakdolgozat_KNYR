using System.Collections.ObjectModel;
using Szakdolgozat.ViewModels;

public class CreateChartsViewModel : ViewModelBase
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