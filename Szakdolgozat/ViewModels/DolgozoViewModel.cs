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
            FilteredDolgozok = new ObservableCollection<Dolgozo>(Dolgozok);
            DeleteDolgozoCommand = new ViewModelCommand(ExecuteDeleteDolgozoCommand, CanExecuteDeleteDolgozoCommand);
            ModifyDolgozoCommand = new ViewModelCommand(ExecuteModifyDolgozoCommand, CanExecuteModifyDolgozoCommand);
            AddDolgozoCommand = new ViewModelCommand(ExecuteAddDolgozoCommand, CanExecuteAddDolgozoCommand);
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
        public ICommand ModifyDolgozoCommand { get; }
        public ICommand AddDolgozoCommand { get; }

        private bool CanExecuteDeleteDolgozoCommand(object obj)
        {
            if(SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteDolgozoCommand(object obj)
        {
            MessageBox.Show(SelectedRow.ID.ToString() + SelectedRow.Vezeteknev);
            DeleteDolgozo(SelectedRow.ID);
        }
        private bool CanExecuteModifyDolgozoCommand(object obj)
        {
            return true;
        }
        private void ExecuteModifyDolgozoCommand(object obj)
        {
        }
        private bool CanExecuteAddDolgozoCommand(object obj)
        {
            return true;
        }
        private void ExecuteAddDolgozoCommand(object obj)
        {
        }
    }
}
