using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
    }
}
