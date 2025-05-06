using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Tls;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
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
                Mediator.SetSeriesVisibility += SetDiagramsVisibility;
                viewModel.SeriesType = type;
                this.Closed += viewModel.CloseWindow;
                viewModel._selectionChanged = ChangeSelection_Click;
                viewModel._selectionDeleted = ChangeSelection_Click;
                viewModel._deleteAllSelections = deleteAllDataSelection_Click;
                viewModel.BuildAndSetContextMenu();
            }

            koltsegvetes_mindCB.Checked += CheckBox_Checked;
            koltsegvetes_idCB.Checked += CheckBox_Checked;
            koltsegvetes_osszegCB.Checked += CheckBox_Checked;
            koltsegvetes_penznemCB.Checked += CheckBox_Checked;
            koltsegvetes_bekikodCB.Checked += CheckBox_Checked;
            koltsegvetes_teljesitesiDatumCB.Checked += CheckBox_Checked;
            koltsegvetes_kotelKovetIDCB.Checked += CheckBox_Checked;
            koltsegvetes_partnerIDCB.Checked += CheckBox_Checked;
            koltsegvetes_mindCB.Unchecked += Checkbox_Unchecked;
            koltsegvetes_idCB.Unchecked += Checkbox_Unchecked;
            koltsegvetes_osszegCB.Unchecked += Checkbox_Unchecked;
            koltsegvetes_penznemCB.Unchecked += Checkbox_Unchecked;
            koltsegvetes_bekikodCB.Unchecked += Checkbox_Unchecked;
            koltsegvetes_teljesitesiDatumCB.Unchecked += Checkbox_Unchecked;
            koltsegvetes_kotelKovetIDCB.Unchecked += Checkbox_Unchecked;
            koltsegvetes_partnerIDCB.Unchecked += Checkbox_Unchecked;


            kotelKovet_mindCB.Checked += CheckBox_Checked;
            kotelKovet_idCB.Checked += CheckBox_Checked;
            kotelKovet_osszegCB.Checked += CheckBox_Checked;
            kotelKovet_penznemCB.Checked += CheckBox_Checked;
            kotelKovet_tipusCB.Checked += CheckBox_Checked;
            kotelKovet_kifizetesHataridejeCB.Checked += CheckBox_Checked;
            kotelKovet_kifizetettCB.Checked += CheckBox_Checked;
            kotelKovet_mindCB.Unchecked += Checkbox_Unchecked;
            kotelKovet_idCB.Unchecked += Checkbox_Unchecked;
            kotelKovet_osszegCB.Unchecked += Checkbox_Unchecked;
            kotelKovet_penznemCB.Unchecked += Checkbox_Unchecked;
            kotelKovet_tipusCB.Unchecked += Checkbox_Unchecked;
            kotelKovet_kifizetesHataridejeCB.Unchecked += Checkbox_Unchecked;
            kotelKovet_kifizetettCB.Unchecked += Checkbox_Unchecked;

            GroupByBeKiKodCB.Checked += GroupByCheckBox_Checked;
            GroupByBeKiKodCB.Unchecked += GroupByCheckBox_Unchecked;
            GroupByPenznemCB.Checked += GroupByCheckBox_Checked;
            GroupByPenznemCB.Unchecked += GroupByCheckBox_Unchecked;
            GroupByKifizetettCB.Checked += GroupByCheckBox_Checked;
            GroupByKifizetettCB.Unchecked += GroupByCheckBox_Unchecked;
            GroupByDateCB.Checked += GroupByCheckBox_Checked;
            GroupByDateCB.Unchecked += GroupByCheckBox_Unchecked;
            GroupByYearCB.Checked += GroupByCheckBox_Checked;
            GroupByYearCB.Unchecked += GroupByCheckBox_Unchecked;
            GroupByMonthCB.Checked += GroupByCheckBox_Checked;
            GroupByMonthCB.Unchecked += GroupByCheckBox_Unchecked;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }
        private void pnlControlBar_MouseEnter(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {

                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Mediator.GetTabControl -= () => chartsTabControl;
            Mediator.SetSeriesVisibility -= SetDiagramsVisibility;
            this.Close();
        }
        private void GroupByCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox)
            {
                if (DataContext is CreateChartsViewModel createChartsView)
                {
                    switch (checkBox.Name)
                    {
                        case "GroupByPenznemCB":
                            createChartsView.GroupByPenznemCheckBoxIsChecked = false;
                            if (createChartsView.SeriesType == "RowSeries" || createChartsView.SeriesType == "BasicColumnSeries" || createChartsView.SeriesType == "StackedColumnSeries")
                            {
                                GroupByMonthCB.IsEnabled = true;
                                GroupByMonthYearsCB.IsEnabled = true;
                                GroupByYearCB.IsEnabled = true;
                            }
                            break;
                        case "GroupByBeKiKodCB":
                            createChartsView.GroupByBeKiKodCheckBoxIsChecked = false;
                            if (createChartsView.SeriesType == "RowSeries" || createChartsView.SeriesType == "BasicColumnSeries" || createChartsView.SeriesType == "StackedColumnSeries")
                            {
                                GroupByMonthCB.IsEnabled = true;
                                GroupByMonthYearsCB.IsEnabled = true;
                                GroupByYearCB.IsEnabled = true;
                            }
                            break;
                        case "GroupByKifizetettCB":
                            createChartsView.GroupByKifizetettCheckBoxIsChecked = false;
                            if (createChartsView.SeriesType == "RowSeries" || createChartsView.SeriesType == "BasicColumnSeries" || createChartsView.SeriesType == "StackedColumnSeries")
                            {
                                GroupByMonthCB.IsEnabled = true;
                                GroupByMonthYearsCB.IsEnabled = true;
                                GroupByYearCB.IsEnabled = true;
                            }
                            break;
                        case "GroupByDateCB":
                            createChartsView.GroupByDateCheckBoxIsChecked = false;
                            break;
                        case "GroupByYearCB":
                            createChartsView.GroupByYearCheckBoxIsChecked = false;
                            if (createChartsView.SeriesType == "RowSeries" || createChartsView.SeriesType == "BasicColumnSeries" || createChartsView.SeriesType == "StackedColumnSeries")
                            {
                                GroupByKifizetettCB.IsEnabled = true;
                                GroupByPenznemCB.IsEnabled = true;
                                GroupByBeKiKodCB.IsEnabled = true;
                            }
                            else
                            {
                                GroupByMonthCB.IsEnabled = true;
                                GroupByMonthYearsCB.IsEnabled = true;
                            }
                            break;
                        case "GroupByMonthCB":
                            createChartsView.GroupByMonthCheckBoxIsChecked = false;
                            if (createChartsView.SeriesType == "RowSeries" || createChartsView.SeriesType == "BasicColumnSeries" || createChartsView.SeriesType == "StackedColumnSeries")
                            {
                                GroupByKifizetettCB.IsEnabled = true;
                                GroupByPenznemCB.IsEnabled = true;
                                GroupByBeKiKodCB.IsEnabled = true;
                            }
                            else
                            {
                                GroupByYearCB.IsEnabled = true;
                            }
                            createChartsView.Years.Clear();
                            createChartsView.SelectedYear = 0;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void GroupByCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox)
            {
                if (DataContext is CreateChartsViewModel createChartsView)
                {
                    switch (checkBox.Name)
                    {
                        case "GroupByPenznemCB":
                            createChartsView.GroupByPenznemCheckBoxIsChecked = true;
                            if(createChartsView.SeriesType == "RowSeries" || createChartsView.SeriesType == "BasicColumnSeries" || createChartsView.SeriesType == "StackedColumnSeries")
                            {
                                GroupByMonthCB.IsEnabled = false;
                                GroupByMonthYearsCB.IsEnabled = false;
                                GroupByYearCB.IsEnabled = false;
                            }
                            break;
                        case "GroupByBeKiKodCB":
                            createChartsView.GroupByBeKiKodCheckBoxIsChecked = true;
                            if (createChartsView.SeriesType == "RowSeries" || createChartsView.SeriesType == "BasicColumnSeries" || createChartsView.SeriesType == "StackedColumnSeries")
                            {
                                GroupByMonthCB.IsEnabled = false;
                                GroupByMonthYearsCB.IsEnabled = false;
                                GroupByYearCB.IsEnabled = false;
                            }
                            break;
                        case "GroupByKifizetettCB":
                            createChartsView.GroupByKifizetettCheckBoxIsChecked = true;
                            if (createChartsView.SeriesType == "RowSeries" || createChartsView.SeriesType == "BasicColumnSeries" || createChartsView.SeriesType == "StackedColumnSeries")
                            {
                                GroupByMonthCB.IsEnabled = false;
                                GroupByMonthYearsCB.IsEnabled = false;
                                GroupByYearCB.IsEnabled = false;
                            }
                            break;
                        case "GroupByDateCB":
                            createChartsView.GroupByDateCheckBoxIsChecked = true;
                            break;
                        case "GroupByYearCB":
                            createChartsView.GroupByYearCheckBoxIsChecked = true;
                            if (createChartsView.SeriesType == "RowSeries" || createChartsView.SeriesType == "BasicColumnSeries" || createChartsView.SeriesType == "StackedColumnSeries")
                            {
                                GroupByKifizetettCB.IsEnabled = false;
                                GroupByPenznemCB.IsEnabled = false;
                                GroupByBeKiKodCB.IsEnabled = false;
                            }
                            else
                            {
                                GroupByMonthCB.IsEnabled = false;
                                GroupByMonthYearsCB.IsEnabled = false;
                            }
                            break;
                        case "GroupByMonthCB":
                            createChartsView.GroupByMonthCheckBoxIsChecked = true;
                            if (createChartsView.SeriesType == "RowSeries" || createChartsView.SeriesType == "BasicColumnSeries" || createChartsView.SeriesType == "StackedColumnSeries")
                            {
                                GroupByKifizetettCB.IsEnabled = false;
                                GroupByPenznemCB.IsEnabled = false;
                                GroupByBeKiKodCB.IsEnabled = false;
                            }
                            else
                            {
                                GroupByYearCB.IsEnabled = false;
                            }
                            if (createChartsView.SelectedBevetelekKiadasok.Count > 0)
                            {
                                createChartsView.Years = new ObservableCollection<int>(
                                    createChartsView._selectedBevetelekKiadasok
                                    .Select(x => x.TeljesitesiDatum.Year)
                                    .Distinct()
                                    .OrderBy(year => year)
                                );
                            }
                            else if (createChartsView.SelectedKotelezettsegekKovetelesek.Count > 0)
                            {
                                createChartsView.Years = new ObservableCollection<int>(
                                    createChartsView._selectedKotelezettsegekKovetelesek
                                    .Select(x => x.KifizetesHatarideje.Year)
                                    .Distinct()
                                    .OrderBy(year => year)
                                );
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ChangeSelection_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateChartsViewModel viewModel)
            {
                string senderName = (sender as FrameworkElement)?.Name;
                //SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90EE90"));
                if (chartsTabControl.SelectedContent is System.Windows.Controls.DataGrid dataGrid)
                {
                    if (dataGrid.Name.Equals("bevetelek_kiadasok"))
                    {
                        int[] changeableItems = new int[dataGrid.SelectedItems.Count];
                        int a = 0;
                        foreach (var item in dataGrid.SelectedItems)
                        {
                            BevetelKiadas i = (BevetelKiadas)item;
                            changeableItems[a] = i.ID;
                            if (senderName == "cellSelectionTrue")
                            {
                                if(viewModel.SelectedBevetelekKiadasok.FirstOrDefault(x => x.ID == i.ID) == null)
                                {
                                    viewModel.SelectedBevetelekKiadasok.Add(i);
                                    viewModel.IsEnabledChangerOnTabItems();
                                }
                            }
                            else
                            {
                                if (viewModel.SelectedBevetelekKiadasok.FirstOrDefault(x => x.ID == i.ID) != null)
                                {
                                    foreach(var b in viewModel.SelectedBevetelekKiadasok)
                                    {
                                        if(b.ID == i.ID)
                                        {
                                            viewModel.SelectedBevetelekKiadasok.Remove(b);
                                            viewModel.IsEnabledChangerOnTabItems();
                                            break;
                                        }
                                    }
                                }
                            }
                            a++;
                        }
                        if (senderName == "cellSelectionTrue")
                        {
                            viewModel.ChangeIsSelectedAndRefreshBevetelKiadas(changeableItems, true);
                        }
                        else
                        {
                            viewModel.ChangeIsSelectedAndRefreshBevetelKiadas(changeableItems, false);
                        }
                        if (viewModel.GroupByMonthCheckBoxIsChecked)
                        {
                            viewModel.Years = new ObservableCollection<int>(
                                viewModel._selectedBevetelekKiadasok
                                .Select(x => x.TeljesitesiDatum.Year)
                                .Distinct()
                                .OrderBy(year => year)
                            );
                        }
                    }
                    else if (dataGrid.Name.Equals("kotelezettsegek_kovetelesek"))
                    {
                        int[] changeableItems = new int[dataGrid.SelectedItems.Count];
                        int a = 0;
                        foreach (var item in dataGrid.SelectedItems)
                        {
                            KotelezettsegKoveteles i = (KotelezettsegKoveteles)item;
                            changeableItems[a] = i.ID;
                            if (senderName == "cellSelectionTrue")
                            {
                                if (viewModel.SelectedKotelezettsegekKovetelesek.FirstOrDefault(x => x.ID == i.ID) == null)
                                {
                                    viewModel.SelectedKotelezettsegekKovetelesek.Add(i);
                                    viewModel.IsEnabledChangerOnTabItems();
                                }
                            }
                            else
                            {
                                if (viewModel.SelectedKotelezettsegekKovetelesek.FirstOrDefault(x => x.ID == i.ID) != null)
                                {
                                    foreach (var b in viewModel.SelectedKotelezettsegekKovetelesek)
                                    {
                                        if (b.ID == i.ID)
                                        {
                                            viewModel.SelectedKotelezettsegekKovetelesek.Remove(b);
                                            viewModel.IsEnabledChangerOnTabItems();
                                            break;
                                        }
                                    }
                                }
                            }
                            a++;
                        }
                        if (senderName == "cellSelectionTrue")
                        {
                            viewModel.ChangeIsSelectedAndRefreshKotelKovet(changeableItems, true);
                        }
                        else
                        {
                            viewModel.ChangeIsSelectedAndRefreshKotelKovet(changeableItems, false);
                        }
                    }
                }
            }
        }
        private void SetDiagramsVisibility(string seriesType)
        {
            switch (seriesType)
            {
                case "DoghnutSeries":
                    pieSeries.Visibility = Visibility.Visible;
                    pieSeriesRadiusSlider.Visibility = Visibility.Visible;
                    break;
                case "RowSeries":
                    rowSeries.Visibility = Visibility.Visible;
                    GroupByMonthYearsCB.Visibility = Visibility.Hidden;
                    break;
                case "StackedColumnSeries":
                    stackedColumnSeries.Visibility = Visibility.Visible;
                    GroupByMonthYearsCB.Visibility = Visibility.Hidden;
                    break;
                case "BasicColumnSeries":
                    columnSeries.Visibility = Visibility.Visible;
                    GroupByMonthYearsCB.Visibility = Visibility.Hidden;
                    break;
                case "LineSeries":
                    lineSeries.Visibility = Visibility.Visible;
                    GroupByMonthYearsCB.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
        }

        //Ha combobox-on belüli elemen kattintás történik, akkor ez megakadájozza, hogy a placeholder szöveg ne legyen lecserélve
        //If a click occurs on a comboboxitem, then this method prevents any changes to the combobox placeholder text
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox comboBox = (System.Windows.Controls.ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                if (comboBox.Name.Equals("KoltsegvetesCB"))
                {
                    string selectedName = ((ComboBoxItem)comboBox.SelectedItem).Name;
                    if (!selectedName.Equals("Koltsegvetes_CB_default_text"))
                        comboBox.SelectedIndex = 0;
                }
                else if (comboBox.Name.Equals("KotelKovetCB"))
                {
                    string selectedName = ((ComboBoxItem)comboBox.SelectedItem).Name;
                    if (!selectedName.Equals("KotelKovet_CB_default_text"))
                        comboBox.SelectedIndex = 0;
                }
            }
        }

        private void AddDataToChart_Click(object sender, RoutedEventArgs e)
        {
            if(DataContext is CreateChartsViewModel viewModel)
            {
                viewModel.SetSeries();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox)
            {
                if (DataContext is CreateChartsViewModel createChartsView)
                {
                    if (checkBox.Name.Contains("koltsegvetes"))
                    {
                        try
                        {
                            switch (checkBox.Name)
                            {
                                case "koltsegvetes_mindCB":
                                    createChartsView.checkboxStatuses["koltsegvetes_mindCB"] = true;
                                    createChartsView.checkboxStatuses["koltsegvetes_idCB"] = true;
                                    koltsegvetes_idCB.IsChecked = true;
                                    createChartsView.checkboxStatuses["koltsegvetes_osszegCB"] = true;
                                    koltsegvetes_osszegCB.IsChecked = true;
                                    createChartsView.checkboxStatuses["koltsegvetes_penznemCB"] = true;
                                    koltsegvetes_penznemCB.IsChecked = true;
                                    createChartsView.checkboxStatuses["koltsegvetes_bekikodCB"] = true;
                                    koltsegvetes_bekikodCB.IsChecked = true;
                                    createChartsView.checkboxStatuses["koltsegvetes_teljesitesiDatumCB"] = true;
                                    koltsegvetes_teljesitesiDatumCB.IsChecked = true;
                                    createChartsView.checkboxStatuses["koltsegvetes_kotelKovetIDCB"] = true;
                                    koltsegvetes_kotelKovetIDCB.IsChecked = true;
                                    createChartsView.checkboxStatuses["koltsegvetes_partnerIDCB"] = true;
                                    koltsegvetes_partnerIDCB.IsChecked = true;
                                    break;
                                case "koltsegvetes_idCB":
                                    createChartsView.checkboxStatuses["koltsegvetes_idCB"] = true;
                                    break;
                                case "koltsegvetes_osszegCB":
                                    createChartsView.checkboxStatuses["koltsegvetes_osszegCB"] = true;
                                    break;
                                case "koltsegvetes_penznemCB":
                                    createChartsView.checkboxStatuses["koltsegvetes_penznemCB"] = true;
                                    break;
                                case "koltsegvetes_bekikodCB":
                                    createChartsView.checkboxStatuses["koltsegvetes_bekikodCB"] = true;
                                    break;
                                case "koltsegvetes_teljesitesiDatumCB":
                                    createChartsView.checkboxStatuses["koltsegvetes_teljesitesiDatumCB"] = true;
                                    break;
                                case "koltsegvetes_kotelKovetIDCB":
                                    createChartsView.checkboxStatuses["koltsegvetes_kotelKovetIDCB"] = true;
                                    break;
                                case "koltsegvetes_partnerIDCB":
                                    createChartsView.checkboxStatuses["koltsegvetes_partnerIDCB"] = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (checkBox.Name.Contains("kotelKovet"))
                    {
                        switch (checkBox.Name)
                        {
                            case "kotelKovet_mindCB":
                                createChartsView.checkboxStatuses["koltsegvetes_mindCB"] = true;
                                createChartsView.checkboxStatuses["kotelKovet_idCB"] = true;
                                kotelKovet_idCB.IsChecked = true;
                                createChartsView.checkboxStatuses["kotelKovet_osszegCB"] = true;
                                kotelKovet_osszegCB.IsChecked = true;
                                createChartsView.checkboxStatuses["kotelKovet_penznemCB"] = true;
                                kotelKovet_penznemCB.IsChecked = true;
                                createChartsView.checkboxStatuses["kotelKovet_tipusCB"] = true;
                                kotelKovet_tipusCB.IsChecked = true;
                                createChartsView.checkboxStatuses["kotelKovet_kifizetesHataridejeCB"] = true;
                                kotelKovet_kifizetesHataridejeCB.IsChecked = true;
                                createChartsView.checkboxStatuses["kotelKovet_kifizetettCB"] = true;
                                kotelKovet_kifizetettCB.IsChecked = true;
                                break;
                            case "kotelKovet_idCB":
                                createChartsView.checkboxStatuses["kotelKovet_idCB"] = true;
                                break;
                            case "kotelKovet_osszegCB":
                                createChartsView.checkboxStatuses["kotelKovet_osszegCB"] = true;
                                break;
                            case "kotelKovet_penznemCB":
                                createChartsView.checkboxStatuses["kotelKovet_penznemCB"] = true;
                                break;
                            case "kotelKovet_tipusCB":
                                createChartsView.checkboxStatuses["kotelKovet_tipusCB"] = true;
                                break;
                            case "kotelKovet_kifizetesHataridejeCB":
                                createChartsView.checkboxStatuses["kotelKovet_kifizetesHataridejeCB"] = true;
                                break;
                            case "kotelKovet_kifizetettCB":
                                createChartsView.checkboxStatuses["kotelKovet_kifizetettCB"] = true;
                                break;
                            default:
                                break;
                        }
                    }
                    createChartsView.UpdateSearch(createChartsView.SearchQuery);
                }
            }
        }
        

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox)
            {
                if (DataContext is CreateChartsViewModel createChartsView)
                {
                    if (checkBox.Name.Contains("koltsegvetes"))
                    {
                        switch (checkBox.Name)
                        {
                            case "koltsegvetes_mindCB":
                                createChartsView.checkboxStatuses["koltsegvetes_mindCB"] = false;
                                break;
                            case "koltsegvetes_idCB":
                                createChartsView.checkboxStatuses["koltsegvetes_idCB"] = false;
                                break;
                            case "koltsegvetes_osszegCB":
                                createChartsView.checkboxStatuses["koltsegvetes_osszegCB"] = false;
                                break;
                            case "koltsegvetes_penznemCB":
                                createChartsView.checkboxStatuses["koltsegvetes_penznemCB"] = false;
                                break;
                            case "koltsegvetes_bekikodCB":
                                createChartsView.checkboxStatuses["koltsegvetes_bekikodCB"] = false;
                                break;
                            case "koltsegvetes_teljesitesiDatumCB":
                                createChartsView.checkboxStatuses["koltsegvetes_teljesitesiDatumCB"] = false;
                                break;
                            case "koltsegvetes_kotelKovetIDCB":
                                createChartsView.checkboxStatuses["koltsegvetes_kotelKovetIDCB"] = false;
                                break;
                            case "koltsegvetes_partnerIDCB":
                                createChartsView.checkboxStatuses["koltsegvetes_partnerIDCB"] = false;
                                break;
                            default:
                                break;
                        }
                    }
                    else if (checkBox.Name.Contains("kotelKovet"))
                    {
                        switch (checkBox.Name)
                        {
                            case "kotelKovet_mindCB":
                                createChartsView.checkboxStatuses["kotelKovet_mindCB"] = false;
                                break;
                            case "kotelKovet_idCB":
                                createChartsView.checkboxStatuses["kotelKovet_idCB"] = false;
                                break;
                            case "kotelKovet_osszegCB":
                                createChartsView.checkboxStatuses["kotelKovet_osszegCB"] = false;
                                break;
                            case "kotelKovet_penznemCB":
                                createChartsView.checkboxStatuses["kotelKovet_penznemCB"] = false;
                                break;
                            case "kotelKovet_tipusCB":
                                createChartsView.checkboxStatuses["kotelKovet_tipusCB"] = false;
                                break;
                            case "kotelKovet_kifizetesHataridejeCB":
                                createChartsView.checkboxStatuses["kotelKovet_kifizetesHataridejeCB"] = false;
                                break;
                            case "kotelKovet_kifizetettCB":
                                createChartsView.checkboxStatuses["kotelKovet_kifizetettCB"] = false;
                                break;
                            default:
                                break;
                        }
                    }
                    createChartsView.UpdateSearch(createChartsView.SearchQuery);
                }
            }
        }

        private void chartsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource is System.Windows.Controls.TabControl)
            {
                if (DataContext is CreateChartsViewModel viewModel)
                {
                    if (chartsTabControl.SelectedItem is System.Windows.Controls.TabItem tabItem)
                    {
                        if (viewModel.SeriesType == "DoghnutSeries")
                        {
                            if (tabItem.Name.Contains("Koltsegvetes"))
                            {
                                viewModel.IsBevetelekKiadasokTabIsSelected = true;
                                KoltsegvetesCB.Visibility = Visibility.Visible;
                                KotelKovetCB.Visibility = Visibility.Collapsed;
                                GroupByKifizetettCB.Visibility = Visibility.Collapsed;
                                GroupByBeKiKodCB.Visibility = Visibility.Visible;
                            }
                            else if (tabItem.Name.Contains("KotelKovet"))
                            {
                                viewModel.IsBevetelekKiadasokTabIsSelected = false;
                                KoltsegvetesCB.Visibility = Visibility.Collapsed;
                                KotelKovetCB.Visibility = Visibility.Visible;
                                GroupByKifizetettCB.Visibility = Visibility.Visible;
                                GroupByBeKiKodCB.Visibility = Visibility.Collapsed;
                            }
                            GroupByYearCB.Visibility = Visibility.Visible;
                            GroupByMonthCB.Visibility = Visibility.Visible;
                            GroupByDateCB.Visibility = Visibility.Collapsed;
                            cimkekAdatsorokDataGrid.Visibility = Visibility.Collapsed;
                        }
                        else if (viewModel.SeriesType == "LineSeries")
                        {
                            if (tabItem.Name.Contains("Koltsegvetes"))
                            {
                                viewModel.IsBevetelekKiadasokTabIsSelected = true;
                                KoltsegvetesCB.Visibility = Visibility.Visible;
                                KotelKovetCB.Visibility = Visibility.Collapsed;
                                GroupByKifizetettCB.Visibility = Visibility.Collapsed;
                                GroupByBeKiKodCB.Visibility = Visibility.Visible;
                            }
                            else if (tabItem.Name.Contains("KotelKovet"))
                            {
                                viewModel.IsBevetelekKiadasokTabIsSelected = false;
                                KoltsegvetesCB.Visibility = Visibility.Collapsed;
                                KotelKovetCB.Visibility = Visibility.Visible;
                                GroupByKifizetettCB.Visibility = Visibility.Visible;
                                GroupByBeKiKodCB.Visibility = Visibility.Collapsed;
                            }
                            GroupByYearCB.Visibility = Visibility.Collapsed;
                            GroupByMonthCB.Visibility = Visibility.Collapsed;
                            cimkekAdatsorokDataGrid.Visibility = Visibility.Collapsed;
                        }
                        else if (viewModel.SeriesType == "RowSeries")
                        {
                            if (tabItem.Name.Contains("Koltsegvetes"))
                            {
                                viewModel.IsBevetelekKiadasokTabIsSelected = true;
                                KoltsegvetesCB.Visibility = Visibility.Visible;
                                KotelKovetCB.Visibility = Visibility.Collapsed;
                                GroupByKifizetettCB.Visibility = Visibility.Collapsed;
                                GroupByBeKiKodCB.Visibility = Visibility.Visible;
                            }
                            else if (tabItem.Name.Contains("KotelKovet"))
                            {
                                viewModel.IsBevetelekKiadasokTabIsSelected = false;
                                KoltsegvetesCB.Visibility = Visibility.Collapsed;
                                KotelKovetCB.Visibility = Visibility.Visible;
                                GroupByKifizetettCB.Visibility = Visibility.Visible;
                                GroupByBeKiKodCB.Visibility = Visibility.Collapsed;
                            }
                            GroupByYearCB.Visibility = Visibility.Visible;
                            GroupByMonthCB.Visibility = Visibility.Visible;
                            GroupByDateCB.Visibility = Visibility.Collapsed;
                        }
                        else if (viewModel.SeriesType == "BasicColumnSeries")
                        {
                            if (tabItem.Name.Contains("Koltsegvetes"))
                            {
                                viewModel.IsBevetelekKiadasokTabIsSelected = true;
                                KoltsegvetesCB.Visibility = Visibility.Visible;
                                KotelKovetCB.Visibility = Visibility.Collapsed;
                                GroupByKifizetettCB.Visibility = Visibility.Collapsed;
                                GroupByBeKiKodCB.Visibility = Visibility.Visible;
                            }
                            else if (tabItem.Name.Contains("KotelKovet"))
                            {
                                viewModel.IsBevetelekKiadasokTabIsSelected = false;
                                KoltsegvetesCB.Visibility = Visibility.Collapsed;
                                KotelKovetCB.Visibility = Visibility.Visible;
                                GroupByKifizetettCB.Visibility = Visibility.Visible;
                                GroupByBeKiKodCB.Visibility = Visibility.Collapsed;
                            }
                            GroupByYearCB.Visibility = Visibility.Visible;
                            GroupByMonthCB.Visibility = Visibility.Visible;
                            GroupByDateCB.Visibility = Visibility.Collapsed;
                        }
                        else if (viewModel.SeriesType == "StackedColumnSeries")
                        {
                            if (tabItem.Name.Contains("Koltsegvetes"))
                            {
                                viewModel.IsBevetelekKiadasokTabIsSelected = true;
                                KoltsegvetesCB.Visibility = Visibility.Visible;
                                KotelKovetCB.Visibility = Visibility.Collapsed;
                                GroupByKifizetettCB.Visibility = Visibility.Collapsed;
                                GroupByBeKiKodCB.Visibility = Visibility.Visible;
                            }
                            else if (tabItem.Name.Contains("KotelKovet"))
                            {
                                viewModel.IsBevetelekKiadasokTabIsSelected = false;
                                KoltsegvetesCB.Visibility = Visibility.Collapsed;
                                KotelKovetCB.Visibility = Visibility.Visible;
                                GroupByKifizetettCB.Visibility = Visibility.Visible;
                                GroupByBeKiKodCB.Visibility = Visibility.Collapsed;
                            }
                            GroupByYearCB.Visibility = Visibility.Visible;
                            GroupByMonthCB.Visibility = Visibility.Visible;
                            GroupByDateCB.Visibility = Visibility.Collapsed;
                        }

                        GroupByPenznemCB.IsChecked = false;
                        GroupByKifizetettCB.IsChecked = false;
                        GroupByMonthCB.IsChecked = false;
                        GroupByDateCB.IsChecked = false;
                        GroupByBeKiKodCB.IsChecked = false;
                        GroupByYearCB.IsChecked = false;
                    }
                }
            }
        }

        private void deleteAllDataSelection_Click(object sender, RoutedEventArgs e)
        {
            if(DataContext is CreateChartsViewModel createChartsViewModel)
            {
                Mediator.GetTabControl -= () => chartsTabControl;
                Mediator.SetSeriesVisibility -= SetDiagramsVisibility;
                createChartsViewModel.UnCheckAllSelections();
            }
        }

        private void pnlControlBar_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
    }
}
