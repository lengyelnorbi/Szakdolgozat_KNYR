using MaterialDesignThemes.Wpf;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Szakdolgozat.ViewModels
{
    //Új diagramm készítésekor megnyitott ablak viewmodelje
    //The window's viewmodel that shows when creating a new diagram
    public class CreateChartsViewModel : ViewModelBase
    {
        public event Action checkboxChange;
        private bool AllCheckBoxChecked = false;
        private ObservableCollection<System.Windows.Controls.TabItem> _tabs = new ObservableCollection<System.Windows.Controls.TabItem>();
        public List<System.Windows.Controls.DataGridCell> selectedDataGridCells = new List<System.Windows.Controls.DataGridCell>();
        public ObservableCollection<System.Windows.Controls.TabItem> Tabs
        {
            get { return _tabs; }
            set
            {
                _tabs = value;
                OnPropertyChanged(nameof(Tabs));
            }
        }
        private DolgozoRepository dolgozoRepository = new DolgozoRepository();
        private GazdalkodoSzervezetRepository gazdalkodoSzervezetRepository = new GazdalkodoSzervezetRepository();
        private KoltsegvetesRepository koltsegvetesRepository = new KoltsegvetesRepository();
        private KotelezettsegKovetelesRepository kotelezettsegKovetelesRepository = new KotelezettsegKovetelesRepository();
        private MaganSzemelyRepository maganSzemelyRepository = new MaganSzemelyRepository();

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
        public Dictionary<string, bool> monthCheckboxStatuses = new Dictionary<string, bool>();
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
        private readonly ObservableCollection<string> _dbTableNames = new ObservableCollection<string> {"bevetelek_kiadasok", "felhasznalok", "dolgozok", "gazdalkodo_szervezetek", "kotelezettsegek_kovetelesek", "magan_szemelyek"};
        
        //Egy szótár, amiben a tábla nevekhez vannak rendelve a táblában található oszlop nevekből készített lista
        //A dictionary that holds the table name and the column names that belongs to it as key-value pairs 
        private Dictionary<string, ObservableCollection<string>> _dbTablesWithColumnNames = new Dictionary<string, ObservableCollection<string>>();

        public ObservableCollection<System.Windows.Controls.ComboBoxItem> _checkBoxes = new ObservableCollection<System.Windows.Controls.ComboBoxItem>();
        public ObservableCollection<System.Windows.Controls.ComboBoxItem> CheckBoxes
        {
            get { return _checkBoxes; }
        }
        public ICommand ShowSelectDataForNewChartViewCommand { get; }
        public ICommand ShowAddOptionToNewChartViewCommand { get; }

        public CreateChartsViewModel()
        {
            //nem biztos hogy kell innen
            Mediator.SelectedRowsChangedOnChildView += UpdateSelectedRows;
            Mediator.DataRequest += ReturnRequestedData;

            CurrentChildView = new SelectDataForNewChartViewModel();

            ShowSelectDataForNewChartViewCommand = new ViewModelCommand(ExecuteShowSelectDataForNewChartViewCommand);
            ShowAddOptionToNewChartViewCommand = new ViewModelCommand(ExecuteShowAddOptionToNewChartViewCommand);
            //idáig


            //Adat szűrő feltöltése (comboboxitem-ekkel)
            //Update the Data filter combobox with comboboxitems - START
            UserRepository userRepository = new UserRepository();
            foreach (var table in _dbTableNames)
            {
                //Combobox title
                ComboBoxItem cbitem2 = new ComboBoxItem();
                cbitem2.Name = "datafilter";
                cbitem2.Content = "Adat szűrő";
                cbitem2.IsEnabled = false;
                cbitem2.Visibility = Visibility.Collapsed;
                _checkBoxes.Add(cbitem2);

                //A comboboxitem that shows which table's checkboxes are under it
                ComboBoxItem cbitem3 = new ComboBoxItem();
                cbitem3.Name = table.ToString() + "_CBI";
                cbitem3.Content = table.ToString();
                cbitem3.IsEnabled = false;
                _checkBoxes.Add(cbitem3);

                //A checkbox that later sets all the corresponding checkboxes to checked
                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = "Mind";
                System.Windows.Controls.CheckBox checkBox2 = new System.Windows.Controls.CheckBox();
                checkBox2.Name = table + "_All";
                checkBox2.Content = textBlock2;
                checkBox2.Checked += OptionsCheckBoxAllChecked;
                checkBox2.Unchecked += OptionsCheckBoxAllUnchecked;

                ComboBoxItem cbitem4 = new ComboBoxItem();
                cbitem4.Name = table.ToString() + "_CBI_All";
                cbitem4.Content = checkBox2;
                _checkBoxes.Add(cbitem4);

                _dbTablesWithColumnNames.Add(table, userRepository.GetColumnNamesForTables(table));
                foreach(var s in userRepository.GetColumnNamesForTables(table))
                {
                    //Pl: Name = dolgozok_id_ChB
                    //A checkbox to all the column name for each table
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = s;
                    System.Windows.Controls.CheckBox checkBox = new System.Windows.Controls.CheckBox();
                    checkBox.Name = table + "_ChB" + s;
                    checkBox.Content = textBlock;
                    checkBox.Checked += OptionsCheckboxCheckedChange;
                    checkBox.Unchecked += OptionsCheckboxCheckedChange;

                    //Pl: Name = id_CB
                    ComboBoxItem cbitem = new ComboBoxItem();
                    cbitem.Name = s + "_CB";
                    cbitem.Content = checkBox;
                    _checkBoxes.Add(cbitem);
                }
            }
            //FINISH
        }

        //Egy checkbox névből előállítja a hozzá tartozó oszlop nevét
        //It gives back the tablename that can be taken from the checkbox's name
        private string GetTableName(string chName)
        {
            string[] t = chName.Split('_');
            
            string tableName = t[0];
            if (!t[1].Contains("ChB"))
                tableName += "_" + t[1];
            return tableName;
        }

        //Ha egy checkbox meglett nyomva, ami tartalmazza az All résszöveget, akkor annak a checkbox-nak a nevéből kinyert táblanevet tartalmazó többi checkboxot is ki pipálja
        //If a checkbox that was clicked contains the All substring then it sets all the checkboxes state to checked that holds the substring that we get from the checkbox's name
        private void OptionsCheckBoxAllChecked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox)
            {
                //chartDataFilterCHB 
                if (checkBox.Name.Contains("All"))
                {
                    AllCheckBoxChecked = true;
                    foreach (var ch in CheckBoxes)
                    {
                        if (ch.Content is System.Windows.Controls.CheckBox checkbox2)
                        {
                            if (checkBox.Name.Contains(checkbox2.Name.Split('_')[0]))
                            {
                                checkbox2.IsChecked = true;
                            }
                        }
                    }
                }
            }
        }

        //Ha a checkbox, ami meglett nyomva tartalmazza az All résszöveget, akkor az AllCheckBoxChecked értékét false-re állítja
        //If the checkbox that was clicked contains the All substring then the AllCheckBoxChecked variable is set to false
        private void OptionsCheckBoxAllUnchecked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox)
            {
                if (checkBox.Name.Contains("All"))
                {
                    AllCheckBoxChecked = false;
                }
            }
        }

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
        private void OptionsCheckboxCheckedChange(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox)
            {
                string tableName = GetTableName(checkBox.Name);

                if ((bool)checkBox.IsChecked)
                {
                    System.Windows.Controls.TabItem tabItem = null;
                    switch (tableName)
                    {
                        case "dolgozok":
                            tabItem = CreateTabItem("Dolgozok", new ObservableCollection<object>(dolgozoRepository.GetDolgozok().Cast<object>().ToList()), tableName, checkBox.Name);
                            if (tabItem != null)
                            {
                                Tabs.Add(tabItem);
                            }
                            else
                            {
                                SetColumnByCheckBox(tableName, checkBox.Name, true);
                            }
                            break;
                        case "gazdalkodo_szervezetek":
                            tabItem = CreateTabItem("GazdalkodoSzervezetek", new ObservableCollection<object>(gazdalkodoSzervezetRepository.GetGazdalkodoSzervezetek().Cast<object>().ToList()), tableName, checkBox.Name);
                            if (tabItem != null)
                            {
                                Tabs.Add(tabItem);
                            }
                            else
                            {
                                SetColumnByCheckBox(tableName, checkBox.Name, true);
                            }
                            break;
                        case "bevetelek_kiadasok":
                            tabItem = CreateTabItem("Koltsegvetes", new ObservableCollection<object>(koltsegvetesRepository.GetKoltsegvetesek().Cast<object>().ToList()), tableName, checkBox.Name);
                            if (tabItem != null)
                            {
                                Tabs.Add(tabItem);
                            }
                            else
                            {
                                SetColumnByCheckBox(tableName, checkBox.Name, true);
                            }
                            break;
                        case "kotelezettsegek_kovetelesek":
                            tabItem = CreateTabItem("KotelKovetelesek", new ObservableCollection<object>(kotelezettsegKovetelesRepository.GetKotelezettsegekKovetelesek().Cast<object>().ToList()), tableName, checkBox.Name);
                            if (tabItem != null)
                            {
                                Tabs.Add(tabItem);
                            }
                            else
                            {
                                SetColumnByCheckBox(tableName, checkBox.Name, true);
                            }
                            break;
                        case "magan_szemelyek":
                            tabItem = CreateTabItem("MaganSzemelyek", new ObservableCollection<object>(maganSzemelyRepository.GetMaganSzemelyek().Cast<object>().ToList()), tableName, checkBox.Name);
                            if (tabItem != null)
                            {
                                Tabs.Add(tabItem);
                            }
                            else
                            {
                                SetColumnByCheckBox(tableName, checkBox.Name, true);
                            }
                            break;
                        default: break;
                    }
                    OnPropertyChanged(nameof(Tabs));
                }
                else
                {
                    SetColumnByCheckBox(tableName, checkBox.Name, false);
                }
            }
        }

        //Miután a datagrid létre lett hozva, attól függően, hogy melyik checkbox hatására jött lére beállítja az elrejteni kívánt oszlopokat
        //After the datagrid is created, depending on the click checkbox the columns that shouldn't be seen are hidden
        private void HideColumnsAfterCreatingTabItem(System.Windows.Controls.DataGrid dataGrid, string chName)
        {
            if (!AllCheckBoxChecked)
            {
                foreach (var c in dataGrid.Columns)
                {
                    if (c.Header.ToString().ToLower() != GetCheckBoxColumnName(chName))
                    {
                        c.Visibility = Visibility.Collapsed;
                    }
                }
            }
            AllCheckBoxChecked = false;
        }

        //Beállítja annak a checkbox állapotát nem kijelölt-re, amelyik _All-ra végződik és a chName-ből szerzett tábla nevet is tartalmazza
        //Sets the checkbox to unchecked if the checkbox's name contains the _All substring and the tablename substring that is coming from the chName
        private void UncheckAllTypeCheckBox(string chName)
        {
            string tableName = GetTableName(chName);
            foreach(var a in CheckBoxes)
            {
                if(a.Content is System.Windows.Controls.CheckBox checkBox)
                {
                    if(checkBox.Name == tableName + "_All")
                    {
                        checkBox.IsChecked = false; 
                        break;
                    }
                }
            }
        }

        //Létrehoz egy TabItem-et, aminek a tartalma egy datagrid lesz, ez lesz megjelenítve a felhasználónak
        //Created a TabItem that contains a datagrid, this will be shown to the user on the UI
        private System.Windows.Controls.TabItem CreateTabItem(string header, ObservableCollection<object> data, string table, string chName)
        {
            System.Windows.Controls.TabControl tabControl = Mediator.NotifyGetTabControl();

            var existingTab = tabControl.Items.OfType<System.Windows.Controls.TabItem>().FirstOrDefault(t => t.Header.ToString() == header);

            if (existingTab != null)
            {
                // Bring the existing tab to the front
                tabControl.SelectedItem = existingTab;
                return null;
            }
            var dataGrid = new System.Windows.Controls.DataGrid
            {
                Name = table,
                ItemsSource = data,
                CanUserSortColumns = false,
                AutoGenerateColumns = true,
                SelectionUnit = DataGridSelectionUnit.Cell,
                IsReadOnly = true,
            };
            dataGrid.AutoGeneratedColumns += (sender, e) =>
            {
                HideColumnsAfterCreatingTabItem(dataGrid, chName);
            };
            var tabItem = new System.Windows.Controls.TabItem
            {
                Header = header,
                Content = dataGrid,
                Name = "Tab_" + header,
                Margin = new Thickness(0.0),
                Padding = new Thickness(0.0),
                IsSelected = true,
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
        private void SetColumnByCheckBox(string dataGridName, string chName, bool isChecked)
        {
            System.Windows.Controls.DataGrid dataGrid = GetDataGrid(dataGridName); 
            if(dataGrid != null)
            {
                foreach (var column in dataGrid.Columns)
                {
                    if (column.Header != null)
                    {
                        if (column.Header.ToString().ToLower() == GetCheckBoxColumnName(chName))
                        {
                            if (!isChecked)
                            {
                                column.Visibility = Visibility.Collapsed;
                                UncheckAllTypeCheckBox(chName);
                            }
                            else
                            {
                                column.Visibility = Visibility.Visible;
                            }
                            break;
                        }
                    }
                }
                if (GetCollapsedColumnNumberInDataGrid(dataGrid) == 0 && !isChecked)
                {
                    DeleteDataGrid(dataGridName);
                    OnPropertyChanged(nameof(Tabs));
                }
            }
        }

        //Visszaadja, hogy mennyi oszlop van elrejtve az adott datagrid-ben
        //Give back how many collapsed columns are in the given datagrid
        private int GetCollapsedColumnNumberInDataGrid(System.Windows.Controls.DataGrid dataGrid)
        {
            int counter = 0;
            foreach (var column in dataGrid.Columns)
            {
                if (column.Visibility != Visibility.Collapsed)
                    counter++;
            }
            return counter;
        }

        //Visszaadja az addot checkbox név-ből az oszlop nevet
        //Gives back the column name from the given checkbox name
        private string GetCheckBoxColumnName(string chName)
        {
            string tmp;
            string tmp2;
            string tmp3 = "";
            if (chName.Contains(GetTableName(chName)))
            {
                tmp = chName.Replace(GetTableName(chName) + "_", "");
                if (tmp.Contains("ChB"))
                {
                    tmp2 = tmp.Replace("ChB", "");
                    tmp3 = tmp2.Replace("_", "");
                }
            }
            return tmp3.ToLower();
        }


        //Törli a megadott nevű datagrid-et
        //Deletes the datagrid that's name is the given in the parameter
        private void DeleteDataGrid(string dataGridName)
        {
            foreach (var o in Tabs)
            {
                if (o.Content is System.Windows.Controls.DataGrid dataGrid)
                    if (dataGrid.Name == dataGridName)
                    {
                        Tabs.Remove(o);
                        if(Tabs.Count == 0)
                            Tabs.Clear();
                        break;
                    }
            }
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

        private void ExecuteShowSelectDataForNewChartViewCommand(object obj)
        {
            CurrentChildView = new SelectDataForNewChartViewModel();
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
