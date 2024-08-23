using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Series;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Szakdolgozat.Models;
using Szakdolgozat.Repositories;
using LiveCharts.Helpers;
using Google.Protobuf.WellKnownTypes;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Data;

namespace Szakdolgozat.ViewModels
{
    //Új diagramm készítésekor megnyitott ablak viewmodelje.
    //The window's viewmodel that shows when creating a new diagram.
    public class CreateChartsViewModel : ViewModelBase
    {
        public bool GroupByPenznemCheckBoxIsChecked = false;
        public bool GroupByBeKiKodCheckBoxIsChecked = false;
        SolidColorBrush[] baseColors = new SolidColorBrush[] { Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Magenta };
        public SeriesCollection Series { get; set; }
        public Func<ChartPoint, string> LabelFormatter { get; set; }

        public SeriesCollection LineSeries { get; set; }
        public string[] LineSeriesLabels { get; set; }
        public Func<double, string> LineSeriesYFormatter { get; set; }

        public event Action checkboxChange;

        public List<System.Windows.Controls.DataGridCell> selectedDataGridCells = new List<System.Windows.Controls.DataGridCell>();

        public ObservableCollection<BevetelKiadas> _selectedBevetelekKiadasok = new ObservableCollection<BevetelKiadas>();
        public ObservableCollection<KotelezettsegKoveteles> _selectedKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>();

        private ObservableCollection<System.Windows.Controls.TabItem> _tabs = new ObservableCollection<System.Windows.Controls.TabItem>();
        public ObservableCollection<System.Windows.Controls.TabItem> Tabs
        {
            get { return _tabs; }
            set 
            { 
                _tabs = value;
                OnPropertyChanged(nameof(Tabs));
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

        private ObservableCollection<BevetelKiadas> _filteredBevetelekKiadasok;

        public ObservableCollection<BevetelKiadas> FilteredBevetelekKiadasok
        {
            get { return _filteredBevetelekKiadasok; }
            set
            {
                _filteredBevetelekKiadasok = value;
                OnPropertyChanged(nameof(FilteredBevetelekKiadasok));
            }
        }

        private ObservableCollection<KotelezettsegKoveteles> _kotelKovetelesek;
        public ObservableCollection<KotelezettsegKoveteles> KotelKovetelesek
        {
            get { return _kotelKovetelesek; }
            set
            {
                _kotelKovetelesek = value;
                OnPropertyChanged(nameof(KotelKovetelesek));
            }
        }

        private ObservableCollection<KotelezettsegKoveteles> _filteredKotelKovetelesek;

        public ObservableCollection<KotelezettsegKoveteles> FilteredKotelKovetelesek
        {
            get { return _filteredKotelKovetelesek; }
            set
            {
                _filteredKotelKovetelesek = value;
                OnPropertyChanged(nameof(FilteredKotelKovetelesek));
            }
        }

        //private GazdalkodoSzervezetRepository gazdalkodoSzervezetRepository = new GazdalkodoSzervezetRepository();
        private KoltsegvetesRepository koltsegvetesRepository = new KoltsegvetesRepository();
        private KotelezettsegKovetelesRepository kotelezettsegKovetelesRepository = new KotelezettsegKovetelesRepository();
        //private MaganSzemelyRepository maganSzemelyRepository = new MaganSzemelyRepository();

        //Egy másik viewmodel betöltéséhez szükséges property, ami a már nyitott ablakon jelenik meg
        //Needed for loading in childviews (viewmodels/subviews) onto the window that is already open
        private ViewModelBase _currentChildView;
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        private string _seriesType;
        public string SeriesType
        {
            get
            {
                return _seriesType;
            }
            set
            {
                _seriesType = value;
                OnPropertyChanged(nameof(SeriesType));
                Mediator.NotifySetSeriesVisibility(value);
            }
        }

        private ObservableCollection<object> _selectedRows = new ObservableCollection<object>();
        public ObservableCollection<object> SelectedRows
        {
            get
            {
                return _selectedRows;
            }
            set 
            {
                _selectedRows = value;
                OnPropertyChanged(nameof(SelectedRows));
            }
        }
        //public Dictionary<string, bool> monthCheckboxStatuses = new Dictionary<string, bool>();
        public void UpdateSelectedRows(ObservableCollection<object> selectedRows)
        {
            SelectedRows = selectedRows;
        }
        public ObservableCollection<object> ReturnRequestedData()
        {
            return SelectedRows;
        }
        
        //A jelenleg szükséges tábla neveket tartalmazó kollekció
        //A collection that holds the table names of the datatables
        //private readonly ObservableCollection<string> _dbTableNames = new ObservableCollection<string> {"bevetelek_kiadasok", "felhasznalok", "dolgozok", "gazdalkodo_szervezetek", "kotelezettsegek_kovetelesek", "magan_szemelyek"};
        
        //Egy szótár, amiben a tábla nevekhez vannak rendelve a táblában található oszlop nevekből készített lista
        //A dictionary that holds the table name and the column names that belongs to it as key-value pairs 
        private Dictionary<string, ObservableCollection<string>> _dbTablesWithColumnNames = new Dictionary<string, ObservableCollection<string>>();

        //public ObservableCollection<System.Windows.Controls.ComboBoxItem> _checkBoxes = new ObservableCollection<System.Windows.Controls.ComboBoxItem>();
        //public ObservableCollection<System.Windows.Controls.ComboBoxItem> CheckBoxes
        public ObservableCollection<GroupBySelection> _groupBySelections = new ObservableCollection<GroupBySelection>();
        public ObservableCollection<GroupBySelection> GroupBySelections 
        {
            get { return _groupBySelections; }
            set 
            {
                _groupBySelections = value;
                OnPropertyChanged(nameof(GroupBySelections));
            }
        }

        public Dictionary<string, bool> checkboxStatuses = new Dictionary<string, bool>();

        public ICommand ShowSelectDataForNewChartViewCommand { get; }
        public ICommand ShowAddOptionToNewChartViewCommand { get; }
        public ICommand TesztCommand{ get; }

        public CreateChartsViewModel()
        {
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("osszegCB", true);
            checkboxStatuses.Add("penznemCB", true);
            checkboxStatuses.Add("bekikodCB", true);
            checkboxStatuses.Add("teljesitesiDatumCB", true);
            checkboxStatuses.Add("kotelKovetIDCB", true);
            checkboxStatuses.Add("partnerIDCB", true);

            //nem biztos hogy kell innen
            Mediator.SelectedRowsChangedOnChildView += UpdateSelectedRows;
            Mediator.DataRequest += ReturnRequestedData;


            ShowSelectDataForNewChartViewCommand = new ViewModelCommand(ExecuteShowSelectDataForNewChartViewCommand);
            ShowAddOptionToNewChartViewCommand = new ViewModelCommand(ExecuteShowAddOptionToNewChartViewCommand);
            //idáig
            Mediator.HideOrShowLineSeries += (name, isSelected) => HideOrShowLineSeriesBySelection(name, isSelected);
            Mediator.SetLineSeriesNewColor += (name, color) => SetLineSeriesNewColor(name, color);

            BevetelekKiadasok = koltsegvetesRepository.GetKoltsegvetesek();
            //Deep Copy - to ensure that the FilteredDolgozok does not affect the Dolgozok collection, and vica versa
            FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(
                BevetelekKiadasok.Select(d => new BevetelKiadas(d.ID, d.Osszeg, d.Penznem, d.BeKiKod, d.TeljesitesiDatum, d.KotelKovetID, d.PartnerID)).ToList()
            );

            KotelKovetelesek = kotelezettsegKovetelesRepository.GetKotelezettsegekKovetelesek();
            //Deep Copy - to ensure that the FilteredDolgozok does not affect the Dolgozok collection, and vica versa
            FilteredKotelKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(
                KotelKovetelesek.Select(d => new KotelezettsegKoveteles(d.ID, d.Tipus, d.Osszeg, d.Penznem, d.KifizetesHatarideje, d.Kifizetett)).ToList()
            );

            Tabs.Add(CreateTabItem("Koltsegvetes", true, "bevetelek_kiadasok"));
            Tabs.Add(CreateTabItem("KotelKovetelesek", false, "kotelezettsegek_kovetelesek"));

            UserRepository userRepository = new UserRepository();

            //Adat szűrő feltöltése (comboboxitem-ekkel)
            //Update the Data filter combobox with comboboxitems - START
            //foreach (var table in _dbTableNames)
            //{
            //    //sortedSelectedCells.Add(table, new Dictionary<string, ObservableCollection<object>>
            //    //{
            //    //    { "Strings", new ObservableCollection<object>() },
            //    //    { "Ints", new ObservableCollection<object>() },
            //    //    { "Doubles", new ObservableCollection<object>() },
            //    //    { "Bools", new ObservableCollection<object>() },
            //    //    { "Dates", new ObservableCollection<object>() }
            //    //});

            //    //Combobox title
            //    ComboBoxItem cbitem2 = new ComboBoxItem();
            //    cbitem2.Name = "datafilter";
            //    cbitem2.Content = "Adat szűrő";
            //    cbitem2.IsEnabled = false;
            //    cbitem2.Visibility = Visibility.Collapsed;
            //    _checkBoxes.Add(cbitem2);

            //    //A comboboxitem that shows which table's checkboxes are under it
            //    ComboBoxItem cbitem3 = new ComboBoxItem();
            //    cbitem3.Name = table.ToString() + "_CBI";
            //    cbitem3.Content = table.ToString();
            //    cbitem3.IsEnabled = false;
            //    _checkBoxes.Add(cbitem3);

            //    //A checkbox that later sets all the corresponding checkboxes to checked
            //    TextBlock textBlock2 = new TextBlock();
            //    textBlock2.Text = "Mind";
            //    System.Windows.Controls.CheckBox checkBox2 = new System.Windows.Controls.CheckBox();
            //    checkBox2.Name = table + "_All";
            //    checkBox2.Content = textBlock2;
            //    checkBox2.Checked += OptionsCheckBoxAllChecked;
            //    checkBox2.Unchecked += OptionsCheckBoxAllUnchecked;

            //    ComboBoxItem cbitem4 = new ComboBoxItem();
            //    cbitem4.Name = table.ToString() + "_CBI_All";
            //    cbitem4.Content = checkBox2;
            //    _checkBoxes.Add(cbitem4);

            //    _dbTablesWithColumnNames.Add(table, userRepository.GetColumnNamesForTables(table));
            //    foreach(var s in userRepository.GetColumnNamesForTables(table))
            //    {
            //        //Pl: Name = dolgozok_id_ChB
            //        //A checkbox to all the column name for each table
            //        TextBlock textBlock = new TextBlock();
            //        textBlock.Text = s;
            //        System.Windows.Controls.CheckBox checkBox = new System.Windows.Controls.CheckBox();
            //        checkBox.Name = table + "_ChB" + s;
            //        checkBox.Content = textBlock;
            //        checkBox.Checked += OptionsCheckboxCheckedChange;
            //        checkBox.Unchecked += OptionsCheckboxCheckedChange;

            //        //Pl: Name = id_CB
            //        ComboBoxItem cbitem = new ComboBoxItem();
            //        cbitem.Name = s + "_CB";
            //        cbitem.Content = checkBox;
            //        _checkBoxes.Add(cbitem);
            //    }
            //}
            //FINISH
        }

        private void FilterData(string searchQuery)
        {
            System.Windows.Controls.TabControl tabControl = Mediator.NotifyGetTabControl();
            if(tabControl.SelectedItem is TabItem tabItem)
            {
                if (tabItem.Name.Contains("Koltsegvetes"))
                {
                    if (!string.IsNullOrWhiteSpace(searchQuery) && !string.IsNullOrEmpty(searchQuery))
                    {
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
                            if (checkboxStatuses["bekikodCB"] == true)
                            {
                                if (d.BeKiKod.ToString().Contains(searchQuery))
                                {
                                    FilteredBevetelekKiadasok.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["teljesitesiDatumCB"] == true)
                            {
                                if (d.TeljesitesiDatum.ToString().Contains(searchQuery))
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
                        OnPropertyChanged(nameof(Tabs));
                    }
                }
            }
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }
        private void SetLineSeriesNewColor(string name, SolidColorBrush color)
        {
            foreach(var a in LineSeries)
            {
                if(a is LineSeries lineSeries)
                {
                    if(lineSeries.Name == name)
                    {
                        lineSeries.PointForeground = color;
                        lineSeries.Stroke = color;
                        OnPropertyChanged(nameof(LineSeries));
                        break;
                    }
                }
            }
        }

        public void SetSeries()
        {
            switch(SeriesType)
            {
                case "DoghnutSeries":
                    SetDoghnutSeries();
                    break;
                case "RowSeries":
                    SetRowSeries();
                    break;
                case "StackedSeries":
                    SetStackedSeries();
                    break;
                case "BasicColumnSeries":
                    SetBasicColumnSeries();
                    break;
                case "LineSeries":
                    SetLineSeries();
                    break;
                default:
                    break;
            }
        }

        public void SetStackedSeries()
        {

            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
            var kotelezetsegekKovetelesekAdatsor = new ChartValues<ObservableValue>();

            foreach (var a in _selectedBevetelekKiadasok)
            {
                bevetelekKiadasokAdatsor.Add(new ObservableValue(a.Osszeg));
            }

            foreach (var a in _selectedKotelezettsegekKovetelesek)
            {
                kotelezetsegekKovetelesekAdatsor.Add(new ObservableValue(a.Osszeg));
            }

            var totalBevetelekKiadasokAdatsor = bevetelekKiadasokAdatsor.Sum(x => x.Value);
            var totalKotelezetsegekKovetelesekAdatsor = kotelezetsegekKovetelesekAdatsor.Sum(x => x.Value);
            var totalSum = totalBevetelekKiadasokAdatsor + totalKotelezetsegekKovetelesekAdatsor;

            Series = new SeriesCollection();

            // Add Chrome series
            //AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalSum, "Bevételek és Kiadások", Brushes.Blue);

            // Add Firefox series
            //AddPieSeries(kotelezetsegekKovetelesekAdatsor, totalKotelezetsegekKovetelesekAdatsor, totalSum, "Kötelezettségek és Követelések", Brushes.Red);

            OnPropertyChanged(nameof(Series));
        }

        public void SetBasicColumnSeries()
        {

            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
            var kotelezetsegekKovetelesekAdatsor = new ChartValues<ObservableValue>();

            foreach (var a in _selectedBevetelekKiadasok)
            {
                bevetelekKiadasokAdatsor.Add(new ObservableValue(a.Osszeg));
            }

            foreach (var a in _selectedKotelezettsegekKovetelesek)
            {
                kotelezetsegekKovetelesekAdatsor.Add(new ObservableValue(a.Osszeg));
            }

            var totalBevetelekKiadasokAdatsor = bevetelekKiadasokAdatsor.Sum(x => x.Value);
            var totalKotelezetsegekKovetelesekAdatsor = kotelezetsegekKovetelesekAdatsor.Sum(x => x.Value);
            var totalSum = totalBevetelekKiadasokAdatsor + totalKotelezetsegekKovetelesekAdatsor;

            Series = new SeriesCollection();

            // Add Chrome series
            //AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalSum, "Bevételek és Kiadások", Brushes.Blue);

            // Add Firefox series
            //AddPieSeries(kotelezetsegekKovetelesekAdatsor, totalKotelezetsegekKovetelesekAdatsor, totalSum, "Kötelezettségek és Követelések", Brushes.Red);

            OnPropertyChanged(nameof(Series));
        }

        public void SetLineSeries()
        {

            LineSeries = new SeriesCollection();
            GroupBySelections.Clear();

            if (GroupByBeKiKodCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
            {
                var groupedByPenznem = _selectedBevetelekKiadasok.GroupBy(p => new { p.Penznem, p.BeKiKod });

                // Create a dictionary to hold the HashSet for each group
                var hashSetsByPenznem = new Dictionary<(Penznem Penznem, BeKiKod BeKiKod), HashSet<BevetelKiadas>>();

                foreach (var group in groupedByPenznem)
                {
                    // Create a HashSet for each group
                    var hashSet = new HashSet<BevetelKiadas>(group);
                    hashSetsByPenznem[(group.Key.Penznem, group.Key.BeKiKod)] = hashSet;
                }

                int a = 0;
                // Display the results
                foreach (var kvp in hashSetsByPenznem)
                {
                    var bevetelekKiadasokAdatsor = new ChartValues<double>();
                    foreach (var bevetelKiadas in kvp.Value)
                    {
                        bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas.Osszeg));
                    }
                    var totalBevetelekKiadasokAdatsor = bevetelekKiadasokAdatsor.Sum(x => x);
                    AddLineSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem} + {kvp.Key.BeKiKod}", $"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}", baseColors[a]);
                    AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}", a);
                    if (a + 1 > 3)
                        a = 0;
                    else a += 1;
                }
            }
            else if(GroupByBeKiKodCheckBoxIsChecked)
            {
                var groupedByPenznem = _selectedBevetelekKiadasok.GroupBy(p => new { p.BeKiKod });

                // Create a dictionary to hold the HashSet for each group
                var hashSetsByPenznem = new Dictionary<BeKiKod, HashSet<BevetelKiadas>>();

                foreach (var group in groupedByPenznem)
                {
                    // Create a HashSet for each group
                    var hashSet = new HashSet<BevetelKiadas>(group);
                    hashSetsByPenznem[group.Key.BeKiKod] = hashSet;
                }

                int a = 0;
                // Display the results
                foreach (var kvp in hashSetsByPenznem)
                {
                    var bevetelekKiadasokAdatsor = new ChartValues<double>();
                    foreach (var bevetelKiadas in kvp.Value)
                    {
                        bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas.Osszeg));
                    }
                    var totalBevetelekKiadasokAdatsor = bevetelekKiadasokAdatsor.Sum(x => x);
                    AddLineSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key}", kvp.Key.ToString(), baseColors[a]);
                    AddGroupByDataToCollection(kvp.Key.ToString(), a);
                    if (a + 1 > 3)
                        a = 0;
                    else a += 1;
                }
            }
            else if (GroupByPenznemCheckBoxIsChecked)
            {
                var groupedByPenznem = _selectedBevetelekKiadasok.GroupBy(p => new { p.Penznem });

                // Create a dictionary to hold the HashSet for each group
                var hashSetsByPenznem = new Dictionary<Penznem, HashSet<BevetelKiadas>>();

                foreach (var group in groupedByPenznem)
                {
                    // Create a HashSet for each group
                    var hashSet = new HashSet<BevetelKiadas>(group);
                    hashSetsByPenznem[group.Key.Penznem] = hashSet;
                }

                int a = 0;
                // Display the results
                foreach (var kvp in hashSetsByPenznem)
                {
                    var bevetelekKiadasokAdatsor = new ChartValues<double>();
                    foreach (var bevetelKiadas in kvp.Value)
                    {
                        bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas.Osszeg));
                    }
                    var totalBevetelekKiadasokAdatsor = bevetelekKiadasokAdatsor.Sum(x => x);
                    AddLineSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key}", kvp.Key.ToString(), baseColors[a]);
                    AddGroupByDataToCollection(kvp.Key.ToString(), a);
                    if (a + 1 > 3)
                        a = 0;
                    else a += 1;
                }
            }
            else
            {
                var bevetelekKiadasokAdatsor = new ChartValues<double>();
                var kotelezetsegekKovetelesekAdatsor = new ChartValues<double>();

                foreach (var a in _selectedBevetelekKiadasok)
                {
                    bevetelekKiadasokAdatsor.Add(Convert.ToDouble(a.Osszeg));
                }

                foreach (var a in _selectedKotelezettsegekKovetelesek)
                {
                    kotelezetsegekKovetelesekAdatsor.Add(Convert.ToDouble(a.Osszeg));
                }

                var totalBevetelekKiadasokAdatsor = bevetelekKiadasokAdatsor.Sum(x => x);
                var totalKotelezetsegekKovetelesekAdatsor = kotelezetsegekKovetelesekAdatsor.Sum(x => x);
                var totalSum = totalBevetelekKiadasokAdatsor + totalKotelezetsegekKovetelesekAdatsor;


                AddLineSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalSum, "Bevételek és Kiadások", "bevetelekKiadasok", baseColors[0]);
                AddGroupByDataToCollection("bevetelekKiadasok", 0);
                AddLineSeries(kotelezetsegekKovetelesekAdatsor, totalKotelezetsegekKovetelesekAdatsor, totalSum, "Kötelezettségek és Követelések", "kotKov", baseColors[1]);
                AddGroupByDataToCollection("kotKov", 1);
            }

            OnPropertyChanged(nameof(Series));
            OnPropertyChanged(nameof(GroupBySelections));
        }

        //Hozzáadja a GroupBySelections kollekcióhoz az új GroupBySelection-t.
        //It adds a new GroupBySelection to the GroupBySelections collection.
        //További információért a GroupBySelection class-t nyissa meg. - For more information open the GroupBySelection class.
        private void AddGroupByDataToCollection(string groupByName, int a)
        {
            bool isExistingAlready = false;
            //Ellenőrzi, hogy a GroupBySelection létezik-e.
            //Check if a GroupBySelection already exists.
            foreach (var b in GroupBySelections)
            {
                if (b.Name.Equals(groupByName))
                {
                    isExistingAlready = true;
                    break;
                }
            }
            if (!isExistingAlready)
            {
                GroupBySelections.Add(new GroupBySelection(groupByName, true, baseColors[a]));
            }
        }

        //A LineSeries-ek megjelenítését szabályozza, a name és isSelected paraméterek által.
        //It changes the LineSeries's display by the name and isSelected parameter.
        private void HideOrShowLineSeriesBySelection(string name, bool isSelected)
        {
            foreach (var a in LineSeries)
            {
                if (a is LineSeries lineSeries)
                {
                    if (lineSeries.Name == name)
                    {
                        if (isSelected)
                        {
                            lineSeries.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lineSeries.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
        }

        public void SetRowSeries()
        {

            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
            var kotelezetsegekKovetelesekAdatsor = new ChartValues<ObservableValue>();

            foreach (var a in _selectedBevetelekKiadasok)
            {
                bevetelekKiadasokAdatsor.Add(new ObservableValue(a.Osszeg));
            }

            foreach (var a in _selectedKotelezettsegekKovetelesek)
            {
                kotelezetsegekKovetelesekAdatsor.Add(new ObservableValue(a.Osszeg));
            }

            var totalBevetelekKiadasokAdatsor = bevetelekKiadasokAdatsor.Sum(x => x.Value);
            var totalKotelezetsegekKovetelesekAdatsor = kotelezetsegekKovetelesekAdatsor.Sum(x => x.Value);
            var totalSum = totalBevetelekKiadasokAdatsor + totalKotelezetsegekKovetelesekAdatsor;

            Series = new SeriesCollection();

            // Add Chrome series
            //AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalSum, "Bevételek és Kiadások", Brushes.Blue);

            // Add Firefox series
            //AddPieSeries(kotelezetsegekKovetelesekAdatsor, totalKotelezetsegekKovetelesekAdatsor, totalSum, "Kötelezettségek és Követelések", Brushes.Red);

            OnPropertyChanged(nameof(Series));
        }

        public void SetDoghnutSeries()
        {
            //NORMAL SETTING STARTS
            var kotelezetsegekKovetelesekAdatsor = new ChartValues<ObservableValue>();

            //foreach (var a in _selectedBevetelekKiadasok)
            //{
            //    bevetelekKiadasokAdatsor.Add(new ObservableValue(a.Osszeg));
            //}

            //foreach (var a in _selectedKotelezettsegekKovetelesek)
            //{
            //    kotelezetsegekKovetelesekAdatsor.Add(new ObservableValue(a.Osszeg));
            //}

            //var totalKotelezetsegekKovetelesekAdatsor = kotelezetsegekKovetelesekAdatsor.Sum(x => x.Value);
            //var totalSum = totalBevetelekKiadasokAdatsor + totalKotelezetsegekKovetelesekAdatsor;

            Series = new SeriesCollection();

            // Add Chrome series
            //AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalSum, "Bevételek és Kiadások", baseColors[0]);

            //// Add Firefox series
            //AddPieSeries(kotelezetsegekKovetelesekAdatsor, totalKotelezetsegekKovetelesekAdatsor, totalSum, "Kötelezettségek és Követelések", baseColors[1]);
            //NORMAL SETTING ENDS

            //Grouping by Penznem (Currency) STARTS

            var groupedByPenznem = _selectedBevetelekKiadasok.GroupBy(p => new {p.Penznem, p.BeKiKod});

            // Create a dictionary to hold the HashSet for each group
            var hashSetsByPenznem = new Dictionary<(Penznem Penznem, BeKiKod BeKiKod), HashSet<BevetelKiadas>>();

            foreach (var group in groupedByPenznem)
            {
                // Create a HashSet for each group
                var hashSet = new HashSet<BevetelKiadas>(group);
                hashSetsByPenznem[(group.Key.Penznem, group.Key.BeKiKod)] = hashSet;
            }

            // Display the results
            foreach (var kvp in hashSetsByPenznem)
            {
                var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                int a = 0;
                foreach (var bevetelKiadas in kvp.Value)
                {
                    bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                }
                var totalBevetelekKiadasokAdatsor = bevetelekKiadasokAdatsor.Sum(x => x.Value);
                AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem} + {kvp.Key.BeKiKod}", baseColors[a]);
                if (a + 1 > 3)
                    a = 0;
            }

            OnPropertyChanged(nameof(Series));
        }

        //Egy checkbox névből előállítja a hozzá tartozó oszlop nevét
        //It gives back the tablename that can be taken from the checkbox's name
        //private string GetTableName(string chName)
        //{
        //    string[] t = chName.Split('_');
            
        //    string tableName = t[0];
        //    if (!t[1].Contains("ChB"))
        //        tableName += "_" + t[1];
        //    return tableName;
        //}

        //Ha egy checkbox meglett nyomva, ami tartalmazza az All résszöveget, akkor annak a checkbox-nak a nevéből kinyert táblanevet tartalmazó többi checkboxot is ki pipálja
        //If a checkbox that was clicked contains the All substring then it sets all the checkboxes state to checked that holds the substring that we get from the checkbox's name
        //private void OptionsCheckBoxAllChecked(object sender, RoutedEventArgs e)
        //{
        //    if (sender is System.Windows.Controls.CheckBox checkBox)
        //    {
        //        //chartDataFilterCHB 
        //        if (checkBox.Name.Contains("All"))
        //        {
        //            AllCheckBoxChecked = true;
        //            foreach (var ch in CheckBoxes)
        //            {
        //                if (ch.Content is System.Windows.Controls.CheckBox checkbox2)
        //                {
        //                    if (checkBox.Name.Contains(checkbox2.Name.Split('_')[0]))
        //                    {
        //                        checkbox2.IsChecked = true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //Ha a checkbox, ami meglett nyomva tartalmazza az All résszöveget, akkor az AllCheckBoxChecked értékét false-re állítja
        //If the checkbox that was clicked contains the All substring then the AllCheckBoxChecked variable is set to false
        //private void OptionsCheckBoxAllUnchecked(object sender, RoutedEventArgs e)
        //{
        //    if (sender is System.Windows.Controls.CheckBox checkBox)
        //    {
        //        if (checkBox.Name.Contains("All"))
        //        {
        //            AllCheckBoxChecked = false;
        //        }
        //    }
        //}

        //private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    var dataGrid = sender as System.Windows.Controls.DataGrid;
        //    var column = e.Column;
        //    var columnName = e.PropertyName;

        //    var template = new DataTemplate();

        //    var factory = new FrameworkElementFactory(typeof(StackPanel));
        //    factory.SetValue(StackPanel.OrientationProperty, System.Windows.Controls.Orientation.Horizontal);

        //    var checkBoxFactory = new FrameworkElementFactory(typeof(System.Windows.Controls.CheckBox));
        //    checkBoxFactory.SetBinding(System.Windows.Controls.CheckBox.IsCheckedProperty, new Binding($"IsSelected{columnName}"));

        //    var textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
        //    textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding(columnName));
        //    textBlockFactory.SetValue(TextBlock.MarginProperty, new Thickness(5, 0, 0, 0));

        //    factory.AppendChild(checkBoxFactory);
        //    factory.AppendChild(textBlockFactory);

        //    template.VisualTree = factory;

        //    var templateColumn = new DataGridTemplateColumn
        //    {
        //        Header = column.Header,
        //        CellTemplate = template
        //    };

        //    e.Column = templateColumn;
        //}

        //Megváltoztatja a bool értéket a chartDataFilterCHB szótárban, annak megfelelően, hogy ki vagy be lett pipálva a checkbox
        //Changes the bool value in the chartDataFilterCHB dictionary depending on the chechbox changes (Checked/Unchecked)
        //private void OptionsCheckboxCheckedChange(object sender, RoutedEventArgs e)
        //{
        //    if (sender is System.Windows.Controls.CheckBox checkBox)
        //    {
        //        string tableName = GetTableName(checkBox.Name);

        //        if ((bool)checkBox.IsChecked)
        //        {
        //            System.Windows.Controls.TabItem tabItem = null;
        //            switch (tableName)
        //            {
                        //case "dolgozok":
                        //    tabItem = CreateTabItem("Dolgozok", new ObservableCollection<object>(dolgozoRepository.GetDolgozok().Cast<object>().ToList()), tableName, checkBox.Name);
                        //    if (tabItem != null)
                        //    {
                        //        Tabs.Add(tabItem);
                        //    }
                        //    else
                        //    {
                        //        SetColumnByCheckBox(tableName, checkBox.Name, true);
                        //    }
                        //    break;
                        //case "gazdalkodo_szervezetek":
                        //    tabItem = CreateTabItem("GazdalkodoSzervezetek", new ObservableCollection<object>(gazdalkodoSzervezetRepository.GetGazdalkodoSzervezetek().Cast<object>().ToList()), tableName, checkBox.Name);
                        //    if (tabItem != null)
                        //    {
                        //        Tabs.Add(tabItem);
                        //    }
                        //    else
                        //    {
                        //        SetColumnByCheckBox(tableName, checkBox.Name, true);
                        //    }
                        //    break;
                        //case "bevetelek_kiadasok":
                        //    tabItem = CreateTabItem("Koltsegvetes", new ObservableCollection<object>(koltsegvetesRepository.GetKoltsegvetesek().Cast<object>().ToList()), tableName, checkBox.Name);
                        //    if (tabItem != null)
                        //    {
                        //        Tabs.Add(tabItem);
                        //    }
                        //    else
                        //    {
                        //        SetColumnByCheckBox(tableName, checkBox.Name, true);
                        //    }
                        //    break;
                        //case "kotelezettsegek_kovetelesek":
                        //    tabItem = CreateTabItem("KotelKovetelesek", new ObservableCollection<object>(kotelezettsegKovetelesRepository.GetKotelezettsegekKovetelesek().Cast<object>().ToList()), tableName, checkBox.Name);
                        //    if (tabItem != null)
                        //    {
                        //        Tabs.Add(tabItem);
                        //    }
                        //    else
                        //    {
                        //        SetColumnByCheckBox(tableName, checkBox.Name, true);
                        //    }
                        //    break;
                        //case "magan_szemelyek":
                        //    tabItem = CreateTabItem("MaganSzemelyek", new ObservableCollection<object>(maganSzemelyRepository.GetMaganSzemelyek().Cast<object>().ToList()), tableName, checkBox.Name);
                        //    if (tabItem != null)
                        //    {
                        //        Tabs.Add(tabItem);
                        //    }
                        //    else
                        //    {
                        //        SetColumnByCheckBox(tableName, checkBox.Name, true);
                        //    }
                        //    break;
        //                default: break;
        //            }
        //            OnPropertyChanged(nameof(Tabs));
        //        }
        //        else
        //        {
        //            SetColumnByCheckBox(tableName, checkBox.Name, false);
        //        }
        //    }
        //}

        //Miután a datagrid létre lett hozva, attól függően, hogy melyik checkbox hatására jött lére beállítja az elrejteni kívánt oszlopokat
        //After the datagrid is created, depending on the click checkbox the columns that shouldn't be seen are hidden
        //private void HideColumnsAfterCreatingTabItem(System.Windows.Controls.DataGrid dataGrid, string chName)
        //{
        //    if (!AllCheckBoxChecked)
        //    {
        //        foreach (var c in dataGrid.Columns)
        //        {
        //            if (c.Header.ToString().ToLower() != GetCheckBoxColumnName(chName))
        //            {
        //                c.Visibility = Visibility.Collapsed;
        //            }
        //        }
        //    }
        //    AllCheckBoxChecked = false;
        //}

        //Beállítja annak a checkbox állapotát nem kijelölt-re, amelyik _All-ra végződik és a chName-ből szerzett tábla nevet is tartalmazza
        //Sets the checkbox to unchecked if the checkbox's name contains the _All substring and the tablename substring that is coming from the chName
        //private void UncheckAllTypeCheckBox(string chName)
        //{
        //    string tableName = GetTableName(chName);
        //    foreach(var a in CheckBoxes)
        //    {
        //        if(a.Content is System.Windows.Controls.CheckBox checkBox)
        //        {
        //            if(checkBox.Name == tableName + "_All")
        //            {
        //                checkBox.IsChecked = false; 
        //                break;
        //            }
        //        }
        //    }
        //}

        //Létrehoz egy TabItem-et, aminek a tartalma egy datagrid lesz, ez lesz megjelenítve a felhasználónak
        //Created a TabItem that contains a datagrid, this will be shown to the user on the UI
        private System.Windows.Controls.TabItem CreateTabItem(string header, bool bevetel, string table)
        {
            //System.Windows.Controls.TabControl tabControl = Mediator.NotifyGetTabControl();

            //var existingTab = tabControl.Items.OfType<System.Windows.Controls.TabItem>().FirstOrDefault(t => t.Header.ToString() == header);

            //if (existingTab != null)
            //{
            //    // Bring the existing tab to the front
            //    tabControl.SelectedItem = existingTab;
            //    return null;
            //}
            System.Windows.Controls.DataGrid dataGrid;
            if (bevetel)
            {
                dataGrid = new System.Windows.Controls.DataGrid
                {
                    Name = table,
                    CanUserSortColumns = false,
                    AutoGenerateColumns = true,
                    SelectionUnit = DataGridSelectionUnit.FullRow,
                    IsReadOnly = true,
                    CanUserResizeColumns = false,
                    ColumnWidth = DataGridLength.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                };
                var itemsSourceBinding = new System.Windows.Data.Binding("FilteredBevetelekKiadasok")
                {
                    Source = this,  // your ViewModel instance
                    Mode = BindingMode.TwoWay, // or OneWay if needed
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                // Set the binding to the ItemsSource property
                dataGrid.SetBinding(System.Windows.Controls.DataGrid.ItemsSourceProperty, itemsSourceBinding);
            }
            else
            {
                dataGrid = new System.Windows.Controls.DataGrid
                {
                    Name = table,
                    ItemsSource = FilteredKotelKovetelesek,
                    CanUserSortColumns = false,
                    AutoGenerateColumns = true,
                    SelectionUnit = DataGridSelectionUnit.FullRow,
                    IsReadOnly = true,
                    CanUserResizeColumns = false,
                    ColumnWidth = DataGridLength.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                };
            }
            

            //dataGrid.AutoGeneratedColumns += (sender, e) =>
            //{
            //    HideColumnsAfterCreatingTabItem(dataGrid, chName);
            //};
            var tabItem = new System.Windows.Controls.TabItem
            {
                Header = header,
                Content = dataGrid,
                Name = "Tab_" + header,
                Margin = new Thickness(0.0),
                Padding = new Thickness(0.0),
            };
            return tabItem;
        }

        //Visszaadja azt a datagrid-et a Tabs listából, aminek megegyezik a neve az paraméterben kapottal
        //Gives back a datagrid from the Tabs collection which name is equal to string given in the parameter
        private System.Windows.Controls.DataGrid GetDataGrid(string dataGridName)
        {
            System.Windows.Controls.DataGrid grid = null;
            foreach (var o in Tabs)
            {
                if (o.Content is System.Windows.Controls.DataGrid dataGrid)
                    if (dataGrid.Name == dataGridName)
                    {
                        grid = dataGrid;
                        break;
                    }
            }
            return grid;
        }

        //Beállítja az adott oszlop láthatóságát a hozzá tartozó checkbox állapota szerint
        //Sets the column visibility by the corresponding checkbox's state
        //private void SetColumnByCheckBox(string dataGridName, string chName, bool isChecked)
        //{
        //    System.Windows.Controls.DataGrid dataGrid = GetDataGrid(dataGridName); 
        //    if(dataGrid != null)
        //    {
        //        foreach (var column in dataGrid.Columns)
        //        {
        //            if (column.Header != null)
        //            {
        //                if (column.Header.ToString().ToLower() == GetCheckBoxColumnName(chName))
        //                {
        //                    if (!isChecked)
        //                    {
        //                        column.Visibility = Visibility.Collapsed;
        //                        UncheckAllTypeCheckBox(chName);
        //                    }
        //                    else
        //                    {
        //                        column.Visibility = Visibility.Visible;
        //                    }
        //                    break;
        //                }
        //            }
        //        }
        //        if (GetCollapsedColumnNumberInDataGrid(dataGrid) == 0 && !isChecked)
        //        {
        //            DeleteDataGrid(dataGridName);
        //            OnPropertyChanged(nameof(Tabs));
        //        }
        //    }
        //}

        //Visszaadja, hogy mennyi oszlop van elrejtve az adott datagrid-ben
        //Give back how many collapsed columns are in the given datagrid
        //private int GetCollapsedColumnNumberInDataGrid(System.Windows.Controls.DataGrid dataGrid)
        //{
        //    int counter = 0;
        //    foreach (var column in dataGrid.Columns)
        //    {
        //        if (column.Visibility != Visibility.Collapsed)
        //            counter++;
        //    }
        //    return counter;
        //}

        //Visszaadja az addot checkbox név-ből az oszlop nevet
        //Gives back the column name from the given checkbox name
        //private string GetCheckBoxColumnName(string chName)
        //{
        //    string tmp;
        //    string tmp2;
        //    string tmp3 = "";
        //    if (chName.Contains(GetTableName(chName)))
        //    {
        //        tmp = chName.Replace(GetTableName(chName) + "_", "");
        //        if (tmp.Contains("ChB"))
        //        {
        //            tmp2 = tmp.Replace("ChB", "");
        //            tmp3 = tmp2.Replace("_", "");
        //        }
        //    }
        //    return tmp3.ToLower();
        //}


        //Törli a megadott nevű datagrid-et
        //Deletes the datagrid that's name is the given in the parameter
        //private void DeleteDataGrid(string dataGridName)
        //{
        //    foreach (var o in Tabs)
        //    {
        //        if (o.Content is System.Windows.Controls.DataGrid dataGrid)
        //            if (dataGrid.Name == dataGridName)
        //            {
        //                Tabs.Remove(o);
        //                if(Tabs.Count == 0)
        //                    Tabs.Clear();
        //                break;
        //            }
        //    }
        //}

        private void GetSelectedCells()
        {
            foreach (var o in selectedDataGridCells)
            {
                if (o.Content is TextBlock textBox)
                {
                    System.Windows.MessageBox.Show(textBox.Text.ToString());
                }
            }
        }
        private void AddPieSeries(ChartValues<ObservableValue> values, double categoryTotal, double totalSum, string title, Brush baseColor)
        {
            //Every value is a PieSeries not while grouping
            foreach (var value in values)
                {
                    Series.Add(new PieSeries
                    {
                        Title = title,
                        Values = new ChartValues<ObservableValue> { value },
                        DataLabels = true,
                        LabelPoint = chartPoint => $"{value.Value} ({(value.Value / totalSum):P})",
                        Fill = baseColor
                    });
            }


            //Grouping by Penznem (Currency)
            //Series.Add(new PieSeries
            //{
            //    Title = title,
            //    Values = new ChartValues<ObservableValue> { new ObservableValue(values.Sum(x => x.Value)) },
            //    DataLabels = true,
            //    LabelPoint = chartPoint => $"{totalSum}",
            //    Fill = baseColor
            //});


            LabelFormatter = chartPoint => $"{chartPoint.Y} ({chartPoint.Participation:P})";

            OnPropertyChanged(nameof(LabelFormatter));
        }

        private void AddLineSeries(ChartValues<double> asd, double categoryTotal, double totalSum, string title, string name, SolidColorBrush baseColor)
        {

            LineSeries.Add(new LineSeries
            {
                Name = name,
                Title = title,
                Values = asd,
                DataLabels = true,
                PointForeground = baseColor,
                Stroke = baseColor
            });

            LineSeriesLabels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            LineSeriesYFormatter = value => value.ToString("C");

            OnPropertyChanged(nameof(LineSeries));
            OnPropertyChanged(nameof(LineSeriesLabels));
            OnPropertyChanged(nameof(LineSeriesYFormatter));
        }

        private void ExecuteShowSelectDataForNewChartViewCommand(object obj)
        {
        }
        private void ExecuteShowAddOptionToNewChartViewCommand(object obj)
        {
            CurrentChildView = new AddOptionsToNewChartViewModel(SelectedRows);
        }


        //lekérni az adattábla oszlop neveit PIPA
        //típusonkénti kezelés:
        //string
        //bevetelek_kiadasok: id, be_ki_kod, kotel_kovet_id, partner_id
        //double
        //bevetelek_kiadasok: osszeg
        //boolean
        //bevetelek_kiadasok: penznem (kinda)
        //date
        //bevetelek_kiadasok: teljesitesi_datum
        //lehetőségek az értékekkel:
        //szűrés - számok szűrése
        //adatsorok létrehozása(csoportosítása oszlopok szerint) - ez esetben pl dátum, az előforduló dátumok megadása, felhasználó kiválasztja az általa kívántakat és azok alapján adatsorok kreálása
        //boolean értékkel szűrni és csoportosítani is lehetne(akár double és string alapján is)
        //kisablak annak a gecis módosításnak és hozzáadásnak PIPA

    }
}
