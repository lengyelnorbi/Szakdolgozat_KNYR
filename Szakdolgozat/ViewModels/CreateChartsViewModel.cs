using System.Collections.ObjectModel;
using System.Windows.Input;
using Szakdolgozat.ViewModels;

public class CreateChartsViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    public ViewModelBase CurrentChildView
    {
        get
        {
            return _currentChildView;
        }
        set
        {
            _currentChildView = value;
            OnPropertyChanged(nameof(CurrentChildView));
        }
    }

    public ICommand ShowSelectDataForNewChartViewCommand { get; }

    public CreateChartsViewModel()
    {
        CurrentChildView = new SelectDataForNewChartViewModel();
        ShowSelectDataForNewChartViewCommand = new ViewModelCommand(ExecuteShowSelectDataForNewChartViewCommand);
    }

    private void ExecuteShowSelectDataForNewChartViewCommand(object obj)
    {
        CurrentChildView = new SelectDataForNewChartViewModel();
    }
}