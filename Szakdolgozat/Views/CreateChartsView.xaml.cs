using Org.BouncyCastle.Utilities;
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
using System.Windows.Forms;
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
                viewModel.SeriesType = type;
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if(DataContext is CreateChartsViewModel viewmodel)
        //    {
        //        MessageBox.Show(viewmodel.SeriesType);
        //    }
        //    goBackButton.Visibility = Visibility.Visible;
        //}

        //private void goBackButton_Click(object sender, RoutedEventArgs e)
        //{
        //    goBackButton.Visibility = Visibility.Hidden;
        //}

        private void ChangeCellColor_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateChartsViewModel viewModel)
            {
                SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90EE90"));
                if (chartsTabControl.SelectedContent is System.Windows.Controls.DataGrid dataGrid)
                {
                    if (dataGrid.Name.Equals("bevetelek_kiadasok"))
                    {
                        foreach (var item in dataGrid.SelectedItems)
                        {
                            BevetelKiadas i = (BevetelKiadas)item;
                            if (sender == cellSelectionTrue)
                            {
                                viewModel._selectedBevetelekKiadasok.Add(i);
                            }
                            else
                            {
                                viewModel._selectedBevetelekKiadasok.Remove(i);
                            }
                        }
                    }
                    else if (dataGrid.Name.Equals("kotelezettsegek_kovetelesek"))
                    {
                        foreach (var item in dataGrid.SelectedItems)
                        {
                            KotelezettsegKoveteles i = (KotelezettsegKoveteles)item;
                            if (sender == cellSelectionTrue)
                            {
                                viewModel._selectedKotelezettsegekKovetelesek.Add(i);

                            }
                            else
                            {
                                viewModel._selectedKotelezettsegekKovetelesek.Remove(i);
                            }
                        }
                    }
                    foreach (var cellInfo in dataGrid.SelectedCells)
                    {
                        var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
                        if (cellContent != null)
                        {
                            var cell = cellContent.Parent as System.Windows.Controls.DataGridCell;
                            if (cell != null)
                            {
                                if (sender == cellSelectionTrue)
                                {
                                    cell.Style = (Style)this.FindResource("DataGridCellStyle");
                                }
                                else
                                {
                                    cell.ClearValue(System.Windows.Controls.DataGridCell.StyleProperty);
                                }
                            }
                        }
                    }
                }
            }
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
            System.Windows.Controls.ComboBox comboBox = (System.Windows.Controls.ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                string selectedName = ((ComboBoxItem)comboBox.SelectedItem).Name;
                if (!selectedName.Equals("datafilter"))
                    comboBox.SelectedIndex = 0;
            }
        }

        private void asd_Click(object sender, RoutedEventArgs e)
        {
            if(DataContext is CreateChartsViewModel viewModel)
            {
                viewModel.SetSeries();
            }
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    GetSelectedCells();
        //}
    }
}
