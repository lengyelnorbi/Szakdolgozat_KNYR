using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Szakdolgozat.Models;
using Szakdolgozat.ViewModels;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for MaganSzemelyekView.xaml
    /// </summary>
    public partial class MaganSzemelyekView : UserControl
    {
        public MaganSzemelyekView()
        {
            InitializeComponent();

            mindCB.Checked += CheckBox_Checked;
            idCB.Checked += CheckBox_Checked;
            nevCB.Checked += CheckBox_Checked;
            telefonszamCB.Checked += CheckBox_Checked;
            emailCB.Checked += CheckBox_Checked;
            lakcimCB.Checked += CheckBox_Checked;
            mindCB.Unchecked += Checkbox_Unchecked;
            idCB.Unchecked += Checkbox_Unchecked;
            nevCB.Unchecked += Checkbox_Unchecked;
            telefonszamCB.Unchecked += Checkbox_Unchecked;
            emailCB.Unchecked += Checkbox_Unchecked;
            lakcimCB.Unchecked += Checkbox_Unchecked;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is MaganSzemelyekViewModel maganSzemelyekViewModel)
                {
                    try
                    {
                        switch (checkBox.Name)
                        {
                            case "mindCB":
                                maganSzemelyekViewModel.checkboxStatuses["mindCB"] = true;
                                maganSzemelyekViewModel.checkboxStatuses["idCB"] = true;
                                idCB.IsChecked = true;
                                maganSzemelyekViewModel.checkboxStatuses["nevCB"] = true;
                                nevCB.IsChecked = true;
                                maganSzemelyekViewModel.checkboxStatuses["telefonszamCB"] = true;
                                telefonszamCB.IsChecked = true;
                                maganSzemelyekViewModel.checkboxStatuses["emailCB"] = true;
                                emailCB.IsChecked = true;
                                maganSzemelyekViewModel.checkboxStatuses["lakcimCB"] = true;
                                lakcimCB.IsChecked = true;
                                break;
                            case "idCB":
                                maganSzemelyekViewModel.checkboxStatuses["idCB"] = true;
                                break;
                            case "nevCB":
                                maganSzemelyekViewModel.checkboxStatuses["nevCB"] = true;
                                break;
                            case "telefonszamCB":
                                maganSzemelyekViewModel.checkboxStatuses["telefonszamCB"] = true;
                                break;
                            case "emailCB":
                                maganSzemelyekViewModel.checkboxStatuses["emailCB"] = true;
                                break;
                            case "lakcimCB":
                                maganSzemelyekViewModel.checkboxStatuses["lakcimCB"] = true;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    maganSzemelyekViewModel.UpdateSearch(maganSzemelyekViewModel.SearchQuery);
                }
            }

        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is MaganSzemelyekViewModel maganSzemelyekViewModel)
                {
                    switch (checkBox.Name)
                    {
                        case "mindCB":
                            maganSzemelyekViewModel.checkboxStatuses["mindCB"] = false;
                            break;
                        case "idCB":
                            maganSzemelyekViewModel.checkboxStatuses["idCB"] = false;
                            break;
                        case "nevCB":
                            maganSzemelyekViewModel.checkboxStatuses["nevCB"] = false;
                            break;
                        case "telefonszamCB":
                            maganSzemelyekViewModel.checkboxStatuses["telefonszamCB"] = false;
                            break;
                        case "emailCB":
                            maganSzemelyekViewModel.checkboxStatuses["emailCB"] = false;
                            break;
                        case "lakcimCB":
                            maganSzemelyekViewModel.checkboxStatuses["lakcimCB"] = false;
                            break;
                        default:
                            break;
                    }
                    maganSzemelyekViewModel.UpdateSearch(maganSzemelyekViewModel.SearchQuery);
                }
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                string selectedName = ((ComboBoxItem)comboBox.SelectedItem).Name;
                if (!selectedName.Equals("CB_default_text"))
                    comboBox.SelectedIndex = 0;
            }
        }
    }
}
