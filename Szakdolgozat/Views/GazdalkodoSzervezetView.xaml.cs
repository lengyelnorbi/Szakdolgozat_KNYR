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
using Szakdolgozat.ViewModels;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for GazdalkodoSzervezetView.xaml
    /// </summary>
    public partial class GazdalkodoSzervezetView : UserControl
    {
        public GazdalkodoSzervezetView()
        {
            InitializeComponent();
            
            mindCB.Checked += CheckBox_Checked;
            idCB.Checked += CheckBox_Checked;
            nevCB.Checked += CheckBox_Checked;
            kapcsolattartoCB.Checked += CheckBox_Checked;
            emailCB.Checked += CheckBox_Checked;
            telefonszamCB.Checked += CheckBox_Checked;
            mindCB.Unchecked += Checkbox_Unchecked;
            idCB.Unchecked += Checkbox_Unchecked;
            nevCB.Unchecked += Checkbox_Unchecked;
            kapcsolattartoCB.Unchecked += Checkbox_Unchecked;
            emailCB.Unchecked += Checkbox_Unchecked;
            telefonszamCB.Unchecked += Checkbox_Unchecked;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is GazdalkodoSzervezetViewModel gazdalkodoSzervezetViewModel)
                {
                    try
                    {
                        switch (checkBox.Name)
                        {
                            case "mindCB":
                                gazdalkodoSzervezetViewModel.checkboxStatuses["mindCB"] = true;
                                gazdalkodoSzervezetViewModel.checkboxStatuses["idCB"] = true;
                                idCB.IsChecked = true;
                                gazdalkodoSzervezetViewModel.checkboxStatuses["nevCB"] = true;
                                nevCB.IsChecked = true;
                                gazdalkodoSzervezetViewModel.checkboxStatuses["kapcsolattartoCB"] = true;
                                kapcsolattartoCB.IsChecked = true;
                                gazdalkodoSzervezetViewModel.checkboxStatuses["emailCB"] = true;
                                emailCB.IsChecked = true;
                                gazdalkodoSzervezetViewModel.checkboxStatuses["telefonszamCB"] = true;
                                telefonszamCB.IsChecked = true;
                                break;
                            case "idCB":
                                gazdalkodoSzervezetViewModel.checkboxStatuses["idCB"] = true;
                                break;
                            case "nevCB":
                                gazdalkodoSzervezetViewModel.checkboxStatuses["nevCB"] = true;
                                break;
                            case "kapcsolattartoCB":
                                gazdalkodoSzervezetViewModel.checkboxStatuses["kapcsolattartoCB"] = true;
                                break;
                            case "emailCB":
                                gazdalkodoSzervezetViewModel.checkboxStatuses["emailCB"] = true;
                                break;
                            case "telefonszamCB":
                                gazdalkodoSzervezetViewModel.checkboxStatuses["telefonszamCB"] = true;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    gazdalkodoSzervezetViewModel.UpdateSearch(gazdalkodoSzervezetViewModel.SearchQuery);
                }
            }

        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is GazdalkodoSzervezetViewModel gazdalkodoSzervezetViewModel)
                {
                    switch (checkBox.Name)
                    {
                        case "mindCB":
                            gazdalkodoSzervezetViewModel.checkboxStatuses["mindCB"] = false;
                            break;
                        case "idCB":
                            gazdalkodoSzervezetViewModel.checkboxStatuses["idCB"] = false;
                            break;
                        case "nevCB":
                            gazdalkodoSzervezetViewModel.checkboxStatuses["nevCB"] = false;
                            break;
                        case "kapcsolattartoCB":
                            gazdalkodoSzervezetViewModel.checkboxStatuses["kapcsolattartoCB"] = false;
                            break;
                        case "emailCB":
                            gazdalkodoSzervezetViewModel.checkboxStatuses["emailCB"] = false;
                            break;
                        case "telefonszamCB":
                            gazdalkodoSzervezetViewModel.checkboxStatuses["telefonszamCB"] = false;
                            break;
                        default:
                            break;
                    }
                    gazdalkodoSzervezetViewModel.UpdateSearch(gazdalkodoSzervezetViewModel.SearchQuery);
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
