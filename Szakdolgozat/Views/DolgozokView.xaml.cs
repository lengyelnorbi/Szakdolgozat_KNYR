using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
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
using Szakdolgozat.Models;
using Szakdolgozat.ViewModels;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for DolgozokView.xaml
    /// </summary>
    public partial class DolgozokView : UserControl
    {
        private Boolean _isAddOpen = false;
        private Boolean _isModifyOpen = false;

        public DolgozokView()
        {
            InitializeComponent();

            var viewModel = (DolgozoViewModel)this.DataContext;

            mindCB.Checked += CheckBox_Checked;
            idCB.Checked += CheckBox_Checked;
            vezeteknevCB.Checked += CheckBox_Checked;
            keresztnevCB.Checked += CheckBox_Checked;
            emailCB.Checked += CheckBox_Checked;
            telefonszamCB.Checked += CheckBox_Checked;
            mindCB.Unchecked += Checkbox_Unchecked;
            idCB.Unchecked += Checkbox_Unchecked;
            vezeteknevCB.Unchecked += Checkbox_Unchecked;
            keresztnevCB.Unchecked += Checkbox_Unchecked;
            emailCB.Unchecked += Checkbox_Unchecked;
            telefonszamCB.Unchecked += Checkbox_Unchecked;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is DolgozoViewModel dolgozoViewModel)
                {
                    try
                    {
                        switch (checkBox.Name)
                        {
                            case "mindCB":
                                dolgozoViewModel.checkboxStatuses["mindCB"] = true;
                                dolgozoViewModel.checkboxStatuses["idCB"] = true;
                                idCB.IsChecked = true;
                                dolgozoViewModel.checkboxStatuses["vezeteknevCB"] = true;
                                vezeteknevCB.IsChecked = true;
                                dolgozoViewModel.checkboxStatuses["keresztnevCB"] = true;
                                keresztnevCB.IsChecked = true;
                                dolgozoViewModel.checkboxStatuses["emailCB"] = true;
                                emailCB.IsChecked = true;
                                dolgozoViewModel.checkboxStatuses["telefonszamCB"] = true;
                                telefonszamCB.IsChecked = true;
                                break;
                            case "idCB":
                                dolgozoViewModel.checkboxStatuses["idCB"] = true;
                                break;
                            case "vezeteknevCB":
                                dolgozoViewModel.checkboxStatuses["vezeteknevCB"] = true;
                                break;
                            case "keresztnevCB":
                                dolgozoViewModel.checkboxStatuses["keresztnevCB"] = true;
                                break;
                            case "emailCB":
                                dolgozoViewModel.checkboxStatuses["emailCB"] = true;
                                break;
                            case "telefonszamCB":
                                dolgozoViewModel.checkboxStatuses["telefonszamCB"] = true;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    dolgozoViewModel.UpdateSearch(dolgozoViewModel.SearchQuery);
                }
            }

        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is DolgozoViewModel dolgozoViewModel)
                {
                    switch (checkBox.Name)
                    {
                        case "mindCB":
                            dolgozoViewModel.checkboxStatuses["mindCB"] = false;
                            break;
                        case "idCB":
                            dolgozoViewModel.checkboxStatuses["idCB"] = false;
                            break;
                        case "vezeteknevCB":
                            dolgozoViewModel.checkboxStatuses["vezeteknevCB"] = false;
                            break;
                        case "keresztnevCB":
                            dolgozoViewModel.checkboxStatuses["keresztnevCB"] = false;
                            break;
                        case "emailCB":
                            dolgozoViewModel.checkboxStatuses["emailCB"] = false;
                            break;
                        case "telefonszamCB":
                            dolgozoViewModel.checkboxStatuses["telefonszamCB"] = false;
                            break;
                        default:
                            break;
                    }
                    dolgozoViewModel.UpdateSearch(dolgozoViewModel.SearchQuery);
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                string selectedName = ((ComboBoxItem)comboBox.SelectedItem).Name;
                if(!selectedName.Equals("CB_default_text"))
                    comboBox.SelectedIndex = 0;
            }
        }

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (DataContext is DolgozoViewModel viewModel && viewModel.IsExportModeActive)
            {
                // Cancel the context menu when in export mode
                e.Handled = true;
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is DolgozoViewModel viewModel)
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
    }
}
