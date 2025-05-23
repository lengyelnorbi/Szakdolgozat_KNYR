using System.Windows;
using System.Windows.Controls;
using Szakdolgozat.ViewModels;
using Szakdolgozat.Models;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for GazdalkodoSzervezetekModifyOrAddView.xaml
    /// </summary>
    public partial class GazdalkodoSzervezetModifyOrAddView : Window
    {
        public GazdalkodoSzervezetModifyOrAddView(EditMode mode)
        {
            InitializeComponent();
            if (DataContext is GazdalkodoSzervezetModifyOrAddViewModel viewModel)
            {
                viewModel.EditMode = mode;
                viewModel.RequestClose += () => this.Close();
            }
            if (mode == EditMode.Add)
            {
                resetButton.Visibility = Visibility.Collapsed;
                Grid.SetColumnSpan(saveButton, 2);
            }
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is GazdalkodoSzervezetModifyOrAddViewModel viewModel)
            {
                viewModel.RequestClose -= () => this.Close();
            }
            this.Close();
        }
        public GazdalkodoSzervezetModifyOrAddView(EditMode mode, GazdalkodoSzervezet modifiableGazdalkodoSzervezet)
        {
            InitializeComponent();

            if (DataContext is GazdalkodoSzervezetModifyOrAddViewModel viewModel)
            {
                viewModel.EditMode = mode;
                viewModel.ModifiableGazdalkodoSzervezet = modifiableGazdalkodoSzervezet;
                viewModel.RequestClose += () => this.Close();
            }
        }
    }
}
