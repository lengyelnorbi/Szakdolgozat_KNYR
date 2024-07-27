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
    /// Interaction logic for KotelezettsegKovetelesModifyOrAddView.xaml
    /// </summary>
    public partial class KotelezettsegKovetelesModifyOrAddView : Window
    {
        public KotelezettsegKovetelesModifyOrAddView(EditMode mode)
        {
            InitializeComponent();

            if (DataContext is KotelezettsegKovetelesModifyOrAddViewModel viewModel)
            {
                viewModel.EditMode = mode;
                viewModel.RequestClose += () => this.Close();
            }
        }

        public KotelezettsegKovetelesModifyOrAddView(EditMode mode, KotelezettsegKoveteles modifiableKotelezettsegKoveteles)
        {
            InitializeComponent();

            if (DataContext is KotelezettsegKovetelesModifyOrAddViewModel viewModel)
            {
                viewModel.EditMode = mode;
                viewModel.ModifiableKotelezettsegKoveteles = modifiableKotelezettsegKoveteles;
                viewModel.RequestClose += () => this.Close();
            }
        }
    }
}
