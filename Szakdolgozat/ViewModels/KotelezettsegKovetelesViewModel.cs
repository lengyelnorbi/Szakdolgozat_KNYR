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
    class KotelezettsegKovetelesViewModel : ViewModelBase
    {
        private ObservableCollection<KotelezettsegKoveteles> _kotelezettsegekKovetelesek;

        public ObservableCollection<KotelezettsegKoveteles> KotelezettsegekKovetelesek
        {
            get { return _kotelezettsegekKovetelesek; }
            set
            {
                _kotelezettsegekKovetelesek = value;
                OnPropertyChanged(nameof(KotelezettsegekKovetelesek));
            }
        }

        private ObservableCollection<KotelezettsegKoveteles> _filteredKotelezettsegekKovetelesek;

        public ObservableCollection<KotelezettsegKoveteles> FilteredKotelezettsegekKovetelesek
        {
            get { return _filteredKotelezettsegekKovetelesek; }
            set
            {
                _filteredKotelezettsegekKovetelesek = value;
                OnPropertyChanged(nameof(FilteredKotelezettsegekKovetelesek));
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

        private ObservableCollection<KotelezettsegKoveteles> GetYourData()
        {
            ObservableCollection<KotelezettsegKoveteles> data = new ObservableCollection<KotelezettsegKoveteles>();

            // Replace the connection string and query with your actual database details
            string connectionString = "Server=localhost;Database=nyilvantarto_rendszer;User=Norbi;Password=/-j@DoZ*S-_7w@EP";

            //string connectionString = "Server = localhost; Database = nyilvantarto_rendszer; Integrated Security = true; ";
            string query = "SELECT * FROM kotelezettsegek_kovetelesek;";

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
                            string tipus = Convert.ToString(reader["tipus"]);
                            int osszeg = Convert.ToInt32(reader["osszeg"]);
                            Penznem penznem = (Penznem)Enum.Parse(typeof(Penznem), reader["penznem"].ToString());
                            DateTime kifizetesHatarideje = Convert.ToDateTime(reader["kifizetes_hatarideje"]);
                            Boolean kifizetett = Convert.ToBoolean(reader["kifizetett"]);

                            KotelezettsegKoveteles item = new KotelezettsegKoveteles(id, tipus, osszeg, penznem, kifizetesHatarideje, kifizetett);

                            data.Add(item);
                        }
                    }
                }
            }

            return data;
        }

        public KotelezettsegKovetelesViewModel()
        {
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("tipusCB", true);
            checkboxStatuses.Add("osszegCB", true);
            checkboxStatuses.Add("penznemCB", true);
            checkboxStatuses.Add("kifizetesHataridejeCB", true);
            checkboxStatuses.Add("kifizetettCB", true);
            KotelezettsegekKovetelesek = GetYourData();
            FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(KotelezettsegekKovetelesek);
        }

        private void FilterData(string searchQuery)
        {
            Debug.WriteLine(searchQuery);
            if (!string.IsNullOrWhiteSpace(searchQuery) && !string.IsNullOrEmpty(searchQuery))
            {
                // Filter data based on the search query
                FilteredKotelezettsegekKovetelesek.Clear();
                foreach (var d in KotelezettsegekKovetelesek)
                {
                    if (checkboxStatuses["idCB"] == true)
                    {
                        if (d.ID.ToString().Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["tipusCB"] == true)
                    {
                        if (d.Tipus.Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["osszegCB"] == true)
                    {
                        if (d.Osszeg.ToString().Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["penznemCB"] == true)
                    {
                        if (d.Penznem.ToString().Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["kifizetesHataridejeCB"] == true)
                    {
                        if (d.KifizetesHatarideje.ToShortDateString().Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["kifizetettCB"] == true)
                    {
                        if (d.Kifizetett.ToString().Contains(searchQuery))
                        {
                            FilteredKotelezettsegekKovetelesek.Add(d);
                            continue;
                        }
                    }
                }
            }
            else
            {
                // If the search query is empty, reset to the original data
                FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(KotelezettsegekKovetelesek);
            }

            // Notify PropertyChanged for the FilteredDolgozok property
            Debug.WriteLine(FilteredKotelezettsegekKovetelesek.Count);
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }
    }
}
