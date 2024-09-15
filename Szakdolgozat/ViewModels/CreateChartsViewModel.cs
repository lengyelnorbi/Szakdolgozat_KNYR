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

namespace Szakdolgozat.ViewModels
{
    //Új diagramm készítésekor megnyitott ablak viewmodelje.
    //The window's viewmodel that shows when creating a new diagram.
    public class CreateChartsViewModel : ViewModelBase
    {
        public bool GroupByPenznemCheckBoxIsChecked = false;
        public bool GroupByBeKiKodCheckBoxIsChecked = false;
        public bool GroupByKifizetettCheckBoxIsChecked = false;
        public bool GroupByMonthCheckBoxIsChecked = false;
        public bool GroupByYearCheckBoxIsChecked = false;
        public bool IsBevetelekKiadasokTabIsSelected = true;
        public bool GroupByDateCheckBoxIsChecked = false;
        public bool _isValidStartDateExists = false;
        public bool _isValidEndDateExists = false;
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
            else if (SelectedBevetelekKiadasok.Count == 0 || SelectedKotelezettsegekKovetelesek.Count == 0)
            {
                System.Windows.Controls.TabControl tabControl = Mediator.NotifyGetTabControl();
                if (SelectedBevetelekKiadasok.Count == 0)
                {
                    foreach (TabItem tItem in tabControl.Items)
                    {
                        if (tItem.Name.Contains("KotelKovet"))
                        {
                            tItem.IsEnabled = true;
                            break;
                        }
                    }
                }
                else if (SelectedKotelezettsegekKovetelesek.Count == 0)
                {
                    foreach (TabItem tItem in tabControl.Items)
                    {
                        if (tItem.Name.Contains("Koltsegvetes"))
                        {
                            tItem.IsEnabled = true;
                            break;
                        }
                    }
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

        public CreateChartsViewModel()
        {
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
            else if(SeriesType == "DoghnutSeries")
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
                                                   monthGroup => monthGroup.Sum(x => Convert.ToDouble(x.Osszeg))
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
                                                   monthGroup => monthGroup.Sum(x => Convert.ToDouble(x.Osszeg))
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
                                                       monthGroup => monthGroup.Sum(x => Convert.ToDouble(x.Osszeg))
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
                    var groupedByPenznemAndBeKiKod = _selectedBevetelekKiadasok.GroupBy(p => new { p.Penznem, p.BeKiKod });

                    // Create a dictionary to hold the HashSet for each group
                    var hashSetsByPenznemAndBeKiKod = new Dictionary<(Penznem Penznem, BeKiKod BeKiKod), HashSet<BevetelKiadas>>();

                    foreach (var group in groupedByPenznemAndBeKiKod)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<BevetelKiadas>(group);
                        hashSetsByPenznemAndBeKiKod[(group.Key.Penznem, group.Key.BeKiKod)] = hashSet;
                    }

                    int a = 0;
                    // Display the results
                    foreach (var kvp in hashSetsByPenznemAndBeKiKod)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas.Osszeg));
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
                    var groupedByBeKiKod = _selectedBevetelekKiadasok.GroupBy(p => new { p.BeKiKod });

                    // Create a dictionary to hold the HashSet for each group
                    var hashSetsByBeKiKod = new Dictionary<BeKiKod, HashSet<BevetelKiadas>>();

                    foreach (var group in groupedByBeKiKod)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<BevetelKiadas>(group);
                        hashSetsByBeKiKod[group.Key.BeKiKod] = hashSet;
                    }

                    int a = 0;
                    // Display the results
                    foreach (var kvp in hashSetsByBeKiKod)
                    {
                        var bevetelekKiadasokAdatsor = new ChartValues<double>();
                        foreach (var bevetelKiadas in kvp.Value)
                        {
                            bevetelekKiadasokAdatsor.Add(Convert.ToDouble(bevetelKiadas.Osszeg));
                        }
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key}", kvp.Key.ToString(), baseColors[a]);
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
                        AddLineSeries(bevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key}", kvp.Key.ToString(), baseColors[a]);
                        AddGroupByDataToCollection(kvp.Key.ToString(), a);
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
                                                   monthGroup => monthGroup.Sum(x => Convert.ToDouble(x.Osszeg))
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
                        AddLineSeries(bevetelekKiadasokAdatsor2, $"Bevételek és Kiadások - {kvp.Key}", "Datum_" + kvp.Key.ToString(), baseColors[b]);
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
                                                   monthGroup => monthGroup.Sum(x => Convert.ToDouble(x.Osszeg))
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
                                                   monthGroup => monthGroup.Sum(x => Convert.ToDouble(x.Osszeg))
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
                                                       monthGroup => monthGroup.Sum(x => Convert.ToDouble(x.Osszeg))
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
                    var groupedByPenznemAndKifizetett = _selectedKotelezettsegekKovetelesek.GroupBy(p => new { p.Penznem, p.Kifizetett });

                    // Create a dictionary to hold the HashSet for each group
                    var hashSetsByPenznemAndKifizetett = new Dictionary<(Penznem Penznem, Int16 Kifizetett), HashSet<KotelezettsegKoveteles>>();

                    foreach (var group in groupedByPenznemAndKifizetett)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                        hashSetsByPenznemAndKifizetett[(group.Key.Penznem, group.Key.Kifizetett)] = hashSet;
                    }

                    int a = 0;
                    // Display the results
                    foreach (var kvp in hashSetsByPenznemAndKifizetett)
                    {
                        var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();
                        foreach (var kotelezettsegKoveteles in kvp.Value)
                        {
                            kotelezettsegekKovetelesekAdatsor.Add(Convert.ToDouble(kotelezettsegKoveteles.Osszeg));
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
                    var groupedByKifizetett = _selectedKotelezettsegekKovetelesek.GroupBy(p => new { p.Kifizetett });

                    // Create a dictionary to hold the HashSet for each group
                    var hashSetsByKifizetett = new Dictionary<Int16, HashSet<KotelezettsegKoveteles>>();

                    foreach (var group in groupedByKifizetett)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                        hashSetsByKifizetett[group.Key.Kifizetett] = hashSet;
                    }

                    int a = 0;
                    // Display the results
                    foreach (var kvp in hashSetsByKifizetett)
                    {
                        var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();
                        foreach (var kotelezettsegKoveteles in kvp.Value)
                        {
                            kotelezettsegekKovetelesekAdatsor.Add(Convert.ToDouble(kotelezettsegKoveteles.Osszeg));
                        }
                        AddLineSeries(kotelezettsegekKovetelesekAdatsor, $"Kotelezettségek és Követelések - {kvp.Key}", "Kifizetett_" + kvp.Key.ToString(), baseColors[a]);
                        AddGroupByDataToCollection("Kifizetett_" + kvp.Key.ToString(), a);
                        if (a + 1 > 3)
                            a = 0;
                        else a += 1;
                    }
                }
                else if (GroupByPenznemCheckBoxIsChecked)
                {
                    var groupedByPenznem = _selectedKotelezettsegekKovetelesek.GroupBy(p => new { p.Penznem });

                    // Create a dictionary to hold the HashSet for each group
                    var hashSetsByPenznem = new Dictionary<Penznem, HashSet<KotelezettsegKoveteles>>();

                    foreach (var group in groupedByPenznem)
                    {
                        // Create a HashSet for each group
                        var hashSet = new HashSet<KotelezettsegKoveteles>(group);
                        hashSetsByPenznem[group.Key.Penznem] = hashSet;
                    }

                    int a = 0;
                    // Display the results
                    foreach (var kvp in hashSetsByPenznem)
                    {
                        var kotelezettsegekKovetelesekAdatsor = new ChartValues<double>();
                        foreach (var kotelezettsegKoveteles in kvp.Value)
                        {
                            kotelezettsegekKovetelesekAdatsor.Add(Convert.ToDouble(kotelezettsegKoveteles.Osszeg));
                        }
                        AddLineSeries(kotelezettsegekKovetelesekAdatsor, $"Kotelezettségek és Követelések - {kvp.Key}", kvp.Key.ToString(), baseColors[a]);
                        AddGroupByDataToCollection(kvp.Key.ToString(), a);
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
                                                   monthGroup => monthGroup.Sum(x => Convert.ToDouble(x.Osszeg))
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
           else if(SeriesType == "DoghnutSeries")
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
            PieSeries = new SeriesCollection();
            GroupBySelections.Clear();

            if (IsBevetelekKiadasokTabIsSelected)
            {
                if(GroupByYearCheckBoxIsChecked)
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
                else if (GroupByMonthCheckBoxIsChecked && SelectedYear != 0)
                {
                    var bevetelekKiadasokGroupedByDatum = _selectedBevetelekKiadasok.Where(x => x.TeljesitesiDatum.Year == SelectedYear).GroupBy(x => x.TeljesitesiDatum.Month);
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
                        AddPieSeries(bevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, totalBevetelekKiadasokAdatsor, $"Bevételek és Kiadások - {kvp.Key}", $"Hónap_{kvp.Key}", baseColors[a]);
                        AddGroupByDataToCollection($"Hónap_{kvp.Key}", a);
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
                if (GroupByPenznemCheckBoxIsChecked && GroupByKifizetettCheckBoxIsChecked)
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
            SelectedBevetelekKiadasok.Clear();
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
            //Every value is a PieSeries not while grouping
            PieSeries.Add(new PieSeries
            {
                Title = title,
                Name = name,
                Values = new ChartValues<ObservableValue> { new ObservableValue(values.Sum(x => x.Value)) },
                DataLabels = true,
                LabelPoint = chartPoint => $"({(values.Sum(x => x.Value) / totalSum):P})",
                Fill = color
            }); ;


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

        private void AddLineSeries(ChartValues<double> asd, string title, string name, SolidColorBrush baseColor)
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

            if (GroupByDateCheckBoxIsChecked)
            {
                LineSeriesLabels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            }
            else
            {
                LineSeriesLabels = new[] {""};
            }
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
    }
}
