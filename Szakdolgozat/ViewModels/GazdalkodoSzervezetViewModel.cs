using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Szakdolgozat.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;
using Szakdolgozat.Views;
using Szakdolgozat.Repositories;
using ClosedXML.Excel;
using System.Collections;
using System.Windows.Forms;

namespace Szakdolgozat.ViewModels
{
    class GazdalkodoSzervezetViewModel : ViewModelBase
    {
        private ObservableCollection<GazdalkodoSzervezet> _gazdalkodoSzervezetek;
        private GazdalkodoSzervezetRepository _gazdalkodoSzervezetRepository;

        public ObservableCollection<GazdalkodoSzervezet> GazdalkodoSzervezetek
        {
            get { return _gazdalkodoSzervezetek; }
            set
            {
                _gazdalkodoSzervezetek = value;
                OnPropertyChanged(nameof(GazdalkodoSzervezetek));
            }
        }

        private ObservableCollection<GazdalkodoSzervezet> _filteredGazdalkodoSzervezetek;

        public ObservableCollection<GazdalkodoSzervezet> FilteredGazdalkodoSzervezetek
        {
            get { return _filteredGazdalkodoSzervezetek; }
            set
            {
                _filteredGazdalkodoSzervezetek = value;
                OnPropertyChanged(nameof(FilteredGazdalkodoSzervezetek));
            }
        }

        private string _searchQuery;

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                UpdateSearch(SearchQuery);
            }
        }

        private GazdalkodoSzervezet _selectedRow;

        public GazdalkodoSzervezet SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        public Dictionary<string, bool> checkboxStatuses = new Dictionary<string, bool>();
        private ObservableCollection<GazdalkodoSzervezet> _selectedItems;
        public ObservableCollection<GazdalkodoSzervezet> SelectedItems
        {
            get { return _selectedItems ?? (_selectedItems = new ObservableCollection<GazdalkodoSzervezet>()); }
            set
            {
                _selectedItems = value;
                OnPropertyChanged(nameof(SelectedItems));
            }
        }

        private bool _isMultiSelectionMode;
        public bool IsMultiSelectionMode
        {
            get { return _isMultiSelectionMode; }
            set
            {
                _isMultiSelectionMode = value;
                OnPropertyChanged(nameof(IsMultiSelectionMode));

                // Clear selections when disabling multi-selection mode
                if (!value)
                {
                    SelectedItems.Clear();
                }
            }
        }
        private bool _isExportModeActive;
        public bool IsExportModeActive
        {
            get { return _isExportModeActive; }
            set
            {
                _isExportModeActive = value;
                OnPropertyChanged(nameof(IsExportModeActive));
            }
        }
        private bool _isSingleRowSelected;
        public bool IsSingleRowSelected
        {
            get { return _isSingleRowSelected; }
            set
            {
                _isSingleRowSelected = value;
                OnPropertyChanged(nameof(IsSingleRowSelected));
            }
        }
        public GazdalkodoSzervezetViewModel()
        {
            _gazdalkodoSzervezetRepository = new GazdalkodoSzervezetRepository();
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("nevCB", true);
            checkboxStatuses.Add("kapcsolattartoCB", true);
            checkboxStatuses.Add("emailCB", true);
            checkboxStatuses.Add("telefonszamCB", true);
            GazdalkodoSzervezetek = _gazdalkodoSzervezetRepository.GetGazdalkodoSzervezetek();
            FilteredGazdalkodoSzervezetek = new ObservableCollection<GazdalkodoSzervezet>(
                GazdalkodoSzervezetek.Select(d => new GazdalkodoSzervezet(d.ID, d.Nev, d.Kapcsolattarto, d.Email, d.Telefonszam)).ToList()
            );

            DeleteGazdalkodoSzervezetCommand = new ViewModelCommand(ExecuteDeleteGazdalkodoSzervezetCommand, CanExecuteDeleteGazdalkodoSzervezetCommand);

            OpenGazdalkodoSzervezetModifyOrAddWindowCommand = new ViewModelCommand(ExecuteOpenGazdalkodoSzervezetModifyOrAddWindowCommand, CanExecuteOpenGazdalkodoSzervezetModifyOrAddWindowCommand);


            _selectedItems = new ObservableCollection<GazdalkodoSzervezet>();
            ExportAllDataToExcelCommand = new ViewModelCommand(ExecuteExportAllDataToExcelCommand);
            ExportFilteredDataToExcelCommand = new ViewModelCommand(ExecuteExportFilteredDataToExcelCommand);
            ToggleMultiSelectionModeCommand = new ViewModelCommand(ExecuteToggleMultiSelectionModeCommand);
            ExportMultiSelectedItemsCommand = new ViewModelCommand(ExecuteExportMultiSelectedItemsCommand, CanExecuteExportMultiSelectedItemsCommand);
        }
        private void ExecuteExportAllDataToExcelCommand(object obj)
        {
            // Export all data from the database
            ExportToExcel(GazdalkodoSzervezetek.ToList(), "All_Database_Dolgozok");
        }

        private void ExecuteExportFilteredDataToExcelCommand(object obj)
        {
            // Export all filtered data
            ExportToExcel(FilteredGazdalkodoSzervezetek.ToList(), "Filtered_Dolgozok");
        }

        private void ExecuteToggleMultiSelectionModeCommand(object obj)
        {
            IsMultiSelectionMode = !IsMultiSelectionMode;
            IsExportModeActive = IsMultiSelectionMode; // This will control context menu

            // Clear selections when exiting multi-selection mode
            if (!IsMultiSelectionMode)
            {
                SelectedItems.Clear();
                OnPropertyChanged(nameof(SelectedItems));
            }
        }

        private bool CanExecuteExportMultiSelectedItemsCommand(object obj)
        {
            return SelectedItems != null && SelectedItems.Count > 0;
        }

        private void ExecuteExportMultiSelectedItemsCommand(object obj)
        {
            if (SelectedItems.Count > 0)
            {
                ExportToExcel(SelectedItems.ToList(), "MultiSelected_Dolgozok");
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "No items selected for export",
                    "Export Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
            }
        }

        // Add method to handle selected items from DataGrid in multi-selection mode
        public void UpdateSelectedItems(IList selectedItems)
        {
            SelectedItems.Clear();
            foreach (GazdalkodoSzervezet item in selectedItems)
            {
                SelectedItems.Add(item);
            }

           // Notify that the command can execute status might have changed
           (ExportMultiSelectedItemsCommand as ViewModelCommand)?.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Exports the specified data to an Excel file using ClosedXML
        /// </summary>
        /// <param name="dataToExport">The data items to export</param>
        /// <param name="defaultFileName">Default file name (without extension)</param>
        private void ExportToExcel(List<GazdalkodoSzervezet> dataToExport, string defaultFileName)
        {
            try
            {
                // Allow user to choose where to save the file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"{defaultFileName}_{DateTime.Now:yyyyMMdd}.xlsx",
                    Title = "Save Excel File"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        // Create a new worksheet
                        var worksheet = workbook.Worksheets.Add("Gazdalkodo Szervezetek");

                        // Add headers
                        worksheet.Cell(1, 1).Value = "ID";
                        worksheet.Cell(1, 2).Value = "Nev";
                        worksheet.Cell(1, 3).Value = "Kapcsolattarto";
                        worksheet.Cell(1, 4).Value = "Email";
                        worksheet.Cell(1, 5).Value = "Telefonszam";

                        // Format header row
                        var headerRange = worksheet.Range(1, 1, 1, 5);
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                        headerRange.Style.Font.FontColor = XLColor.White;

                        // Add data rows
                        for (int i = 0; i < dataToExport.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = dataToExport[i].ID;
                            worksheet.Cell(i + 2, 2).Value = dataToExport[i].Nev;
                            worksheet.Cell(i + 2, 3).Value = dataToExport[i].Kapcsolattarto;
                            worksheet.Cell(i + 2, 4).Value = dataToExport[i].Email;
                            worksheet.Cell(i + 2, 5).Value = dataToExport[i].Telefonszam;
                        }

                        // Auto-fit columns
                        worksheet.Columns().AdjustToContents();

                        // Create a table
                        var range = worksheet.Range(1, 1, dataToExport.Count + 1, 5);
                        var table = range.CreateTable("GazdalkodoSzervezetekTable");
                        table.Theme = XLTableTheme.TableStyleMedium2;

                        // Save the file
                        workbook.SaveAs(saveFileDialog.FileName);

                        System.Windows.MessageBox.Show(
                            $"Data successfully exported to {saveFileDialog.FileName}",
                            "Export Success",
                            System.Windows.MessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error exporting data: {ex.Message}",
                    "Export Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Exports multiple selected data to Excel file
        /// </summary>
        /// <param name="selectedItems">Collection of selected items</param>
        public void ExportMultipleSelectedToExcel(IEnumerable<GazdalkodoSzervezet> selectedItems)
        {
            if (selectedItems != null && selectedItems.Any())
            {
                ExportToExcel(selectedItems.ToList(), "Selected_Dolgozok");
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "No items selected for export",
                    "Export Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
            }
        }
        private void FilterData(string searchQuery)
        {
            Debug.WriteLine(searchQuery);
            if (!string.IsNullOrWhiteSpace(searchQuery) && !string.IsNullOrEmpty(searchQuery))
            {
                FilteredGazdalkodoSzervezetek.Clear();
                foreach (var d in GazdalkodoSzervezetek)
                {
                    if (checkboxStatuses["idCB"] == true)
                    {
                        if (d.ID.ToString().Contains(searchQuery))
                        {
                            FilteredGazdalkodoSzervezetek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["nevCB"] == true)
                    {
                        if (d.Nev.Contains(searchQuery))
                        {
                            FilteredGazdalkodoSzervezetek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["kapcsolattartoCB"] == true)
                    {
                        if (d.Kapcsolattarto.Contains(searchQuery))
                        {
                            FilteredGazdalkodoSzervezetek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["emailCB"] == true)
                    {
                        if (d.Email.Contains(searchQuery))
                        {
                            FilteredGazdalkodoSzervezetek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["telefonszamCB"] == true)
                    {
                        if (d.Telefonszam.Contains(searchQuery))
                        {
                            FilteredGazdalkodoSzervezetek.Add(d);
                            continue;
                        }
                    }
                }
            }
            else
            {
                // If the search query is empty, reset to the original data
                FilteredGazdalkodoSzervezetek = new ObservableCollection<GazdalkodoSzervezet>(GazdalkodoSzervezetek);
            }

            // Notify PropertyChanged for the FilteredDolgozok property
            Debug.WriteLine(FilteredGazdalkodoSzervezetek.Count);
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }
        public void DeleteGazdalkodoSzervezet(int id)
        {
            try
            {
                _gazdalkodoSzervezetRepository.DeleteGazdalkodoSzervezet(id);
                for (int i = 0; i < FilteredGazdalkodoSzervezetek.Count; i++)
                {
                    if (FilteredGazdalkodoSzervezetek.ElementAt(i).ID == id)
                        FilteredGazdalkodoSzervezetek.RemoveAt(i);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public ICommand DeleteGazdalkodoSzervezetCommand { get; }
        public ICommand ExportAllDataToExcelCommand { get; }
        public ICommand ExportFilteredDataToExcelCommand { get; }
        public ICommand ToggleMultiSelectionModeCommand { get; }
        public ICommand ExportMultiSelectedItemsCommand { get; }
        public ICommand OpenGazdalkodoSzervezetModifyOrAddWindowCommand { get; }

        private bool CanExecuteDeleteGazdalkodoSzervezetCommand(object obj)
        {
            if (SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteGazdalkodoSzervezetCommand(object obj)
        {
            var temp = new ObservableCollection<GazdalkodoSzervezet>(SelectedItems);
            foreach (var item in temp)
            {
                DeleteGazdalkodoSzervezet(item.ID);
            }
        }

        private bool CanExecuteOpenGazdalkodoSzervezetModifyOrAddWindowCommand(object obj)
        {
            if ((string)obj == "Modify")
                return SelectedRow != null;
            return true;
        }

        private void ExecuteOpenGazdalkodoSzervezetModifyOrAddWindowCommand(object obj)
        {
            if (obj is string mode)
            {
                switch (mode)
                {
                    case "Add":
                        GazdalkodoSzervezetModifyOrAddView existingWindow;
                        if (!WindowHelper.IsGazdalkodoSzervezetAddWindowOpen(out existingWindow))
                        {
                            // The window is not open, create and show a new instance
                            GazdalkodoSzervezetModifyOrAddView gazdalkodoSzervezetModifyOrAddView = new GazdalkodoSzervezetModifyOrAddView(EditMode.Add);
                            gazdalkodoSzervezetModifyOrAddView.Show();
                            Mediator.NewGazdalkodoSzervezetAdded += RefreshGazdalkodoSzervezet;
                        }
                        else
                        {
                            // The window is already open, bring it to the foreground
                            if (existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();        // Activate the window
                            Mediator.NewGazdalkodoSzervezetAdded += RefreshGazdalkodoSzervezet;
                        }
                        break;
                    case "Modify":
                        GazdalkodoSzervezetModifyOrAddView gazdalkodoSzervezetModifyOrAddView2 = new GazdalkodoSzervezetModifyOrAddView(EditMode.Modify, SelectedRow);
                        gazdalkodoSzervezetModifyOrAddView2.Show();
                        Mediator.GazdalkodoSzervezetModified += RefreshGazdalkodoSzervezetAfterModify;
                        break;
                }
            }
        }

        private void RefreshGazdalkodoSzervezet(GazdalkodoSzervezet gazdalkodoSzervezet)
        {
            GazdalkodoSzervezetek = _gazdalkodoSzervezetRepository.GetGazdalkodoSzervezetek();
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredGazdalkodoSzervezetek = GazdalkodoSzervezetek, in this case when clearing the FilteredGazdalkodoSzervezetek in later times, it will affect the GazdalkodoSzervezetek collection too
            FilteredGazdalkodoSzervezetek = new ObservableCollection<GazdalkodoSzervezet>(GazdalkodoSzervezetek);
        }

        private void RefreshGazdalkodoSzervezetAfterModify(GazdalkodoSzervezet gazdalkodoSzervezet)
        {
            for (int i = 0; i < GazdalkodoSzervezetek.Count; i++)
            {
                if (GazdalkodoSzervezetek.ElementAt(i).ID == gazdalkodoSzervezet.ID)
                {
                    GazdalkodoSzervezetek.ElementAt(i).Nev = gazdalkodoSzervezet.Nev;
                    GazdalkodoSzervezetek.ElementAt(i).Kapcsolattarto = gazdalkodoSzervezet.Kapcsolattarto;
                    GazdalkodoSzervezetek.ElementAt(i).Email = gazdalkodoSzervezet.Email;
                    GazdalkodoSzervezetek.ElementAt(i).Telefonszam = gazdalkodoSzervezet.Telefonszam;
                    break;
                }
            }
            FilteredGazdalkodoSzervezetek = new ObservableCollection<GazdalkodoSzervezet>(GazdalkodoSzervezetek);
        }
    }
}
