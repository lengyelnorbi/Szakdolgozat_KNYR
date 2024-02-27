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
    public class MaganSzemelyekViewModel : ViewModelBase
    {

        private ObservableCollection<MaganSzemely> _maganSzemelyek;

        public ObservableCollection<MaganSzemely> MaganSzemelyek
        {
            get { return _maganSzemelyek; }
            set
            {
                _maganSzemelyek = value;
                OnPropertyChanged(nameof(MaganSzemelyek));
            }
        }

        private ObservableCollection<MaganSzemely> _filteredMaganSzemelyek;

        public ObservableCollection<MaganSzemely> FilteredMaganSzemelyek
        {
            get { return _filteredMaganSzemelyek; }
            set
            {
                _filteredMaganSzemelyek = value;
                OnPropertyChanged(nameof(FilteredMaganSzemelyek));
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

        private ObservableCollection<MaganSzemely> GetYourData()
        {
            ObservableCollection<MaganSzemely> data = new ObservableCollection<MaganSzemely>();

            // Replace the connection string and query with your actual database details
            string connectionString = "Server=localhost;Database=nyilvantarto_rendszer;User=Norbi;Password=/-j@DoZ*S-_7w@EP";

            //string connectionString = "Server = localhost; Database = nyilvantarto_rendszer; Integrated Security = true; ";
            string query = "SELECT * FROM magan_szemelyek;";

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
                            string telefonszam = Convert.ToString(reader["telefonszam"]);
                            string email = Convert.ToString(reader["email"]);
                            string lakcim = Convert.ToString(reader["lakcim"]);

                            MaganSzemely item = new MaganSzemely(id, nev, telefonszam, email, lakcim);

                            data.Add(item);
                        }
                    }
                }
            }

            return data;
        }

        public MaganSzemelyekViewModel()
        {
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("nevCB", true);
            checkboxStatuses.Add("telefonszamCB", true);
            checkboxStatuses.Add("emailCB", true);
            checkboxStatuses.Add("lakcimCB", true);
            MaganSzemelyek = GetYourData();
            FilteredMaganSzemelyek= new ObservableCollection<MaganSzemely>(MaganSzemelyek);
        }

        private void FilterData(string searchQuery)
        {
            Debug.WriteLine(searchQuery);
            if (!string.IsNullOrWhiteSpace(searchQuery) && !string.IsNullOrEmpty(searchQuery))
            {
                // Filter data based on the search query
                FilteredMaganSzemelyek.Clear();
                foreach (var d in MaganSzemelyek)
                {
                    if (checkboxStatuses["idCB"] == true)
                    {
                        if (d.ID.ToString().Contains(searchQuery))
                        {
                            FilteredMaganSzemelyek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["nevCB"] == true)
                    {
                        if (d.Nev.Contains(searchQuery))
                        {
                            FilteredMaganSzemelyek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["telefonszamCB"] == true)
                    {
                        if (d.Telefonszam.ToString().Contains(searchQuery))
                        {
                            FilteredMaganSzemelyek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["emailCB"] == true)
                    {
                        if (d.Email.Contains(searchQuery))
                        {
                            FilteredMaganSzemelyek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["lakcimCB"] == true)
                    {
                        if (d.Lakcim.Contains(searchQuery))
                        {
                            FilteredMaganSzemelyek.Add(d);
                            continue;
                        }
                    }
                }
            }
            else
            {
                // If the search query is empty, reset to the original data
                FilteredMaganSzemelyek = new ObservableCollection<MaganSzemely>(MaganSzemelyek);
            }

            // Notify PropertyChanged for the FilteredDolgozok property
            Debug.WriteLine(FilteredMaganSzemelyek.Count);
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }
    }
}
