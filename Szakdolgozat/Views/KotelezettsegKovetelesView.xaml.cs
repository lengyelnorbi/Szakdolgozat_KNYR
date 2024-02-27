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
    /// Interaction logic for KotelezettsegKovetelesView.xaml
    /// </summary>
    public partial class KotelezettsegKovetelesView : UserControl
    {
        public KotelezettsegKovetelesView()
        {
            InitializeComponent();

            mindCB.Checked += CheckBox_Checked;
            idCB.Checked += CheckBox_Checked;
            tipusCB.Checked += CheckBox_Checked;
            osszegCB.Checked += CheckBox_Checked;
            penznemCB.Checked += CheckBox_Checked;
            kifizetesHataridejeCB.Checked += CheckBox_Checked;
            kifizetettCB.Checked += CheckBox_Checked;
            mindCB.Unchecked += Checkbox_Unchecked;
            idCB.Unchecked += Checkbox_Unchecked;
            tipusCB.Unchecked += Checkbox_Unchecked;
            osszegCB.Unchecked += Checkbox_Unchecked;
            penznemCB.Unchecked += Checkbox_Unchecked;
            kifizetesHataridejeCB.Unchecked += Checkbox_Unchecked;
            kifizetettCB.Unchecked += Checkbox_Unchecked;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is KotelezettsegKovetelesViewModel kotelezettsegKovetelesViewModel)
                {
                    try
                    {
                        switch (checkBox.Name)
                        {
                            case "mindCB":
                                kotelezettsegKovetelesViewModel.checkboxStatuses["mindCB"] = true;
                                kotelezettsegKovetelesViewModel.checkboxStatuses["idCB"] = true;
                                idCB.IsChecked = true;
                                kotelezettsegKovetelesViewModel.checkboxStatuses["tipusCB"] = true;
                                tipusCB.IsChecked = true;
                                kotelezettsegKovetelesViewModel.checkboxStatuses["osszegCB"] = true;
                                osszegCB.IsChecked = true;
                                kotelezettsegKovetelesViewModel.checkboxStatuses["penznemCB"] = true;
                                penznemCB.IsChecked = true;
                                kotelezettsegKovetelesViewModel.checkboxStatuses["kifizetesHataridejeCB"] = true;
                                kifizetesHataridejeCB.IsChecked = true;
                                kotelezettsegKovetelesViewModel.checkboxStatuses["kifizetettCB"] = true;
                                kifizetettCB.IsChecked = true;
                                break;
                            case "idCB":
                                kotelezettsegKovetelesViewModel.checkboxStatuses["idCB"] = true;
                                break;
                            case "tipusCB":
                                kotelezettsegKovetelesViewModel.checkboxStatuses["tipusCB"] = true;
                                break;
                            case "osszegCB":
                                kotelezettsegKovetelesViewModel.checkboxStatuses["osszegCB"] = true;
                                break;
                            case "penznemCB":
                                kotelezettsegKovetelesViewModel.checkboxStatuses["penznemCB"] = true;
                                break;
                            case "kifizetesHataridejeCB":
                                kotelezettsegKovetelesViewModel.checkboxStatuses["kifizetesHataridejeCB"] = true;
                                break;
                            case "kifizetettCB":
                                kotelezettsegKovetelesViewModel.checkboxStatuses["kifizetettCB"] = true;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    kotelezettsegKovetelesViewModel.UpdateSearch(kotelezettsegKovetelesViewModel.SearchQuery);
                }
            }

        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is KotelezettsegKovetelesViewModel kotelezettsegKovetelesViewModel)
                {
                    switch (checkBox.Name)
                    {
                        case "mindCB":
                            kotelezettsegKovetelesViewModel.checkboxStatuses["mindCB"] = false;
                            break;
                        case "idCB":
                            kotelezettsegKovetelesViewModel.checkboxStatuses["idCB"] = false;
                            break;
                        case "tipusCB":
                            kotelezettsegKovetelesViewModel.checkboxStatuses["tipusCB"] = false;
                            break;
                        case "osszegCB":
                            kotelezettsegKovetelesViewModel.checkboxStatuses["osszegCB"] = false;
                            break;
                        case "penznemCB":
                            kotelezettsegKovetelesViewModel.checkboxStatuses["penznemCB"] = false;
                            break;
                        case "kifizetesHataridejeCB":
                            kotelezettsegKovetelesViewModel.checkboxStatuses["kifizetesHataridejeCB"] = false;
                            break;
                        case "kifizetettCB":
                            kotelezettsegKovetelesViewModel.checkboxStatuses["kifizetettCB"] = false;
                            break;
                        default:
                            break;
                    }
                    kotelezettsegKovetelesViewModel.UpdateSearch(kotelezettsegKovetelesViewModel.SearchQuery);
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
