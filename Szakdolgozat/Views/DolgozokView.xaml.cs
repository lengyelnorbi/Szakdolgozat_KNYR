﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
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
    /// Interaction logic for DolgozokView.xaml
    /// </summary>
    public partial class DolgozokView : UserControl
    {
        private Boolean _isAddOpen = false;
        private Boolean _isModifyOpen = false;

        public DolgozokView()
        {
            InitializeComponent();

            mindCB.Checked += CheckBox_Checked;
            idCB.Checked += CheckBox_Checked;
            vezeteknevCB.Checked += CheckBox_Checked;
            keresztnevCB.Checked += CheckBox_Checked;
            emailCB.Checked += CheckBox_Checked;
            telefonszamCB.Checked += CheckBox_Checked;
            mindCB.Unchecked += Checkbox_Unchecked;
            idCB.Unchecked += Checkbox_Unchecked;
            vezeteknevCB.Unchecked += Checkbox_Unchecked;
            keresztnevCB.Unchecked += Checkbox_Unchecked;
            emailCB.Unchecked += Checkbox_Unchecked;
            telefonszamCB.Unchecked += Checkbox_Unchecked;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is DolgozoViewModel dolgozoViewModel)
                {
                    try
                    {
                        switch (checkBox.Name)
                        {
                            case "mindCB":
                                dolgozoViewModel.checkboxStatuses["mindCB"] = true;
                                dolgozoViewModel.checkboxStatuses["idCB"] = true;
                                idCB.IsChecked = true;
                                dolgozoViewModel.checkboxStatuses["vezeteknevCB"] = true;
                                vezeteknevCB.IsChecked = true;
                                dolgozoViewModel.checkboxStatuses["keresztnevCB"] = true;
                                keresztnevCB.IsChecked = true;
                                dolgozoViewModel.checkboxStatuses["emailCB"] = true;
                                emailCB.IsChecked = true;
                                dolgozoViewModel.checkboxStatuses["telefonszamCB"] = true;
                                telefonszamCB.IsChecked = true;
                                break;
                            case "idCB":
                                dolgozoViewModel.checkboxStatuses["idCB"] = true;
                                break;
                            case "vezeteknevCB":
                                dolgozoViewModel.checkboxStatuses["vezeteknevCB"] = true;
                                break;
                            case "keresztnevCB":
                                dolgozoViewModel.checkboxStatuses["keresztnevCB"] = true;
                                break;
                            case "emailCB":
                                dolgozoViewModel.checkboxStatuses["emailCB"] = true;
                                break;
                            case "telefonszamCB":
                                dolgozoViewModel.checkboxStatuses["telefonszamCB"] = true;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    dolgozoViewModel.UpdateSearch(dolgozoViewModel.SearchQuery);
                }
            }

        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (DataContext is DolgozoViewModel dolgozoViewModel)
                {
                    switch (checkBox.Name)
                    {
                        case "mindCB":
                            dolgozoViewModel.checkboxStatuses["mindCB"] = false;
                            break;
                        case "idCB":
                            dolgozoViewModel.checkboxStatuses["idCB"] = false;
                            break;
                        case "vezeteknevCB":
                            dolgozoViewModel.checkboxStatuses["vezeteknevCB"] = false;
                            break;
                        case "keresztnevCB":
                            dolgozoViewModel.checkboxStatuses["keresztnevCB"] = false;
                            break;
                        case "emailCB":
                            dolgozoViewModel.checkboxStatuses["emailCB"] = false;
                            break;
                        case "telefonszamCB":
                            dolgozoViewModel.checkboxStatuses["telefonszamCB"] = false;
                            break;
                        default:
                            break;
                    }
                    dolgozoViewModel.UpdateSearch(dolgozoViewModel.SearchQuery);
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                string selectedName = ((ComboBoxItem)comboBox.SelectedItem).Name;
                if(!selectedName.Equals("CB_default_text"))
                    comboBox.SelectedIndex = 0;
            }
        }

        private void Add_Button_Clicked(object sender, RoutedEventArgs e)
        {
            if (_isModifyOpen)
            {
                _isModifyOpen = false;
            }
            if (!_isAddOpen)
            {
                dataGridColumnDef.Width = new GridLength(700);
                sidebar_lastnameTB.Text = string.Empty;
                sidebar_firstnameTB.Text = string.Empty;
                sidebar_emailTB.Text = string.Empty;
                sidebar_phonenumberTB.Text = string.Empty;
                _isAddOpen = true;
            }
        }
        private void Modify_Button_Clicked(object sender, RoutedEventArgs e)
        {
            if (_isAddOpen)
            {
                _isAddOpen = false;
            }
            if (DataContext is DolgozoViewModel dolgozoViewModel)
            {
                try
                {
                    if(dolgozoViewModel.SelectedRow != null)
                    {
                        if (!_isModifyOpen)
                        {
                            dataGridColumnDef.Width = new GridLength(700);
                            _isModifyOpen = true;
                            sidebar_lastnameTB.Text = dolgozoViewModel.SelectedRow.Vezeteknev;
                            sidebar_firstnameTB.Text = dolgozoViewModel.SelectedRow.Keresztnev;
                            sidebar_emailTB.Text = dolgozoViewModel.SelectedRow.Email;
                            sidebar_phonenumberTB.Text = dolgozoViewModel.SelectedRow.Telefonszam;
                        }
                    }
                }
                catch { }
            }
        }

        private void Megse_Button_Clicked(object sender, RoutedEventArgs e)
        {
            if (_isAddOpen || _isModifyOpen)
            {
                dataGridColumnDef.Width = new GridLength(900);
                _isAddOpen = false;
                _isModifyOpen = false;
            }
        }
    }
}
