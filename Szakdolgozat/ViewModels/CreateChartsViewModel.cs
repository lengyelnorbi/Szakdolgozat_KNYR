using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Szakdolgozat.Models;

namespace Szakdolgozat.ViewModels
{
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
        private ObservableCollection<object> _selectedRows = new ObservableCollection<object>();
        public ObservableCollection<object> SelectedRows
        {
            get
            {
                return _selectedRows;
            }
            set 
            {
                _selectedRows = value;
                OnPropertyChanged(nameof(SelectedRows));
            }
        }

        public void UpdateSelectedRows(ObservableCollection<object> selectedRows)
        {
            SelectedRows = selectedRows;
        }

        public ICommand ShowSelectDataForNewChartViewCommand { get; }
        public ICommand ShowAddOptionToNewChartViewCommand { get; }

        public CreateChartsViewModel()
        {
            Mediator.SelectedRowsChangedOnChildView += UpdateSelectedRows;
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
}
