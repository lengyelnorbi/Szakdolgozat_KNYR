using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Szakdolgozat.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Szakdolgozat.ViewModels
{
    class GazdalkodoSzervezetViewModel : ViewModelBase
    {
        private ObservableCollection<GazdalkodoSzervezet> _gazdalkodoSzervezetek;

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

        public Dictionary<string, bool> checkboxStatuses = new Dictionary<string, bool>();

        private ObservableCollection<GazdalkodoSzervezet> GetYourData()
        {
            ObservableCollection<GazdalkodoSzervezet> data = new ObservableCollection<GazdalkodoSzervezet>();

            // Replace the connection string and query with your actual database details
            string connectionString = "Server=localhost;Database=nyilvantarto_rendszer;User=Norbi;Password=/-j@DoZ*S-_7w@EP";

            //string connectionString = "Server = localhost; Database = nyilvantarto_rendszer; Integrated Security = true; ";
            string query = "SELECT * FROM gazdalkodo_szervezetek;";

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
                            string nev = Convert.ToString(reader["nev"]);
                            string kapcsolattarto = Convert.ToString(reader["kapcsolattarto"]);
                            string email = Convert.ToString(reader["email"]);
                            string telefonszam = Convert.ToString(reader["telefonszam"]);

                            GazdalkodoSzervezet item = new GazdalkodoSzervezet(id, nev, kapcsolattarto, email, telefonszam);

                            data.Add(item);
                        }
                    }
                }
            }

            return data;
        }

        public GazdalkodoSzervezetViewModel()
        {
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("nevCB", true);
            checkboxStatuses.Add("kapcsolattartoCB", true);
            checkboxStatuses.Add("emailCB", true);
            checkboxStatuses.Add("telefonszamCB", true);
            GazdalkodoSzervezetek = GetYourData();
            FilteredGazdalkodoSzervezetek = new ObservableCollection<GazdalkodoSzervezet>(GazdalkodoSzervezetek);
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
    }
}
