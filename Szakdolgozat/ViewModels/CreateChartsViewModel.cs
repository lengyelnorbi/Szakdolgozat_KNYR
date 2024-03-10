using System.Collections.Generic;
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
    public ObservableCollection<object> SelectedRows
    {
        get
        {
            if (CurrentChildView is SelectDataForNewChartViewModel selectDataViewModel)
            {
                return selectDataViewModel.SelectedRows;
            }
            return null; // or an empty list depending on your preference
        }
    }

    public ICommand ShowSelectDataForNewChartViewCommand { get; }
    public ICommand ShowAddOptionToNewChartViewCommand { get; }

    public CreateChartsViewModel()
    {
        CurrentChildView = new SelectDataForNewChartViewModel();
        ShowSelectDataForNewChartViewCommand = new ViewModelCommand(ExecuteShowSelectDataForNewChartViewCommand);
        ShowAddOptionToNewChartViewCommand = new ViewModelCommand(ExecuteShowAddOptionToNewChartViewCommand);
    }

    private void ExecuteShowSelectDataForNewChartViewCommand(object obj)
    {
        CurrentChildView = new SelectDataForNewChartViewModel();
    }
    private void ExecuteShowAddOptionToNewChartViewCommand(object obj)
    {
        CurrentChildView = new AddOptionsToNewChartViewModel(SelectedRows);
    }
}