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
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
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
