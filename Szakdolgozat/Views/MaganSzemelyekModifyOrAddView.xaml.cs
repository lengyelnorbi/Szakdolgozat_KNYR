using System.Windows;
using System.Windows.Controls;
using Szakdolgozat.Models;
using Szakdolgozat.ViewModels;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for MaganSzemelyekModifyOrAddView.xaml
    /// </summary>
    public partial class MaganSzemelyekModifyOrAddView : Window
    {
        public MaganSzemelyekModifyOrAddView(EditMode mode)
        {
            InitializeComponent();

            if (DataContext is MaganSzemelyekModifyOrAddViewModel viewModel)
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
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MaganSzemelyekModifyOrAddViewModel viewModel)
            {
                viewModel.RequestClose -= () => this.Close();
            }
            this.Close();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        public MaganSzemelyekModifyOrAddView(EditMode mode, MaganSzemely modifiableMaganSzemely)
        {
            InitializeComponent();

            if (DataContext is MaganSzemelyekModifyOrAddViewModel viewModel)
            {
                viewModel.EditMode = mode;
                viewModel.ModifiableMaganSzemely = modifiableMaganSzemely;
                viewModel.RequestClose += () => this.Close();
            }
        }
    }
}
