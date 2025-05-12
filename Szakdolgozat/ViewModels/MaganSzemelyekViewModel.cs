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
using Szakdolgozat.Repositories;
using Szakdolgozat.Views;
using ClosedXML.Excel;
using System.Collections;
using System.Windows.Forms;

namespace Szakdolgozat.ViewModels
{
    public class MaganSzemelyekViewModel : ViewModelBase
    {

        private ObservableCollection<MaganSzemely> _maganSzemelyek;
        private MaganSzemelyRepository _maganSzemelyRepository;

        public ObservableCollection<MaganSzemely> MaganSzemelyek
        {
            get { return _maganSzemelyek; }
            set
            {
                _maganSzemelyek = value;
                OnPropertyChanged(nameof(MaganSzemelyek));
            }
        }

        private ObservableCollection<MaganSzemely> _filteredMaganSzemelyek;

        public ObservableCollection<MaganSzemely> FilteredMaganSzemelyek
        {
            get { return _filteredMaganSzemelyek; }
            set
            {
                _filteredMaganSzemelyek = value;
                OnPropertyChanged(nameof(FilteredMaganSzemelyek));
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
        private MaganSzemely _selectedRow;

        public MaganSzemely SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        public Dictionary<string, bool> checkboxStatuses = new Dictionary<string, bool>();
        private ObservableCollection<MaganSzemely> _selectedItems;
        public ObservableCollection<MaganSzemely> SelectedItems
        {
            get { return _selectedItems ?? (_selectedItems = new ObservableCollection<MaganSzemely>()); }
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
        public MaganSzemelyekViewModel()
        {
            _maganSzemelyRepository = new MaganSzemelyRepository();
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("nevCB", true);
            checkboxStatuses.Add("telefonszamCB", true);
            checkboxStatuses.Add("emailCB", true);
            checkboxStatuses.Add("lakcimCB", true);
            MaganSzemelyek = _maganSzemelyRepository.GetMaganSzemelyek();
            //Deep Copy - to ensure that the FilteredDolgozok does not affect the Dolgozok collection, and vica versa
            FilteredMaganSzemelyek = new ObservableCollection<MaganSzemely>(
                MaganSzemelyek.Select(d => new MaganSzemely(d.ID, d.Nev, d.Lakcim, d.Email, d.Telefonszam)).ToList()
            );

            DeleteMaganSzemelyCommand = new ViewModelCommand(ExecuteDeleteMaganSzemelyCommand, CanExecuteDeleteMaganSzemelyCommand);

            OpenMaganSzemelyModifyOrAddWindowCommand = new ViewModelCommand(ExecuteOpenMaganSzemelyModifyOrAddWindowCommand, CanExecuteOpenMaganSzemelyModifyOrAddWindowCommand);

            _selectedItems = new ObservableCollection<MaganSzemely>();
            ExportAllDataToExcelCommand = new ViewModelCommand(ExecuteExportAllDataToExcelCommand);
            ExportFilteredDataToExcelCommand = new ViewModelCommand(ExecuteExportFilteredDataToExcelCommand);
            ToggleMultiSelectionModeCommand = new ViewModelCommand(ExecuteToggleMultiSelectionModeCommand);
            ExportMultiSelectedItemsCommand = new ViewModelCommand(ExecuteExportMultiSelectedItemsCommand, CanExecuteExportMultiSelectedItemsCommand);
        }
        private void ExecuteExportAllDataToExcelCommand(object obj)
        {
            // Export all data from the database
            ExportToExcel(MaganSzemelyek.ToList(), "All_Database_Dolgozok");
        }

        private void ExecuteExportFilteredDataToExcelCommand(object obj)
        {
            // Export all filtered data
            ExportToExcel(FilteredMaganSzemelyek.ToList(), "Filtered_Dolgozok");
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
            foreach (MaganSzemely item in selectedItems)
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
        private void ExportToExcel(List<MaganSzemely> dataToExport, string defaultFileName)
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
                        var worksheet = workbook.Worksheets.Add("Magan Szemelyek");

                        // Add headers
                        worksheet.Cell(1, 1).Value = "ID";
                        worksheet.Cell(1, 2).Value = "Nev";
                        worksheet.Cell(1, 3).Value = "Email";
                        worksheet.Cell(1, 4).Value = "Telefonszam";
                        worksheet.Cell(1, 5).Value = "Lakcim";

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
                            worksheet.Cell(i + 2, 3).Value = dataToExport[i].Email;
                            worksheet.Cell(i + 2, 4).Value = dataToExport[i].Telefonszam;
                            worksheet.Cell(i + 2, 5).Value = dataToExport[i].Lakcim;
                        }

                        // Auto-fit columns
                        worksheet.Columns().AdjustToContents();

                        // Create a table
                        var range = worksheet.Range(1, 1, dataToExport.Count + 1, 5);
                        var table = range.CreateTable("MaganSzemelyekTable");
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
        public void ExportMultipleSelectedToExcel(IEnumerable<MaganSzemely> selectedItems)
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
                // Filter data based on the search query
                FilteredMaganSzemelyek.Clear();
                foreach (var d in MaganSzemelyek)
                {
                    if (checkboxStatuses["idCB"] == true)
                    {
                        if (d.ID.ToString().Contains(searchQuery))
                        {
                            FilteredMaganSzemelyek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["nevCB"] == true)
                    {
                        if (d.Nev.Contains(searchQuery))
                        {
                            FilteredMaganSzemelyek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["telefonszamCB"] == true)
                    {
                        if (d.Telefonszam.ToString().Contains(searchQuery))
                        {
                            FilteredMaganSzemelyek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["emailCB"] == true)
                    {
                        if (d.Email.Contains(searchQuery))
                        {
                            FilteredMaganSzemelyek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["lakcimCB"] == true)
                    {
                        if (d.Lakcim.Contains(searchQuery))
                        {
                            FilteredMaganSzemelyek.Add(d);
                            continue;
                        }
                    }
                }
            }
            else
            {
                // If the search query is empty, reset to the original data
                FilteredMaganSzemelyek = new ObservableCollection<MaganSzemely>(MaganSzemelyek);
            }

            // Notify PropertyChanged for the FilteredDolgozok property
            Debug.WriteLine(FilteredMaganSzemelyek.Count);
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }

        public void DeleteMaganSzemely(int id)
        {
            try
            {
                _maganSzemelyRepository.DeleteMaganSzemely(id);
                for (int i = 0; i < FilteredMaganSzemelyek.Count; i++)
                {
                    if (FilteredMaganSzemelyek.ElementAt(i).ID == id)
                        FilteredMaganSzemelyek.RemoveAt(i);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ICommand DeleteMaganSzemelyCommand { get; }

        public ICommand ExportAllDataToExcelCommand { get; }
        public ICommand ExportFilteredDataToExcelCommand { get; }
        public ICommand ToggleMultiSelectionModeCommand { get; }
        public ICommand ExportMultiSelectedItemsCommand { get; }

        public ICommand OpenMaganSzemelyModifyOrAddWindowCommand { get; }


        private bool CanExecuteDeleteMaganSzemelyCommand(object obj)
        {
            if (SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteMaganSzemelyCommand(object obj)
        {
            var temp = new ObservableCollection<MaganSzemely>(SelectedItems);
            foreach (var item in temp)
            {
                DeleteMaganSzemely(item.ID);
            }
        }

        private bool CanExecuteOpenMaganSzemelyModifyOrAddWindowCommand(object obj)
        {
            if ((string)obj == "Modify")
                return SelectedRow != null;
            return true;
        }

        private void ExecuteOpenMaganSzemelyModifyOrAddWindowCommand(object obj)
        {
            if (obj is string mode)
            {
                switch (mode)
                {
                    case "Add":
                        MaganSzemelyekModifyOrAddView existingWindow;
                        if (!WindowHelper.IsMaganSzemelyAddWindowOpen(out existingWindow))
                        {
                            // The window is not open, create and show a new instance
                            MaganSzemelyekModifyOrAddView maganSzemelyekModifyOrAddView = new MaganSzemelyekModifyOrAddView(EditMode.Add);
                            maganSzemelyekModifyOrAddView.Show();
                            Mediator.NewMaganSzemelyAdded += RefreshMaganSzemely;
                        }
                        else
                        {
                            // The window is already open, bring it to the foreground
                            if (existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();        // Activate the window
                            Mediator.NewMaganSzemelyAdded += RefreshMaganSzemely;
                        }
                        break;
                    case "Modify":
                        MaganSzemelyekModifyOrAddView maganSzemelyekModifyOrAddView2 = new MaganSzemelyekModifyOrAddView(EditMode.Modify, SelectedRow);
                        maganSzemelyekModifyOrAddView2.Show();
                        Mediator.MaganSzemelyModified += RefreshMaganSzemelyAfterModify;
                        break;
                }
            }
        }

        private void RefreshMaganSzemely(MaganSzemely maganSzemely)
        {
            MaganSzemelyek = _maganSzemelyRepository.GetMaganSzemelyek();
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredMaganSzemelyek = MaganSzemelyek, in this case when clearing the FilteredMaganSzemelyek in later times, it will affect the MaganSzemelyek collection too
            FilteredMaganSzemelyek = new ObservableCollection<MaganSzemely>(MaganSzemelyek);
        }

        private void RefreshMaganSzemelyAfterModify(MaganSzemely maganSzemely)
        {
            for (int i = 0; i < MaganSzemelyek.Count; i++)
            {
                if (MaganSzemelyek.ElementAt(i).ID == maganSzemely.ID)
                {
                    MaganSzemelyek.ElementAt(i).Nev = maganSzemely.Nev;
                    MaganSzemelyek.ElementAt(i).Lakcim = maganSzemely.Lakcim;
                    MaganSzemelyek.ElementAt(i).Email = maganSzemely.Email;
                    MaganSzemelyek.ElementAt(i).Telefonszam = maganSzemely.Telefonszam;
                    break;
                }
            }
            FilteredMaganSzemelyek = new ObservableCollection<MaganSzemely>(MaganSzemelyek);
        }
    }
}
