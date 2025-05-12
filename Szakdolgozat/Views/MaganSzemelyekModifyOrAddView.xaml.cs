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
