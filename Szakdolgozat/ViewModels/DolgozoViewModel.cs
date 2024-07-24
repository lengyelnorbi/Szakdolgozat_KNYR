using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Szakdolgozat.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Szakdolgozat.Repositories;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Principal;
using System.Threading;
using Szakdolgozat.Views;
using System.Windows.Forms;
using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Szakdolgozat.ViewModels
{
    class DolgozoViewModel : ViewModelBase
    {
        private ObservableCollection<Dolgozo> _dolgozok;
        private DolgozoRepository _dolgozoRepository;

        public ObservableCollection<Dolgozo> Dolgozok
        {
            get { return _dolgozok; }
            set
            {
                _dolgozok = value;
                OnPropertyChanged(nameof(Dolgozok));
            }
        }

        private ObservableCollection<Dolgozo> _filteredDolgozok;

        public ObservableCollection<Dolgozo> FilteredDolgozok
        {
            get { return _filteredDolgozok; }
            set
            {
                _filteredDolgozok = value;
                OnPropertyChanged(nameof(FilteredDolgozok));
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

        private Dolgozo _selectedRow;

        public Dolgozo SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        public bool IsEditPanelVisible => CurrentMode != EditMode.None;

        private EditMode _currentMode;
        public EditMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
                OnPropertyChanged(nameof(CurrentMode));
                OnPropertyChanged(nameof(IsEditPanelVisible));
            }
        }

        public Dictionary<string, bool> checkboxStatuses = new Dictionary<string, bool>();

        private ObservableCollection<Dolgozo> GetYourData()
        {
            ObservableCollection<Dolgozo> data = new ObservableCollection<Dolgozo>();

            // Replace the connection string and query with your actual database details
            string connectionString = "Server=localhost;Database=nyilvantarto_rendszer;User=Norbi;Password=/-j@DoZ*S-_7w@EP";

            //string connectionString = "Server = localhost; Database = nyilvantarto_rendszer; Integrated Security = true; ";
            string query = "SELECT * FROM dolgozok;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string vezeteknev = Convert.ToString(reader["vezeteknev"]);
                            string keresztnev = Convert.ToString(reader["keresztnev"]);
                            string email = Convert.ToString(reader["email"]);
                            string telefonszam = Convert.ToString(reader["telefonszam"]);

                            Dolgozo item = new Dolgozo(id, vezeteknev, keresztnev, email, telefonszam);

                            data.Add(item);
                        }
                    }
                }
            }
            return data;
        }

        public DolgozoViewModel()
        {
            _dolgozoRepository = new DolgozoRepository();
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("vezeteknevCB", true);
            checkboxStatuses.Add("keresztnevCB", true);
            checkboxStatuses.Add("emailCB", true);
            checkboxStatuses.Add("telefonszamCB", true);
            Dolgozok = _dolgozoRepository.GetDolgozok(); 
            //Deep Copy - to ensure that the FilteredDolgozok does not affect the Dolgozok collection, and vica versa
            FilteredDolgozok = new ObservableCollection<Dolgozo>(
                Dolgozok.Select(d => new Dolgozo(d.ID, d.Vezeteknev, d.Keresztnev, d.Email, d.Telefonszam)).ToList()
            );
            DeleteDolgozoCommand = new ViewModelCommand(ExecuteDeleteDolgozoCommand, CanExecuteDeleteDolgozoCommand);

            OpenDolgozoModifyOrAddWindowCommand = new ViewModelCommand(ExecuteOpenDolgozoModifyOrAddWindowCommand, CanExecuteOpenDolgozoModifyOrAddWindowCommand);

            Debug.WriteLine("EREDETI FILTERED");
            Debug.WriteLine(FilteredDolgozok.Count);
        }

        private void FilterData(string searchQuery)
        {
            Debug.WriteLine(searchQuery);
            if (!string.IsNullOrWhiteSpace(searchQuery) && !string.IsNullOrEmpty(searchQuery))
            {
                FilteredDolgozok.Clear();
                foreach(var d in Dolgozok)
                {
                    if (checkboxStatuses["idCB"] == true)
                    {
                        if (d.ID.ToString().Contains(searchQuery))
                        {
                            FilteredDolgozok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["vezeteknevCB"] == true)
                    {
                        if (d.Vezeteknev.Contains(searchQuery))
                        {
                            FilteredDolgozok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["keresztnevCB"] == true)
                    {
                        if (d.Keresztnev.Contains(searchQuery))
                        {
                            FilteredDolgozok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["emailCB"] == true)
                    {
                        if (d.Email.Contains(searchQuery))
                        {
                            FilteredDolgozok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["telefonszamCB"] == true)
                    {
                        if (d.Telefonszam.Contains(searchQuery))
                        {
                            FilteredDolgozok.Add(d);
                            continue;
                        }
                    }
                }
            }
            else
            {
                // If the search query is empty, reset to the original data
                FilteredDolgozok = new ObservableCollection<Dolgozo>(Dolgozok);
            }

            // Notify PropertyChanged for the FilteredDolgozok property
            Debug.WriteLine(FilteredDolgozok.Count);
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }

        public void DeleteDolgozo(int id)
        {
            try
            {
                _dolgozoRepository.DeleteDolgozo(id);
                for (int i = 0; i < FilteredDolgozok.Count; i++)
                {
                    if (FilteredDolgozok.ElementAt(i).ID == id)
                        FilteredDolgozok.RemoveAt(i);
                }

            }
            catch (Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public ICommand DeleteDolgozoCommand { get; }



        public ICommand OpenDolgozoModifyOrAddWindowCommand { get; }


        private bool CanExecuteDeleteDolgozoCommand(object obj)
        {
            if(SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteDolgozoCommand(object obj)
        {
            System.Windows.MessageBox.Show(SelectedRow.ID.ToString() + SelectedRow.Vezeteknev);
            DeleteDolgozo(SelectedRow.ID);
        }

        private bool CanExecuteOpenDolgozoModifyOrAddWindowCommand(object obj)
        {
            if((string)obj == "Modify")
                return SelectedRow != null;
            return true;
        }

        private void ExecuteOpenDolgozoModifyOrAddWindowCommand(object obj)
        {
            if (obj is string mode)
            {
                switch (mode)
                {
                    case "Add":
                        DolgozokModifyOrAddView existingWindow;
                        if (!WindowHelper.IsAddWindowOpen(out existingWindow))
                        {
                            // The window is not open, create and show a new instance
                            DolgozokModifyOrAddView dolgozokModifyOrAddView = new DolgozokModifyOrAddView(EditMode.Add);
                            dolgozokModifyOrAddView.Show();
                            Mediator.NewDolgozoAdded += RefreshDolgozok;
                        }
                        else
                        {
                            // The window is already open, bring it to the foreground
                            if(existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();        // Activate the window
                            Mediator.NewDolgozoAdded += RefreshDolgozok;
                        }
                        break;
                    case "Modify":
                        DolgozokModifyOrAddView dolgozokModifyOrAddView2 = new DolgozokModifyOrAddView(EditMode.Modify, SelectedRow);
                        dolgozokModifyOrAddView2.Show();
                        Mediator.DolgozoModified += RefreshDolgozokAfterModify;
                        break;
                }
            }
        }
        
        private void RefreshDolgozok(Dolgozo dolgozo) 
        {
            Dolgozok = GetYourData();
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredDolgozok = Dolgozok, in this case when clearing the FilteredDolgozok in later times, it will affect the Dolgozok collection too
            FilteredDolgozok = new ObservableCollection<Dolgozo>(Dolgozok);
        }

        private void RefreshDolgozokAfterModify(Dolgozo dolgozo)
        {
            for (int i = 0; i < Dolgozok.Count; i++)
            {
                if (Dolgozok.ElementAt(i).ID == dolgozo.ID)
                {
                    Dolgozok.ElementAt(i).Vezeteknev = dolgozo.Vezeteknev;
                    Dolgozok.ElementAt(i).Keresztnev = dolgozo.Keresztnev;
                    Dolgozok.ElementAt(i).Email = dolgozo.Email;
                    Dolgozok.ElementAt(i).Telefonszam = dolgozo.Telefonszam;
                    break;
                }
            }
            FilteredDolgozok = new ObservableCollection<Dolgozo>(Dolgozok);
        }
    }
}
