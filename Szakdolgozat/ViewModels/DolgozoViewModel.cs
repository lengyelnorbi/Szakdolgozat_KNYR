using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Szakdolgozat.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Szakdolgozat.Repositories;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Principal;
using System.Threading;
using Szakdolgozat.Views;
using System.Windows.Forms;
using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Windows.Media;
using ClosedXML.Excel;
using System.Collections;

namespace Szakdolgozat.ViewModels
{
    class DolgozoViewModel : ViewModelBase
    {
        private ObservableCollection<Dolgozo> _dolgozok;
        private DolgozoRepository _dolgozoRepository;

        public ObservableCollection<Dolgozo> Dolgozok
        {
            get { return _dolgozok; }
            set
            {
                _dolgozok = value;
                OnPropertyChanged(nameof(Dolgozok));
            }
        }

        private ObservableCollection<Dolgozo> _filteredDolgozok;

        public ObservableCollection<Dolgozo> FilteredDolgozok
        {
            get { return _filteredDolgozok; }
            set
            {
                _filteredDolgozok = value;
                OnPropertyChanged(nameof(FilteredDolgozok));
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

        private Dolgozo _selectedRow;

        public Dolgozo SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        public bool IsEditPanelVisible => CurrentMode != EditMode.None;

        private EditMode _currentMode;
        public EditMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
                OnPropertyChanged(nameof(CurrentMode));
                OnPropertyChanged(nameof(IsEditPanelVisible));
            }
        }

        public Dictionary<string, bool> checkboxStatuses = new Dictionary<string, bool>();

        private ObservableCollection<Dolgozo> _selectedItems;
        public ObservableCollection<Dolgozo> SelectedItems
        {
            get { return _selectedItems ?? (_selectedItems = new ObservableCollection<Dolgozo>()); }
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
        public DolgozoViewModel()
        {
            _dolgozoRepository = new DolgozoRepository();
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("vezeteknevCB", true);
            checkboxStatuses.Add("keresztnevCB", true);
            checkboxStatuses.Add("emailCB", true);
            checkboxStatuses.Add("telefonszamCB", true);
            Dolgozok = _dolgozoRepository.GetDolgozok(); 
            //Deep Copy - to ensure that the FilteredDolgozok does not affect the Dolgozok collection, and vica versa
            FilteredDolgozok = new ObservableCollection<Dolgozo>(
                Dolgozok.Select(d => new Dolgozo(d.ID, d.Vezeteknev, d.Keresztnev, d.Email, d.Telefonszam)).ToList()
            );
            DeleteDolgozoCommand = new ViewModelCommand(ExecuteDeleteDolgozoCommand, CanExecuteDeleteDolgozoCommand);

            _selectedItems = new ObservableCollection<Dolgozo>();
            ExportAllDataToExcelCommand = new ViewModelCommand(ExecuteExportAllDataToExcelCommand);
            ExportFilteredDataToExcelCommand = new ViewModelCommand(ExecuteExportFilteredDataToExcelCommand);
            ToggleMultiSelectionModeCommand = new ViewModelCommand(ExecuteToggleMultiSelectionModeCommand);
            ExportMultiSelectedItemsCommand = new ViewModelCommand(ExecuteExportMultiSelectedItemsCommand, CanExecuteExportMultiSelectedItemsCommand);


            OpenDolgozoModifyOrAddWindowCommand = new ViewModelCommand(ExecuteOpenDolgozoModifyOrAddWindowCommand, CanExecuteOpenDolgozoModifyOrAddWindowCommand);

            Debug.WriteLine("EREDETI FILTERED");
            Debug.WriteLine(FilteredDolgozok.Count);
        }
        private void ExecuteExportAllDataToExcelCommand(object obj)
        {
            // Export all data from the database
            ExportToExcel(Dolgozok.ToList(), "All_Database_Dolgozok");
        }

        private void ExecuteExportFilteredDataToExcelCommand(object obj)
        {
            // Export all filtered data
            ExportToExcel(FilteredDolgozok.ToList(), "Filtered_Dolgozok");
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
            foreach (Dolgozo item in selectedItems)
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
        private void ExportToExcel(List<Dolgozo> dataToExport, string defaultFileName)
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
                        var worksheet = workbook.Worksheets.Add("Dolgozok");

                        // Add headers
                        worksheet.Cell(1, 1).Value = "ID";
                        worksheet.Cell(1, 2).Value = "Vezeteknev";
                        worksheet.Cell(1, 3).Value = "Keresztnev";
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
                            worksheet.Cell(i + 2, 2).Value = dataToExport[i].Vezeteknev;
                            worksheet.Cell(i + 2, 3).Value = dataToExport[i].Keresztnev;
                            worksheet.Cell(i + 2, 4).Value = dataToExport[i].Email;
                            worksheet.Cell(i + 2, 5).Value = dataToExport[i].Telefonszam;
                        }

                        // Auto-fit columns
                        worksheet.Columns().AdjustToContents();

                        // Create a table
                        var range = worksheet.Range(1, 1, dataToExport.Count + 1, 5);
                        var table = range.CreateTable("DolgozokTable");
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
        public void ExportMultipleSelectedToExcel(IEnumerable<Dolgozo> selectedItems)
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
                FilteredDolgozok.Clear();
                foreach(var d in Dolgozok)
                {
                    if (checkboxStatuses["idCB"] == true)
                    {
                        if (d.ID.ToString().Contains(searchQuery))
                        {
                            FilteredDolgozok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["vezeteknevCB"] == true)
                    {
                        if (d.Vezeteknev.Contains(searchQuery))
                        {
                            FilteredDolgozok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["keresztnevCB"] == true)
                    {
                        if (d.Keresztnev.Contains(searchQuery))
                        {
                            FilteredDolgozok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["emailCB"] == true)
                    {
                        if (d.Email.Contains(searchQuery))
                        {
                            FilteredDolgozok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["telefonszamCB"] == true)
                    {
                        if (d.Telefonszam.Contains(searchQuery))
                        {
                            FilteredDolgozok.Add(d);
                            continue;
                        }
                    }
                }
            }
            else
            {
                // If the search query is empty, reset to the original data
                FilteredDolgozok = new ObservableCollection<Dolgozo>(Dolgozok);
            }

            // Notify PropertyChanged for the FilteredDolgozok property
            Debug.WriteLine(FilteredDolgozok.Count);
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }

        public void DeleteDolgozo(int id)
        {
            try
            {
                _dolgozoRepository.DeleteDolgozo(id);
                for (int i = 0; i < FilteredDolgozok.Count; i++)
                {
                    if (FilteredDolgozok.ElementAt(i).ID == id)
                        FilteredDolgozok.RemoveAt(i);
                }

            }
            catch (Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public ICommand DeleteDolgozoCommand { get; }


        public ICommand ExportAllDataToExcelCommand { get; }
        public ICommand ExportFilteredDataToExcelCommand { get; }
        public ICommand ToggleMultiSelectionModeCommand { get; }
        public ICommand ExportMultiSelectedItemsCommand { get; }

        public ICommand OpenDolgozoModifyOrAddWindowCommand { get; }


        private bool CanExecuteDeleteDolgozoCommand(object obj)
        {
            if(SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteDolgozoCommand(object obj)
        {
            var temp = new ObservableCollection<Dolgozo>(SelectedItems);
            foreach (var item in temp)
            {
                DeleteDolgozo(item.ID);
            }
        }

        private bool CanExecuteOpenDolgozoModifyOrAddWindowCommand(object obj)
        {
            if((string)obj == "Modify")
                return SelectedRow != null;
            return true;
        }

        private void ExecuteOpenDolgozoModifyOrAddWindowCommand(object obj)
        {
            if (obj is string mode)
            {
                switch (mode)
                {
                    case "Add":
                        DolgozokModifyOrAddView existingWindow;
                        if (!WindowHelper.IsDolgozoAddWindowOpen(out existingWindow))
                        {
                            // The window is not open, create and show a new instance
                            DolgozokModifyOrAddView dolgozokModifyOrAddView = new DolgozokModifyOrAddView(EditMode.Add);
                            dolgozokModifyOrAddView.Show();
                            Mediator.NewDolgozoAdded += RefreshDolgozok;
                        }
                        else
                        {
                            // The window is already open, bring it to the foreground
                            if(existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();        // Activate the window
                            Mediator.NewDolgozoAdded += RefreshDolgozok;
                        }
                        break;
                    case "Modify":
                        DolgozokModifyOrAddView dolgozokModifyOrAddView2 = new DolgozokModifyOrAddView(EditMode.Modify, SelectedRow);
                        dolgozokModifyOrAddView2.Show();
                        Mediator.DolgozoModified += RefreshDolgozokAfterModify;
                        break;
                }
            }
        }
        

        private void RefreshDolgozok(Dolgozo dolgozo) 
        {
            Dolgozok = _dolgozoRepository.GetDolgozok();
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredDolgozok = Dolgozok, in this case when clearing the FilteredDolgozok in later times, it will affect the Dolgozok collection too
            FilteredDolgozok = new ObservableCollection<Dolgozo>(Dolgozok);
        }

        private void RefreshDolgozokAfterModify(Dolgozo dolgozo)
        {
            for (int i = 0; i < Dolgozok.Count; i++)
            {
                if (Dolgozok.ElementAt(i).ID == dolgozo.ID)
                {
                    Dolgozok.ElementAt(i).Vezeteknev = dolgozo.Vezeteknev;
                    Dolgozok.ElementAt(i).Keresztnev = dolgozo.Keresztnev;
                    Dolgozok.ElementAt(i).Email = dolgozo.Email;
                    Dolgozok.ElementAt(i).Telefonszam = dolgozo.Telefonszam;
                    break;
                }
            }
            FilteredDolgozok = new ObservableCollection<Dolgozo>(Dolgozok);
        }
    }
}
