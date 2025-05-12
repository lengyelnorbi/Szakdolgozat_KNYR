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
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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
