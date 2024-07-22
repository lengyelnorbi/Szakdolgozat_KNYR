using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;
using Szakdolgozat.ViewModels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for CreateChartsView.xaml
    /// </summary>
    public partial class CreateChartsView : Window
    {
        public CreateChartsView(string type)
        {
            InitializeComponent();

            if(DataContext is CreateChartsViewModel viewModel)
            {
                Mediator.GetTabControl += () => chartsTabControl;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(DataContext is CreateChartsViewModel viewmodel)
            {
                MessageBox.Show(viewmodel.SeriesType);
            }
            goBackButton.Visibility = Visibility.Visible;
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            goBackButton.Visibility = Visibility.Hidden;
        }

        private void ChangeCellColor_Click(object sender, RoutedEventArgs e)
        {
            if(DataContext is CreateChartsViewModel viewModel)
            {
                SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90EE90"));
                if (chartsTabControl.SelectedContent is DataGrid dataGrid)
                {
                    foreach (var cellInfo in dataGrid.SelectedCells)
                    {
                        var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
                        if (cellContent != null)
                        {
                            var cell = cellContent.Parent as DataGridCell;
                            if (cell != null)
                            {
                                if (sender == cellSelectionTrue)
                                {
                                    viewModel.selectedDataGridCells.Add(cell);
                                    cell.Style = (Style)this.FindResource("DataGridCellStyle");
                                }
                                else
                                {
                                    viewModel.selectedDataGridCells.Remove(cell);
                                    cell.ClearValue(DataGridCell.StyleProperty);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void GetSelectedCells()
        {
            if(DataContext is CreateChartsViewModel createChartsViewModel)
            {
                foreach (var o in createChartsViewModel.selectedDataGridCells)
                {
                    var dataGridRow = GetDataGridRow(o);

                    if (dataGridRow != null)
                    {
                        // Get the data item associated with this row
                        var dataItem = dataGridRow.Item;

                        // Get the property name from the column header
                        var propertyName = o.Column.Header.ToString();

                        // Get the property info using reflection
                        var propertyInfo = dataItem.GetType().GetProperty(propertyName);
                        if (propertyInfo != null)
                        {
                            // Get the type of the property
                            var propertyType = propertyInfo.PropertyType;
                            var dataGridInfo = GetDataGrid(o);
                            System.Windows.MessageBox.Show($"Column: {propertyName}, Type: {propertyType}");
                            System.Windows.MessageBox.Show($"DataGrid Name: {dataGridInfo.Name}");
                        }
                    }
                }
            }
        }
        // Helper method to get the DataGridRow containing the specified DataGridCell
        private DataGridRow GetDataGridRow(System.Windows.Controls.DataGridCell cell)
        {
            DependencyObject parent = cell;
            while (parent != null && !(parent is DataGridRow))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as DataGridRow;
        }
        private DataGrid GetDataGrid(DataGridCell cell)
        {
            DependencyObject parent = cell;
            while (parent != null && !(parent is DataGrid))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as DataGrid;
        }
        //private void basicColumnNamesButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (uniqueColumnNamesButton.Visibility is Visibility.Hidden)
        //    {
        //        uniqueColumnNamesButton.Visibility = Visibility.Visible;
        //        basicColumnNameOptions.Visibility = Visibility.Hidden;
        //    }
        //    else
        //    {
        //        uniqueColumnNamesButton.Visibility = Visibility.Hidden;
        //        basicColumnNameOptions.Visibility = Visibility.Visible;
        //    }
        //}

        //private void uniqueColumnNamesButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (basicColumnNamesButton.Visibility is Visibility.Hidden)
        //    {
        //        basicColumnNamesButton.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        basicColumnNamesButton.Visibility = Visibility.Hidden;
        //    }
        //}
        //private CheckBox FindCheckBoxByName(string checkBoxName)
        //{
        //    foreach (ComboBoxItem comboBoxItem in monthsCombobox.Items)
        //    {
        //        if (comboBoxItem.Content is CheckBox checkBox)
        //        {
        //            if (checkBox.Name == checkBoxName)
        //            {
        //                return checkBox;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //private void CheckAllMonthBoxes()
        //{
        //    if (DataContext is CreateChartsViewModel createChartsViewModel)
        //    {
        //        foreach(var kvp in createChartsViewModel.monthCheckboxStatuses)
        //        {
        //            if(kvp.Key != "mindCB")
        //            {
        //                IsCheckedTrueSingleCheckBox(kvp.Key);
        //            }
        //        }
        //    }
        //}

        //private void IsCheckedTrueSingleCheckBox(string name)
        //{
        //    CheckBox tmp = FindCheckBoxByName(name);
        //    tmp.IsChecked = true;
        //}
        //private void IsCheckedFalseSingleCheckBox(string name)
        //{
        //    CheckBox tmp = FindCheckBoxByName(name);
        //    tmp.IsChecked = false;
        //}
        //private void Month_CheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (sender is CheckBox checkBox)
        //    {
        //        if (DataContext is CreateChartsViewModel createChartsViewModel)
        //        {
        //            try
        //            {
        //                if(checkBox.Name == "mindCB")
        //                {
        //                    CheckAllMonthBoxes();
        //                    IsCheckedTrueSingleCheckBox(checkBox.Name);
        //                }
        //                else
        //                {
        //                    IsCheckedTrueSingleCheckBox(checkBox.Name);
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }
        //    }
        //}

        //private void Month_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    if (sender is CheckBox checkBox)
        //    {
        //        if (DataContext is CreateChartsViewModel createChartsViewModel)
        //        {
        //            IsCheckedFalseSingleCheckBox(checkBox.Name);
        //        }
        //    }
        //}

        //Ha combobox-on belüli elemen kattintás történik, akkor ez megakadájozza, hogy a placeholder szöveg ne legyen lecserélve
        //If a click occurs on a comboboxitem, then this method prevents any changes to the combobox placeholder text
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                string selectedName = ((ComboBoxItem)comboBox.SelectedItem).Name;
                if (!selectedName.Equals("datafilter"))
                    comboBox.SelectedIndex = 0;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GetSelectedCells();
        }
    }
}
