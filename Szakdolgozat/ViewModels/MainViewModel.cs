using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Views;

namespace Szakdolgozat.ViewModels
{
    public class CustomMenuItemViewModel
    {
        public string Header { get; set; }
        public ObservableCollection<CustomMenuItemViewModel> MenuItems { get; } = new ObservableCollection<CustomMenuItemViewModel>();
    }

    public class MainViewModel : ViewModelBase
    {
        public int UserID { get; set; }

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

        public ICommand ShowUjDiagrammokViewCommand { get; }
        public ICommand ShowLetezoDiagrammokViewCommand { get; }
        public ICommand ShowElozmenyekViewCommand { get; }
        public ICommand ShowKoltsegvetesViewCommand { get; }
        public ICommand ShowMaganSzemelyekViewCommand { get; }
        public ICommand ShowDolgozoViewCommand { get; }
        public ICommand ShowKotelezettsegKovetelesViewCommand { get; }
        public ICommand ShowGazdalkodasiSzervezetekViewCommand { get; }

        public MainViewModel()
        {
            //Initialize commands
            ShowUjDiagrammokViewCommand = new ViewModelCommand(ExecuteShowUjDiagrammokViewCommand);
            ShowLetezoDiagrammokViewCommand = new ViewModelCommand(ExecuteShowLetezoDiagrammokViewCommand);
            ShowElozmenyekViewCommand = new ViewModelCommand(ExecuteShowElozmenyekViewCommand);
            ShowKoltsegvetesViewCommand = new ViewModelCommand(ExecuteShowKoltsegvetesViewCommand);
            ShowMaganSzemelyekViewCommand = new ViewModelCommand(ExecuteShowMaganSzemelyekViewCommand);
            ShowDolgozoViewCommand = new ViewModelCommand(ExecuteShowDolgozoViewCommand);
            ShowKotelezettsegKovetelesViewCommand = new ViewModelCommand(ExecuteShowKotelezettsegKovetelesViewCommand);
            ShowGazdalkodasiSzervezetekViewCommand = new ViewModelCommand(ExecuteShowGazdalkodasiSzervezetekViewCommand);
            //Default view
            ExecuteShowElozmenyekViewCommand(null);
        }

        private void ExecuteShowUjDiagrammokViewCommand(object obj)
        {
            CurrentChildView = new UjDiagrammokViewModel();
        }
        private void ExecuteShowLetezoDiagrammokViewCommand(object obj)
        {
            CurrentChildView = new LetezoDiagrammokViewModel();
        }
        private void ExecuteShowElozmenyekViewCommand(object obj)
        {
            CurrentChildView = new ElozmenyekViewModel();
        }
        private void ExecuteShowKoltsegvetesViewCommand(object obj)
        {
            CurrentChildView = new KoltsegvetesViewModel();
        }
        private void ExecuteShowMaganSzemelyekViewCommand(object obj)
        {
            CurrentChildView = new MaganSzemelyekViewModel();
        }
        private void ExecuteShowDolgozoViewCommand(object obj)
        {
            CurrentChildView = new DolgozoViewModel();
        }
        private void ExecuteShowKotelezettsegKovetelesViewCommand(object obj)
        {
            CurrentChildView = new KotelezettsegKovetelesViewModel();
        }
        private void ExecuteShowGazdalkodasiSzervezetekViewCommand(object obj)
        {
            CurrentChildView = new GazdalkodoSzervezetViewModel();
        }

        public ObservableCollection<CustomMenuItemViewModel> MenuItems { get; } = new ObservableCollection<CustomMenuItemViewModel>
        {
            new CustomMenuItemViewModel { Header = "Option 1" },
            // Add more sub-menu items as needed
        };

        public ObservableCollection<CustomMenuItemViewModel> HelpItems { get; } = new ObservableCollection<CustomMenuItemViewModel>
        {
            new CustomMenuItemViewModel { Header = "Help Option 1" },
            // Add more sub-menu items for Help
        };
    }
}
