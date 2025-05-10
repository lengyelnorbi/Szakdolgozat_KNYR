using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;
using Szakdolgozat.Views;

namespace Szakdolgozat.ViewModels
{
    public class LetezoDiagrammokViewModel : ViewModelBase
    {
        private ObservableCollection<Diagramm> _diagramms;
        public ObservableCollection<Diagramm> Diagrams
        {
            get { return _diagramms; }
            set
            {
                _diagramms = value;
                OnPropertyChanged(nameof(Diagrams));
            }
        }

        public ICommand OpenDiagramCommand { get; }
        public ICommand DeleteDiagramCommand { get; }
        private DiagrammRepository _diagrammRepository { get; set; } = new DiagrammRepository();

        public LetezoDiagrammokViewModel()
        {
            OpenDiagramCommand = new ViewModelCommand(ExecuteOpenDiagramCommand);
            DeleteDiagramCommand = new ViewModelCommand(ExecuteDeleteDiagramCommand);

            Diagrams = new ObservableCollection<Diagramm>(_diagrammRepository.GetAllDiagramms());
        }

        private void ExecuteOpenDiagramCommand(object parameter)
        {
            if (parameter is Diagramm diagram)
            {
                // Open the CreateChartsView with pre-filled data
                var createChartsView = new CreateChartsView(diagram.ChartType);
                if (createChartsView.DataContext is CreateChartsViewModel viewModel)
                {
                    viewModel.LoadDiagram(diagram);
                    createChartsView.Show();
                    viewModel.UpdateSearch(viewModel.SearchQuery);
                }
            }
        }

        private void ExecuteDeleteDiagramCommand(object parameter)
        {
            if (parameter is Diagramm diagram)
            {
                // Ask for confirmation in Hungarian
                var result = System.Windows.MessageBox.Show(
                    $"Biztosan törölni szeretné a(z) \"{diagram.Name}\" diagramot?",
                    "Törlés megerősítése",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    _diagrammRepository.DeleteDiagramm(diagram.ID);
                    // Refresh the diagrams list
                    Diagrams = new ObservableCollection<Diagramm>(_diagrammRepository.GetAllDiagramms());
                }
            }
        }
    }
}
