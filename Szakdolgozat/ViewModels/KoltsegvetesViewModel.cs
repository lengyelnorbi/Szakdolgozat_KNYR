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
    public class KoltsegvetesViewModel : ViewModelBase
    {
        private KoltsegvetesRepository _koltsegvetesRepository;
        private ObservableCollection<BevetelKiadas> _bevetelekKiadasok;

        public ObservableCollection<BevetelKiadas> BevetelekKiadasok
        {
            get { return _bevetelekKiadasok; }
            set
            {
                _bevetelekKiadasok = value;
                OnPropertyChanged(nameof(BevetelekKiadasok));
            }
        }


        private ObservableCollection<BevetelKiadas> _filteredBevetelKiadasok;

        public ObservableCollection<BevetelKiadas> FilteredBevetelekKiadasok
        {
            get { return _filteredBevetelKiadasok; }
            set
            {
                _filteredBevetelKiadasok = value;
                OnPropertyChanged(nameof(FilteredBevetelekKiadasok));
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

        private BevetelKiadas _selectedRow;

        public BevetelKiadas SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        public Dictionary<string, bool> checkboxStatuses = new Dictionary<string, bool>();
        private ObservableCollection<BevetelKiadas> _selectedItems;
        public ObservableCollection<BevetelKiadas> SelectedItems
        {
            get { return _selectedItems ?? (_selectedItems = new ObservableCollection<BevetelKiadas>()); }
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
        public KoltsegvetesViewModel()
        {
            _koltsegvetesRepository = new KoltsegvetesRepository();
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("osszegCB", true);
            checkboxStatuses.Add("penznemCB", true);
            checkboxStatuses.Add("beKiKodCB", true);
            checkboxStatuses.Add("teljesitesiDatumCB", true);
            checkboxStatuses.Add("kotelKovetIDCB", true);
            checkboxStatuses.Add("partnerIDCB", true);
            BevetelekKiadasok = _koltsegvetesRepository.GetKoltsegvetesek();

            FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(
               BevetelekKiadasok.Select(d => new BevetelKiadas(d.ID, d.Osszeg, d.Penznem, d.BeKiKod, d.TeljesitesiDatum, d.KotelKovetID, d.PartnerID)).ToList()
           );
            DeleteBevetelKiadasCommand = new ViewModelCommand(ExecuteDeleteBevetelKiadasCommand, CanExecuteDeleteBevetelKiadasCommand);

            OpenBevetelKiadasModifyOrAddWindowCommand = new ViewModelCommand(ExecuteOpenBevetelKiadasModifyOrAddWindowCommand, CanExecuteOpenBevetelKiadasModifyOrAddWindowCommand);


            _selectedItems = new ObservableCollection<BevetelKiadas>();
            ExportAllDataToExcelCommand = new ViewModelCommand(ExecuteExportAllDataToExcelCommand);
            ExportFilteredDataToExcelCommand = new ViewModelCommand(ExecuteExportFilteredDataToExcelCommand);
            ToggleMultiSelectionModeCommand = new ViewModelCommand(ExecuteToggleMultiSelectionModeCommand);
            ExportMultiSelectedItemsCommand = new ViewModelCommand(ExecuteExportMultiSelectedItemsCommand, CanExecuteExportMultiSelectedItemsCommand);
        }
        private void ExecuteExportAllDataToExcelCommand(object obj)
        {
            // Export all data from the database
            ExportToExcel(BevetelekKiadasok.ToList(), "All_Database_Dolgozok");
        }

        private void ExecuteExportFilteredDataToExcelCommand(object obj)
        {
            // Export all filtered data
            ExportToExcel(FilteredBevetelekKiadasok.ToList(), "Filtered_Dolgozok");
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
            foreach (BevetelKiadas item in selectedItems)
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
        private void ExportToExcel(List<BevetelKiadas> dataToExport, string defaultFileName)
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
                        var worksheet = workbook.Worksheets.Add("Bevetelek es Kiadasok");

                        // Add headers
                        worksheet.Cell(1, 1).Value = "ID";
                        worksheet.Cell(1, 2).Value = "Osszeg";
                        worksheet.Cell(1, 3).Value = "Penznem";
                        worksheet.Cell(1, 4).Value = "BeKiKod";
                        worksheet.Cell(1, 5).Value = "Teljesitesi Datum";
                        worksheet.Cell(1, 6).Value = "Kotelezettseg es Koveteles ID";

                        // Format header row
                        var headerRange = worksheet.Range(1, 1, 1, 5);
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                        headerRange.Style.Font.FontColor = XLColor.White;

                        // Add data rows
                        for (int i = 0; i < dataToExport.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = dataToExport[i].ID;
                            worksheet.Cell(i + 2, 2).Value = dataToExport[i].Osszeg;
                            worksheet.Cell(i + 2, 3).Value = dataToExport[i].Penznem.ToString();
                            worksheet.Cell(i + 2, 4).Value = dataToExport[i].BeKiKod.ToString();
                            worksheet.Cell(i + 2, 5).Value = dataToExport[i].TeljesitesiDatum;
                            worksheet.Cell(i + 2, 6).Value = dataToExport[i].KotelKovetID;
                        }

                        // Auto-fit columns
                        worksheet.Columns().AdjustToContents();

                        // Create a table
                        var range = worksheet.Range(1, 1, dataToExport.Count + 1, 5);
                        var table = range.CreateTable("KoltsegvetesTable");
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
        public void ExportMultipleSelectedToExcel(IEnumerable<BevetelKiadas> selectedItems)
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
                FilteredBevetelekKiadasok.Clear();
                foreach (var d in BevetelekKiadasok)
                {
                    if (checkboxStatuses["idCB"] == true)
                    {
                        if (d.ID.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["osszegCB"] == true)
                    {
                        if (d.Osszeg.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["penznemCB"] == true)
                    {
                        if (d.Penznem.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["beKiKodCB"] == true)
                    {
                        if (d.BeKiKod.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["teljesitesiDatumCB"] == true)
                    {
                        if (d.TeljesitesiDatum.ToShortDateString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["kotelKovetIDCB"] == true)
                    {
                        if (d.KotelKovetID.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["partnerIDCB"] == true)
                    {
                        if (d.PartnerID.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                }
            }
            else
            {
                // If the search query is empty, reset to the original data
                FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(BevetelekKiadasok);
            }

            // Notify PropertyChanged for the FilteredDolgozok property
            Debug.WriteLine(FilteredBevetelekKiadasok.Count);
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }

        public void DeleteBevetelKiadas(int id)
        {
            try
            {
                _koltsegvetesRepository.DeleteKoltsegvetes(id);
                for (int i = 0; i < FilteredBevetelekKiadasok.Count; i++)
                {
                    if (FilteredBevetelekKiadasok.ElementAt(i).ID == id)
                        FilteredBevetelekKiadasok.RemoveAt(i);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ICommand DeleteBevetelKiadasCommand { get; }

        public ICommand ExportAllDataToExcelCommand { get; }
        public ICommand ExportFilteredDataToExcelCommand { get; }
        public ICommand ToggleMultiSelectionModeCommand { get; }
        public ICommand ExportMultiSelectedItemsCommand { get; }

        public ICommand OpenBevetelKiadasModifyOrAddWindowCommand { get; }


        private bool CanExecuteDeleteBevetelKiadasCommand(object obj)
        {
            if (SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteBevetelKiadasCommand(object obj)
        {
            var temp = new ObservableCollection<BevetelKiadas>(SelectedItems);
            foreach (var item in temp)
            {
                DeleteBevetelKiadas(item.ID);
            }
        }

        private bool CanExecuteOpenBevetelKiadasModifyOrAddWindowCommand(object obj)
        {
            if ((string)obj == "Modify")
                return SelectedRow != null;
            return true;
        }

        private void ExecuteOpenBevetelKiadasModifyOrAddWindowCommand(object obj)
        {
            if (obj is string mode)
            {
                switch (mode)
                {
                    case "Add":
                        KoltsegvetesModifyOrAddView existingWindow;
                        if (!WindowHelper.IsKoltsegvetesAddWindowOpen(out existingWindow))
                        {
                            // The window is not open, create and show a new instance
                            KoltsegvetesModifyOrAddView koltsegvetesModifyOrAddView = new KoltsegvetesModifyOrAddView(EditMode.Add);
                            koltsegvetesModifyOrAddView.Show();
                            Mediator.NewBevetelKiadasAdded += RefreshBevetelKiadas;
                        }
                        else
                        {
                            // The window is already open, bring it to the foreground
                            if (existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();        // Activate the window
                            Mediator.NewBevetelKiadasAdded += RefreshBevetelKiadas;
                        }
                        break;
                    case "Modify":
                        KoltsegvetesModifyOrAddView koltsegvetesModifyOrAddView2 = new KoltsegvetesModifyOrAddView(EditMode.Modify, SelectedRow);
                        koltsegvetesModifyOrAddView2.Show();
                        Mediator.BevetelKiadasModified += RefreshBevetelKiadasAfterModify;
                        break;
                }
            }
        }

        private void RefreshBevetelKiadas(BevetelKiadas bevetelKiadas)
        {
            BevetelekKiadasok = _koltsegvetesRepository.GetKoltsegvetesek();
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredBevetelekKiadasok = BevetelekKiadasok, in this case when clearing the FilteredBevetelekKiadasok in later times, it will affect the BevetelekKiadasok collection too
            FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(BevetelekKiadasok);
        }

        private void RefreshBevetelKiadasAfterModify(BevetelKiadas bevetelKiadas)
        {
            for (int i = 0; i < BevetelekKiadasok.Count; i++)
            {
                if (BevetelekKiadasok.ElementAt(i).ID == bevetelKiadas.ID)
                {
                    BevetelekKiadasok.ElementAt(i).Osszeg = bevetelKiadas.Osszeg;
                    BevetelekKiadasok.ElementAt(i).Penznem = bevetelKiadas.Penznem;
                    BevetelekKiadasok.ElementAt(i).BeKiKod = bevetelKiadas.BeKiKod;
                    BevetelekKiadasok.ElementAt(i).TeljesitesiDatum = bevetelKiadas.TeljesitesiDatum;
                    BevetelekKiadasok.ElementAt(i).KotelKovetID = bevetelKiadas.KotelKovetID;
                    BevetelekKiadasok.ElementAt(i).PartnerID = bevetelKiadas.PartnerID;
                    break;
                }
            }
            FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(BevetelekKiadasok);
        }
    }
}
