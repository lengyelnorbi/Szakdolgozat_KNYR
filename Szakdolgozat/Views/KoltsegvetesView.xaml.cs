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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Szakdolgozat.ViewModels;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for ValamiDataTable.xaml
    /// </summary>
    public partial class KoltsegvetesView : UserControl
    {
        public KoltsegvetesView()
        {
            InitializeComponent();

            mindCB.Checked += CheckBox_Checked;
            idCB.Checked += CheckBox_Checked;
            osszegCB.Checked += CheckBox_Checked;
            penznemCB.Checked += CheckBox_Checked;
            beKiKodCB.Checked += CheckBox_Checked;
            teljesitesiDatumCB.Checked += CheckBox_Checked;
            kotelKovetIDCB.Checked += CheckBox_Checked;
            gazdalkodoSzervIDCB.Checked += CheckBox_Checked;
            maganSzemelyIDCB.Checked += CheckBox_Checked;
            mindCB.Unchecked += Checkbox_Unchecked;
            idCB.Unchecked += Checkbox_Unchecked;
            osszegCB.Unchecked += Checkbox_Unchecked;
            penznemCB.Unchecked += Checkbox_Unchecked;
            beKiKodCB.Unchecked += Checkbox_Unchecked;
            teljesitesiDatumCB.Unchecked += Checkbox_Unchecked;
            kotelKovetIDCB.Unchecked += Checkbox_Unchecked;
            gazdalkodoSzervIDCB.Unchecked += Checkbox_Unchecked;
            maganSzemelyIDCB.Unchecked += Checkbox_Unchecked;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is KoltsegvetesViewModel koltsegvetesViewModel)
                {
                    try
                    {
                        switch (checkBox.Name)
                        {
                            case "mindCB":
                                koltsegvetesViewModel.checkboxStatuses["mindCB"] = true;
                                koltsegvetesViewModel.checkboxStatuses["idCB"] = true;
                                idCB.IsChecked = true;
                                koltsegvetesViewModel.checkboxStatuses["osszegCB"] = true;
                                osszegCB.IsChecked = true;
                                koltsegvetesViewModel.checkboxStatuses["penznemCB"] = true;
                                penznemCB.IsChecked = true;
                                koltsegvetesViewModel.checkboxStatuses["beKiKodCB"] = true;
                                beKiKodCB.IsChecked = true;
                                koltsegvetesViewModel.checkboxStatuses["teljesitesiDatumCB"] = true;
                                teljesitesiDatumCB.IsChecked = true;
                                koltsegvetesViewModel.checkboxStatuses["kotelKovetIDCB"] = true;
                                kotelKovetIDCB.IsChecked = true;
                                koltsegvetesViewModel.checkboxStatuses["gazdalkodoSzervIDCB"] = true;
                                gazdalkodoSzervIDCB.IsChecked = true;
                                koltsegvetesViewModel.checkboxStatuses["maganSzemelyIDCB"] = true;
                                maganSzemelyIDCB.IsChecked = true;
                                break;
                            case "idCB":
                                koltsegvetesViewModel.checkboxStatuses["idCB"] = true;
                                break;
                            case "osszegCB":
                                koltsegvetesViewModel.checkboxStatuses["osszegCB"] = true;
                                break;
                            case "keresztnevCB":
                                koltsegvetesViewModel.checkboxStatuses["keresztnevCB"] = true;
                                break;
                            case "penznemCB":
                                koltsegvetesViewModel.checkboxStatuses["penznemCB"] = true;
                                break;
                            case "beKiKodCB":
                                koltsegvetesViewModel.checkboxStatuses["beKiKodCB"] = true;
                                break;
                            case "teljesitesiDatumCB":
                                koltsegvetesViewModel.checkboxStatuses["teljesitesiDatumCB"] = true;
                                break;
                            case "kotelKovetIDCB":
                                koltsegvetesViewModel.checkboxStatuses["kotelKovetIDCB"] = true;
                                break;
                            case "gazdalkodoSzervIDCB":
                                koltsegvetesViewModel.checkboxStatuses["gazdalkodoSzervIDCB"] = true;
                                break;
                            case "maganSzemelyIDCB":
                                koltsegvetesViewModel.checkboxStatuses["maganSzemelyIDCB"] = true;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    koltsegvetesViewModel.UpdateSearch(koltsegvetesViewModel.SearchQuery);
                }
            }

        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is KoltsegvetesViewModel koltsegvetesViewModel)
                {
                    switch (checkBox.Name)
                    {
                        case "mindCB":
                            koltsegvetesViewModel.checkboxStatuses["mindCB"] = false;
                            break;
                        case "idCB":
                            koltsegvetesViewModel.checkboxStatuses["idCB"] = false;
                            break;
                        case "osszegCB":
                            koltsegvetesViewModel.checkboxStatuses["osszegCB"] = false;
                            break;
                        case "keresztnevCB":
                            koltsegvetesViewModel.checkboxStatuses["keresztnevCB"] = false;
                            break;
                        case "penznemCB":
                            koltsegvetesViewModel.checkboxStatuses["penznemCB"] = false;
                            break;
                        case "beKiKodCB":
                            koltsegvetesViewModel.checkboxStatuses["beKiKodCB"] = false;
                            break;
                        case "teljesitesiDatumCB":
                            koltsegvetesViewModel.checkboxStatuses["teljesitesiDatumCB"] = false;
                            break;
                        case "kotelKovetIDCB":
                            koltsegvetesViewModel.checkboxStatuses["kotelKovetIDCB"] = false;
                            break;
                        case "gazdalkodoSzervIDCB":
                            koltsegvetesViewModel.checkboxStatuses["gazdalkodoSzervIDCB"] = false;
                            break;
                        case "maganSzemelyIDCB":
                            koltsegvetesViewModel.checkboxStatuses["maganSzemelyIDCB"] = false;
                            break;
                        default:
                            break;
                    }
                    koltsegvetesViewModel.UpdateSearch(koltsegvetesViewModel.SearchQuery);
                }
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                string selectedName = ((ComboBoxItem)comboBox.SelectedItem).Name;
                if (!selectedName.Equals("CB_default_text"))
                    comboBox.SelectedIndex = 0;
            }
        }

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (DataContext is KoltsegvetesViewModel viewModel && viewModel.IsExportModeActive)
            {
                // Cancel the context menu when in export mode
                e.Handled = true;
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is KoltsegvetesViewModel viewModel)
            {
                // Get all selected items from the DataGrid
                var dataGrid = sender as DataGrid;
                if (dataGrid != null)
                {
                    // Update selected items collection for export
                    viewModel.UpdateSelectedItems(dataGrid.SelectedItems);

                    // Update the IsSingleRowSelected property for context menu visibility
                    viewModel.IsSingleRowSelected = dataGrid.SelectedItems.Count == 1;
                }
            }
        }
        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                e.Cancel = true; // Prevents this column from being generated
            }
            if (e.PropertyType == typeof(DateTime))
            {
                // Get the column and cast to DataGridTextColumn
                var textColumn = e.Column as DataGridTextColumn;

                if (textColumn != null)
                {
                    // Set the StringFormat for date formatting
                    textColumn.Binding = new Binding(e.PropertyName)
                    {
                        StringFormat = "yyyy.MM.dd" // Format the date as desired
                    };
                }
            }
        }

        private void dataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {

        }
    }
}
