using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.ViewModels;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for KoltsegvetesModifyOrAddView.xaml
    /// </summary>
    public partial class KoltsegvetesModifyOrAddView : Window
    {
        public KoltsegvetesModifyOrAddView(EditMode mode)
        {
            InitializeComponent();

            if (DataContext is KoltsegvetesModifyOrAddViewModel viewModel)
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
            if (DataContext is KoltsegvetesModifyOrAddViewModel viewModel)
            {
                viewModel.RequestClose -= () => this.Close();
            }
            this.Close();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        public KoltsegvetesModifyOrAddView(EditMode mode, BevetelKiadas modifiableBevetelKiadas)
        {
            InitializeComponent();

            if (DataContext is KoltsegvetesModifyOrAddViewModel viewModel)
            {
                viewModel.EditMode = mode;
                viewModel.ModifiableKoltsegvetes = modifiableBevetelKiadas;
                viewModel.RequestClose += () => this.Close();
            }
        }
        private void number_KeyDown(object sender, KeyEventArgs e)
        {
            // Allow digits (0-9)
            bool isDigit = e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9;

            // Allow control keys (Backspace, Delete, Tab, etc.)
            bool isControl = e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab ||
                             e.Key == Key.Left || e.Key == Key.Right ||
                             e.Key == Key.Home || e.Key == Key.End;

            // If the pressed key is neither a digit nor a control key, mark the event as handled
            // to prevent the character from being entered
            if (!isDigit && !isControl)
            {
                e.Handled = true;
            }
        }
    }
}
