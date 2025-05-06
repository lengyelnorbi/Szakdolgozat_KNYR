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
using Szakdolgozat.Views;
using System.Xml.Linq;
using Szakdolgozat.Specials;
using System.Runtime.InteropServices.ComTypes;
using Org.BouncyCastle.Bcpg;
using LiveCharts.Configurations;
using Xceed.Wpf.Toolkit.Panels;
using System.IO;
using System.Windows.Media.Imaging;

namespace Szakdolgozat.ViewModels
{
    //Új diagramm készítésekor megnyitott ablak viewmodelje.
    //The window's viewmodel that shows when creating a new diagram.
    public class CreateChartsViewModel : ViewModelBase
    {
        public RoutedEventHandler _selectionChanged = null;
        public RoutedEventHandler _selectionDeleted = null;
        public RoutedEventHandler _deleteAllSelections = null;

        private string _selectedCimke = "";
        public string SelectedCimke
        {
            get { return _selectedCimke; }
            set
            {
                _selectedCimke = value;
                OnPropertyChanged(nameof(SelectedCimke));
            }
        }   

        private string _selectedAdatsor = "";
        public string SelectedAdatsor
        {
            get { return _selectedAdatsor; }
            set
            {
                _selectedAdatsor = value;
                OnPropertyChanged(nameof(SelectedAdatsor));
            }
        }

        public bool _groupByPenznemCheckBoxIsChecked = false;
        public bool _groupByBeKiKodCheckBoxIsChecked = false;
        public bool _groupByKifizetettCheckBoxIsChecked = false;
        public bool _groupByMonthCheckBoxIsChecked = false;
        public bool _groupByYearCheckBoxIsChecked = false;
        public bool GroupByPenznemCheckBoxIsChecked
        {
            get { return _groupByPenznemCheckBoxIsChecked; }
            set
            {
                _groupByPenznemCheckBoxIsChecked = value;
                OnPropertyChanged(nameof(GroupByPenznemCheckBoxIsChecked));
                if (value)
                    SelectedCimkek.Add("Pénznem");
                else
                {
                    if (SelectedCimkek.Contains("Pénznem"))
                        SelectedCimkek.Remove("Pénznem");
                    else if (SelectedAdatsorok.Contains("Pénznem"))
                        SelectedAdatsorok.Remove("Pénznem");
                }
            }
        }
        public bool GroupByBeKiKodCheckBoxIsChecked
        {
            get { return _groupByBeKiKodCheckBoxIsChecked; }
            set
            {
                _groupByBeKiKodCheckBoxIsChecked = value;
                OnPropertyChanged(nameof(GroupByBeKiKodCheckBoxIsChecked));
                if (value)
                    SelectedCimkek.Add("BeKiKód");
                else
                {
                    if (SelectedCimkek.Contains("BeKiKód"))
                        SelectedCimkek.Remove("BeKiKód");
                    else if (SelectedAdatsorok.Contains("BeKiKód"))
                        SelectedAdatsorok.Remove("BeKiKód");
                }
            }
        }
        public bool GroupByKifizetettCheckBoxIsChecked
        {
            get { return _groupByKifizetettCheckBoxIsChecked; }
            set
            {
                _groupByKifizetettCheckBoxIsChecked = value;
                OnPropertyChanged(nameof(GroupByKifizetettCheckBoxIsChecked));
                if (value)
                    SelectedCimkek.Add("Kifizetett");
                else
                {
                    if (SelectedCimkek.Contains("Kifizetett"))
                        SelectedCimkek.Remove("Kifizetett");
                    else if (SelectedAdatsorok.Contains("Kifizetett"))
                        SelectedAdatsorok.Remove("Kifizetett");
                }
            }
        }
        public bool GroupByMonthCheckBoxIsChecked
        {
            get { return _groupByMonthCheckBoxIsChecked; }
            set
            {
                _groupByMonthCheckBoxIsChecked = value;
                OnPropertyChanged(nameof(GroupByMonthCheckBoxIsChecked));
                if (value)
                    if(SelectedCimkek.Contains("Év"))
                        SelectedAdatsorok.Add("Hónap");
                    else
                        SelectedCimkek.Add("Hónap");
                else
                {
                    if (SelectedCimkek.Contains("Hónap"))
                        SelectedCimkek.Remove("Hónap");
                    else if (SelectedAdatsorok.Contains("Hónap"))
                        SelectedAdatsorok.Remove("Hónap");
                }
            }
        }
        public bool GroupByYearCheckBoxIsChecked
        {
            get { return _groupByYearCheckBoxIsChecked; }
            set
            {
                _groupByYearCheckBoxIsChecked = value;
                OnPropertyChanged(nameof(GroupByYearCheckBoxIsChecked));
                if (value)
                    if (SelectedCimkek.Contains("Hónap"))
                        SelectedAdatsorok.Add("Év");
                    else
                        SelectedCimkek.Add("Év");
                else
                {
                    if (SelectedCimkek.Contains("Év"))
                        SelectedCimkek.Remove("Év");
                    else if (SelectedAdatsorok.Contains("Év"))
                        SelectedAdatsorok.Remove("Év");
                }
            }
        }

        public bool IsBevetelekKiadasokTabIsSelected = true;
        public bool _groupByDateCheckBoxIsChecked = false;
        public bool GroupByDateCheckBoxIsChecked
        {
            get { return _groupByDateCheckBoxIsChecked; }
            set
            {
                _groupByDateCheckBoxIsChecked = value;
                OnPropertyChanged(nameof(GroupByDateCheckBoxIsChecked));
                if(value)
                    SelectedCimkek.Add("Dátum");
                else
                {
                    if (SelectedCimkek.Contains("Dátum"))
                        SelectedCimkek.Remove("Dátum");
                    else if (SelectedAdatsorok.Contains("Dátum"))
                        SelectedAdatsorok.Remove("Dátum");
                }
            }
        }
        public bool _isValidStartDateExists = false;
        public bool _isValidEndDateExists = false;

        private ObservableCollection<string> _selectedCimkek = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedCimkek
        {
            get { return _selectedCimkek; }
            set
            {
                _selectedCimkek = value;
                OnPropertyChanged(nameof(SelectedCimkek));
            }
        }

        private ObservableCollection<string> _selectedAdatsorok = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedAdatsorok
        {
            get { return _selectedAdatsorok; }
            set
            {
                _selectedAdatsorok = value;
                OnPropertyChanged(nameof(SelectedAdatsorok));
            }
        }

        private string _selectedDataStatistics = "Nincs Kiválasztva";
        public string SelectedDataStatistics
        {
            get => _selectedDataStatistics;
            set
            {
                _selectedDataStatistics = value;
                OnPropertyChanged(nameof(SelectedDataStatistics));
            }
        }

        private ObservableCollection<string> _dataStatisticsCB = new ObservableCollection<string> { "Nincs Kiválasztva", "Átlag", "Összeg", "Értékek Szórása", "Mértani Közép", "Minimum Érték", "Maximum Érték" };
        public ObservableCollection<string> DataStatisticsCB
        {
            get => _dataStatisticsCB;
            set 
            {
                _dataStatisticsCB = value;
                OnPropertyChanged(nameof(DataStatisticsCB));
            }
        }

        private double _innerRadius;

        // Property to control the inner radius of PieSeries
        public double InnerRadius
        {
            get => _innerRadius;
            set
            {
                _innerRadius = value;
                OnPropertyChanged(nameof(InnerRadius));
            }
        }


        private ObservableCollection<int> _years = new ObservableCollection<int>();
        public ObservableCollection<int> Years
        {
            get
            {
                return _years;
            }
            set
            {
                _years = value;
                OnPropertyChanged(nameof(Years));
            }
        }
        private int _selectedYear = 0;
        public int SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
            }
        }
        public bool IsValidStartDateExists
        {
            get 
            {
                return _isValidStartDateExists; 
            }
            set 
            {
                _isValidStartDateExists = value;
                UpdateSearch(SearchQuery);
                OnPropertyChanged(nameof(IsValidStartDateExists));
            }
        }
        public bool IsValidEndDateExists
        {
            get
            {
                return _isValidEndDateExists;
            }
            set
            {
                _isValidEndDateExists = value;
                UpdateSearch(SearchQuery);
                OnPropertyChanged(nameof(IsValidEndDateExists));
            }
        }

        public string _startingDate;
        public string StartingDate
        {
            get
            {
                return _startingDate;
            }
            set 
            {
                if(!value.Any(char.IsLetter) && value != _startingDate)
                {
                    _startingDate = value;
                    IsValidStartDateExists = IsValidDate(value, true);
                }
                OnPropertyChanged(nameof(StartingDate));
            }
        }
        public string _endDate;
        public string EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                if (!value.Any(char.IsLetter) && value != _endDate)
                {
                    _endDate = value;
                    IsValidEndDateExists = IsValidDate(value, false);
                }
                OnPropertyChanged(nameof(EndDate));
            }
        }

        SolidColorBrush[] baseColors = new SolidColorBrush[] { Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Magenta };
        public SeriesCollection PieSeries { get; set; }
        public SeriesCollection Series { get; set; }
        public Func<ChartPoint, string> LabelFormatter { get; set; }

        public SeriesCollection LineSeries { get; set; }
        public string[] LineSeriesLabels { get; set; }
        public Func<double, string> LineSeriesYFormatter { get; set; }
        public SeriesCollection RowSeries { get; set; }
        public List<string> RowSeriesLabels { get; set; }
        public Func<double, string> RowSeriesFormatter { get; set; }
        public SeriesCollection ColumnSeries { get; set; }
        public List<string> ColumnSeriesLabels { get; set; }
        public Func<double, string> ColumnSeriesFormatter { get; set; }

        public SeriesCollection StackedColumnSeries { get; set; }
        public List<string> StackedColumnSeriesLabels { get; set; }
        public Func<double, string> StackedColumnSeriesFormatter { get; set; }

        public event Action checkboxChange;

        public List<System.Windows.Controls.DataGridCell> selectedDataGridCells = new List<System.Windows.Controls.DataGridCell>();

        public ObservableCollection<KotelezettsegKoveteles> _selectedKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>();
        public ObservableCollection<KotelezettsegKoveteles> SelectedKotelezettsegekKovetelesek
        {
            get
            {
                return _selectedKotelezettsegekKovetelesek;
            }
            set
            {
                _selectedKotelezettsegekKovetelesek = value;
                OnPropertyChanged(nameof(SelectedKotelezettsegekKovetelesek));

            }
        }

        public ObservableCollection<BevetelKiadas> _selectedBevetelekKiadasok = new ObservableCollection<BevetelKiadas>();
        public ObservableCollection<BevetelKiadas> SelectedBevetelekKiadasok
        {
            get
            {
                return _selectedBevetelekKiadasok;
            }
            set
            {
                _selectedBevetelekKiadasok = value;
                OnPropertyChanged(nameof(SelectedBevetelekKiadasok));
            }
        }

        private bool IsValidDate(string date, bool IsStartDate)
        {
            DateTime? parsedDate = DateTimeParser.ParseDateTime(date);

            if (parsedDate.HasValue)
            {
                if(IsStartDate)
                {
                    StartingDate = parsedDate.Value.Date.ToString("yyyy.MM.dd");
                }
                else
                {
                    EndDate = parsedDate.Value.Date.ToString("yyyy.MM.dd");
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void IsEnabledChangerOnTabItems()
        {
            if (SelectedBevetelekKiadasok.Count > 0 || SelectedKotelezettsegekKovetelesek.Count > 0)
            {
                System.Windows.Controls.TabControl tabControl = Mediator.NotifyGetTabControl();
                if (SelectedBevetelekKiadasok.Count > 0)
                {
                    foreach (TabItem tItem in tabControl.Items)
                    {
                        if (tItem.Name.Contains("KotelKovet"))
                        {
                            tItem.IsEnabled = false;
                            break;
                        }
                    }
                }
                else if (SelectedKotelezettsegekKovetelesek.Count > 0)
                {
                    foreach (TabItem tItem in tabControl.Items)
                    {
                        if (tItem.Name.Contains("Koltsegvetes"))
                        {
                            tItem.IsEnabled = false;
                            break;
                        }
                    }
                }
            }
            else if (SelectedBevetelekKiadasok.Count == 0 && SelectedKotelezettsegekKovetelesek.Count == 0)
            {
                System.Windows.Controls.TabControl tabControl = Mediator.NotifyGetTabControl();
                foreach (TabItem tItem in tabControl.Items)
                {
                    tItem.IsEnabled = true;
                }
            }
        }


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
                switch (value)
                {
                    case "DoughnutSeries":
                        DataStatisticsCB = new ObservableCollection<string> { "Összeg", "Átlag", "Értékek Szórása", "Mértani Közép", "Minimum Érték", "Maximum Érték" };
                        SelectedDataStatistics = "Összeg";
                        break;
                    case "RowSeries":
                        DataStatisticsCB = new ObservableCollection<string> { "Összeg", "Átlag", "Értékek Szórása", "Mértani Közép", "Minimum Érték", "Maximum Érték" };
                        SelectedDataStatistics = "Összeg";
                        break;
                    case "LineSeries":
                        DataStatisticsCB = new ObservableCollection<string> { "Összeg", "Átlag", "Értékek Szórása", "Mértani Közép", "Minimum Érték", "Maximum Érték" };
                        SelectedDataStatistics = "Összeg";
                        break;
                    case "StackedColumnSeries":
                        DataStatisticsCB = new ObservableCollection<string> { "Nincs Kiválasztva", "Összeg", "Átlag", "Értékek Szórása", "Mértani Közép", "Minimum Érték", "Maximum Érték" };
                        break;
                    case "BasicColumnSeries":
                        DataStatisticsCB = new ObservableCollection<string> { "Összeg", "Átlag", "Értékek Szórása", "Mértani Közép", "Minimum Érték", "Maximum Érték" };
                        SelectedDataStatistics = "Összeg";
                        break;
                    default:
                        break;
                }
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
        public ICommand AddGroupByToAdatsorokCommand { get; }
        public ICommand AddGroupByToCimkekCommand { get; }
        public ICommand ExportChartAsImageCommand { get; }

        public CreateChartsViewModel()
        {
            ////ANNYIRA MÁR NEM FONTOS
            //var mapper = Mappers.Xy<CurrencyData>()
            //.X((val, index) => val.Value)
            //.Y(val => val.Position); // Controls row placement

            //Charting.For<CurrencyData>(mapper);
            ////

            checkboxStatuses.Add("koltsegvetes_mindCB", true);
            checkboxStatuses.Add("koltsegvetes_idCB", true);
            checkboxStatuses.Add("koltsegvetes_osszegCB", true);
            checkboxStatuses.Add("koltsegvetes_penznemCB", true);
            checkboxStatuses.Add("koltsegvetes_bekikodCB", true);
            checkboxStatuses.Add("koltsegvetes_teljesitesiDatumCB", true);
            checkboxStatuses.Add("koltsegvetes_kotelKovetIDCB", true);
            checkboxStatuses.Add("koltsegvetes_partnerIDCB", true);

            checkboxStatuses.Add("kotelKovet_mindCB", true);
            checkboxStatuses.Add("kotelKovet_idCB", true);
            checkboxStatuses.Add("kotelKovet_osszegCB", true);
            checkboxStatuses.Add("kotelKovet_penznemCB", true);
            checkboxStatuses.Add("kotelKovet_tipusCB", true);
            checkboxStatuses.Add("kotelKovet_kifizetesHataridejeCB", true);
            checkboxStatuses.Add("kotelKovet_kifizetettCB", true);

            //nem biztos hogy kell innen
            Mediator.SelectedRowsChangedOnChildView += UpdateSelectedRows;
            Mediator.DataRequest += ReturnRequestedData;

            ShowSelectDataForNewChartViewCommand = new ViewModelCommand(ExecuteShowSelectDataForNewChartViewCommand);
            ShowAddOptionToNewChartViewCommand = new ViewModelCommand(ExecuteShowAddOptionToNewChartViewCommand);
            AddGroupByToAdatsorokCommand = new ViewModelCommand(ExecuteAddGroupByToAdatsorokCommand);
            AddGroupByToCimkekCommand = new ViewModelCommand(ExecuteAddGroupByToCimkekCommand);
            ExportChartAsImageCommand = new ViewModelCommand(ExecuteExportChartAsImageCommand);

            //idáig


            Mediator.HideOrShowLineSeries += HideOrShowLineSeriesBySelection;
            Mediator.SetLineSeriesNewColor += SetLineSeriesNewColor;

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
        }

        public void CloseWindow(object sender, EventArgs e)
        {
            Mediator.HideOrShowLineSeries -= HideOrShowLineSeriesBySelection;
            Mediator.SetLineSeriesNewColor -= SetLineSeriesNewColor;
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
                            if (checkboxStatuses["koltsegvetes_idCB"] == true)
                            {
                                if (d.ID.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredBevetelekKiadasok.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["koltsegvetes_osszegCB"] == true)
                            {
                                if (d.Osszeg.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredBevetelekKiadasok.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["koltsegvetes_penznemCB"] == true)
                            {
                                if (d.Penznem.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredBevetelekKiadasok.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["koltsegvetes_bekikodCB"] == true)
                            {
                                if (d.BeKiKod.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredBevetelekKiadasok.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["koltsegvetes_teljesitesiDatumCB"] == true)
                            {
                                if (d.TeljesitesiDatum.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredBevetelekKiadasok.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["koltsegvetes_kotelKovetIDCB"] == true)
                            {
                                if (d.KotelKovetID.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredBevetelekKiadasok.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["koltsegvetes_partnerIDCB"] == true)
                            {
                                if (d.PartnerID.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredBevetelekKiadasok.Add(d);
                                    continue;
                                }
                            }
                        }
                        if(IsValidStartDateExists && IsValidEndDateExists)
                        {
                            for (int i = FilteredBevetelekKiadasok.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredBevetelekKiadasok[i];
                                if (!(item.TeljesitesiDatum.Date >= DateTimeParser.ParseDateTime(StartingDate) && item.TeljesitesiDatum.Date <= DateTimeParser.ParseDateTime(EndDate)))
                                {
                                    FilteredBevetelekKiadasok.Remove(item);
                                }
                            }
                        }
                        else if (IsValidStartDateExists)
                        {
                            for (int i = FilteredBevetelekKiadasok.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredBevetelekKiadasok[i];
                                if (!(item.TeljesitesiDatum.Date >= DateTimeParser.ParseDateTime(StartingDate)))
                                {
                                    FilteredBevetelekKiadasok.Remove(item);
                                }
                            }
                        }
                        else if (IsValidEndDateExists)
                        {
                            for (int i = FilteredBevetelekKiadasok.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredBevetelekKiadasok[i];
                                if (!(item.TeljesitesiDatum.Date <= DateTimeParser.ParseDateTime(EndDate)))
                                {
                                    FilteredBevetelekKiadasok.Remove(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        // If the search query is empty, reset to the original data
                        FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(BevetelekKiadasok);
                        if (IsValidStartDateExists && IsValidEndDateExists)
                        {
                            for (int i = FilteredBevetelekKiadasok.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredBevetelekKiadasok[i];
                                if (!(item.TeljesitesiDatum.Date >= DateTimeParser.ParseDateTime(StartingDate) && item.TeljesitesiDatum.Date <= DateTimeParser.ParseDateTime(EndDate)))
                                {
                                    FilteredBevetelekKiadasok.Remove(item);
                                }
                            }
                        }
                        else if (IsValidStartDateExists)
                        {
                            for (int i = FilteredBevetelekKiadasok.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredBevetelekKiadasok[i];
                                if (!(item.TeljesitesiDatum.Date >= DateTimeParser.ParseDateTime(StartingDate)))
                                {
                                    FilteredBevetelekKiadasok.Remove(item);
                                }
                            }
                        }
                        else if (IsValidEndDateExists)
                        {
                            for (int i = FilteredBevetelekKiadasok.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredBevetelekKiadasok[i];
                                if (!(item.TeljesitesiDatum.Date <= DateTimeParser.ParseDateTime(EndDate)))
                                {
                                    FilteredBevetelekKiadasok.Remove(item);
                                }
                            }
                        }
                        OnPropertyChanged(nameof(Tabs));
                    }
                }
                if (tabItem.Name.Contains("KotelKovet"))
                {
                    if (!string.IsNullOrWhiteSpace(searchQuery) && !string.IsNullOrEmpty(searchQuery))
                    {
                        FilteredKotelKovetelesek.Clear();
                        foreach (var d in KotelKovetelesek)
                        {
                            if (checkboxStatuses["kotelKovet_mindCB"] == true)
                            {
                                if (d.ID.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredKotelKovetelesek.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["kotelKovet_osszegCB"] == true)
                            {
                                if (d.Osszeg.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredKotelKovetelesek.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["kotelKovet_penznemCB"] == true)
                            {
                                if (d.Penznem.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredKotelKovetelesek.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["kotelKovet_tipusCB"] == true)
                            {
                                if (d.Tipus.ToLower().ToString().Contains(searchQuery.ToLower()))
                                {
                                    FilteredKotelKovetelesek.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["kotelKovet_kifizetesHataridejeCB"] == true)
                            {
                                if (d.KifizetesHatarideje.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredKotelKovetelesek.Add(d);
                                    continue;
                                }
                            }
                            if (checkboxStatuses["kotelKovet_kifizetettCB"] == true)
                            {
                                if (d.Kifizetett.ToString().ToLower().Contains(searchQuery.ToLower()))
                                {
                                    FilteredKotelKovetelesek.Add(d);
                                    continue;
                                }
                            }
                        }
                        if (IsValidStartDateExists && IsValidEndDateExists)
                        {
                            for (int i = FilteredKotelKovetelesek.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredKotelKovetelesek[i];
                                if (!(item.KifizetesHatarideje.Date >= DateTimeParser.ParseDateTime(StartingDate) && item.KifizetesHatarideje.Date <= DateTimeParser.ParseDateTime(EndDate)))
                                {
                                    FilteredKotelKovetelesek.Remove(item);
                                }
                            }
                        }
                        else if (IsValidStartDateExists)
                        {
                            for (int i = FilteredKotelKovetelesek.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredKotelKovetelesek[i];
                                if (!(item.KifizetesHatarideje.Date >= DateTimeParser.ParseDateTime(StartingDate)))
                                {
                                    FilteredKotelKovetelesek.Remove(item);
                                }
                            }
                        }
                        else if (IsValidEndDateExists)
                        {
                            for (int i = FilteredKotelKovetelesek.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredKotelKovetelesek[i];
                                if (!(item.KifizetesHatarideje.Date <= DateTimeParser.ParseDateTime(EndDate)))
                                {
                                    FilteredKotelKovetelesek.Remove(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        // If the search query is empty, reset to the original data
                        FilteredKotelKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(KotelKovetelesek);
                        if (IsValidStartDateExists && IsValidEndDateExists)
                        {
                            for (int i = FilteredKotelKovetelesek.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredKotelKovetelesek[i];
                                if (!(item.KifizetesHatarideje.Date >= DateTimeParser.ParseDateTime(StartingDate) && item.KifizetesHatarideje.Date <= DateTimeParser.ParseDateTime(EndDate)))
                                {
                                    FilteredKotelKovetelesek.Remove(item);
                                }
                            }
                        }
                        else if (IsValidStartDateExists)
                        {
                            for (int i = FilteredKotelKovetelesek.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredKotelKovetelesek[i];
                                if (!(item.KifizetesHatarideje.Date >= DateTimeParser.ParseDateTime(StartingDate)))
                                {
                                    FilteredKotelKovetelesek.Remove(item);
                                }
                            }
                        }
                        else if (IsValidEndDateExists)
                        {
                            for (int i = FilteredKotelKovetelesek.Count - 1; i >= 0; i--)
                            {
                                var item = FilteredKotelKovetelesek[i];
                                if (!(item.KifizetesHatarideje.Date <= DateTimeParser.ParseDateTime(EndDate)))
                                {
                                    FilteredKotelKovetelesek.Remove(item);
                                }
                            }
                        }
                        OnPropertyChanged(nameof(Tabs));
                    }
                }
            }
        }

        public void ChangeIsSelectedAndRefreshBevetelKiadas(int[] ids, bool selectOrDeselect)
        {
            foreach (int id in ids)
            {
                for (int i = 0; i < BevetelekKiadasok.Count; i++)
                {
                    if (BevetelekKiadasok.ElementAt(i).ID == id)
                    {
                        BevetelekKiadasok.ElementAt(i).IsSelected = selectOrDeselect;
                        break;
                    }
                }
            }
            UpdateSearch(SearchQuery);
        }

        public void ChangeIsSelectedAndRefreshKotelKovet(int[] ids, bool selectOrDeselect)
        {
            foreach (int id in ids)
            {
                for (int i = 0; i < KotelKovetelesek.Count; i++)
                {
                    if (KotelKovetelesek.ElementAt(i).ID == id)
                    {
                        KotelKovetelesek.ElementAt(i).IsSelected = selectOrDeselect;
                        break;
                    }
                }
            }
            UpdateSearch(SearchQuery);
        }
        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }
        private void SetLineSeriesNewColor(string name, SolidColorBrush color)
        {
            if(SeriesType == "LineSeries")
            {
                foreach (var a in LineSeries)
                {
                    if (a is LineSeries lineSeries)
                    {
                        if (lineSeries.Name == name)
                        {
                            lineSeries.PointForeground = color;
                            lineSeries.Stroke = color;
                            OnPropertyChanged(nameof(LineSeries));
                            break;
                        }
                    }
                }
            }
            else if(SeriesType == "DoughnutSeries")
            {
                foreach (var a in PieSeries)
                {
                    if (a is PieSeries pieSeries)
                    {
                        if (pieSeries.Name == name)
                        {
                            pieSeries.Fill = color;
                            OnPropertyChanged(nameof(PieSeries));
                            break;
                        }
                    }
                }
            }
            else if (SeriesType == "RowSeries")
            {
                foreach (var a in RowSeries)
                {
                    if (a is RowSeries rowSeries)
                    {
                        if (rowSeries.Name == name)
                        {
                            rowSeries.Fill = color;
                            rowSeries.Foreground = color;
                            OnPropertyChanged(nameof(RowSeries));
                            break;
                        }
                    }
                }
            }
            else if (SeriesType == "BasicColumnSeries")
            {
                foreach (var a in ColumnSeries)
                {
                    if (a is ColumnSeries columnSeries)
                    {
                        if (columnSeries.Name == name)
                        {
                            columnSeries.Fill = color;
                            columnSeries.Foreground = color;
                            OnPropertyChanged(nameof(ColumnSeries));
                            break;
                        }
                    }
                }
            }
            else if (SeriesType == "StackedColumnSeries")
            {
                foreach (var a in StackedColumnSeries)
                {
                    if (a is StackedColumnSeries stackedColumnSeries)
                    {
                        if (stackedColumnSeries.Name == name)
                        {
                            stackedColumnSeries.Fill = color;
                            stackedColumnSeries.Foreground = color;
                            OnPropertyChanged(nameof(StackedColumnSeries));
                            break;
                        }
                    }
                }
            }
        }

        public void SetSeries()
        {
            switch(SeriesType)
            {
                case "DoughnutSeries":
                    SetDoughnutSeries();
                    break;
                case "RowSeries":
                    SetRowSeries();
                    break;
                case "StackedColumnSeries":
                    SetStackedColumnSeries();
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

        public void SetStackedColumnSeries()
        {
            StackedColumnSeries = new SeriesCollection();
            GroupBySelections.Clear();
            if (IsBevetelekKiadasokTabIsSelected)
            {
                if (GroupByYearCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Év") && SelectedAdatsorok.Contains("Hónap"))
                    {
                        List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        var groupedByYearAndMonth = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.TeljesitesiDatum.Year, p.TeljesitesiDatum.Month })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var yearsDict = _selectedBevetelekKiadasok
           .GroupBy(x => x.TeljesitesiDatum.Year)
           .ToDictionary(
               g => g.Key,
               g =>
               {
                   var monthlyValues = Enumerable.Range(1, 12)
                       .ToDictionary(
                           month => month,
                           month =>
                           {
                               var matchingGroup = groupedByYearAndMonth
                                   .Where(kvp => kvp.Key.Year == g.Key && kvp.Key.Month == month)
                                   .SelectMany(kvp => kvp.Value)
                                   .ToList();
                               var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                           }
                       );

                   return monthlyValues;
               }
           );

                        List<string> labels = new List<string>();
                        foreach (var year in yearsDict)
                        {
                            labels.Add(year.Key.ToString());
                        }
                        var monthlyData = new List<ChartValues<double>>();
                        for (int i = 0; i < 12; i++)
                        {
                            monthlyData.Add(new ChartValues<double>());
                        }
                        var sortedYears = yearsDict.OrderBy(y => y.Key);

                        for (int month = 1; month <= 12; month++)
                        {
                            foreach (var year in sortedYears)
                            {
                                double monthValue = year.Value[month];
                                monthlyData[month - 1].Add(monthValue);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in monthlyData)
                        {
                            AddStackedColumnSeries(b, $"Bevételek és Kiadások - {honapok[c]}", honapok[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(honapok[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Év") && SelectedCimkek.Contains("Hónap"))
                    {
                        List<string> labels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        // Create a Dictionary<int, Dictionary<int, double>> where the first key is month (1-12),
                        // and the second dictionary maps years to values for that month
                        var monthDict = Enumerable.Range(1, 12)
                            .ToDictionary(
                                month => month,
                                month => _selectedBevetelekKiadasok
                                    .GroupBy(p => p.TeljesitesiDatum.Year)
                                    .ToDictionary(
                                        yearGroup => yearGroup.Key,
                                        yearGroup => {
                                            // Find items for this specific year and month
                                            var itemsForYearAndMonth = _selectedBevetelekKiadasok
                                                .Where(p => p.TeljesitesiDatum.Year == yearGroup.Key &&
                                                           p.TeljesitesiDatum.Month == month)
                                                .ToList();

                                            // Calculate value using existing items or return 0 if none exist
                                            if (itemsForYearAndMonth.Any())
                                            {
                                                var values = itemsForYearAndMonth.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return GetDataStatisticValueBack(values);
                                            }
                                            return 0.0;
                                        }
                                    )
                            );


                        List<ChartValues<double>> yearlyData = new List<ChartValues<double>>();


                        List<int> years = new List<int>();
                        foreach (var year in _selectedBevetelekKiadasok.GroupBy(x => x.TeljesitesiDatum.Year))
                        {
                            years.Add(year.Key);
                        }

                        for (int i = 0; i < years.Count; i++)
                        {
                            yearlyData.Add(new ChartValues<double>());
                        }
                        for (int i = 0; i < years.Count; i++)
                        {
                            foreach (var month in monthDict)
                            {
                                yearlyData[i].Add(month.Value[years[i]]);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in yearlyData)
                        {
                            AddStackedColumnSeries(b, $"Bevételek és Kiadások - {years[c]}", "Év_" + years[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection("Év_" + years[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;

                        }
                    }
                }
                else if (GroupByBeKiKodCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Pénznem") && SelectedCimkek.Contains("BeKiKód"))
                    {
                        var groupedByBeKiKodPenznem = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.BeKiKod, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g =>
                               {
                                   var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                   return g.Any() ? GetDataStatisticValueBack(v) : 0;
                               }
                           );
                        ChartValues<double> values = new ChartValues<double>();
                        List<string> labels = new List<string>();
                        foreach (var beKiKodPenznem in groupedByBeKiKodPenznem)
                        {
                            labels.Add(beKiKodPenznem.Key.ToString());
                            values.Add(beKiKodPenznem.Value);
                        }

                        AddStackedColumnSeries(values, $"Bevételek és Kiadások - BeKiKód és Pénznem", "BeKiKodPenznem", baseColors[0], labels);
                        AddGroupByDataToCollection("BeKiKodPenznem", 0);
                    }
                    else if (SelectedCimkek.Contains("Pénznem") && SelectedAdatsorok.Contains("BeKiKód"))
                    {
                        List<string> beKiKodok = new List<string> { "Be1", "Be2", "Ki1", "Ki2" };
                        var groupedByBeKiKodPenznem = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.BeKiKod, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var penznemDict = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    var beKiKodValues = System.Enum.GetValues(typeof(BeKiKod))
                                        .Cast<BeKiKod>()
                                        .ToDictionary(
                                            beKiKod => beKiKod,
                                            beKiKod =>
                                            {
                                                var matchingGroup = groupedByBeKiKodPenznem
                                                    .Where(kvp => kvp.Key.BeKiKod == beKiKod && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return beKiKodValues;
                                }
                            );

                        List<string> labels = new List<string>();
                        foreach (var penznem in penznemDict)
                        {
                            labels.Add(penznem.Key.ToString());
                        }
                        var beKiKodData = System.Enum.GetValues(typeof(BeKiKod))
                            .Cast<BeKiKod>()
                            .ToDictionary(
                                beKiKod => beKiKod,
                                beKiKod =>
                                {
                                    return new ChartValues<double>();
                                }
                            );
                        var sortedPenznem = penznemDict.OrderBy(y => y.Key);
                        foreach (var penznem in sortedPenznem)
                        {
                            foreach (var beKiKod in System.Enum.GetValues(typeof(BeKiKod)).Cast<BeKiKod>())
                            {
                                beKiKodData[beKiKod].Add(penznem.Value[beKiKod]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in beKiKodData)
                        {
                            AddStackedColumnSeries(b.Value, $"Bevételek és Kiadások - {beKiKodok[c]}", beKiKodok[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(beKiKodok[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Pénznem") && SelectedCimkek.Contains("BeKiKód"))
                    {
                        List<string> penznemek = new List<string> { "Forint", "Euró", "Font", "Dollár" };
                        var groupedByBeKiKodPenznem = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.BeKiKod, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var beKiKodDict = System.Enum.GetValues(typeof(BeKiKod))
                            .Cast<BeKiKod>()
                            .ToDictionary(
                                beKiKod => beKiKod,
                                beKiKod =>
                                {
                                    var penznemValues = System.Enum.GetValues(typeof(Penznem))
                                        .Cast<Penznem>()
                                        .ToDictionary(
                                            penznem => penznem,
                                            penznem =>
                                            {
                                                var matchingGroup = groupedByBeKiKodPenznem
                                                    .Where(kvp => kvp.Key.BeKiKod == beKiKod && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return penznemValues;
                                }
                            );

                        List<string> labels = new List<string>();
                        foreach (var bekikod in beKiKodDict)
                        {
                            labels.Add(bekikod.Key.ToString());
                        }
                        var penznemData = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    return new ChartValues<double>();
                                }
                            );
                        var sortedBeKiKod = beKiKodDict.OrderBy(y => y.Key);
                        foreach (var bekikod in sortedBeKiKod)
                        {
                            foreach (var penznem in System.Enum.GetValues(typeof(Penznem)).Cast<Penznem>())
                            {
                                penznemData[penznem].Add(bekikod.Value[penznem]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in penznemData)
                        {
                            AddStackedColumnSeries(b.Value, $"Bevételek és Kiadások - {penznemek[c]}", penznemek[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(penznemek[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                }
                else if (GroupByBeKiKodCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Count > 0)
                    {
                        // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                        var groupedByBeKiKod = System.Enum.GetValues(typeof(BeKiKod))
                            .Cast<BeKiKod>()
                            .ToDictionary(
                                bekikod => bekikod,
                                bekikod => new HashSet<BevetelKiadas>(
                                    _selectedBevetelekKiadasok.Where(x => x.BeKiKod == bekikod)
                                )
                            );

                        List<string> labels = new List<string>();
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();

                        // Display the results
                        foreach (var kvp in groupedByBeKiKod)
                        {
                            labels.Add(kvp.Key.ToString());
                            switch (SelectedDataStatistics)
                            {
                                case "Nincs kiválasztva":
                                    bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                                case "Összeg":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Átlag":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Mértani Közép":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Minimum Érték":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Maximum Érték":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Értékek Szórása":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                default:
                                    bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                            }
                        }

                        AddStackedColumnSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - BeKiKod", "BeKiKod", baseColors[0], labels);
                        AddGroupByDataToCollection("BeKiKod", 0);
                    }
                    else
                    {
                        //ERROR TODO
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                    var groupedByPenznem = System.Enum.GetValues(typeof(Penznem))
                        .Cast<Penznem>()
                        .ToDictionary(
                            penznem => penznem,
                            penznem => new HashSet<BevetelKiadas>(
                                _selectedBevetelekKiadasok.Where(x => x.Penznem == penznem)
                            )
                        );

                    List<string> labels = new List<string>();
                    var bevetelekKiadasokAdatsor = new ChartValues<double>();

                    // Display the results
                    foreach (var kvp in groupedByPenznem)
                    {
                        labels.Add(kvp.Key.ToString());
                        switch (SelectedDataStatistics)
                        {
                            case "Nincs kiválasztva":
                                bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                            case "Összeg":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Átlag":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Mértani Közép":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Minimum Érték":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Maximum Érték":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Értékek Szórása":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            default:
                                bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                        }
                    }

                    AddStackedColumnSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - Pénznem", "Pénznem", baseColors[0], labels);
                    AddGroupByDataToCollection("Pénznem", 0);
                }
                else if (GroupByYearCheckBoxIsChecked)
                {
                    var groupedByYear = _selectedBevetelekKiadasok
                       .GroupBy(p => new { p.TeljesitesiDatum.Year })
                       .ToDictionary(
                           g => g.Key,
                           g =>
                           {
                               var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return g.Any() ? GetDataStatisticValueBack(v) : 0;
                           }
                       );

                    List<string> labels = new List<string>();
                    ChartValues<double> values = new ChartValues<double>();
                    foreach (var year in groupedByYear)
                    {
                        labels.Add(year.Key.ToString());
                        values.Add(year.Value);
                    }

                    var sortedYears = groupedByYear.OrderBy(y => y.Key);

                    AddStackedColumnSeries(values, $"Bevételek és Kiadások - Év", "Év", baseColors[0], labels);
                    AddGroupByDataToCollection("Év", 0);
                }
                else if (GroupByMonthCheckBoxIsChecked)
                {

                    List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    var monthValuesDict = Enumerable.Range(1, 12)
                        .ToDictionary(
                            month => month,
                            month => 0.0  // Default value for months with no data
                        );

                    // Group by month and calculate statistics for each month that has data
                    var groupedByMonth = _selectedBevetelekKiadasok
                        .GroupBy(p => p.TeljesitesiDatum.Month)
                        .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                return g.Any() ? GetDataStatisticValueBack(v) : 0;
                            }
                        );

                    // Update our complete dictionary with values from grouped data
                    foreach (var item in groupedByMonth)
                    {
                        monthValuesDict[item.Key] = item.Value;
                    }

                    // Convert to ordered list for display
                    List<string> labels = honapok;
                    ChartValues<double> values = new ChartValues<double>(
                        Enumerable.Range(1, 12).Select(month => monthValuesDict[month])
                    );

                    AddStackedColumnSeries(values, $"Bevételek és Kiadások - Hónapok", "Hónapok", baseColors[0], honapok);
                    AddGroupByDataToCollection("Hónapok", 0);
                }
                else
                {
                    //TODO ERROR
                }
            }
            else
            {
                if (GroupByYearCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Év") && SelectedAdatsorok.Contains("Hónap"))
                    {
                        List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        var groupedByYearAndMonth = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.KifizetesHatarideje.Year, p.KifizetesHatarideje.Month })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var yearsDict = _selectedKotelezettsegekKovetelesek
           .GroupBy(x => x.KifizetesHatarideje.Year)
           .ToDictionary(
               g => g.Key,
               g =>
               {
                   var monthlyValues = Enumerable.Range(1, 12)
                       .ToDictionary(
                           month => month,
                           month =>
                           {
                               var matchingGroup = groupedByYearAndMonth
                                   .Where(kvp => kvp.Key.Year == g.Key && kvp.Key.Month == month)
                                   .SelectMany(kvp => kvp.Value)
                                   .ToList();
                               var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                           }
                       );

                   return monthlyValues;
               }
           );

                        List<string> labels = new List<string>();
                        foreach (var year in yearsDict)
                        {
                            labels.Add(year.Key.ToString());
                        }
                        var monthlyData = new List<ChartValues<double>>();
                        for (int i = 0; i < 12; i++)
                        {
                            monthlyData.Add(new ChartValues<double>());
                        }
                        var sortedYears = yearsDict.OrderBy(y => y.Key);

                        for (int month = 1; month <= 12; month++)
                        {
                            foreach (var year in sortedYears)
                            {
                                double monthValue = year.Value[month];
                                monthlyData[month - 1].Add(monthValue);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in monthlyData)
                        {
                            AddStackedColumnSeries(b, $"Kötelezettségek és Követelések - {honapok[c]}", honapok[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(honapok[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Év") && SelectedCimkek.Contains("Hónap"))
                    {
                        List<string> labels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        // Create a Dictionary<int, Dictionary<int, double>> where the first key is month (1-12),
                        // and the second dictionary maps years to values for that month
                        var monthDict = Enumerable.Range(1, 12)
                            .ToDictionary(
                                month => month,
                                month => _selectedKotelezettsegekKovetelesek
                                    .GroupBy(p => p.KifizetesHatarideje.Year)
                                    .ToDictionary(
                                        yearGroup => yearGroup.Key,
                                        yearGroup => {
                                            // Find items for this specific year and month
                                            var itemsForYearAndMonth = _selectedKotelezettsegekKovetelesek
                                                .Where(p => p.KifizetesHatarideje.Year == yearGroup.Key &&
                                                           p.KifizetesHatarideje.Month == month)
                                                .ToList();

                                            // Calculate value using existing items or return 0 if none exist
                                            if (itemsForYearAndMonth.Any())
                                            {
                                                var values = itemsForYearAndMonth.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return GetDataStatisticValueBack(values);
                                            }
                                            return 0.0;
                                        }
                                    )
                            );


                        List<ChartValues<double>> yearlyData = new List<ChartValues<double>>();


                        List<int> years = new List<int>();
                        foreach (var year in _selectedKotelezettsegekKovetelesek.GroupBy(x => x.KifizetesHatarideje.Year))
                        {
                            years.Add(year.Key);
                        }

                        for (int i = 0; i < years.Count; i++)
                        {
                            yearlyData.Add(new ChartValues<double>());
                        }
                        for (int i = 0; i < years.Count; i++)
                        {
                            foreach (var month in monthDict)
                            {
                                yearlyData[i].Add(month.Value[years[i]]);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in yearlyData)
                        {
                            AddStackedColumnSeries(b, $"Kötelezettségek és Követelések - {years[c]}", "Év_" + years[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection("Év_" + years[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;

                        }
                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Pénznem") && SelectedCimkek.Contains("Kifizetett"))
                    {
                        var groupedKifizetettPenznem = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.Kifizetett, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g =>
                               {
                                   var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                   return g.Any() ? GetDataStatisticValueBack(v) : 0;
                               }
                           );
                        ChartValues<double> values = new ChartValues<double>();
                        List<string> labels = new List<string>();
                        foreach (var kifizetettPenznem in groupedKifizetettPenznem)
                        {
                            labels.Add(kifizetettPenznem.Key.ToString());
                            values.Add(kifizetettPenznem.Value);
                        }

                        AddStackedColumnSeries(values, $"Kötelezettségek és Követelések - Kifizetett és Pénznem", "KifizetettPenznem", baseColors[0], labels);
                        AddGroupByDataToCollection("KifizetettPenznem", 0);
                    }
                    else if (SelectedCimkek.Contains("Pénznem") && SelectedAdatsorok.Contains("Kifizetett"))
                    {
                        List<string> kifizetettLabels = new List<string> { "0", "1" };
                        var groupedByKifizetettPenznem = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.Kifizetett, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var penznemDict = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    var kifizetettValues = _selectedKotelezettsegekKovetelesek
                                        .GroupBy(x => x.Kifizetett)
                                        .ToDictionary(
                                            kifizetett => kifizetett.Key,
                                            kifizetett =>
                                            {
                                                var matchingGroup = groupedByKifizetettPenznem
                                                    .Where(kvp => kvp.Key.Kifizetett == kifizetett.Key && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return kifizetettValues;
                                }
                            );

                        List<string> labels = new List<string>();
                        foreach (var penznem in penznemDict)
                        {
                            labels.Add(penznem.Key.ToString());
                        }
                        var kifizetettData = new List<ChartValues<double>>();
                        for (int i = 0; i < 2; i++)
                        {
                            kifizetettData.Add(new ChartValues<double>());
                        }

                        var sortedPenznem = penznemDict.OrderBy(y => y.Key);
                        foreach (var penznem in sortedPenznem)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                kifizetettData[i].Add(penznem.Value[(short)i]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in kifizetettData)
                        {
                            AddStackedColumnSeries(b, $"Kötelezettségek és Követelések - {kifizetettLabels[c]}", "Kifizetett_" + kifizetettLabels[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection("Kifizetett_" + kifizetettLabels[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Pénznem") && SelectedCimkek.Contains("Kifizetett"))
                    {
                        List<string> penznemek = new List<string> { "Forint", "Euró", "Font", "Dollár" };
                        var groupedByBeKiKodPenznem = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.Kifizetett, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var beKiKodDict = _selectedKotelezettsegekKovetelesek
                            .GroupBy(x => x.Kifizetett)
                            .ToDictionary(
                                kifizetett => kifizetett,
                                kifizetett =>
                                {
                                    var penznemValues = System.Enum.GetValues(typeof(Penznem))
                                        .Cast<Penznem>()
                                        .ToDictionary(
                                            penznem => penznem,
                                            penznem =>
                                            {
                                                var matchingGroup = groupedByBeKiKodPenznem
                                                    .Where(kvp => kvp.Key.Kifizetett == kifizetett.Key && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return penznemValues;
                                }
                            );

                        List<string> labels = new List<string>() { "0", "1" };

                        var penznemData = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    return new ChartValues<double>();
                                }
                            );
                        var sortedBeKiKod = beKiKodDict.OrderBy(y => y.Key.Key);
                        foreach (var bekikod in sortedBeKiKod)
                        {
                            foreach (var penznem in System.Enum.GetValues(typeof(Penznem)).Cast<Penznem>())
                            {
                                penznemData[penznem].Add(bekikod.Value[penznem]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in penznemData)
                        {
                            AddStackedColumnSeries(b.Value, $"Kötelezettségek és Követelések - {penznemek[c]}", penznemek[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(penznemek[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Count > 0)
                    {
                        // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                        var groupedByKifizetett = _selectedKotelezettsegekKovetelesek
                            .GroupBy(x => x.Kifizetett)
                            .ToDictionary(
                                kifizetett => kifizetett,
                                kifizetett => new HashSet<KotelezettsegKoveteles>(
                                    _selectedKotelezettsegekKovetelesek.Where(x => x.Kifizetett == kifizetett.Key)
                                )
                            );

                        List<string> labels = new List<string>();
                        var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();

                        // Display the results
                        foreach (var kvp in groupedByKifizetett)
                        {
                            labels.Add(kvp.Key.Key.ToString());
                            switch (SelectedDataStatistics)
                            {
                                case "Nincs kiválasztva":
                                    kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                                case "Összeg":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Átlag":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Mértani Közép":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Minimum Érték":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Maximum Érték":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Értékek Szórása":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                default:
                                    kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                            }
                        }

                        AddStackedColumnSeries(kotelezettsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - Kifizetett", "Kifizetett", baseColors[0], labels);
                        AddGroupByDataToCollection("Kifizetett", 0);
                    }
                    else
                    {
                        //ERROR TODO
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                    var groupedByPenznem = System.Enum.GetValues(typeof(Penznem))
                        .Cast<Penznem>()
                        .ToDictionary(
                            penznem => penznem,
                            penznem => new HashSet<KotelezettsegKoveteles>(
                                _selectedKotelezettsegekKovetelesek.Where(x => x.Penznem == penznem)
                            )
                        );

                    List<string> labels = new List<string>();
                    var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();

                    // Display the results
                    foreach (var kvp in groupedByPenznem)
                    {
                        labels.Add(kvp.Key.ToString());
                        switch (SelectedDataStatistics)
                        {
                            case "Nincs kiválasztva":
                                kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                            case "Összeg":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Átlag":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Mértani Közép":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Minimum Érték":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Maximum Érték":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Értékek Szórása":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            default:
                                kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                        }
                    }

                    AddStackedColumnSeries(kotelezettsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - Pénznem", "Pénznem", baseColors[0], labels);
                    AddGroupByDataToCollection("Pénznem", 0);
                }
                else if (GroupByYearCheckBoxIsChecked)
                {
                    var groupedByYear = _selectedKotelezettsegekKovetelesek
                       .GroupBy(p => new { p.KifizetesHatarideje.Year })
                       .ToDictionary(
                           g => g.Key,
                           g =>
                           {
                               var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return g.Any() ? GetDataStatisticValueBack(v) : 0;
                           }
                       );

                    List<string> labels = new List<string>();
                    ChartValues<double> values = new ChartValues<double>();
                    foreach (var year in groupedByYear)
                    {
                        labels.Add(year.Key.ToString());
                        values.Add(year.Value);
                    }

                    var sortedYears = groupedByYear.OrderBy(y => y.Key);

                    AddStackedColumnSeries(values, $"Kötelezettségek és Követelések - Év", "Év", baseColors[0], labels);
                    AddGroupByDataToCollection("Év", 0);
                }
                else if (GroupByMonthCheckBoxIsChecked)
                {

                    List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    var monthValuesDict = Enumerable.Range(1, 12)
                        .ToDictionary(
                            month => month,
                            month => 0.0  // Default value for months with no data
                        );

                    // Group by month and calculate statistics for each month that has data
                    var groupedByMonth = _selectedKotelezettsegekKovetelesek
                        .GroupBy(p => p.KifizetesHatarideje.Month)
                        .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                return g.Any() ? GetDataStatisticValueBack(v) : 0;
                            }
                        );

                    // Update our complete dictionary with values from grouped data
                    foreach (var item in groupedByMonth)
                    {
                        monthValuesDict[item.Key] = item.Value;
                    }

                    // Convert to ordered list for display
                    List<string> labels = honapok;
                    ChartValues<double> values = new ChartValues<double>(
                        Enumerable.Range(1, 12).Select(month => monthValuesDict[month])
                    );

                    AddStackedColumnSeries(values, $"Kötelezettségek és Követelések - Hónapok", "Hónapok", baseColors[0], honapok);
                    AddGroupByDataToCollection("Hónapok", 0);
                }
                else
                {
                    //TODO ERROR
                }
            }

            OnPropertyChanged(nameof(Series));
        }

        public void SetBasicColumnSeries()
        {
            ColumnSeries = new SeriesCollection();
            GroupBySelections.Clear();
            if (IsBevetelekKiadasokTabIsSelected)
            {
                if (GroupByYearCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Év") && SelectedAdatsorok.Contains("Hónap"))
                    {
                        List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        var groupedByYearAndMonth = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.TeljesitesiDatum.Year, p.TeljesitesiDatum.Month })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var yearsDict = _selectedBevetelekKiadasok
           .GroupBy(x => x.TeljesitesiDatum.Year)
           .ToDictionary(
               g => g.Key,
               g =>
               {
                   var monthlyValues = Enumerable.Range(1, 12)
                       .ToDictionary(
                           month => month,
                           month =>
                           {
                               var matchingGroup = groupedByYearAndMonth
                                   .Where(kvp => kvp.Key.Year == g.Key && kvp.Key.Month == month)
                                   .SelectMany(kvp => kvp.Value)
                                   .ToList();
                               var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                           }
                       );

                   return monthlyValues;
               }
           );

                        List<string> labels = new List<string>();
                        foreach (var year in yearsDict)
                        {
                            labels.Add(year.Key.ToString());
                        }
                        var monthlyData = new List<ChartValues<double>>();
                        for (int i = 0; i < 12; i++)
                        {
                            monthlyData.Add(new ChartValues<double>());
                        }
                        var sortedYears = yearsDict.OrderBy(y => y.Key);

                        for (int month = 1; month <= 12; month++)
                        {
                            foreach (var year in sortedYears)
                            {
                                double monthValue = year.Value[month];
                                monthlyData[month - 1].Add(monthValue);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in monthlyData)
                        {
                            AddColumnSeries(b, $"Bevételek és Kiadások - {honapok[c]}", honapok[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(honapok[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Év") && SelectedCimkek.Contains("Hónap"))
                    {
                        List<string> labels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        // Create a Dictionary<int, Dictionary<int, double>> where the first key is month (1-12),
                        // and the second dictionary maps years to values for that month
                        var monthDict = Enumerable.Range(1, 12)
                            .ToDictionary(
                                month => month,
                                month => _selectedBevetelekKiadasok
                                    .GroupBy(p => p.TeljesitesiDatum.Year)
                                    .ToDictionary(
                                        yearGroup => yearGroup.Key,
                                        yearGroup => {
                                            // Find items for this specific year and month
                                            var itemsForYearAndMonth = _selectedBevetelekKiadasok
                                                .Where(p => p.TeljesitesiDatum.Year == yearGroup.Key &&
                                                           p.TeljesitesiDatum.Month == month)
                                                .ToList();

                                            // Calculate value using existing items or return 0 if none exist
                                            if (itemsForYearAndMonth.Any())
                                            {
                                                var values = itemsForYearAndMonth.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return GetDataStatisticValueBack(values);
                                            }
                                            return 0.0;
                                        }
                                    )
                            );


                        List<ChartValues<double>> yearlyData = new List<ChartValues<double>>();


                        List<int> years = new List<int>();
                        foreach (var year in _selectedBevetelekKiadasok.GroupBy(x => x.TeljesitesiDatum.Year))
                        {
                            years.Add(year.Key);
                        }

                        for (int i = 0; i < years.Count; i++)
                        {
                            yearlyData.Add(new ChartValues<double>());
                        }
                        for (int i = 0; i < years.Count; i++)
                        {
                            foreach (var month in monthDict)
                            {
                                yearlyData[i].Add(month.Value[years[i]]);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in yearlyData)
                        {
                            AddColumnSeries(b, $"Bevételek és Kiadások - {years[c]}", "Év_" + years[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection("Év_" + years[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;

                        }
                    }
                }
                else if (GroupByBeKiKodCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Pénznem") && SelectedCimkek.Contains("BeKiKód"))
                    {
                        var groupedByBeKiKodPenznem = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.BeKiKod, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g =>
                               {
                                   var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                   return g.Any() ? GetDataStatisticValueBack(v) : 0;
                               }
                           );
                        ChartValues<double> values = new ChartValues<double>();
                        List<string> labels = new List<string>();
                        foreach (var beKiKodPenznem in groupedByBeKiKodPenznem)
                        {
                            labels.Add(beKiKodPenznem.Key.ToString());
                            values.Add(beKiKodPenznem.Value);
                        }

                        AddColumnSeries(values, $"Bevételek és Kiadások - BeKiKód és Pénznem", "BeKiKodPenznem", baseColors[0], labels);
                        AddGroupByDataToCollection("BeKiKodPenznem", 0);
                    }
                    else if (SelectedCimkek.Contains("Pénznem") && SelectedAdatsorok.Contains("BeKiKód"))
                    {
                        List<string> beKiKodok = new List<string> { "Be1", "Be2", "Ki1", "Ki2" };
                        var groupedByBeKiKodPenznem = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.BeKiKod, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var penznemDict = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    var beKiKodValues = System.Enum.GetValues(typeof(BeKiKod))
                                        .Cast<BeKiKod>()
                                        .ToDictionary(
                                            beKiKod => beKiKod,
                                            beKiKod =>
                                            {
                                                var matchingGroup = groupedByBeKiKodPenznem
                                                    .Where(kvp => kvp.Key.BeKiKod == beKiKod && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return beKiKodValues;
                                }
                            );

                        List<string> labels = new List<string>();
                        foreach (var penznem in penznemDict)
                        {
                            labels.Add(penznem.Key.ToString());
                        }
                        var beKiKodData = System.Enum.GetValues(typeof(BeKiKod))
                            .Cast<BeKiKod>()
                            .ToDictionary(
                                beKiKod => beKiKod,
                                beKiKod =>
                                {
                                    return new ChartValues<double>();
                                }
                            );
                        var sortedPenznem = penznemDict.OrderBy(y => y.Key);
                        foreach (var penznem in sortedPenznem)
                        {
                            foreach (var beKiKod in System.Enum.GetValues(typeof(BeKiKod)).Cast<BeKiKod>())
                            {
                                beKiKodData[beKiKod].Add(penznem.Value[beKiKod]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in beKiKodData)
                        {
                            AddColumnSeries(b.Value, $"Bevételek és Kiadások - {beKiKodok[c]}", beKiKodok[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(beKiKodok[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Pénznem") && SelectedCimkek.Contains("BeKiKód"))
                    {
                        List<string> penznemek = new List<string> { "Forint", "Euró", "Font", "Dollár" };
                        var groupedByBeKiKodPenznem = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.BeKiKod, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var beKiKodDict = System.Enum.GetValues(typeof(BeKiKod))
                            .Cast<BeKiKod>()
                            .ToDictionary(
                                beKiKod => beKiKod,
                                beKiKod =>
                                {
                                    var penznemValues = System.Enum.GetValues(typeof(Penznem))
                                        .Cast<Penznem>()
                                        .ToDictionary(
                                            penznem => penznem,
                                            penznem =>
                                            {
                                                var matchingGroup = groupedByBeKiKodPenznem
                                                    .Where(kvp => kvp.Key.BeKiKod == beKiKod && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return penznemValues;
                                }
                            );

                        List<string> labels = new List<string>();
                        foreach (var bekikod in beKiKodDict)
                        {
                            labels.Add(bekikod.Key.ToString());
                        }
                        var penznemData = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    return new ChartValues<double>();
                                }
                            );
                        var sortedBeKiKod = beKiKodDict.OrderBy(y => y.Key);
                        foreach (var bekikod in sortedBeKiKod)
                        {
                            foreach (var penznem in System.Enum.GetValues(typeof(Penznem)).Cast<Penznem>())
                            {
                                penznemData[penznem].Add(bekikod.Value[penznem]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in penznemData)
                        {
                            AddColumnSeries(b.Value, $"Bevételek és Kiadások - {penznemek[c]}", penznemek[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(penznemek[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                }
                else if (GroupByBeKiKodCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Count > 0)
                    {
                        // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                        var groupedByBeKiKod = System.Enum.GetValues(typeof(BeKiKod))
                            .Cast<BeKiKod>()
                            .ToDictionary(
                                bekikod => bekikod,
                                bekikod => new HashSet<BevetelKiadas>(
                                    _selectedBevetelekKiadasok.Where(x => x.BeKiKod == bekikod)
                                )
                            );

                        List<string> labels = new List<string>();
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();

                        // Display the results
                        foreach (var kvp in groupedByBeKiKod)
                        {
                            labels.Add(kvp.Key.ToString());
                            switch (SelectedDataStatistics)
                            {
                                case "Nincs kiválasztva":
                                    bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                                case "Összeg":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Átlag":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Mértani Közép":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Minimum Érték":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Maximum Érték":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Értékek Szórása":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                default:
                                    bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                            }
                        }

                        AddColumnSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - BeKiKod", "BeKiKod", baseColors[0], labels);
                        AddGroupByDataToCollection("BeKiKod", 0);
                    }
                    else
                    {
                        //ERROR TODO
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                    var groupedByPenznem = System.Enum.GetValues(typeof(Penznem))
                        .Cast<Penznem>()
                        .ToDictionary(
                            penznem => penznem,
                            penznem => new HashSet<BevetelKiadas>(
                                _selectedBevetelekKiadasok.Where(x => x.Penznem == penznem)
                            )
                        );

                    List<string> labels = new List<string>();
                    var bevetelekKiadasokAdatsor = new ChartValues<double>();

                    // Display the results
                    foreach (var kvp in groupedByPenznem)
                    {
                        labels.Add(kvp.Key.ToString());
                        switch (SelectedDataStatistics)
                        {
                            case "Nincs kiválasztva":
                                bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                            case "Összeg":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Átlag":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Mértani Közép":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Minimum Érték":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Maximum Érték":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Értékek Szórása":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            default:
                                bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                        }
                    }

                    AddColumnSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - Pénznem", "Pénznem", baseColors[0], labels);
                    AddGroupByDataToCollection("Pénznem", 0);
                }
                else if (GroupByYearCheckBoxIsChecked)
                {
                    var groupedByYear = _selectedBevetelekKiadasok
                       .GroupBy(p => new { p.TeljesitesiDatum.Year })
                       .ToDictionary(
                           g => g.Key,
                           g =>
                           {
                               var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return g.Any() ? GetDataStatisticValueBack(v) : 0;
                           }
                       );

                    List<string> labels = new List<string>();
                    ChartValues<double> values = new ChartValues<double>();
                    foreach (var year in groupedByYear)
                    {
                        labels.Add(year.Key.ToString());
                        values.Add(year.Value);
                    }

                    var sortedYears = groupedByYear.OrderBy(y => y.Key);

                    AddColumnSeries(values, $"Bevételek és Kiadások - Év", "Év", baseColors[0], labels);
                    AddGroupByDataToCollection("Év", 0);
                }
                else if (GroupByMonthCheckBoxIsChecked)
                {

                    List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    var monthValuesDict = Enumerable.Range(1, 12)
                        .ToDictionary(
                            month => month,
                            month => 0.0  // Default value for months with no data
                        );

                    // Group by month and calculate statistics for each month that has data
                    var groupedByMonth = _selectedBevetelekKiadasok
                        .GroupBy(p => p.TeljesitesiDatum.Month)
                        .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                return g.Any() ? GetDataStatisticValueBack(v) : 0;
                            }
                        );

                    // Update our complete dictionary with values from grouped data
                    foreach (var item in groupedByMonth)
                    {
                        monthValuesDict[item.Key] = item.Value;
                    }

                    // Convert to ordered list for display
                    List<string> labels = honapok;
                    ChartValues<double> values = new ChartValues<double>(
                        Enumerable.Range(1, 12).Select(month => monthValuesDict[month])
                    );

                    AddColumnSeries(values, $"Bevételek és Kiadások - Hónapok", "Hónapok", baseColors[0], honapok);
                    AddGroupByDataToCollection("Hónapok", 0);
                }
                else
                {
                    //TODO ERROR
                }
            }
            else
            {
                if (GroupByYearCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Év") && SelectedAdatsorok.Contains("Hónap"))
                    {
                        List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        var groupedByYearAndMonth = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.KifizetesHatarideje.Year, p.KifizetesHatarideje.Month })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var yearsDict = _selectedKotelezettsegekKovetelesek
           .GroupBy(x => x.KifizetesHatarideje.Year)
           .ToDictionary(
               g => g.Key,
               g =>
               {
                   var monthlyValues = Enumerable.Range(1, 12)
                       .ToDictionary(
                           month => month,
                           month =>
                           {
                               var matchingGroup = groupedByYearAndMonth
                                   .Where(kvp => kvp.Key.Year == g.Key && kvp.Key.Month == month)
                                   .SelectMany(kvp => kvp.Value)
                                   .ToList();
                               var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                           }
                       );

                   return monthlyValues;
               }
           );

                        List<string> labels = new List<string>();
                        foreach (var year in yearsDict)
                        {
                            labels.Add(year.Key.ToString());
                        }
                        var monthlyData = new List<ChartValues<double>>();
                        for (int i = 0; i < 12; i++)
                        {
                            monthlyData.Add(new ChartValues<double>());
                        }
                        var sortedYears = yearsDict.OrderBy(y => y.Key);

                        for (int month = 1; month <= 12; month++)
                        {
                            foreach (var year in sortedYears)
                            {
                                double monthValue = year.Value[month];
                                monthlyData[month - 1].Add(monthValue);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in monthlyData)
                        {
                            AddColumnSeries(b, $"Kötelezettségek és Követelések - {honapok[c]}", honapok[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(honapok[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Év") && SelectedCimkek.Contains("Hónap"))
                    {
                        List<string> labels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        // Create a Dictionary<int, Dictionary<int, double>> where the first key is month (1-12),
                        // and the second dictionary maps years to values for that month
                        var monthDict = Enumerable.Range(1, 12)
                            .ToDictionary(
                                month => month,
                                month => _selectedKotelezettsegekKovetelesek
                                    .GroupBy(p => p.KifizetesHatarideje.Year)
                                    .ToDictionary(
                                        yearGroup => yearGroup.Key,
                                        yearGroup => {
                                            // Find items for this specific year and month
                                            var itemsForYearAndMonth = _selectedKotelezettsegekKovetelesek
                                                .Where(p => p.KifizetesHatarideje.Year == yearGroup.Key &&
                                                           p.KifizetesHatarideje.Month == month)
                                                .ToList();

                                            // Calculate value using existing items or return 0 if none exist
                                            if (itemsForYearAndMonth.Any())
                                            {
                                                var values = itemsForYearAndMonth.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return GetDataStatisticValueBack(values);
                                            }
                                            return 0.0;
                                        }
                                    )
                            );


                        List<ChartValues<double>> yearlyData = new List<ChartValues<double>>();


                        List<int> years = new List<int>();
                        foreach (var year in _selectedKotelezettsegekKovetelesek.GroupBy(x => x.KifizetesHatarideje.Year))
                        {
                            years.Add(year.Key);
                        }

                        for (int i = 0; i < years.Count; i++)
                        {
                            yearlyData.Add(new ChartValues<double>());
                        }
                        for (int i = 0; i < years.Count; i++)
                        {
                            foreach (var month in monthDict)
                            {
                                yearlyData[i].Add(month.Value[years[i]]);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in yearlyData)
                        {
                            AddColumnSeries(b, $"Kötelezettségek és Követelések - {years[c]}", "Év_" + years[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection("Év_" + years[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;

                        }
                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Pénznem") && SelectedCimkek.Contains("Kifizetett"))
                    {
                        var groupedKifizetettPenznem = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.Kifizetett, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g =>
                               {
                                   var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                   return g.Any() ? GetDataStatisticValueBack(v) : 0;
                               }
                           );
                        ChartValues<double> values = new ChartValues<double>();
                        List<string> labels = new List<string>();
                        foreach (var kifizetettPenznem in groupedKifizetettPenznem)
                        {
                            labels.Add(kifizetettPenznem.Key.ToString());
                            values.Add(kifizetettPenznem.Value);
                        }

                        AddColumnSeries(values, $"Kötelezettségek és Követelések - Kifizetett és Pénznem", "KifizetettPenznem", baseColors[0], labels);
                        AddGroupByDataToCollection("KifizetettPenznem", 0);
                    }
                    else if (SelectedCimkek.Contains("Pénznem") && SelectedAdatsorok.Contains("Kifizetett"))
                    {
                        List<string> kifizetettLabels = new List<string> { "0", "1" };
                        var groupedByKifizetettPenznem = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.Kifizetett, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var penznemDict = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    var kifizetettValues = _selectedKotelezettsegekKovetelesek
                                        .GroupBy(x => x.Kifizetett)
                                        .ToDictionary(
                                            kifizetett => kifizetett.Key,
                                            kifizetett =>
                                            {
                                                var matchingGroup = groupedByKifizetettPenznem
                                                    .Where(kvp => kvp.Key.Kifizetett == kifizetett.Key && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return kifizetettValues;
                                }
                            );

                        List<string> labels = new List<string>();
                        foreach (var penznem in penznemDict)
                        {
                            labels.Add(penznem.Key.ToString());
                        }
                        var kifizetettData = new List<ChartValues<double>>();
                        for (int i = 0; i < 2; i++)
                        {
                            kifizetettData.Add(new ChartValues<double>());
                        }

                        var sortedPenznem = penznemDict.OrderBy(y => y.Key);
                        foreach (var penznem in sortedPenznem)
                        {
                            for(int i = 0; i < 2; i++)
                            {
                                kifizetettData[i].Add(penznem.Value[(short)i]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in kifizetettData)
                        {
                            AddColumnSeries(b, $"Kötelezettségek és Követelések - {kifizetettLabels[c]}", "Kifizetett_" + kifizetettLabels[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection("Kifizetett_" + kifizetettLabels[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Pénznem") && SelectedCimkek.Contains("Kifizetett"))
                    {
                        List<string> penznemek = new List<string> { "Forint", "Euró", "Font", "Dollár" };
                        var groupedByBeKiKodPenznem = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.Kifizetett, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var beKiKodDict = _selectedKotelezettsegekKovetelesek
                            .GroupBy(x => x.Kifizetett)
                            .ToDictionary(
                                kifizetett => kifizetett,
                                kifizetett =>
                                {
                                    var penznemValues = System.Enum.GetValues(typeof(Penznem))
                                        .Cast<Penznem>()
                                        .ToDictionary(
                                            penznem => penznem,
                                            penznem =>
                                            {
                                                var matchingGroup = groupedByBeKiKodPenznem
                                                    .Where(kvp => kvp.Key.Kifizetett == kifizetett.Key && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return penznemValues;
                                }
                            );

                        List<string> labels = new List<string>() { "0", "1" };
                        
                        var penznemData = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    return new ChartValues<double>();
                                }
                            );
                        var sortedBeKiKod = beKiKodDict.OrderBy(y => y.Key.Key);
                        foreach (var bekikod in sortedBeKiKod)
                        {
                            foreach (var penznem in System.Enum.GetValues(typeof(Penznem)).Cast<Penznem>())
                            {
                                penznemData[penznem].Add(bekikod.Value[penznem]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in penznemData)
                        {
                            AddColumnSeries(b.Value, $"Kötelezettségek és Követelések - {penznemek[c]}", penznemek[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(penznemek[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Count > 0)
                    {
                        // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                        var groupedByKifizetett = _selectedKotelezettsegekKovetelesek
                            .GroupBy(x => x.Kifizetett)
                            .ToDictionary(
                                kifizetett => kifizetett,
                                kifizetett => new HashSet<KotelezettsegKoveteles>(
                                    _selectedKotelezettsegekKovetelesek.Where(x => x.Kifizetett == kifizetett.Key)
                                )
                            );

                        List<string> labels = new List<string>();
                        var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();

                        // Display the results
                        foreach (var kvp in groupedByKifizetett)
                        {
                            labels.Add(kvp.Key.Key.ToString());
                            switch (SelectedDataStatistics)
                            {
                                case "Nincs kiválasztva":
                                    kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                                case "Összeg":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Átlag":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Mértani Közép":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Minimum Érték":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Maximum Érték":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Értékek Szórása":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                default:
                                    kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                            }
                        }

                        AddColumnSeries(kotelezettsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - Kifizetett", "Kifizetett", baseColors[0], labels);
                        AddGroupByDataToCollection("Kifizetett", 0);
                    }
                    else
                    {
                        //ERROR TODO
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                    var groupedByPenznem = System.Enum.GetValues(typeof(Penznem))
                        .Cast<Penznem>()
                        .ToDictionary(
                            penznem => penznem,
                            penznem => new HashSet<KotelezettsegKoveteles>(
                                _selectedKotelezettsegekKovetelesek.Where(x => x.Penznem == penznem)
                            )
                        );

                    List<string> labels = new List<string>();
                    var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();

                    // Display the results
                    foreach (var kvp in groupedByPenznem)
                    {
                        labels.Add(kvp.Key.ToString());
                        switch (SelectedDataStatistics)
                        {
                            case "Nincs kiválasztva":
                                kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                            case "Összeg":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Átlag":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Mértani Közép":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Minimum Érték":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Maximum Érték":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Értékek Szórása":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            default:
                                kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                        }
                    }

                    AddColumnSeries(kotelezettsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - Pénznem", "Pénznem", baseColors[0], labels);
                    AddGroupByDataToCollection("Pénznem", 0);
                }
                else if (GroupByYearCheckBoxIsChecked)
                {
                    var groupedByYear = _selectedKotelezettsegekKovetelesek
                       .GroupBy(p => new { p.KifizetesHatarideje.Year })
                       .ToDictionary(
                           g => g.Key,
                           g =>
                           {
                               var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return g.Any() ? GetDataStatisticValueBack(v) : 0;
                           }
                       );

                    List<string> labels = new List<string>();
                    ChartValues<double> values = new ChartValues<double>();
                    foreach (var year in groupedByYear)
                    {
                        labels.Add(year.Key.ToString());
                        values.Add(year.Value);
                    }

                    var sortedYears = groupedByYear.OrderBy(y => y.Key);

                    AddColumnSeries(values, $"Kötelezettségek és Követelések - Év", "Év", baseColors[0], labels);
                    AddGroupByDataToCollection("Év", 0);
                }
                else if (GroupByMonthCheckBoxIsChecked)
                {

                    List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    var monthValuesDict = Enumerable.Range(1, 12)
                        .ToDictionary(
                            month => month,
                            month => 0.0  // Default value for months with no data
                        );

                    // Group by month and calculate statistics for each month that has data
                    var groupedByMonth = _selectedKotelezettsegekKovetelesek
                        .GroupBy(p => p.KifizetesHatarideje.Month)
                        .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                return g.Any() ? GetDataStatisticValueBack(v) : 0;
                            }
                        );

                    // Update our complete dictionary with values from grouped data
                    foreach (var item in groupedByMonth)
                    {
                        monthValuesDict[item.Key] = item.Value;
                    }

                    // Convert to ordered list for display
                    List<string> labels = honapok;
                    ChartValues<double> values = new ChartValues<double>(
                        Enumerable.Range(1, 12).Select(month => monthValuesDict[month])
                    );

                    AddColumnSeries(values, $"Kötelezettségek és Követelések - Hónapok", "Hónapok", baseColors[0], honapok);
                    AddGroupByDataToCollection("Hónapok", 0);
                }
                else
                {
                    //TODO ERROR
                }
            }

            OnPropertyChanged(nameof(Series));
        }

        public double GetDataStatisticValueBack(List<double> values)
        {
            if (SelectedDataStatistics == "Összeg")
            {
                return DataStatistics.GetSum(values);
            }
            else if (SelectedDataStatistics == "Átlag")
            {
                return DataStatistics.GetAvarage(values);
            }
            else if (SelectedDataStatistics == "Mértani Közép")
            {
                return DataStatistics.GetMedian(values);
            }
            else if (SelectedDataStatistics == "Minimum Érték")
            {
                return DataStatistics.GetMinimumValue(values);
            }
            else if (SelectedDataStatistics == "Maximum Érték")
            {
                return DataStatistics.GetMaximumValue(values);
            }
            else if (SelectedDataStatistics == "Értékek Szórása")
            {
                return DataStatistics.GetStandardDeviation(values);
            }
            else
            {
                return DataStatistics.GetSum(values);
            }
        }

        public void SetLineSeries()
        {
            LineSeries = new SeriesCollection();
            GroupBySelections.Clear();

            if(IsBevetelekKiadasokTabIsSelected)
            {
                if (GroupByBeKiKodCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked && GroupByDateCheckBoxIsChecked)
                {
                    var groupedByYearAndPenznem = _selectedBevetelekKiadasok
                    .GroupBy(p => new { p.TeljesitesiDatum.Year, p.BeKiKod, p.Penznem })
                    .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            // Create a dictionary to store sums by month
                            var monthlySums = g.GroupBy(p => p.TeljesitesiDatum.Month)
                                               .ToDictionary(
                                                   monthGroup => monthGroup.Key,
                                                  monthGroup =>
                                                  {
                                                      var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                      return GetDataStatisticValueBack(values);
                                                  }
                                               );

                            // Ensure all months (1-12) are accounted for
                            for (int month = 1; month <= 12; month++)
                            {
                                if (!monthlySums.ContainsKey(month))
                                {
                                    monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                }
                            }
                            // Return the values as a HashSet
                            return monthlySums.OrderBy(m => m.Key)
                           .Select(m => m.Value)
                           .ToList();
                        }
                    );

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByYearAndPenznem)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem} + {kvp.Key.BeKiKod} + {kvp.Key.Year}", $"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}_{kvp.Key.Year}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}_{kvp.Key.Year}", a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if(GroupByBeKiKodCheckBoxIsChecked && GroupByDateCheckBoxIsChecked)
                {
                    var groupedByYearAndPenznem = _selectedBevetelekKiadasok
                    .GroupBy(p => new { p.TeljesitesiDatum.Year, p.BeKiKod })
                    .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            // Create a dictionary to store sums by month
                            var monthlySums = g.GroupBy(p => p.TeljesitesiDatum.Month)
                                               .ToDictionary(
                                                   monthGroup => monthGroup.Key,
                                                   monthGroup =>
                                                   {
                                                       var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                       return GetDataStatisticValueBack(values);
                                                   }
                                                );

                            // Ensure all months (1-12) are accounted for
                            for (int month = 1; month <= 12; month++)
                            {
                                if (!monthlySums.ContainsKey(month))
                                {
                                    monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                }
                            }
                            // Return the values as a HashSet
                            return monthlySums.OrderBy(m => m.Key)
                           .Select(m => m.Value)
                           .ToList();
                        }
                    );

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByYearAndPenznem)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.BeKiKod} + {kvp.Key.Year}", $"{kvp.Key.BeKiKod}_{kvp.Key.Year}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.BeKiKod}_{kvp.Key.Year}", a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByDateCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
                {
                    var groupedByYearAndPenznem = _selectedBevetelekKiadasok
                        .GroupBy(p => new { p.TeljesitesiDatum.Year, p.Penznem })
                        .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                // Create a dictionary to store sums by month
                                var monthlySums = g.GroupBy(p => p.TeljesitesiDatum.Month)
                                                   .ToDictionary(
                                                       monthGroup => monthGroup.Key,
                                                        monthGroup =>
                                                        {
                                                            var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                            return GetDataStatisticValueBack(values);
                                                        }
                                                   );

                                // Ensure all months (1-12) are accounted for
                                for (int month = 1; month <= 12; month++)
                                {
                                    if (!monthlySums.ContainsKey(month))
                                    {
                                        monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                    }
                                }
                                // Return the values as a HashSet
                                return monthlySums.OrderBy(m => m.Key)
                               .Select(m => m.Value)
                               .ToList();
                            }
                        );

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByYearAndPenznem)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem} + {kvp.Key.Year}", $"{kvp.Key.Penznem}_{kvp.Key.Year}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Year}", a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if(GroupByBeKiKodCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
                {
                    var groupedByPenznemAndBeKiKod = _selectedBevetelekKiadasok.GroupBy(p => new { p.Penznem, p.BeKiKod })
                        .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                // Create a dictionary to store sums by month
                                var monthlySums = g.GroupBy(p => p.TeljesitesiDatum.Month)
                                                   .ToDictionary(
                                                       monthGroup => monthGroup.Key,
                                                        monthGroup =>
                                                        {
                                                            var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                            return GetDataStatisticValueBack(values);
                                                        }
                                                   );

                                // Ensure all months (1-12) are accounted for
                                for (int month = 1; month <= 12; month++)
                                {
                                    if (!monthlySums.ContainsKey(month))
                                    {
                                        monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                    }
                                }
                                // Return the values as a HashSet
                                return monthlySums.OrderBy(m => m.Key)
                               .Select(m => m.Value)
                               .ToList();
                            }
                        ); ;

                    // Create a dictionary to hold the HashSet for each group
                    //var hashSetsByPenznemAndBeKiKod = new Dictionary<(Penznem Penznem, BeKiKod BeKiKod), HashSet<BevetelKiadas>>();

                    //foreach (var group in groupedByPenznemAndBeKiKod)
                    //{
                    //    // Create a HashSet for each group
                    //    var hashSet = new HashSet<BevetelKiadas>(group);
                    //    hashSetsByPenznemAndBeKiKod[(group.Key.Penznem, group.Key.BeKiKod)] = hashSet;
                    //}

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByPenznemAndBeKiKod)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem} + {kvp.Key.BeKiKod}", $"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}", a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByBeKiKodCheckBoxIsChecked)
                {
                    var groupedByBeKiKod = _selectedBevetelekKiadasok.GroupBy(p => new { p.BeKiKod })
                        .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                // Create a dictionary to store sums by month
                                var monthlySums = g.GroupBy(p => p.TeljesitesiDatum.Month)
                                                   .ToDictionary(
                                                       monthGroup => monthGroup.Key,
                                                        monthGroup =>
                                                        {
                                                            var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                            return GetDataStatisticValueBack(values);
                                                        }
                                                   );

                                // Ensure all months (1-12) are accounted for
                                for (int month = 1; month <= 12; month++)
                                {
                                    if (!monthlySums.ContainsKey(month))
                                    {
                                        monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                    }
                                }
                                // Return the values as a HashSet
                                return monthlySums.OrderBy(m => m.Key)
                               .Select(m => m.Value)
                               .ToList();
                            }
                        ); ;

                    // Create a dictionary to hold the HashSet for each group
                    //var hashSetsByBeKiKod = new Dictionary<BeKiKod, HashSet<BevetelKiadas>>();

                    //foreach (var group in groupedByBeKiKod)
                    //{
                    //    // Create a HashSet for each group
                    //    var hashSet = new HashSet<BevetelKiadas>(group);
                    //    hashSetsByBeKiKod[group.Key.BeKiKod] = hashSet;
                    //}

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByBeKiKod)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.BeKiKod}", kvp.Key.BeKiKod.ToString(), baseColors[a]);
                        AddGroupByDataToCollection(kvp.Key.BeKiKod.ToString(), a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    var groupedByPenznem = _selectedBevetelekKiadasok.GroupBy(p => new { p.Penznem })
                         .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                // Create a dictionary to store sums by month
                                var monthlySums = g.GroupBy(p => p.TeljesitesiDatum.Month)
                                                   .ToDictionary(
                                                       monthGroup => monthGroup.Key,
                                                        monthGroup =>
                                                        {
                                                            var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                            return GetDataStatisticValueBack(values);
                                                        }
                                                   );

                                // Ensure all months (1-12) are accounted for
                                for (int month = 1; month <= 12; month++)
                                {
                                    if (!monthlySums.ContainsKey(month))
                                    {
                                        monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                    }
                                }
                                // Return the values as a HashSet
                                return monthlySums.OrderBy(m => m.Key)
                               .Select(m => m.Value)
                               .ToList();
                            }
                        ); ;

                    // Create a dictionary to hold the HashSet for each group
                    //var hashSetsByPenznem = new Dictionary<Penznem, HashSet<BevetelKiadas>>();

                    //foreach (var group in groupedByPenznem)
                    //{
                    //    // Create a HashSet for each group
                    //    var hashSet = new HashSet<BevetelKiadas>(group);
                    //    hashSetsByPenznem[group.Key.Penznem] = hashSet;
                    //}

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByPenznem)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem}", kvp.Key.Penznem.ToString(), baseColors[a]);
                        AddGroupByDataToCollection(kvp.Key.Penznem.ToString(), a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByDateCheckBoxIsChecked)
                {
                    var groupedByYear = _selectedBevetelekKiadasok
                    .GroupBy(p => p.TeljesitesiDatum.Year)
                    .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            // Create a dictionary to store sums by month
                            var monthlySums = g.GroupBy(p => p.TeljesitesiDatum.Month)
                                               .ToDictionary(
                                                   monthGroup => monthGroup.Key,
                                                    monthGroup =>
                                                    {
                                                        var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                        return GetDataStatisticValueBack(values);
                                                    }
                                               );

                            // Ensure all months (1-12) are accounted for
                            for (int month = 1; month <= 12; month++)
                            {
                                if (!monthlySums.ContainsKey(month))
                                {
                                    monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                }
                            }
                            // Return the values as a HashSet
                            return monthlySums.OrderBy(m => m.Key)
                           .Select(m => m.Value)
                           .ToList();
                        }
                    );

                    int b = 0;
                    foreach (var kvp in groupedByYear)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key}", "Datum_" + kvp.Key.ToString(), baseColors[b]);
                        AddGroupByDataToCollection("Datum_" + kvp.Key.ToString(), b);
                        if (b + 1 > 3)
                            b = 0;
                        else b += 1;
                    }
                }
                else
                {
                    var bevetelekKiadasokAdatsor = new ChartValues<double>();

                    foreach (var a in _selectedBevetelekKiadasok)
                    {
                        bevetelekKiadasokAdatsor.Add(Convert.ToDouble(a.Osszeg));
                    }

                    if(bevetelekKiadasokAdatsor.Count != 0)
                    {
                        AddLineSeries(bevetelekKiadasokAdatsor, "Bevételek és Kiadások", "bevetelekKiadasok", baseColors[0]);
                        AddGroupByDataToCollection("bevetelekKiadasok", 0);
                    }
                }
            }
            else
            {
                if (GroupByKifizetettCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked && GroupByDateCheckBoxIsChecked)
                {
                    var groupedByYearAndPenznem = _selectedKotelezettsegekKovetelesek
                    .GroupBy(p => new { p.KifizetesHatarideje.Year, p.Kifizetett, p.Penznem })
                    .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            // Create a dictionary to store sums by month
                            var monthlySums = g.GroupBy(p => p.KifizetesHatarideje.Month)
                                               .ToDictionary(
                                                   monthGroup => monthGroup.Key,
                                                   monthGroup =>
                                                   {
                                                       var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                       return GetDataStatisticValueBack(values);
                                                   }
                                               );

                            // Ensure all months (1-12) are accounted for
                            for (int month = 1; month <= 12; month++)
                            {
                                if (!monthlySums.ContainsKey(month))
                                {
                                    monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                }
                            }
                            // Return the values as a HashSet
                            return monthlySums.OrderBy(m => m.Key)
                           .Select(m => m.Value)
                           .ToList();
                        }
                    );

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByYearAndPenznem)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Kötelezettségek és Követelések - {kvp.Key.Penznem} + {kvp.Key.Kifizetett} + {kvp.Key.Year}", $"{kvp.Key.Penznem}_{kvp.Key.Kifizetett}_{kvp.Key.Year}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Kifizetett}_{kvp.Key.Year}", a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked && GroupByDateCheckBoxIsChecked)
                {
                    var groupedByYearAndPenznem = _selectedKotelezettsegekKovetelesek
                    .GroupBy(p => new { p.KifizetesHatarideje.Year, p.Kifizetett })
                    .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            // Create a dictionary to store sums by month
                            var monthlySums = g.GroupBy(p => p.KifizetesHatarideje.Month)
                                               .ToDictionary(
                                                   monthGroup => monthGroup.Key,
                                                   monthGroup =>
                                                   {
                                                       var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                       return GetDataStatisticValueBack(values);
                                                   }
                                               );

                            // Ensure all months (1-12) are accounted for
                            for (int month = 1; month <= 12; month++)
                            {
                                if (!monthlySums.ContainsKey(month))
                                {
                                    monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                }
                            }
                            // Return the values as a HashSet
                            return monthlySums.OrderBy(m => m.Key)
                           .Select(m => m.Value)
                           .ToList();
                        }
                    );

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByYearAndPenznem)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Kötelezettségek és Követelések - {kvp.Key.Kifizetett} + {kvp.Key.Year}", "Kifizetett_" + $"{kvp.Key.Kifizetett}_{kvp.Key.Year}", baseColors[a]);
                        AddGroupByDataToCollection("Kifizetett_" + $"{kvp.Key.Kifizetett}_{kvp.Key.Year}", a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByDateCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
                {
                    var groupedByYearAndPenznem = _selectedKotelezettsegekKovetelesek
                        .GroupBy(p => new { p.KifizetesHatarideje.Year, p.Penznem })
                        .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                // Create a dictionary to store sums by month
                                var monthlySums = g.GroupBy(p => p.KifizetesHatarideje.Month)
                                                   .ToDictionary(
                                                       monthGroup => monthGroup.Key,
                                                       monthGroup =>
                                                       {
                                                           var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                           return GetDataStatisticValueBack(values);
                                                       }
                                                   );

                                // Ensure all months (1-12) are accounted for
                                for (int month = 1; month <= 12; month++)
                                {
                                    if (!monthlySums.ContainsKey(month))
                                    {
                                        monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                    }
                                }
                                // Return the values as a HashSet
                                return monthlySums.OrderBy(m => m.Key)
                               .Select(m => m.Value)
                               .ToList();
                            }
                        );

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByYearAndPenznem)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Kötelezettségek és Követelések - {kvp.Key.Penznem} + {kvp.Key.Year}", $"{kvp.Key.Penznem}_{kvp.Key.Year}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Year}", a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
                {
                    var groupedByPenznemAndKifizetett = _selectedKotelezettsegekKovetelesek.GroupBy(p => new { p.Penznem, p.Kifizetett })
                        .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            // Create a dictionary to store sums by month
                            var monthlySums = g.GroupBy(p => p.KifizetesHatarideje.Month)
                                               .ToDictionary(
                                                   monthGroup => monthGroup.Key,
                                                   monthGroup =>
                                                   {
                                                       var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                       return GetDataStatisticValueBack(values);
                                                   }
                                               );

                            // Ensure all months (1-12) are accounted for
                            for (int month = 1; month <= 12; month++)
                            {
                                if (!monthlySums.ContainsKey(month))
                                {
                                    monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                }
                            }
                            // Return the values as a HashSet
                            return monthlySums.OrderBy(m => m.Key)
                           .Select(m => m.Value)
                           .ToList();
                        }
                    ); ;

                    // Create a dictionary to hold the HashSet for each group
                    //var hashSetsByPenznemAndKifizetett = new Dictionary<(Penznem Penznem, Int16 Kifizetett), HashSet<KotelezettsegKoveteles>>();

                    //foreach (var group in groupedByPenznemAndKifizetett)
                    //{
                    //    // Create a HashSet for each group
                    //    var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                    //    hashSetsByPenznemAndKifizetett[(group.Key.Penznem, group.Key.Kifizetett)] = hashSet;
                    //}

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByPenznemAndKifizetett)
                    {
                        var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();
                        foreach (var kotelezettsegKoveteles in kvp.Value)
                        {
                            kotelezettsegekKovetelesekAdatsor.Add(Convert.ToDouble(kotelezettsegKoveteles));
                        }
                        AddLineSeries(kotelezettsegekKovetelesekAdatsor, $"Kotelezettségek és Követelések - {kvp.Key.Penznem} + {kvp.Key.Kifizetett}", $"{kvp.Key.Penznem}_{kvp.Key.Kifizetett}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Kifizetett}", a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked)
                {
                    var groupedByKifizetett = _selectedKotelezettsegekKovetelesek.GroupBy(p => new { p.Kifizetett })
                        .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            // Create a dictionary to store sums by month
                            var monthlySums = g.GroupBy(p => p.KifizetesHatarideje.Month)
                                               .ToDictionary(
                                                   monthGroup => monthGroup.Key,
                                                   monthGroup =>
                                                   {
                                                       var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                       return GetDataStatisticValueBack(values);
                                                   }
                                               );

                            // Ensure all months (1-12) are accounted for
                            for (int month = 1; month <= 12; month++)
                            {
                                if (!monthlySums.ContainsKey(month))
                                {
                                    monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                }
                            }
                            // Return the values as a HashSet
                            return monthlySums.OrderBy(m => m.Key)
                           .Select(m => m.Value)
                           .ToList();
                        }
                    ); ;

                    // Create a dictionary to hold the HashSet for each group
                    //var hashSetsByKifizetett = new Dictionary<Int16, HashSet<KotelezettsegKoveteles>>();

                    //foreach (var group in groupedByKifizetett)
                    //{
                    //    // Create a HashSet for each group
                    //    var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                    //    hashSetsByKifizetett[group.Key.Kifizetett] = hashSet;
                    //}

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByKifizetett)
                    {
                        var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();
                        foreach (var kotelezettsegKoveteles in kvp.Value)
                        {
                            kotelezettsegekKovetelesekAdatsor.Add(Convert.ToDouble(kotelezettsegKoveteles));
                        }
                        AddLineSeries(kotelezettsegekKovetelesekAdatsor, $"Kotelezettségek és Követelések - {kvp.Key.Kifizetett}", "Kifizetett_" + kvp.Key.Kifizetett.ToString(), baseColors[a]);
                        AddGroupByDataToCollection("Kifizetett_" + kvp.Key.Kifizetett.ToString(), a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    var groupedByPenznem = _selectedKotelezettsegekKovetelesek.GroupBy(p => new { p.Penznem })
                        .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            // Create a dictionary to store sums by month
                            var monthlySums = g.GroupBy(p => p.KifizetesHatarideje.Month)
                                               .ToDictionary(
                                                   monthGroup => monthGroup.Key,
                                                   monthGroup =>
                                                   {
                                                       var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                       return GetDataStatisticValueBack(values);
                                                   }
                                               );

                            // Ensure all months (1-12) are accounted for
                            for (int month = 1; month <= 12; month++)
                            {
                                if (!monthlySums.ContainsKey(month))
                                {
                                    monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                }
                            }
                            // Return the values as a HashSet
                            return monthlySums.OrderBy(m => m.Key)
                           .Select(m => m.Value)
                           .ToList();
                        }
                    ); ;

                    // Create a dictionary to hold the HashSet for each group
                    //var hashSetsByPenznem = new Dictionary<Penznem, HashSet<KotelezettsegKoveteles>>();

                    //foreach (var group in groupedByPenznem)
                    //{
                    //    // Create a HashSet for each group
                    //    var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                    //    hashSetsByPenznem[group.Key.Penznem] = hashSet;
                    //}

                    int a = 0;
                    // Display the results
                    foreach (var kvp in groupedByPenznem)
                    {
                        var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();
                        foreach (var kotelezettsegKoveteles in kvp.Value)
                        {
                            kotelezettsegekKovetelesekAdatsor.Add(Convert.ToDouble(kotelezettsegKoveteles));
                        }
                        AddLineSeries(kotelezettsegekKovetelesekAdatsor, $"Kotelezettségek és Követelések - {kvp.Key.Penznem}", kvp.Key.Penznem.ToString(), baseColors[a]);
                        AddGroupByDataToCollection(kvp.Key.Penznem.ToString(), a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByDateCheckBoxIsChecked)
                {
                    var groupedByYear = _selectedKotelezettsegekKovetelesek
                    .GroupBy(p => p.KifizetesHatarideje.Year)
                    .ToDictionary(
                        g => g.Key,
                        g =>
                        {
                            // Create a dictionary to store sums by month
                            var monthlySums = g.GroupBy(p => p.KifizetesHatarideje.Month)
                                               .ToDictionary(
                                                   monthGroup => monthGroup.Key,
                                                   monthGroup =>
                                                   {
                                                       var values = monthGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                       return GetDataStatisticValueBack(values);
                                                   }
                                               );

                            // Ensure all months (1-12) are accounted for
                            for (int month = 1; month <= 12; month++)
                            {
                                if (!monthlySums.ContainsKey(month))
                                {
                                    monthlySums[month] = 0; // Add a month with a sum of 0 if it's missing
                                }
                            }
                            // Return the values as a HashSet
                            return monthlySums.OrderBy(m => m.Key)
                           .Select(m => m.Value)
                           .ToList();
                        }
                    );

                    int b = 0;
                    foreach (var kvp in groupedByYear)
                    {
                        var bevetelekKiadasokAdatsor2 = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor2.Add(Convert.ToDouble(bevetelKiadas));
                        }
                        var totalBevetelekKiadasokAdatsor2 = bevetelekKiadasokAdatsor2.Sum(x => x);
                        AddLineSeries(bevetelekKiadasokAdatsor2, $"Kötelezettségek és Követelések - {kvp.Key}", "Datum_" + kvp.Key.ToString(), baseColors[b]);
                        AddGroupByDataToCollection("Datum_" + kvp.Key.ToString(), b);
                        if (b + 1 > 3)
                            b = 0;
                        else b += 1;
                    }
                }
                else
                {
                    var kotelezetsegekKovetelesekAdatsor = new ChartValues<double>();

                    foreach (var a in _selectedKotelezettsegekKovetelesek)
                    {
                        kotelezetsegekKovetelesekAdatsor.Add(Convert.ToDouble(a.Osszeg));
                    }
                    
                    if(GroupBySelections.Count != 0)
                    {
                        AddLineSeries(kotelezetsegekKovetelesekAdatsor, "Kötelezettségek és Követelések", "kotKov", baseColors[1]);
                        AddGroupByDataToCollection("kotKov", 1);
                    }
                }
            }
            if(GroupBySelections.Count == 0)
            {
                LineSeries.Clear();
            }
            OnPropertyChanged(nameof(LineSeries));
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
            if(SeriesType == "LineSeries")
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
           else if(SeriesType == "DoughnutSeries")
            {
                foreach (var a in PieSeries)
                {
                    if (a is PieSeries pieSeries)
                    {
                        if (pieSeries.Name == name)
                        {
                            if (isSelected)
                            {
                                pieSeries.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                pieSeries.Visibility = Visibility.Hidden;
                            }
                        }
                    }
                }
            }
            else if (SeriesType == "RowSeries")
            {
                foreach (var a in RowSeries)
                {
                    if (a is RowSeries rowSeries)
                    {
                        if (rowSeries.Name == name)
                        {
                            if (isSelected)
                            {
                                rowSeries.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                rowSeries.Visibility = Visibility.Hidden;
                            }
                        }
                    }
                }
            }
            else if (SeriesType == "BasicColumnSeries")
            {
                foreach (var a in ColumnSeries)
                {
                    if (a is ColumnSeries columnSeries)
                    {
                        if (columnSeries.Name == name)
                        {
                            if (isSelected)
                            {
                                columnSeries.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                columnSeries.Visibility = Visibility.Hidden;
                            }
                        }
                    }
                }
            }
            else if (SeriesType == "StackedColumnSeries")
            {
                foreach (var a in StackedColumnSeries)
                {
                    if (a is StackedColumnSeries stackedColumnSeries)
                    {
                        if (stackedColumnSeries.Name == name)
                        {
                            if (isSelected)
                            {
                                stackedColumnSeries.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                stackedColumnSeries.Visibility = Visibility.Hidden;
                            }
                        }
                    }
                }
            }
        }

        public void SetRowSeries()
        {
            RowSeries = new SeriesCollection();
            GroupBySelections.Clear();

            if (IsBevetelekKiadasokTabIsSelected)
            {
                if (GroupByYearCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Év") && SelectedAdatsorok.Contains("Hónap"))
                    {
                        List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        var groupedByYearAndMonth = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.TeljesitesiDatum.Year, p.TeljesitesiDatum.Month })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var yearsDict = _selectedBevetelekKiadasok
           .GroupBy(x => x.TeljesitesiDatum.Year)
           .ToDictionary(
               g => g.Key,
               g =>
               {
                   var monthlyValues = Enumerable.Range(1, 12)
                       .ToDictionary(
                           month => month,
                           month =>
                           {
                               var matchingGroup = groupedByYearAndMonth
                                   .Where(kvp => kvp.Key.Year == g.Key && kvp.Key.Month == month)
                                   .SelectMany(kvp => kvp.Value)
                                   .ToList();
                               var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                           }
                       );

                   return monthlyValues;
               }
           );

                        List<string> labels = new List<string>();
                        foreach (var year in yearsDict)
                        {
                            labels.Add(year.Key.ToString());
                        }
                        var monthlyData = new List<ChartValues<double>>();
                        for (int i = 0; i < 12; i++)
                        {
                            monthlyData.Add(new ChartValues<double>());
                        }
                        var sortedYears = yearsDict.OrderBy(y => y.Key);

                        for (int month = 1; month <= 12; month++)
                        {
                            foreach (var year in sortedYears)
                            {
                                double monthValue = year.Value[month];
                                monthlyData[month - 1].Add(monthValue);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in monthlyData)
                        {
                            AddRowSeries(b, $"Bevételek és Kiadások - {honapok[c]}", honapok[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(honapok[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Év") && SelectedCimkek.Contains("Hónap"))
                    {
                        List<string> labels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        // Create a Dictionary<int, Dictionary<int, double>> where the first key is month (1-12),
                        // and the second dictionary maps years to values for that month
                        var monthDict = Enumerable.Range(1, 12)
                            .ToDictionary(
                                month => month,
                                month => _selectedBevetelekKiadasok
                                    .GroupBy(p => p.TeljesitesiDatum.Year)
                                    .ToDictionary(
                                        yearGroup => yearGroup.Key,
                                        yearGroup => {
                                            // Find items for this specific year and month
                                            var itemsForYearAndMonth = _selectedBevetelekKiadasok
                                                .Where(p => p.TeljesitesiDatum.Year == yearGroup.Key &&
                                                           p.TeljesitesiDatum.Month == month)
                                                .ToList();

                                            // Calculate value using existing items or return 0 if none exist
                                            if (itemsForYearAndMonth.Any())
                                            {
                                                var values = itemsForYearAndMonth.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return GetDataStatisticValueBack(values);
                                            }
                                            return 0.0;
                                        }
                                    )
                            );


                        List<ChartValues<double>> yearlyData = new List<ChartValues<double>>();


                        List<int> years = new List<int>();
                        foreach (var year in _selectedBevetelekKiadasok.GroupBy(x => x.TeljesitesiDatum.Year))
                        {
                            years.Add(year.Key);
                        }

                        for (int i = 0; i < years.Count; i++)
                        {
                            yearlyData.Add(new ChartValues<double>());
                        }
                        for (int i = 0; i < years.Count; i++)
                        {
                            foreach (var month in monthDict)
                            {
                                yearlyData[i].Add(month.Value[years[i]]);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in yearlyData)
                        {
                            AddRowSeries(b, $"Bevételek és Kiadások - {years[c]}", "Év_" + years[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection("Év_" + years[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;

                        }
                    }
                }
                else if (GroupByBeKiKodCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Pénznem") && SelectedCimkek.Contains("BeKiKód"))
                    {
                        var groupedByBeKiKodPenznem = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.BeKiKod, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g =>
                               {
                                   var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                   return g.Any() ? GetDataStatisticValueBack(v) : 0;
                               }
                           );
                        ChartValues<double> values = new ChartValues<double>();
                        List<string> labels = new List<string>();
                        foreach (var beKiKodPenznem in groupedByBeKiKodPenznem)
                        {
                            labels.Add(beKiKodPenznem.Key.ToString());
                            values.Add(beKiKodPenznem.Value);
                        }

                        AddRowSeries(values, $"Bevételek és Kiadások - BeKiKód és Pénznem", "BeKiKodPenznem", baseColors[0], labels);
                        AddGroupByDataToCollection("BeKiKodPenznem", 0);
                    }
                    else if (SelectedCimkek.Contains("Pénznem") && SelectedAdatsorok.Contains("BeKiKód"))
                    {
                        List<string> beKiKodok = new List<string> { "Be1", "Be2", "Ki1", "Ki2" };
                        var groupedByBeKiKodPenznem = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.BeKiKod, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var penznemDict = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    var beKiKodValues = System.Enum.GetValues(typeof(BeKiKod))
                                        .Cast<BeKiKod>()
                                        .ToDictionary(
                                            beKiKod => beKiKod,
                                            beKiKod =>
                                            {
                                                var matchingGroup = groupedByBeKiKodPenznem
                                                    .Where(kvp => kvp.Key.BeKiKod == beKiKod && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return beKiKodValues;
                                }
                            );

                        List<string> labels = new List<string>();
                        foreach (var penznem in penznemDict)
                        {
                            labels.Add(penznem.Key.ToString());
                        }
                        var beKiKodData = System.Enum.GetValues(typeof(BeKiKod))
                            .Cast<BeKiKod>()
                            .ToDictionary(
                                beKiKod => beKiKod,
                                beKiKod =>
                                {
                                    return new ChartValues<double>();
                                }
                            );
                        var sortedPenznem = penznemDict.OrderBy(y => y.Key);
                        foreach (var penznem in sortedPenznem)
                        {
                            foreach (var beKiKod in System.Enum.GetValues(typeof(BeKiKod)).Cast<BeKiKod>())
                            {
                                beKiKodData[beKiKod].Add(penznem.Value[beKiKod]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in beKiKodData)
                        {
                            AddRowSeries(b.Value, $"Bevételek és Kiadások - {beKiKodok[c]}", beKiKodok[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(beKiKodok[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Pénznem") && SelectedCimkek.Contains("BeKiKód"))
                    {
                        List<string> penznemek = new List<string> { "Forint", "Euró", "Font", "Dollár" };
                        var groupedByBeKiKodPenznem = _selectedBevetelekKiadasok
                           .GroupBy(p => new { p.BeKiKod, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var beKiKodDict = System.Enum.GetValues(typeof(BeKiKod))
                            .Cast<BeKiKod>()
                            .ToDictionary(
                                beKiKod => beKiKod,
                                beKiKod =>
                                {
                                    var penznemValues = System.Enum.GetValues(typeof(Penznem))
                                        .Cast<Penznem>()
                                        .ToDictionary(
                                            penznem => penznem,
                                            penznem =>
                                            {
                                                var matchingGroup = groupedByBeKiKodPenznem
                                                    .Where(kvp => kvp.Key.BeKiKod == beKiKod && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return penznemValues;
                                }
                            );

                        List<string> labels = new List<string>();
                        foreach (var bekikod in beKiKodDict)
                        {
                            labels.Add(bekikod.Key.ToString());
                        }
                        var penznemData = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    return new ChartValues<double>();
                                }
                            );
                        var sortedBeKiKod = beKiKodDict.OrderBy(y => y.Key);
                        foreach (var bekikod in sortedBeKiKod)
                        {
                            foreach (var penznem in System.Enum.GetValues(typeof(Penznem)).Cast<Penznem>())
                            {
                                penznemData[penznem].Add(bekikod.Value[penznem]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in penznemData)
                        {
                            AddRowSeries(b.Value, $"Bevételek és Kiadások - {penznemek[c]}", penznemek[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(penznemek[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                }
                else if (GroupByBeKiKodCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Count > 0)
                    {
                        // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                        var groupedByBeKiKod = System.Enum.GetValues(typeof(BeKiKod))
                            .Cast<BeKiKod>()
                            .ToDictionary(
                                bekikod => bekikod,
                                bekikod => new HashSet<BevetelKiadas>(
                                    _selectedBevetelekKiadasok.Where(x => x.BeKiKod == bekikod)
                                )
                            );

                        List<string> labels = new List<string>();
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();

                        // Display the results
                        foreach (var kvp in groupedByBeKiKod)
                        {
                            labels.Add(kvp.Key.ToString());
                            switch (SelectedDataStatistics)
                            {
                                case "Nincs kiválasztva":
                                    bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                                case "Összeg":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Átlag":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Mértani Közép":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Minimum Érték":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Maximum Érték":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Értékek Szórása":
                                    bevetelekKiadasokAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                default:
                                    bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                            }
                        }

                        AddRowSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - BeKiKod", "BeKiKod", baseColors[0], labels);
                        AddGroupByDataToCollection("BeKiKod", 0);
                    }
                    else
                    {
                        //ERROR TODO
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                    var groupedByPenznem = System.Enum.GetValues(typeof(Penznem))
                        .Cast<Penznem>()
                        .ToDictionary(
                            penznem => penznem,
                            penznem => new HashSet<BevetelKiadas>(
                                _selectedBevetelekKiadasok.Where(x => x.Penznem == penznem)
                            )
                        );

                    List<string> labels = new List<string>();
                    var bevetelekKiadasokAdatsor = new ChartValues<double>();

                    // Display the results
                    foreach (var kvp in groupedByPenznem)
                    {
                        labels.Add(kvp.Key.ToString());
                        switch (SelectedDataStatistics)
                        {
                            case "Nincs kiválasztva":
                                bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                            case "Összeg":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Átlag":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Mértani Közép":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Minimum Érték":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Maximum Érték":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Értékek Szórása":
                                bevetelekKiadasokAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            default:
                                bevetelekKiadasokAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                        }
                    }

                    AddRowSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - Pénznem", "Pénznem", baseColors[0], labels);
                    AddGroupByDataToCollection("Pénznem", 0);
                }
                else if (GroupByYearCheckBoxIsChecked)
                {
                    var groupedByYear = _selectedBevetelekKiadasok
                       .GroupBy(p => new { p.TeljesitesiDatum.Year })
                       .ToDictionary(
                           g => g.Key,
                           g =>
                           {
                               var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return g.Any() ? GetDataStatisticValueBack(v) : 0;
                           }
                       );

                    List<string> labels = new List<string>();
                    ChartValues<double> values = new ChartValues<double>();
                    foreach (var year in groupedByYear)
                    {
                        labels.Add(year.Key.ToString());
                        values.Add(year.Value);
                    }

                    var sortedYears = groupedByYear.OrderBy(y => y.Key);

                    AddRowSeries(values, $"Bevételek és Kiadások - Év", "Év", baseColors[0], labels);
                    AddGroupByDataToCollection("Év", 0);
                }
                else if (GroupByMonthCheckBoxIsChecked)
                {

                    List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    var monthValuesDict = Enumerable.Range(1, 12)
                        .ToDictionary(
                            month => month,
                            month => 0.0  // Default value for months with no data
                        );

                    // Group by month and calculate statistics for each month that has data
                    var groupedByMonth = _selectedBevetelekKiadasok
                        .GroupBy(p => p.TeljesitesiDatum.Month)
                        .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                return g.Any() ? GetDataStatisticValueBack(v) : 0;
                            }
                        );

                    // Update our complete dictionary with values from grouped data
                    foreach (var item in groupedByMonth)
                    {
                        monthValuesDict[item.Key] = item.Value;
                    }

                    // Convert to ordered list for display
                    List<string> labels = honapok;
                    ChartValues<double> values = new ChartValues<double>(
                        Enumerable.Range(1, 12).Select(month => monthValuesDict[month])
                    );

                    AddRowSeries(values, $"Bevételek és Kiadások - Hónapok", "Hónapok", baseColors[0], honapok);
                    AddGroupByDataToCollection("Hónapok", 0);
                }
                else
                {
                    //TODO ERROR
                }
            }
            else
            {
                if (GroupByYearCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Év") && SelectedAdatsorok.Contains("Hónap"))
                    {
                        List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        var groupedByYearAndMonth = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.KifizetesHatarideje.Year, p.KifizetesHatarideje.Month })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var yearsDict = _selectedKotelezettsegekKovetelesek
           .GroupBy(x => x.KifizetesHatarideje.Year)
           .ToDictionary(
               g => g.Key,
               g =>
               {
                   var monthlyValues = Enumerable.Range(1, 12)
                       .ToDictionary(
                           month => month,
                           month =>
                           {
                               var matchingGroup = groupedByYearAndMonth
                                   .Where(kvp => kvp.Key.Year == g.Key && kvp.Key.Month == month)
                                   .SelectMany(kvp => kvp.Value)
                                   .ToList();
                               var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                           }
                       );

                   return monthlyValues;
               }
           );

                        List<string> labels = new List<string>();
                        foreach (var year in yearsDict)
                        {
                            labels.Add(year.Key.ToString());
                        }
                        var monthlyData = new List<ChartValues<double>>();
                        for (int i = 0; i < 12; i++)
                        {
                            monthlyData.Add(new ChartValues<double>());
                        }
                        var sortedYears = yearsDict.OrderBy(y => y.Key);

                        for (int month = 1; month <= 12; month++)
                        {
                            foreach (var year in sortedYears)
                            {
                                double monthValue = year.Value[month];
                                monthlyData[month - 1].Add(monthValue);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in monthlyData)
                        {
                            AddRowSeries(b, $"Kötelezettségek és Követelések - {honapok[c]}", honapok[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(honapok[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Év") && SelectedCimkek.Contains("Hónap"))
                    {
                        List<string> labels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        // Create a Dictionary<int, Dictionary<int, double>> where the first key is month (1-12),
                        // and the second dictionary maps years to values for that month
                        var monthDict = Enumerable.Range(1, 12)
                            .ToDictionary(
                                month => month,
                                month => _selectedKotelezettsegekKovetelesek
                                    .GroupBy(p => p.KifizetesHatarideje.Year)
                                    .ToDictionary(
                                        yearGroup => yearGroup.Key,
                                        yearGroup => {
                                            // Find items for this specific year and month
                                            var itemsForYearAndMonth = _selectedKotelezettsegekKovetelesek
                                                .Where(p => p.KifizetesHatarideje.Year == yearGroup.Key &&
                                                           p.KifizetesHatarideje.Month == month)
                                                .ToList();

                                            // Calculate value using existing items or return 0 if none exist
                                            if (itemsForYearAndMonth.Any())
                                            {
                                                var values = itemsForYearAndMonth.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return GetDataStatisticValueBack(values);
                                            }
                                            return 0.0;
                                        }
                                    )
                            );


                        List<ChartValues<double>> yearlyData = new List<ChartValues<double>>();


                        List<int> years = new List<int>();
                        foreach (var year in _selectedKotelezettsegekKovetelesek.GroupBy(x => x.KifizetesHatarideje.Year))
                        {
                            years.Add(year.Key);
                        }

                        for (int i = 0; i < years.Count; i++)
                        {
                            yearlyData.Add(new ChartValues<double>());
                        }
                        for (int i = 0; i < years.Count; i++)
                        {
                            foreach (var month in monthDict)
                            {
                                yearlyData[i].Add(month.Value[years[i]]);
                            }
                        }
                        int a = 0;
                        int c = 0;
                        foreach (var b in yearlyData)
                        {
                            AddRowSeries(b, $"Kötelezettségek és Követelések - {years[c]}", "Év_" + years[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection("Év_" + years[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;

                        }
                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked && GroupByPenznemCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Contains("Pénznem") && SelectedCimkek.Contains("Kifizetett"))
                    {
                        var groupedKifizetettPenznem = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.Kifizetett, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g =>
                               {
                                   var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                   return g.Any() ? GetDataStatisticValueBack(v) : 0;
                               }
                           );
                        ChartValues<double> values = new ChartValues<double>();
                        List<string> labels = new List<string>();
                        foreach (var kifizetettPenznem in groupedKifizetettPenznem)
                        {
                            labels.Add(kifizetettPenznem.Key.ToString());
                            values.Add(kifizetettPenznem.Value);
                        }

                        AddRowSeries(values, $"Kötelezettségek és Követelések - Kifizetett és Pénznem", "KifizetettPenznem", baseColors[0], labels);
                        AddGroupByDataToCollection("KifizetettPenznem", 0);
                    }
                    else if (SelectedCimkek.Contains("Pénznem") && SelectedAdatsorok.Contains("Kifizetett"))
                    {
                        List<string> kifizetettLabels = new List<string> { "0", "1" };
                        var groupedByKifizetettPenznem = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.Kifizetett, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var penznemDict = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    var kifizetettValues = _selectedKotelezettsegekKovetelesek
                                        .GroupBy(x => x.Kifizetett)
                                        .ToDictionary(
                                            kifizetett => kifizetett.Key,
                                            kifizetett =>
                                            {
                                                var matchingGroup = groupedByKifizetettPenznem
                                                    .Where(kvp => kvp.Key.Kifizetett == kifizetett.Key && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return kifizetettValues;
                                }
                            );

                        List<string> labels = new List<string>();
                        foreach (var penznem in penznemDict)
                        {
                            labels.Add(penznem.Key.ToString());
                        }
                        var kifizetettData = new List<ChartValues<double>>();
                        for (int i = 0; i < 2; i++)
                        {
                            kifizetettData.Add(new ChartValues<double>());
                        }

                        var sortedPenznem = penznemDict.OrderBy(y => y.Key);
                        foreach (var penznem in sortedPenznem)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                kifizetettData[i].Add(penznem.Value[(short)i]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in kifizetettData)
                        {
                            AddRowSeries(b, $"Kötelezettségek és Követelések - {kifizetettLabels[c]}", "Kifizetett_" + kifizetettLabels[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection("Kifizetett_" + kifizetettLabels[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                    else if (SelectedAdatsorok.Contains("Pénznem") && SelectedCimkek.Contains("Kifizetett"))
                    {
                        List<string> penznemek = new List<string> { "Forint", "Euró", "Font", "Dollár" };
                        var groupedByBeKiKodPenznem = _selectedKotelezettsegekKovetelesek
                           .GroupBy(p => new { p.Kifizetett, p.Penznem })
                           .ToDictionary(
                               g => g.Key,
                               g => g.ToList()
                           );
                        var beKiKodDict = _selectedKotelezettsegekKovetelesek
                            .GroupBy(x => x.Kifizetett)
                            .ToDictionary(
                                kifizetett => kifizetett,
                                kifizetett =>
                                {
                                    var penznemValues = System.Enum.GetValues(typeof(Penznem))
                                        .Cast<Penznem>()
                                        .ToDictionary(
                                            penznem => penznem,
                                            penznem =>
                                            {
                                                var matchingGroup = groupedByBeKiKodPenznem
                                                    .Where(kvp => kvp.Key.Kifizetett == kifizetett.Key && kvp.Key.Penznem == penznem)
                                                    .SelectMany(kvp => kvp.Value)
                                                    .ToList();
                                                var values = matchingGroup.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                                return matchingGroup.Any() ? GetDataStatisticValueBack(values) : 0;
                                            }
                                        );

                                    return penznemValues;
                                }
                            );

                        List<string> labels = new List<string>() { "0", "1" };

                        var penznemData = System.Enum.GetValues(typeof(Penznem))
                            .Cast<Penznem>()
                            .ToDictionary(
                                penznem => penznem,
                                penznem =>
                                {
                                    return new ChartValues<double>();
                                }
                            );
                        var sortedBeKiKod = beKiKodDict.OrderBy(y => y.Key.Key);
                        foreach (var bekikod in sortedBeKiKod)
                        {
                            foreach (var penznem in System.Enum.GetValues(typeof(Penznem)).Cast<Penznem>())
                            {
                                penznemData[penznem].Add(bekikod.Value[penznem]);
                            }
                        }

                        int a = 0;
                        int c = 0;
                        foreach (var b in penznemData)
                        {
                            AddRowSeries(b.Value, $"Kötelezettségek és Követelések - {penznemek[c]}", penznemek[c].ToString(), baseColors[a], labels);
                            AddGroupByDataToCollection(penznemek[c].ToString(), a);
                            if (a + 1 > 3)
                                a = 0;
                            else
                                a++;
                            c++;
                        }
                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked)
                {
                    if (SelectedCimkek.Count > 0)
                    {
                        // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                        var groupedByKifizetett = _selectedKotelezettsegekKovetelesek
                            .GroupBy(x => x.Kifizetett)
                            .ToDictionary(
                                kifizetett => kifizetett,
                                kifizetett => new HashSet<KotelezettsegKoveteles>(
                                    _selectedKotelezettsegekKovetelesek.Where(x => x.Kifizetett == kifizetett.Key)
                                )
                            );

                        List<string> labels = new List<string>();
                        var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();

                        // Display the results
                        foreach (var kvp in groupedByKifizetett)
                        {
                            labels.Add(kvp.Key.Key.ToString());
                            switch (SelectedDataStatistics)
                            {
                                case "Nincs kiválasztva":
                                    kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                                case "Összeg":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Átlag":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Mértani Közép":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Minimum Érték":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Maximum Érték":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                case "Értékek Szórása":
                                    kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                    break;
                                default:
                                    kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                    break;
                            }
                        }

                        AddRowSeries(kotelezettsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - Kifizetett", "Kifizetett", baseColors[0], labels);
                        AddGroupByDataToCollection("Kifizetett", 0);
                    }
                    else
                    {
                        //ERROR TODO
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    // Modify the grouping logic to include all Penznem enum values, even if there are no matching entries.
                    var groupedByPenznem = System.Enum.GetValues(typeof(Penznem))
                        .Cast<Penznem>()
                        .ToDictionary(
                            penznem => penznem,
                            penznem => new HashSet<KotelezettsegKoveteles>(
                                _selectedKotelezettsegekKovetelesek.Where(x => x.Penznem == penznem)
                            )
                        );

                    List<string> labels = new List<string>();
                    var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();

                    // Display the results
                    foreach (var kvp in groupedByPenznem)
                    {
                        labels.Add(kvp.Key.ToString());
                        switch (SelectedDataStatistics)
                        {
                            case "Nincs kiválasztva":
                                kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                            case "Összeg":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetSum(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Átlag":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetAvarage(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Mértani Közép":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMedian(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Minimum Érték":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMinimumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Maximum Érték":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetMaximumValue(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            case "Értékek Szórása":
                                kotelezettsegekKovetelesekAdatsor.Add(DataStatistics.GetStandardDeviation(kvp.Value.Select(x => (int)x.Osszeg).ToList()));
                                break;
                            default:
                                kotelezettsegekKovetelesekAdatsor.Add(kvp.Value.Sum(x => x.Osszeg));
                                break;
                        }
                    }

                    AddRowSeries(kotelezettsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - Pénznem", "Pénznem", baseColors[0], labels);
                    AddGroupByDataToCollection("Pénznem", 0);
                }
                else if (GroupByYearCheckBoxIsChecked)
                {
                    var groupedByYear = _selectedKotelezettsegekKovetelesek
                       .GroupBy(p => new { p.KifizetesHatarideje.Year })
                       .ToDictionary(
                           g => g.Key,
                           g =>
                           {
                               var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                               return g.Any() ? GetDataStatisticValueBack(v) : 0;
                           }
                       );

                    List<string> labels = new List<string>();
                    ChartValues<double> values = new ChartValues<double>();
                    foreach (var year in groupedByYear)
                    {
                        labels.Add(year.Key.ToString());
                        values.Add(year.Value);
                    }

                    var sortedYears = groupedByYear.OrderBy(y => y.Key);

                    AddRowSeries(values, $"Kötelezettségek és Követelések - Év", "Év", baseColors[0], labels);
                    AddGroupByDataToCollection("Év", 0);
                }
                else if (GroupByMonthCheckBoxIsChecked)
                {

                    List<string> honapok = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    var monthValuesDict = Enumerable.Range(1, 12)
                        .ToDictionary(
                            month => month,
                            month => 0.0  // Default value for months with no data
                        );

                    // Group by month and calculate statistics for each month that has data
                    var groupedByMonth = _selectedKotelezettsegekKovetelesek
                        .GroupBy(p => p.KifizetesHatarideje.Month)
                        .ToDictionary(
                            g => g.Key,
                            g =>
                            {
                                var v = g.Select(x => Convert.ToDouble(x.Osszeg)).ToList();
                                return g.Any() ? GetDataStatisticValueBack(v) : 0;
                            }
                        );

                    // Update our complete dictionary with values from grouped data
                    foreach (var item in groupedByMonth)
                    {
                        monthValuesDict[item.Key] = item.Value;
                    }

                    // Convert to ordered list for display
                    List<string> labels = honapok;
                    ChartValues<double> values = new ChartValues<double>(
                        Enumerable.Range(1, 12).Select(month => monthValuesDict[month])
                    );

                    AddRowSeries(values, $"Kötelezettségek és Követelések - Hónapok", "Hónapok", baseColors[0], honapok);
                    AddGroupByDataToCollection("Hónapok", 0);
                }
                else
                {
                    //TODO ERROR
                }
            }

            OnPropertyChanged(nameof(RowSeries));
        }

        public void SetDoughnutSeries()
        {
            //NORMAL SETTING STARTS
            PieSeries = new SeriesCollection();
            GroupBySelections.Clear();

            if (IsBevetelekKiadasokTabIsSelected)
            {
                if (GroupByPenznemCheckBoxIsChecked && GroupByBeKiKodCheckBoxIsChecked && GroupByYearCheckBoxIsChecked)
                {
                    var bevetelekKiadasokGroupedByYearAndPenznemAndBeKiKod = _selectedBevetelekKiadasok.GroupBy(x => new { x.TeljesitesiDatum.Year, x.Penznem, x.BeKiKod });
                    var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Sum(x => x.Osszeg);
                    var hashSetsByYearAndPenznemAndBeKiKod = new Dictionary<(int Datum, Penznem Penznem, BeKiKod BeKiKod), HashSet<BevetelKiadas>>();

                    foreach (var group in bevetelekKiadasokGroupedByYearAndPenznemAndBeKiKod)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<BevetelKiadas>(group);
                        hashSetsByYearAndPenznemAndBeKiKod[(group.Key.Year, group.Key.Penznem, group.Key.BeKiKod)] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByYearAndPenznemAndBeKiKod)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem}_{kvp.Key.BeKiKod}_{kvp.Key.Datum}", $"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}_{kvp.Key.Datum}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}_{kvp.Key.Datum}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked && GroupByBeKiKodCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedYear != 0)
                    {
                        var bevetelekKiadasokGroupedByMonthAndPenznemAndBeKiKod = _selectedBevetelekKiadasok.Where(x => x.TeljesitesiDatum.Year == SelectedYear).GroupBy(x => new { x.TeljesitesiDatum.Month, x.Penznem, x.BeKiKod });
                        var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Where(x => x.TeljesitesiDatum.Year == SelectedYear).Sum(x => x.Osszeg);
                        var hashSetsByMonthAndPenznemAndBeKiKod = new Dictionary<(int Datum, Penznem Penznem, BeKiKod BeKiKod), HashSet<BevetelKiadas>>();

                        foreach (var group in bevetelekKiadasokGroupedByMonthAndPenznemAndBeKiKod)
                        {
                            // Create a HashSet for each group
                            var hashSet = new HashSet<BevetelKiadas>(group);
                            hashSetsByMonthAndPenznemAndBeKiKod[(group.Key.Month, group.Key.Penznem, group.Key.BeKiKod)] = hashSet;
                        }
                        int a = 0;
                        foreach (var kvp in hashSetsByMonthAndPenznemAndBeKiKod)
                        {
                            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                            foreach (var bevetelKiadas in kvp.Value)
                            {
                                bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                            }
                            AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem}_{kvp.Key.BeKiKod}_{kvp.Key.Datum}", $"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}_{kvp.Key.Datum}", baseColors[a]);
                            AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}_{kvp.Key.Datum}", a);
                            if (a + 1 > 3)
                                a = 0;
                            a++;
                        }
                    }
                    else
                    {
                        throw new Exception("Must select a year!");
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedYear != 0)
                    {
                        var bevetelekKiadasokGroupedByMonthAndPenznem = _selectedBevetelekKiadasok.Where(x => x.TeljesitesiDatum.Year == SelectedYear).GroupBy(x => new { x.TeljesitesiDatum.Month, x.Penznem });
                        var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Where(x => x.TeljesitesiDatum.Year == SelectedYear).Sum(x => x.Osszeg);
                        var hashSetsByMonthAndPenznem = new Dictionary<(int Datum, Penznem Penznem), HashSet<BevetelKiadas>>();

                        foreach (var group in bevetelekKiadasokGroupedByMonthAndPenznem)
                        {
                            // Create a HashSet for each group
                            var hashSet = new HashSet<BevetelKiadas>(group);
                            hashSetsByMonthAndPenznem[(group.Key.Month, group.Key.Penznem)] = hashSet;
                        }
                        int a = 0;
                        foreach (var kvp in hashSetsByMonthAndPenznem)
                        {
                            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                            foreach (var bevetelKiadas in kvp.Value)
                            {
                                bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                            }
                            AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem}_{kvp.Key.Datum}", $"{kvp.Key.Penznem}_{kvp.Key.Datum}", baseColors[a]);
                            AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Datum}", a);
                            if (a + 1 > 3)
                                a = 0;
                            a++;
                        }
                    }
                    else 
                    {
                    }
                }
                else if (GroupByBeKiKodCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedYear != 0)
                    {
                        var bevetelekKiadasokGroupedByMonthAndBeKiKod = _selectedBevetelekKiadasok.Where(x => x.TeljesitesiDatum.Year == SelectedYear).GroupBy(x => new { x.TeljesitesiDatum.Month, x.BeKiKod });
                        var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Where(x => x.TeljesitesiDatum.Year == SelectedYear).Sum(x => x.Osszeg);
                        var hashSetsByMonthAndBeKiKod = new Dictionary<(int Datum, BeKiKod BeKiKod), HashSet<BevetelKiadas>>();

                        foreach (var group in bevetelekKiadasokGroupedByMonthAndBeKiKod)
                        {
                            // Create a HashSet for each group
                            var hashSet = new HashSet<BevetelKiadas>(group);
                            hashSetsByMonthAndBeKiKod[(group.Key.Month, group.Key.BeKiKod)] = hashSet;
                        }
                        int a = 0;
                        foreach (var kvp in hashSetsByMonthAndBeKiKod)
                        {
                            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                            foreach (var bevetelKiadas in kvp.Value)
                            {
                                bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                            }
                            AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.BeKiKod}_{kvp.Key.Datum}", $"{kvp.Key.BeKiKod}_{kvp.Key.Datum}", baseColors[a]);
                            AddGroupByDataToCollection($"{kvp.Key.BeKiKod}_{kvp.Key.Datum}", a);
                            if (a + 1 > 3)
                                a = 0;
                            a++;
                        }
                    }
                    else
                    {

                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked && GroupByYearCheckBoxIsChecked)
                {
                    var bevetelekKiadasokGroupedByYearAndPenznem = _selectedBevetelekKiadasok.GroupBy(x => new { x.TeljesitesiDatum.Year, x.Penznem });
                    var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Sum(x => x.Osszeg);
                    var hashSetsByYearAndPenznem = new Dictionary<(int Datum, Penznem Penznem), HashSet<BevetelKiadas>>();

                    foreach (var group in bevetelekKiadasokGroupedByYearAndPenznem)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<BevetelKiadas>(group);
                        hashSetsByYearAndPenznem[(group.Key.Year, group.Key.Penznem)] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByYearAndPenznem)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem}_{kvp.Key.Datum}", $"{kvp.Key.Penznem}_{kvp.Key.Datum}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Datum}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByYearCheckBoxIsChecked && GroupByBeKiKodCheckBoxIsChecked)
                {
                    var bevetelekKiadasokGroupedByYearAndBeKiKod = _selectedBevetelekKiadasok.GroupBy(x => new { x.TeljesitesiDatum.Year, x.BeKiKod });
                    var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Sum(x => x.Osszeg);
                    var hashSetsByYearAndBeKiKod = new Dictionary<(int Datum, BeKiKod BeKiKod), HashSet<BevetelKiadas>>();

                    foreach (var group in bevetelekKiadasokGroupedByYearAndBeKiKod)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<BevetelKiadas>(group);
                        hashSetsByYearAndBeKiKod[(group.Key.Year, group.Key.BeKiKod)] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByYearAndBeKiKod)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.BeKiKod}_{kvp.Key.Datum}", $"{kvp.Key.BeKiKod}_{kvp.Key.Datum}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.BeKiKod}_{kvp.Key.Datum}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked && GroupByBeKiKodCheckBoxIsChecked)
                {
                    var bevetelekKiadasokGroupedByPenznemAndBeKiKod = _selectedBevetelekKiadasok.GroupBy(x => new { x.Penznem, x.BeKiKod });
                    var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Sum(x => x.Osszeg);
                    var hashSetsByPenznemAndBeKiKod = new Dictionary<(Penznem Penznem, BeKiKod BeKiKod), HashSet<BevetelKiadas>>();

                    foreach (var group in bevetelekKiadasokGroupedByPenznemAndBeKiKod)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<BevetelKiadas>(group);
                        hashSetsByPenznemAndBeKiKod[(group.Key.Penznem, group.Key.BeKiKod)] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByPenznemAndBeKiKod)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem}_{kvp.Key.BeKiKod}", $"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.BeKiKod}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    var bevetelekKiadasokGroupedByPenznem = _selectedBevetelekKiadasok.GroupBy(x => x.Penznem);
                    var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Sum(x => x.Osszeg);
                    var hashSetsByPenznem = new Dictionary<Penznem, HashSet<BevetelKiadas>>();

                    foreach (var group in bevetelekKiadasokGroupedByPenznem)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<BevetelKiadas>(group);
                        hashSetsByPenznem[group.Key] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByPenznem)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key}", $"{kvp.Key}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByBeKiKodCheckBoxIsChecked)
                {
                    var bevetelekKiadasokGroupedByBeKiKod = _selectedBevetelekKiadasok.GroupBy(x => x.BeKiKod);
                    var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Sum(x => x.Osszeg);
                    var hashSetsByBeKiKod = new Dictionary<BeKiKod, HashSet<BevetelKiadas>>();

                    foreach (var group in bevetelekKiadasokGroupedByBeKiKod)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<BevetelKiadas>(group);
                        hashSetsByBeKiKod[group.Key] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByBeKiKod)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key}", $"{kvp.Key}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByYearCheckBoxIsChecked)
                {
                    var bevetelekKiadasokGroupedByDatum = _selectedBevetelekKiadasok.GroupBy(x => x.TeljesitesiDatum.Year);
                    var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Sum(x => x.Osszeg);
                    var hashSetsByDatum = new Dictionary<int, HashSet<BevetelKiadas>>();

                    foreach (var group in bevetelekKiadasokGroupedByDatum)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<BevetelKiadas>(group);
                        hashSetsByDatum[group.Key] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByDatum)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key}", $"Év_{kvp.Key}", baseColors[a]);
                        AddGroupByDataToCollection($"Év_{kvp.Key}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedYear != 0)
                    {
                        var bevetelekKiadasokGroupedByDatum = _selectedBevetelekKiadasok.Where(x => x.TeljesitesiDatum.Year == SelectedYear).GroupBy(x => x.TeljesitesiDatum.Month);
                        var totalBevetelekKiadasokAdatsor = _selectedBevetelekKiadasok.Where(x => x.TeljesitesiDatum.Year == SelectedYear).Sum(x => x.Osszeg);
                        var hashSetsByDatum = new Dictionary<int, HashSet<BevetelKiadas>>();

                        foreach (var group in bevetelekKiadasokGroupedByDatum)
                        {
                            // Create a HashSet for each group
                            var hashSet = new HashSet<BevetelKiadas>(group);
                            hashSetsByDatum[group.Key] = hashSet;
                        }
                        int a = 0;
                        foreach (var kvp in hashSetsByDatum)
                        {
                            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                            foreach (var bevetelKiadas in kvp.Value)
                            {
                                bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                            }
                            AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key}", $"Hónap_{kvp.Key}", baseColors[a]);
                            AddGroupByDataToCollection($"Hónap_{kvp.Key}", a);
                            if (a + 1 > 3)
                                a = 0;
                            a++;
                        }
                    }
                    else
                    {
                        throw new Exception("Must select a year!");
                    }
                }
                else
                {
                    var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                    foreach (var a in _selectedBevetelekKiadasok)
                    {
                        bevetelekKiadasokAdatsor.Add(new ObservableValue(a.Osszeg));
                    }
                    var totalBevetelekKiadasokAdatsor = bevetelekKiadasokAdatsor.Sum(x => x.Value);

                    AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, "Bevételek és Kiadások", "Bevételek_és_Kiadások", baseColors[0]);
                }
            }
            else
            {
                if (GroupByPenznemCheckBoxIsChecked && GroupByKifizetettCheckBoxIsChecked && GroupByYearCheckBoxIsChecked)
                {
                    var kotelezetsegekKovetelesekGroupedByYearAndPenznemAndKifizetett = _selectedKotelezettsegekKovetelesek.GroupBy(x => new { x.KifizetesHatarideje.Year, x.Penznem, x.Kifizetett });
                    var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Sum(x => x.Osszeg);
                    var hashSetsByYearAndPenznemAndKifizetett = new Dictionary<(int Datum, Penznem Penznem, Int16 Kifizetett), HashSet<KotelezettsegKoveteles>>();

                    foreach (var group in kotelezetsegekKovetelesekGroupedByYearAndPenznemAndKifizetett)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                        hashSetsByYearAndPenznemAndKifizetett[(group.Key.Year, group.Key.Penznem, group.Key.Kifizetett)] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByYearAndPenznemAndKifizetett)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key.Penznem}_{kvp.Key.Kifizetett}_{kvp.Key.Datum}", $"{kvp.Key.Penznem}_{kvp.Key.Kifizetett}_{kvp.Key.Datum}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Kifizetett}_{kvp.Key.Datum}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked && GroupByKifizetettCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if(SelectedYear != 0)
                    {
                        var kotelezetsegekKovetelesekGroupedByMonthAndPenznemAndKifizetett = _selectedKotelezettsegekKovetelesek.GroupBy(x => new { x.KifizetesHatarideje.Month, x.Penznem, x.Kifizetett });
                        var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Where(x => x.KifizetesHatarideje.Year == SelectedYear).Sum(x => x.Osszeg);
                        var hashSetsByMonthAndPenznemAndKifizetett = new Dictionary<(int Datum, Penznem Penznem, Int16 Kifizetett), HashSet<KotelezettsegKoveteles>>();

                        foreach (var group in kotelezetsegekKovetelesekGroupedByMonthAndPenznemAndKifizetett)
                        {
                            // Create a HashSet for each group
                            var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                            hashSetsByMonthAndPenznemAndKifizetett[(group.Key.Month, group.Key.Penznem, group.Key.Kifizetett)] = hashSet;
                        }
                        int a = 0;
                        foreach (var kvp in hashSetsByMonthAndPenznemAndKifizetett)
                        {
                            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                            foreach (var bevetelKiadas in kvp.Value)
                            {
                                bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                            }
                            AddPieSeries(bevetelekKiadasokAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key.Penznem}_{kvp.Key.Kifizetett}_{kvp.Key.Datum}", $"{kvp.Key.Penznem}_{kvp.Key.Kifizetett}_{kvp.Key.Datum}", baseColors[a]);
                            AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Kifizetett}_{kvp.Key.Datum}", a);
                            if (a + 1 > 3)
                                a = 0;
                            a++;
                        }
                    }
                    else
                    {

                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedYear != 0)
                    {
                        var kotelezetsegekKovetelesekGroupedByMonthAndPenznem = _selectedKotelezettsegekKovetelesek.GroupBy(x => new { x.KifizetesHatarideje.Month, x.Penznem });
                        var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Where(x => x.KifizetesHatarideje.Year == SelectedYear).Sum(x => x.Osszeg);
                        var hashSetsByMonthAndPenznem = new Dictionary<(int Datum, Penznem Penznem), HashSet<KotelezettsegKoveteles>>();

                        foreach (var group in kotelezetsegekKovetelesekGroupedByMonthAndPenznem)
                        {
                            // Create a HashSet for each group
                            var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                            hashSetsByMonthAndPenznem[(group.Key.Month, group.Key.Penznem)] = hashSet;
                        }
                        int a = 0;
                        foreach (var kvp in hashSetsByMonthAndPenznem)
                        {
                            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                            foreach (var bevetelKiadas in kvp.Value)
                            {
                                bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                            }
                            AddPieSeries(bevetelekKiadasokAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key.Penznem}_{kvp.Key.Datum}", $"{kvp.Key.Penznem}_{kvp.Key.Datum}", baseColors[a]);
                            AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Datum}", a);
                            if (a + 1 > 3)
                                a = 0;
                            a++;
                        }
                    }
                    else
                    {

                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked && GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedYear != 0)
                    {
                        var kotelezetsegekKovetelesekGroupedByMonthAndKifizetett = _selectedKotelezettsegekKovetelesek.GroupBy(x => new { x.KifizetesHatarideje.Month, x.Kifizetett });
                        var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Where(x => x.KifizetesHatarideje.Year == SelectedYear).Sum(x => x.Osszeg);
                        var hashSetsByMonthAndPenznem = new Dictionary<(int Datum, Int16 Kifizetett), HashSet<KotelezettsegKoveteles>>();

                        foreach (var group in kotelezetsegekKovetelesekGroupedByMonthAndKifizetett)
                        {
                            // Create a HashSet for each group
                            var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                            hashSetsByMonthAndPenznem[(group.Key.Month, group.Key.Kifizetett)] = hashSet;
                        }
                        int a = 0;
                        foreach (var kvp in hashSetsByMonthAndPenznem)
                        {
                            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                            foreach (var bevetelKiadas in kvp.Value)
                            {
                                bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                            }
                            AddPieSeries(bevetelekKiadasokAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key.Kifizetett}_{kvp.Key.Datum}", $"Kifizetett_{kvp.Key.Kifizetett}_{kvp.Key.Datum}", baseColors[a]);
                            AddGroupByDataToCollection($"Kifizetett_{kvp.Key.Kifizetett}_{kvp.Key.Datum}", a);
                            if (a + 1 > 3)
                                a = 0;
                            a++;
                        }
                    }
                    else
                    {

                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked && GroupByYearCheckBoxIsChecked)
                {
                    var kotelezetsegekKovetelesekGroupedByYearAndPenznem = _selectedKotelezettsegekKovetelesek.GroupBy(x => new { x.KifizetesHatarideje.Year, x.Penznem });
                    var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Sum(x => x.Osszeg);
                    var hashSetsByYearAndPenznem = new Dictionary<(int Datum, Penznem Penznem), HashSet<KotelezettsegKoveteles>>();

                    foreach (var group in kotelezetsegekKovetelesekGroupedByYearAndPenznem)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                        hashSetsByYearAndPenznem[(group.Key.Year, group.Key.Penznem)] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByYearAndPenznem)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key.Penznem}_{kvp.Key.Datum}", $"{kvp.Key.Penznem}_{kvp.Key.Datum}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Datum}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByYearCheckBoxIsChecked && GroupByKifizetettCheckBoxIsChecked)
                {
                    var kotelezetsegekKovetelesekGroupedByYearAndKifizetett = _selectedKotelezettsegekKovetelesek.GroupBy(x => new { x.KifizetesHatarideje.Year, x.Kifizetett });
                    var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Sum(x => x.Osszeg);
                    var hashSetsByYearAndBeKiKod = new Dictionary<(int Datum, Int16 Kifizetett), HashSet<KotelezettsegKoveteles>>();

                    foreach (var group in kotelezetsegekKovetelesekGroupedByYearAndKifizetett)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                        hashSetsByYearAndBeKiKod[(group.Key.Year, group.Key.Kifizetett)] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByYearAndBeKiKod)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key.Kifizetett}_{kvp.Key.Datum}", $"Kifizetett_{kvp.Key.Kifizetett}_{kvp.Key.Datum}", baseColors[a]);
                        AddGroupByDataToCollection($"Kifizetett_{kvp.Key.Kifizetett}_{kvp.Key.Datum}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked && GroupByKifizetettCheckBoxIsChecked)
                {
                    var kotelezetsegekKovetelesekGroupedByPenznemAndKifizetett = _selectedKotelezettsegekKovetelesek.GroupBy(x => new { x.Penznem, x.Kifizetett });
                    var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Sum(x => x.Osszeg);
                    var hashSetsByPenznemAndKifizetett = new Dictionary<(Penznem Penznem, Int16 Kifizetett), HashSet<KotelezettsegKoveteles>>();

                    foreach (var group in kotelezetsegekKovetelesekGroupedByPenznemAndKifizetett)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                        hashSetsByPenznemAndKifizetett[(group.Key.Penznem, group.Key.Kifizetett)] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByPenznemAndKifizetett)
                    {
                        var kotelezetsegekKovetelesekAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            kotelezetsegekKovetelesekAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(kotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key.Penznem}_{kvp.Key.Kifizetett}", $"{kvp.Key.Penznem}_{kvp.Key.Kifizetett}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key.Penznem}_{kvp.Key.Kifizetett}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    var kotelezetsegekKovetelesekGroupedByPenznem = _selectedKotelezettsegekKovetelesek.GroupBy(x => x.Penznem);
                    var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Sum(x => x.Osszeg);
                    var hashSetsByPenznem = new Dictionary<Penznem, HashSet<KotelezettsegKoveteles>>();

                    foreach (var group in kotelezetsegekKovetelesekGroupedByPenznem)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                        hashSetsByPenznem[group.Key] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByPenznem)
                    {
                        var kotelezetsegekKovetelesekAdatsor = new ChartValues<ObservableValue>();
                        foreach (var kotelKovet in kvp.Value)
                        {
                            kotelezetsegekKovetelesekAdatsor.Add(new ObservableValue(kotelKovet.Osszeg));
                        }
                        AddPieSeries(kotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key}", $"{kvp.Key}", baseColors[a]);
                        AddGroupByDataToCollection($"{kvp.Key}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByKifizetettCheckBoxIsChecked)
                {
                    var kotelezetsegekKovetelesekGroupedByKifizetett = _selectedKotelezettsegekKovetelesek.GroupBy(x => x.Kifizetett);
                    var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Sum(x => x.Osszeg);
                    var hashSetsByKifizetett = new Dictionary<Int16, HashSet<KotelezettsegKoveteles>>();

                    foreach (var group in kotelezetsegekKovetelesekGroupedByKifizetett)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                        hashSetsByKifizetett[group.Key] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByKifizetett)
                    {
                        var kotelezetsegekKovetelesekAdatsor = new ChartValues<ObservableValue>();
                        foreach (var kotelKovet in kvp.Value)
                        {
                            kotelezetsegekKovetelesekAdatsor.Add(new ObservableValue(kotelKovet.Osszeg));
                        }
                        AddPieSeries(kotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key}", $"Kifizetett_{kvp.Key}", baseColors[a]);
                        AddGroupByDataToCollection($"Kifizetett_{kvp.Key}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByYearCheckBoxIsChecked)
                {
                    var kotelezetsegekKovetelesekGroupedByDatum = _selectedKotelezettsegekKovetelesek.GroupBy(x => x.KifizetesHatarideje.Year);
                    var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Sum(x => x.Osszeg);
                    var hashSetsByDatum = new Dictionary<int, HashSet<KotelezettsegKoveteles>>();

                    foreach (var group in kotelezetsegekKovetelesekGroupedByDatum)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                        hashSetsByDatum[group.Key] = hashSet;
                    }
                    int a = 0;
                    foreach (var kvp in hashSetsByDatum)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                        }
                        AddPieSeries(bevetelekKiadasokAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key}", $"Év_{kvp.Key}", baseColors[a]);
                        AddGroupByDataToCollection($"Év_{kvp.Key}", a);
                        if (a + 1 > 3)
                            a = 0;
                        a++;
                    }
                }
                else if (GroupByMonthCheckBoxIsChecked)
                {
                    if (SelectedYear != 0)
                    {
                        var kotelezetsegekKovetelesekGroupedByDatum = _selectedKotelezettsegekKovetelesek.Where(x => x.KifizetesHatarideje.Year == SelectedYear).GroupBy(x => x.KifizetesHatarideje.Month);
                        var totalkotelezetsegekKovetelesekAdatsor = _selectedKotelezettsegekKovetelesek.Where(x => x.KifizetesHatarideje.Year == SelectedYear).Sum(x => x.Osszeg);
                        var hashSetsByDatum = new Dictionary<int, HashSet<KotelezettsegKoveteles>>();

                        foreach (var group in kotelezetsegekKovetelesekGroupedByDatum)
                        {
                            // Create a HashSet for each group
                            var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                            hashSetsByDatum[group.Key] = hashSet;
                        }
                        int a = 0;
                        foreach (var kvp in hashSetsByDatum)
                        {
                            var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
                            foreach (var bevetelKiadas in kvp.Value)
                            {
                                bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
                            }
                            AddPieSeries(bevetelekKiadasokAdatsor, totalkotelezetsegekKovetelesekAdatsor, totalkotelezetsegekKovetelesekAdatsor, $"Kötelezettségek és Követelések - {kvp.Key}", $"Hónap_{kvp.Key}", baseColors[a]);
                            AddGroupByDataToCollection($"Hónap_{kvp.Key}", a);
                            if (a + 1 > 3)
                                a = 0;
                            a++;
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    var kotelezetsegekKovetelesekAdatsor = new ChartValues<ObservableValue>();

                    foreach (var a in _selectedKotelezettsegekKovetelesek)
                    {
                        kotelezetsegekKovetelesekAdatsor.Add(new ObservableValue(a.Osszeg));
                    }
                    var totalKotelezetsegekKovetelesekAdatsor = kotelezetsegekKovetelesekAdatsor.Sum(x => x.Value);

                    AddPieSeries(kotelezetsegekKovetelesekAdatsor, totalKotelezetsegekKovetelesekAdatsor, totalKotelezetsegekKovetelesekAdatsor, "Kötelezettségek és Követelések", "Kötelezettségek_és_Követelések", baseColors[0]);
                }
            }
            
            //foreach (var a in _selectedKotelezettsegekKovetelesek)
            //{
            //    kotelezetsegekKovetelesekAdatsor.Add(new ObservableValue(a.Osszeg));
            //}

            //var totalKotelezetsegekKovetelesekAdatsor = kotelezetsegekKovetelesekAdatsor.Sum(x => x.Value);
            //var totalSum = totalBevetelekKiadasokAdatsor + totalKotelezetsegekKovetelesekAdatsor;


            // Add Chrome series
            //AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalSum, "Bevételek és Kiadások", baseColors[0]);

            //// Add Firefox series
            //AddPieSeries(kotelezetsegekKovetelesekAdatsor, totalKotelezetsegekKovetelesekAdatsor, totalSum, "Kötelezettségek és Követelések", baseColors[1]);
            //NORMAL SETTING ENDS

            //Grouping by Penznem (Currency) STARTS

            //var groupedByPenznem = _selectedBevetelekKiadasok.GroupBy(p => new {p.Penznem, p.BeKiKod});

            //// Create a dictionary to hold the HashSet for each group
            //var hashSetsByPenznem = new Dictionary<(Penznem Penznem, BeKiKod BeKiKod), HashSet<BevetelKiadas>>();

            //foreach (var group in groupedByPenznem)
            //{
            //    // Create a HashSet for each group
            //    var hashSet = new HashSet<BevetelKiadas>(group);
            //    hashSetsByPenznem[(group.Key.Penznem, group.Key.BeKiKod)] = hashSet;
            //}

            //// Display the results
            //foreach (var kvp in hashSetsByPenznem)
            //{
            //    var bevetelekKiadasokAdatsor = new ChartValues<ObservableValue>();
            //    int a = 0;
            //    foreach (var bevetelKiadas in kvp.Value)
            //    {
            //        bevetelekKiadasokAdatsor.Add(new ObservableValue(bevetelKiadas.Osszeg));
            //    }
            //    var totalBevetelekKiadasokAdatsor = bevetelekKiadasokAdatsor.Sum(x => x.Value);
            //    AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key.Penznem} + {kvp.Key.BeKiKod}", baseColors[a]);
            //    if (a + 1 > 3)
            //        a = 0;
            //}

            OnPropertyChanged(nameof(PieSeries));
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                e.Column.Header = "Kiválasztva";
            }
            if (e.PropertyType == typeof(DateTime))
            {
                // Get the column and cast to DataGridTextColumn
                var textColumn = e.Column as DataGridTextColumn;

                if (textColumn != null)
                {
                    // Set the StringFormat for date formatting
                    textColumn.Binding = new System.Windows.Data.Binding(e.PropertyName)
                    {
                        StringFormat = "yyyy.MM.dd" // Format the date as desired
                    };
                }
            }
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        public void UnCheckAllSelections()
        {
            _selectedBevetelekKiadasok.Clear();
            for (int i = 0; i < BevetelekKiadasok.Count; i++)
            {
                BevetelekKiadasok.ElementAt(i).IsSelected = false;
            }
            for (int i = 0; i < KotelKovetelesek.Count; i++)
            {
                BevetelekKiadasok.ElementAt(i).IsSelected = false;
            }
            UpdateSearch(SearchQuery);
            SelectedKotelezettsegekKovetelesek.Clear();
            Years.Clear();
            IsEnabledChangerOnTabItems();
        }

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
                    CanUserSortColumns = true,
                    AutoGenerateColumns = true,
                    SelectionUnit = DataGridSelectionUnit.FullRow,
                    IsReadOnly = true,
                    CanUserResizeColumns = true,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                };
                var itemsSourceBinding = new System.Windows.Data.Binding("FilteredBevetelekKiadasok")
                {
                    Source = this,  // your ViewModel instance
                    Mode = BindingMode.TwoWay, // or OneWay if needed
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                dataGrid.AutoGeneratingColumn += dataGrid_AutoGeneratingColumn;
                // Set the binding to the ItemsSource property
                dataGrid.SetBinding(System.Windows.Controls.DataGrid.ItemsSourceProperty, itemsSourceBinding);

            }
            else
            {
                dataGrid = new System.Windows.Controls.DataGrid
                {
                    Name = table,
                    CanUserSortColumns = false,
                    AutoGenerateColumns = true,
                    SelectionUnit = DataGridSelectionUnit.FullRow,
                    IsReadOnly = true,
                    CanUserResizeColumns = false,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                };
                var itemsSourceBinding = new System.Windows.Data.Binding("FilteredKotelKovetelesek")
                {
                    Source = this,  // your ViewModel instance
                    Mode = BindingMode.TwoWay, // or OneWay if needed
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                dataGrid.AutoGeneratingColumn += dataGrid_AutoGeneratingColumn;
                // Set the binding to the ItemsSource property
                dataGrid.SetBinding(System.Windows.Controls.DataGrid.ItemsSourceProperty, itemsSourceBinding);
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

        public void BuildAndSetContextMenu()
        {
            System.Windows.Controls.ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();

            System.Windows.Controls.MenuItem select = new System.Windows.Controls.MenuItem { Header = "Kijelölés" };
            select.Click += _selectionChanged;
            select.Name = "cellSelectionTrue";
            System.Windows.Controls.MenuItem selectionDelete = new System.Windows.Controls.MenuItem { Header = "Kijelölés(ek) törlése" };
            selectionDelete.Click += _selectionDeleted;
            selectionDelete.Name = "cellSelectionFalse";
            contextMenu.Items.Add(select);
            contextMenu.Items.Add(selectionDelete);

            System.Windows.Controls.DataGrid bevetelKiadasok = GetDataGrid("bevetelek_kiadasok");
            System.Windows.Controls.DataGrid kotelKovet = GetDataGrid("kotelezettsegek_kovetelesek");

            bevetelKiadasok.ContextMenu = contextMenu;
            kotelKovet.ContextMenu = contextMenu;
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

        private void AddPieSeries(ChartValues<ObservableValue> values, double categoryTotal, double totalSum, string title, string name, Brush color)
        {
            double value;
            string label = "";
            switch (SelectedDataStatistics)
            {
                case "Nincs kiválasztva":
                    value = values.Sum(x => x.Value);
                    label = $"{value}";
                    break;
                case "Összeg":
                    value = DataStatistics.GetSum(values.Select(x => (int)x.Value).ToList());
                    label = $"{value}";
                    break;
                case "Átlag":
                    value = DataStatistics.GetAvarage(values.Select(x => (int)x.Value).ToList());
                    label = $"{value}";
                    break;
                case "Mértani Közép":
                    value = DataStatistics.GetMedian(values.Select(x => (int)x.Value).ToList());
                    label = $"{value}";
                    break;
                case "Minimum Érték":
                    value = DataStatistics.GetMinimumValue(values.Select(x => (int)x.Value).ToList());
                    label = $"{value}";
                    break;
                case "Maximum Érték":
                    value = DataStatistics.GetMaximumValue(values.Select(x => (int)x.Value).ToList());
                    label = $"{value}";
                    break;
                case "Értékek Szórása":
                    value = DataStatistics.GetStandardDeviation(values.Select(x => (int)x.Value).ToList());
                    label = $"Átlag : {DataStatistics.GetAvarage(values.Select(x => (int)x.Value).ToList())} -tól -> Szórás: {value}";
                    break;
                default:
                    value = values.Sum(x => x.Value);
                    label = $"{value}";
                    break;
            }
            PieSeries.Add(new PieSeries
            {
                Title = title,
                Name = name,
                Values = new ChartValues<ObservableValue> { new ObservableValue(value) },
                DataLabels = true,
                LabelPoint = chartPoint => $"{label}",
                Fill = color,
                Foreground = Brushes.Black
            }); ;

            LabelFormatter = chartPoint => $"{chartPoint.Y} ({chartPoint.Participation:P})";

            OnPropertyChanged(nameof(LabelFormatter));
        }

        private void AddRowSeries(ChartValues<double> values, string title, string name, Brush color, List<string> labels)
        {
            //StackedRowSeries-el megoldható a csoportokra bontás - viszont a RowSeries-el nem
            //További feltételek szükségesek a RowSeries-hez, hogy a felhasználó tudjon több adatot is megjeleníteni

            RowSeries.Add(new RowSeries
            {
                Title = title,
                Name = name,
                Values = values,
                DataLabels = true,
                Fill = color,
            });

            RowSeriesLabels = labels;

            RowSeriesFormatter = value => value == 0 ? "" : value.ToString("N");

            OnPropertyChanged(nameof(RowSeries));
            OnPropertyChanged(nameof(RowSeriesLabels));
            OnPropertyChanged(nameof(RowSeriesFormatter));
        }

        private void AddColumnSeries(ChartValues<double> values, string title, string name, Brush color, List<string> labels)
        {
            //StackedRowSeries-el megoldható a csoportokra bontás - viszont a RowSeries-el nem
            //További feltételek szükségesek a RowSeries-hez, hogy a felhasználó tudjon több adatot is megjeleníteni

            ColumnSeries.Add(new ColumnSeries
            {
                Title = title,
                Name = name,
                Values = values,
                DataLabels = true,
                Fill = color,
            });

            ColumnSeriesLabels = labels;

            ColumnSeriesFormatter = value => value == 0 ? "" : value.ToString("N");

            OnPropertyChanged(nameof(ColumnSeries));
            OnPropertyChanged(nameof(ColumnSeriesLabels));
            OnPropertyChanged(nameof(ColumnSeriesFormatter));
        }

        private void AddStackedColumnSeries(ChartValues<double> values, string title, string name, Brush color, List<string> labels)
        {
            //StackedRowSeries-el megoldható a csoportokra bontás - viszont a RowSeries-el nem
            //További feltételek szükségesek a RowSeries-hez, hogy a felhasználó tudjon több adatot is megjeleníteni

            StackedColumnSeries.Add(new StackedColumnSeries
            {
                Title = title,
                Name = name,
                Values = values,
                DataLabels = true,
                Fill = color,
            });

            StackedColumnSeriesLabels = labels;

            StackedColumnSeriesFormatter = value => value == 0 ? "" : value.ToString("N");

            OnPropertyChanged(nameof(StackedColumnSeries));
            OnPropertyChanged(nameof(StackedColumnSeriesLabels));
            OnPropertyChanged(nameof(StackedColumnSeriesFormatter));
        }

        private void AddLineSeries(ChartValues<double> values, string title, string name, SolidColorBrush baseColor)
        {
            double value;
            string label = "";
            switch (SelectedDataStatistics)
            {
                case "Nincs kiválasztva":
                    value = values.Sum(x => x);
                    label = $"{value}";
                    break;
                case "Összeg":
                    value = DataStatistics.GetSum(values.Select(x => (int)x).ToList());
                    label = $"{value}";
                    break;
                case "Átlag":
                    value = DataStatistics.GetAvarage(values.Select(x => (int)x).ToList());
                    label = $"{value}";
                    break;
                case "Mértani Közép":
                    value = DataStatistics.GetMedian(values.Select(x => (int)x).ToList());
                    label = $"{value}";
                    break;
                case "Minimum Érték":
                    value = DataStatistics.GetMinimumValue(values.Select(x => (int)x).ToList());
                    label = $"{value}";
                    break;
                case "Maximum Érték":
                    value = DataStatistics.GetMaximumValue(values.Select(x => (int)x).ToList());
                    label = $"{value}";
                    break;
                case "Értékek Szórása":
                    value = DataStatistics.GetStandardDeviation(values.Select(x => (int)x).ToList());
                    label = $"Átlag : {DataStatistics.GetAvarage(values.Select(x => (int)x).ToList())} -tól -> Szórás: {value}";
                    break;
                default:
                    value = values.Sum(x => x);
                    label = $"{value}";
                    break;
            }

            LineSeries.Add(new LineSeries
            {
                Name = name,
                Title = title,
                Values = values,
                DataLabels = true,
                PointForeground = baseColor,
                Stroke = baseColor
            });

            LineSeriesLabels = new[] { "Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec" };

            LineSeriesYFormatter = v => v.ToString("C");

            OnPropertyChanged(nameof(LineSeries));
            OnPropertyChanged(nameof(LineSeriesLabels));
            OnPropertyChanged(nameof(LineSeriesYFormatter));
        }

        private void ExecuteShowSelectDataForNewChartViewCommand(object obj)
        {
        }
        private void ExecuteAddGroupByToAdatsorokCommand(object obj)
        {
            if (SelectedCimke == null)
            {
                return;
            }
            if (SelectedCimkek.Count < 2 && SelectedCimke != "Év" && SelectedCimke != "Hónap")
                return;
            if(SelectedCimke == "Hónap" && SelectedAdatsorok.Contains("Év"))
            {
                SelectedAdatsorok.Remove("Év");
                SelectedCimkek.Add("Év");
            }
            if (SelectedCimke == "Év" && SelectedAdatsorok.Contains("Hónap"))
            {
                SelectedAdatsorok.Remove("Hónap");
                SelectedCimkek.Add("Hónap");
            }
            SelectedAdatsorok.Add(SelectedCimke);
            SelectedCimkek.Remove(SelectedCimke);
        }

        private void ExecuteAddGroupByToCimkekCommand(object obj)
        {
            if(SelectedAdatsor == null)
            {
                return;
            }
            if (SelectedAdatsor == "Hónap" && SelectedCimkek.Contains("Év"))
            {
                SelectedCimkek.Remove("Év");
                SelectedAdatsorok.Add("Év");
            }
            if (SelectedAdatsor == "Év" && SelectedCimkek.Contains("Hónap"))
            {
                SelectedCimkek.Remove("Hónap");
                SelectedAdatsorok.Add("Hónap");
            }
            SelectedCimkek.Add(SelectedAdatsor);
            SelectedAdatsorok.Remove(SelectedAdatsor);
        }

        private void ExecuteShowAddOptionToNewChartViewCommand(object obj)
        {
            CurrentChildView = new AddOptionsToNewChartViewModel(SelectedRows);
        }

        private void ExecuteExportChartAsImageCommand(object obj)
        {
            ExportSpecificChart(SeriesType);
        }

        public void ExportChartAsImage(UIElement chart, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Generate a unique file name based on date/time
            string fileName = $"Chart_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            // Combine folder path and file name
            string filePath = Path.Combine(folderPath, fileName);

            // Define the size of the chart to render
            var size = new Size(chart.RenderSize.Width, chart.RenderSize.Height);

            // Measure and arrange the chart to ensure it is properly rendered
            chart.Measure(size);
            chart.Arrange(new Rect(size));

            // Create a RenderTargetBitmap to render the chart
            var renderBitmap = new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96, // DPI X
                96, // DPI Y
                PixelFormats.Pbgra32);

            // Create a DrawingVisual to draw a white background
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                // Fill the entire area with white
                drawingContext.DrawRectangle(Brushes.White, null, new Rect(new Point(0, 0), size));

                // Create a VisualBrush from the chart
                VisualBrush visualBrush = new VisualBrush(chart);
                drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(0, 0), size));
            }

            // Render the visual with white background to the bitmap
            renderBitmap.Render(drawingVisual);

            // Save the bitmap to a file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(fileStream);
            }
        }

        public void ExportSpecificChart(string chartName)
        {
            UIElement chart = Mediator.NotifyGetSpecificChart(chartName);
            if (chart != null)
            {
                ExportChartAsImage(chart, "C:\\Users\\NorbiPC\\Downloads\\teszt\\");
            }
        }
    }
}
