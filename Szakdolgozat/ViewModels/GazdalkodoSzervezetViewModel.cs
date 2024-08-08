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
using Szakdolgozat.Views;
using Szakdolgozat.Repositories;

namespace Szakdolgozat.ViewModels
{
    class GazdalkodoSzervezetViewModel : ViewModelBase
    {
        private ObservableCollection<GazdalkodoSzervezet> _gazdalkodoSzervezetek;
        private GazdalkodoSzervezetRepository _gazdalkodoSzervezetRepository;

        public ObservableCollection<GazdalkodoSzervezet> GazdalkodoSzervezetek
        {
            get { return _gazdalkodoSzervezetek; }
            set
            {
                _gazdalkodoSzervezetek = value;
                OnPropertyChanged(nameof(GazdalkodoSzervezetek));
            }
        }

        private ObservableCollection<GazdalkodoSzervezet> _filteredGazdalkodoSzervezetek;

        public ObservableCollection<GazdalkodoSzervezet> FilteredGazdalkodoSzervezetek
        {
            get { return _filteredGazdalkodoSzervezetek; }
            set
            {
                _filteredGazdalkodoSzervezetek = value;
                OnPropertyChanged(nameof(FilteredGazdalkodoSzervezetek));
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

        private GazdalkodoSzervezet _selectedRow;

        public GazdalkodoSzervezet SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        public Dictionary<string, bool> checkboxStatuses = new Dictionary<string, bool>();

        public GazdalkodoSzervezetViewModel()
        {
            _gazdalkodoSzervezetRepository = new GazdalkodoSzervezetRepository();
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("nevCB", true);
            checkboxStatuses.Add("kapcsolattartoCB", true);
            checkboxStatuses.Add("emailCB", true);
            checkboxStatuses.Add("telefonszamCB", true);
            GazdalkodoSzervezetek = _gazdalkodoSzervezetRepository.GetGazdalkodoSzervezetek();
            FilteredGazdalkodoSzervezetek = new ObservableCollection<GazdalkodoSzervezet>(
                GazdalkodoSzervezetek.Select(d => new GazdalkodoSzervezet(d.ID, d.Nev, d.Kapcsolattarto, d.Email, d.Telefonszam)).ToList()
            );

            DeleteGazdalkodoSzervezetCommand = new ViewModelCommand(ExecuteDeleteGazdalkodoSzervezetCommand, CanExecuteDeleteGazdalkodoSzervezetCommand);

            OpenGazdalkodoSzervezetModifyOrAddWindowCommand = new ViewModelCommand(ExecuteOpenGazdalkodoSzervezetModifyOrAddWindowCommand, CanExecuteOpenGazdalkodoSzervezetModifyOrAddWindowCommand);
        }

        private void FilterData(string searchQuery)
        {
            Debug.WriteLine(searchQuery);
            if (!string.IsNullOrWhiteSpace(searchQuery) && !string.IsNullOrEmpty(searchQuery))
            {
                FilteredGazdalkodoSzervezetek.Clear();
                foreach (var d in GazdalkodoSzervezetek)
                {
                    if (checkboxStatuses["idCB"] == true)
                    {
                        if (d.ID.ToString().Contains(searchQuery))
                        {
                            FilteredGazdalkodoSzervezetek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["nevCB"] == true)
                    {
                        if (d.Nev.Contains(searchQuery))
                        {
                            FilteredGazdalkodoSzervezetek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["kapcsolattartoCB"] == true)
                    {
                        if (d.Kapcsolattarto.Contains(searchQuery))
                        {
                            FilteredGazdalkodoSzervezetek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["emailCB"] == true)
                    {
                        if (d.Email.Contains(searchQuery))
                        {
                            FilteredGazdalkodoSzervezetek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["telefonszamCB"] == true)
                    {
                        if (d.Telefonszam.Contains(searchQuery))
                        {
                            FilteredGazdalkodoSzervezetek.Add(d);
                            continue;
                        }
                    }
                }
            }
            else
            {
                // If the search query is empty, reset to the original data
                FilteredGazdalkodoSzervezetek = new ObservableCollection<GazdalkodoSzervezet>(GazdalkodoSzervezetek);
            }

            // Notify PropertyChanged for the FilteredDolgozok property
            Debug.WriteLine(FilteredGazdalkodoSzervezetek.Count);
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }
        public void DeleteGazdalkodoSzervezet(int id)
        {
            try
            {
                _gazdalkodoSzervezetRepository.DeleteGazdalkodoSzervezet(id);
                for (int i = 0; i < FilteredGazdalkodoSzervezetek.Count; i++)
                {
                    if (FilteredGazdalkodoSzervezetek.ElementAt(i).ID == id)
                        FilteredGazdalkodoSzervezetek.RemoveAt(i);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public ICommand DeleteGazdalkodoSzervezetCommand { get; }
        public ICommand OpenGazdalkodoSzervezetModifyOrAddWindowCommand { get; }

        private bool CanExecuteDeleteGazdalkodoSzervezetCommand(object obj)
        {
            if (SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteGazdalkodoSzervezetCommand(object obj)
        {
            System.Windows.MessageBox.Show(SelectedRow.ID.ToString() + SelectedRow.Nev);
            DeleteGazdalkodoSzervezet(SelectedRow.ID);
        }

        private bool CanExecuteOpenGazdalkodoSzervezetModifyOrAddWindowCommand(object obj)
        {
            if ((string)obj == "Modify")
                return SelectedRow != null;
            return true;
        }

        private void ExecuteOpenGazdalkodoSzervezetModifyOrAddWindowCommand(object obj)
        {
            if (obj is string mode)
            {
                switch (mode)
                {
                    case "Add":
                        GazdalkodoSzervezetModifyOrAddView existingWindow;
                        if (!WindowHelper.IsGazdalkodoSzervezetAddWindowOpen(out existingWindow))
                        {
                            // The window is not open, create and show a new instance
                            GazdalkodoSzervezetModifyOrAddView gazdalkodoSzervezetModifyOrAddView = new GazdalkodoSzervezetModifyOrAddView(EditMode.Add);
                            gazdalkodoSzervezetModifyOrAddView.Show();
                            Mediator.NewGazdalkodoSzervezetAdded += RefreshGazdalkodoSzervezet;
                        }
                        else
                        {
                            // The window is already open, bring it to the foreground
                            if (existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();        // Activate the window
                            Mediator.NewGazdalkodoSzervezetAdded += RefreshGazdalkodoSzervezet;
                        }
                        break;
                    case "Modify":
                        GazdalkodoSzervezetModifyOrAddView gazdalkodoSzervezetModifyOrAddView2 = new GazdalkodoSzervezetModifyOrAddView(EditMode.Modify, SelectedRow);
                        gazdalkodoSzervezetModifyOrAddView2.Show();
                        Mediator.GazdalkodoSzervezetModified += RefreshGazdalkodoSzervezetAfterModify;
                        break;
                }
            }
        }

        private void RefreshGazdalkodoSzervezet(GazdalkodoSzervezet gazdalkodoSzervezet)
        {
            GazdalkodoSzervezetek = _gazdalkodoSzervezetRepository.GetGazdalkodoSzervezetek();
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredGazdalkodoSzervezetek = GazdalkodoSzervezetek, in this case when clearing the FilteredGazdalkodoSzervezetek in later times, it will affect the GazdalkodoSzervezetek collection too
            FilteredGazdalkodoSzervezetek = new ObservableCollection<GazdalkodoSzervezet>(GazdalkodoSzervezetek);
        }

        private void RefreshGazdalkodoSzervezetAfterModify(GazdalkodoSzervezet gazdalkodoSzervezet)
        {
            for (int i = 0; i < GazdalkodoSzervezetek.Count; i++)
            {
                if (GazdalkodoSzervezetek.ElementAt(i).ID == gazdalkodoSzervezet.ID)
                {
                    GazdalkodoSzervezetek.ElementAt(i).Nev = gazdalkodoSzervezet.Nev;
                    GazdalkodoSzervezetek.ElementAt(i).Kapcsolattarto = gazdalkodoSzervezet.Kapcsolattarto;
                    GazdalkodoSzervezetek.ElementAt(i).Email = gazdalkodoSzervezet.Email;
                    GazdalkodoSzervezetek.ElementAt(i).Telefonszam = gazdalkodoSzervezet.Telefonszam;
                    break;
                }
            }
            FilteredGazdalkodoSzervezetek = new ObservableCollection<GazdalkodoSzervezet>(GazdalkodoSzervezetek);
        }
    }
}
