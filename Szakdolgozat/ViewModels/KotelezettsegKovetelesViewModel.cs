using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Szakdolgozat.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Szakdolgozat.Repositories;
using System.Windows.Input;
using System.Windows;
using Szakdolgozat.Views;
using ClosedXML.Excel;
using System.Collections;
using System.Windows.Forms;

namespace Szakdolgozat.ViewModels
{
    class KotelezettsegKovetelesViewModel : ViewModelBase
    {
        private KotelezettsegKovetelesRepository _kotelezettsegKovetelesRepository;
        private ObservableCollection<KotelezettsegKoveteles> _kotelezettsegekKovetelesek;

        public ObservableCollection<KotelezettsegKoveteles> KotelezettsegekKovetelesek
        {
            get { return _kotelezettsegekKovetelesek; }
            set
            {
                _kotelezettsegekKovetelesek = value;
                OnPropertyChanged(nameof(KotelezettsegekKovetelesek));
            }
        }

        private ObservableCollection<KotelezettsegKoveteles> _filteredKotelezettsegekKovetelesek;

        public ObservableCollection<KotelezettsegKoveteles> FilteredKotelezettsegekKovetelesek
        {
            get { return _filteredKotelezettsegekKovetelesek; }
            set
            {
                _filteredKotelezettsegekKovetelesek = value;
                OnPropertyChanged(nameof(FilteredKotelezettsegekKovetelesek));
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

        private KotelezettsegKoveteles _selectedRow;

        public KotelezettsegKoveteles SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        public Dictionary<string, bool> checkboxStatuses = new Dictionary<string, bool>();
        private ObservableCollection<KotelezettsegKoveteles> _selectedItems;
        public ObservableCollection<KotelezettsegKoveteles> SelectedItems
        {
            get { return _selectedItems ?? (_selectedItems = new ObservableCollection<KotelezettsegKoveteles>()); }
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
        public KotelezettsegKovetelesViewModel()
        {
            _kotelezettsegKovetelesRepository = new KotelezettsegKovetelesRepository();
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("tipusCB", true);
            checkboxStatuses.Add("osszegCB", true);
            checkboxStatuses.Add("penznemCB", true);
            checkboxStatuses.Add("kifizetesHataridejeCB", true);
            checkboxStatuses.Add("kifizetettCB", true);
            KotelezettsegekKovetelesek = _kotelezettsegKovetelesRepository.GetKotelezettsegekKovetelesek();
            FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(KotelezettsegekKovetelesek);

            FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(
               KotelezettsegekKovetelesek.Select(d => new KotelezettsegKoveteles(d.ID, d.Tipus, d.Osszeg, d.Penznem, d.KifizetesHatarideje, d.Kifizetett)).ToList()
            );

            DeleteKotelezettsegKovetelesCommand = new ViewModelCommand(ExecuteDeleteKotelezettsegKovetelesCommand, CanExecuteDeleteKotelezettsegKovetelesCommand);

            OpenKotelezettsegKovetelesModifyOrAddWindowCommand = new ViewModelCommand(ExecuteOpenKotelezettsegKovetelesModifyOrAddWindowCommand, CanExecuteOpenKotelezettsegKovetelesModifyOrAddWindowCommand);

            _selectedItems = new ObservableCollection<KotelezettsegKoveteles>();
            ExportAllDataToExcelCommand = new ViewModelCommand(ExecuteExportAllDataToExcelCommand);
            ExportFilteredDataToExcelCommand = new ViewModelCommand(ExecuteExportFilteredDataToExcelCommand);
            ToggleMultiSelectionModeCommand = new ViewModelCommand(ExecuteToggleMultiSelectionModeCommand);
            ExportMultiSelectedItemsCommand = new ViewModelCommand(ExecuteExportMultiSelectedItemsCommand, CanExecuteExportMultiSelectedItemsCommand);
        }
        private void ExecuteExportAllDataToExcelCommand(object obj)
        {
            // Export all data from the database
            ExportToExcel(KotelezettsegekKovetelesek.ToList(), "All_Database_Dolgozok");
        }

        private void ExecuteExportFilteredDataToExcelCommand(object obj)
        {
            // Export all filtered data
            ExportToExcel(FilteredKotelezettsegekKovetelesek.ToList(), "Filtered_Dolgozok");
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
            foreach (KotelezettsegKoveteles item in selectedItems)
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
        private void ExportToExcel(List<KotelezettsegKoveteles> dataToExport, string defaultFileName)
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
                        var worksheet = workbook.Worksheets.Add("Kotelezettsegek es Kovetelesek");

                        // Add headers
                        worksheet.Cell(1, 1).Value = "ID";
                        worksheet.Cell(1, 2).Value = "Tipus";
                        worksheet.Cell(1, 3).Value = "Osszeg";
                        worksheet.Cell(1, 4).Value = "Penznem";
                        worksheet.Cell(1, 5).Value = "Kifizetes Hatarideje";
                        worksheet.Cell(1, 6).Value = "Kifizetett";

                        // Format header row
                        var headerRange = worksheet.Range(1, 1, 1, 5);
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                        headerRange.Style.Font.FontColor = XLColor.White;

                        // Add data rows
                        for (int i = 0; i < dataToExport.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = dataToExport[i].ID;
                            worksheet.Cell(i + 2, 2).Value = dataToExport[i].Tipus;
                            worksheet.Cell(i + 2, 3).Value = dataToExport[i].Osszeg;
                            worksheet.Cell(i + 2, 4).Value = dataToExport[i].Penznem.ToString();
                            worksheet.Cell(i + 2, 5).Value = dataToExport[i].KifizetesHatarideje;
                            worksheet.Cell(i + 2, 6).Value = dataToExport[i].Kifizetett;
                        }

                        // Auto-fit columns
                        worksheet.Columns().AdjustToContents();

                        // Create a table
                        var range = worksheet.Range(1, 1, dataToExport.Count + 1, 5);
                        var table = range.CreateTable("KotelKovetTable");
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
        public void ExportMultipleSelectedToExcel(IEnumerable<KotelezettsegKoveteles> selectedItems)
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
                FilteredKotelezettsegekKovetelesek.Clear();
                foreach (var d in KotelezettsegekKovetelesek)
                {
                    if (checkboxStatuses["idCB"] == true)
                    {
                        if (d.ID.ToString().Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["tipusCB"] == true)
                    {
                        if (d.Tipus.Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["osszegCB"] == true)
                    {
                        if (d.Osszeg.ToString().Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["penznemCB"] == true)
                    {
                        if (d.Penznem.ToString().Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["kifizetesHataridejeCB"] == true)
                    {
                        if (d.KifizetesHatarideje.ToShortDateString().Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["kifizetettCB"] == true)
                    {
                        if (d.Kifizetett.ToString().Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                }
            }
            else
            {
                // If the search query is empty, reset to the original data
                FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(KotelezettsegekKovetelesek);
            }

            // Notify PropertyChanged for the FilteredDolgozok property
            Debug.WriteLine(FilteredKotelezettsegekKovetelesek.Count);
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }

        //public void DeleteKotelezettsegKoveteles(int id)
        //{
        //    try
        //    {
        //        _kotelezettsegKovetelesRepository.DeleteKotelezettsegKoveteles(id);
        //        for (int i = 0; i < FilteredKotelezettsegekKovetelesek.Count; i++)
        //        {
        //            if (FilteredKotelezettsegekKovetelesek.ElementAt(i).ID == id)
        //                FilteredKotelezettsegekKovetelesek.RemoveAt(i);
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}
        public void DeleteKotelezettsegKoveteles(int id)
        {
            try
            {
                // Check for related records and get information about them
                var hasRelatedRecords = _kotelezettsegKovetelesRepository.CheckForRelatedRecords(id, out string relatedInfo);

                // If there are related records, ask for confirmation
                bool shouldDelete = true;
                if (hasRelatedRecords)
                {
                    shouldDelete = ConfirmCascadeDelete(
                        "Biztosan törölni szeretné ezt a kötelezettséget/követelést?",
                        relatedInfo);
                }

                if (shouldDelete)
                {
                    _kotelezettsegKovetelesRepository.DeleteKotelezettsegKoveteles(id, true);

                    // Remove from filtered collections
                    for (int i = 0; i < FilteredKotelezettsegekKovetelesek.Count; i++)
                    {
                        if (FilteredKotelezettsegekKovetelesek.ElementAt(i).ID == id)
                            FilteredKotelezettsegekKovetelesek.RemoveAt(i);
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(
                    $"Hiba történt a törlés során: {e.Message}",
                    "Hiba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Shows a confirmation dialog for deletion with cascade information
        /// </summary>
        /// <param name="message">Primary message to display</param>
        /// <param name="affectedData">Description of data that will be affected</param>
        /// <returns>True if user confirms deletion, false otherwise</returns>
        private bool ConfirmCascadeDelete(string message, string affectedData)
        {
            var result = System.Windows.MessageBox.Show(
                $"{message}\n\nA következő kapcsolódó adatok is törlésre kerülnek:\n{affectedData}",
                "Megerősítés szükséges",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            return result == MessageBoxResult.Yes;
        }
        public ICommand DeleteKotelezettsegKovetelesCommand { get; }

        public ICommand ExportAllDataToExcelCommand { get; }
        public ICommand ExportFilteredDataToExcelCommand { get; }
        public ICommand ToggleMultiSelectionModeCommand { get; }
        public ICommand ExportMultiSelectedItemsCommand { get; }

        public ICommand OpenKotelezettsegKovetelesModifyOrAddWindowCommand { get; }


        private bool CanExecuteDeleteKotelezettsegKovetelesCommand(object obj)
        {
            if (SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteKotelezettsegKovetelesCommand(object obj)
        {
            var temp = new ObservableCollection<KotelezettsegKoveteles>(SelectedItems);
            foreach (var item in temp)
            {
                DeleteKotelezettsegKoveteles(item.ID);
            }
        }

        private bool CanExecuteOpenKotelezettsegKovetelesModifyOrAddWindowCommand(object obj)
        {
            if ((string)obj == "Modify")
                return SelectedRow != null;
            return true;
        }

        private void ExecuteOpenKotelezettsegKovetelesModifyOrAddWindowCommand(object obj)
        {
            if (obj is string mode)
            {
                switch (mode)
                {
                    case "Add":
                        KotelezettsegKovetelesModifyOrAddView existingWindow;
                        if (!WindowHelper.IsKotelezettsegKovetelesAddWindowOpen(out existingWindow))
                        {
                            // The window is not open, create and show a new instance
                            KotelezettsegKovetelesModifyOrAddView kotelezettsegKovetelesModifyOrAddView = new KotelezettsegKovetelesModifyOrAddView(EditMode.Add);
                            kotelezettsegKovetelesModifyOrAddView.Show();
                            Mediator.NewKotelezettsegKovetelesAdded += RefreshKotelezettsegKoveteles;
                        }
                        else
                        {
                            // The window is already open, bring it to the foreground
                            if (existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();        // Activate the window
                            Mediator.NewKotelezettsegKovetelesAdded += RefreshKotelezettsegKoveteles;
                        }
                        break;
                    case "Modify":
                        KotelezettsegKovetelesModifyOrAddView kotelezettsegKovetelesModifyOrAddView2 = new KotelezettsegKovetelesModifyOrAddView(EditMode.Modify, SelectedRow);
                        kotelezettsegKovetelesModifyOrAddView2.Show();
                        Mediator.KotelezettsegKovetelesModified += RefreshKotelezettsegKovetelesAfterModify;
                        break;
                }
            }
        }

        private void RefreshKotelezettsegKoveteles(KotelezettsegKoveteles kotelezettsegKoveteles)
        {
            KotelezettsegekKovetelesek = _kotelezettsegKovetelesRepository.GetKotelezettsegekKovetelesek();
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredKotelezettsegekKovetelesek = KotelezettsegekKovetelesek, in this case when clearing the FilteredKotelezettsegekKovetelesek in later times, it will affect the KotelezettsegekKovetelesek collection too
            FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(KotelezettsegekKovetelesek);
        }

        private void RefreshKotelezettsegKovetelesAfterModify(KotelezettsegKoveteles kotelezettsegKoveteles)
        {
            for (int i = 0; i < KotelezettsegekKovetelesek.Count; i++)
            {
                if (KotelezettsegekKovetelesek.ElementAt(i).ID == kotelezettsegKoveteles.ID)
                {
                    KotelezettsegekKovetelesek.ElementAt(i).Tipus = kotelezettsegKoveteles.Tipus;
                    KotelezettsegekKovetelesek.ElementAt(i).Osszeg = kotelezettsegKoveteles.Osszeg;
                    KotelezettsegekKovetelesek.ElementAt(i).Penznem = kotelezettsegKoveteles.Penznem;
                    KotelezettsegekKovetelesek.ElementAt(i).KifizetesHatarideje = kotelezettsegKoveteles.KifizetesHatarideje;
                    KotelezettsegekKovetelesek.ElementAt(i).Kifizetett = kotelezettsegKoveteles.Kifizetett;
                    break;
                }
            }
            FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(KotelezettsegekKovetelesek);
        }
    }
}