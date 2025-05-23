using System.Windows;
using System.Windows.Controls;
using Szakdolgozat.Models;
using Szakdolgozat.ViewModels;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for DolgozokModifyOrAddView.xaml
    /// </summary>
    public partial class DolgozokModifyOrAddView : Window
    {
        public DolgozokModifyOrAddView(EditMode mode)
        {
            InitializeComponent();

            if(DataContext is DolgozokModifyOrAddViewModel viewModel)
            {
                viewModel.EditMode = mode;
                viewModel.RequestClose += () => this.Close();
            }
            if(mode == EditMode.Add)
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
            if (DataContext is DolgozokModifyOrAddViewModel viewModel)
            {
                viewModel.RequestClose -= () => this.Close();
            }
            this.Close();
        }

        public DolgozokModifyOrAddView(EditMode mode, Dolgozo modifiableDolgozo)
        {
            InitializeComponent();

            if (DataContext is DolgozokModifyOrAddViewModel viewModel)
            {
                viewModel.EditMode = mode;
                viewModel.ModifiableDolgozo = modifiableDolgozo;
                viewModel.RequestClose += () => this.Close();
            }
        }
    }
}
