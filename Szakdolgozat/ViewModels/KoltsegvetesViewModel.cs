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
    public class KoltsegvetesViewModel : ViewModelBase
    {

        private ObservableCollection<BevetelKiadas> _bevetelekKiadasok;

        public ObservableCollection<BevetelKiadas> BevetelekKiadasok
        {
            get { return _bevetelekKiadasok; }
            set
            {
                _bevetelekKiadasok = value;
                OnPropertyChanged(nameof(BevetelekKiadasok));
            }
        }


        private ObservableCollection<BevetelKiadas> _filteredBevetelKiadasok;

        public ObservableCollection<BevetelKiadas> FilteredBevetelekKiadasok
        {
            get { return _filteredBevetelKiadasok; }
            set
            {
                _filteredBevetelKiadasok = value;
                OnPropertyChanged(nameof(FilteredBevetelekKiadasok));
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

        private ObservableCollection<BevetelKiadas> GetYourData()
        {
            ObservableCollection<BevetelKiadas> data = new ObservableCollection<BevetelKiadas>();

            // Replace the connection string and query with your actual database details
            string connectionString = "Server=localhost;Database=nyilvantarto_rendszer;User=Norbi;Password=/-j@DoZ*S-_7w@EP";

            //string connectionString = "Server = localhost; Database = nyilvantarto_rendszer; Integrated Security = true; ";
            string query = "SELECT * FROM bevetelek_kiadasok;";

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
                            int osszeg = Convert.ToInt32(reader["osszeg"]);
                            Penznem penznem = (Penznem)Enum.Parse(typeof(Penznem), reader["penznem"].ToString());
                            string beKiKod = Convert.ToString(reader["be_ki_kod"]);
                            DateTime teljesitesiDatum = Convert.ToDateTime(reader["teljesitesi_datum"]);
                            int kotelKovetID;
                            if (reader["kotel_kovet_id"] != DBNull.Value)
                            {
                                kotelKovetID = Convert.ToInt32(reader["kotel_kovet_id"]);
                            }
                            else
                            {
                                kotelKovetID = 0;
                            }

                            int partnerID;
                            if (reader["partner_id"] != DBNull.Value)
                            {
                                partnerID = Convert.ToInt32(reader["partner_id"]);
                            }
                            else
                            {
                                partnerID = 0;
                            }

                            BevetelKiadas item = new BevetelKiadas(id, osszeg, penznem, beKiKod, teljesitesiDatum, kotelKovetID, partnerID);

                            data.Add(item);
                        }
                    }
                }
            }

            return data;
        }

        public KoltsegvetesViewModel()
        {
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("osszegCB", true);
            checkboxStatuses.Add("penznemCB", true);
            checkboxStatuses.Add("beKiKodCB", true);
            checkboxStatuses.Add("teljesitesiDatumCB", true);
            checkboxStatuses.Add("kotelKovetIDCB", true);
            checkboxStatuses.Add("partnerIDCB", true);
            BevetelekKiadasok = GetYourData();
            FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(BevetelekKiadasok);
        }

        private void FilterData(string searchQuery)
        {
            Debug.WriteLine(searchQuery);
            if (!string.IsNullOrWhiteSpace(searchQuery) && !string.IsNullOrEmpty(searchQuery))
            {
                // Filter data based on the search query
                FilteredBevetelekKiadasok.Clear();
                foreach (var d in BevetelekKiadasok)
                {
                    if (checkboxStatuses["idCB"] == true)
                    {
                        if (d.ID.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["osszegCB"] == true)
                    {
                        if (d.Osszeg.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["penznemCB"] == true)
                    {
                        if (d.Penznem.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["beKiKodCB"] == true)
                    {
                        if (d.BeKiKod.Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["teljesitesiDatumCB"] == true)
                    {
                        if (d.TeljesitesiDatum.ToShortDateString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["kotelKovetIDCB"] == true)
                    {
                        if (d.KotelKovetID.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                    if (checkboxStatuses["partnerIDCB"] == true)
                    {
                        if (d.PartnerID.ToString().Contains(searchQuery))
                        {
                            FilteredBevetelekKiadasok.Add(d);
                            continue;
                        }
                    }
                }
            }
            else
            {
                // If the search query is empty, reset to the original data
                FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(BevetelekKiadasok);
            }

            // Notify PropertyChanged for the FilteredDolgozok property
            Debug.WriteLine(FilteredBevetelekKiadasok.Count);
        }


        // Call this method when the search query changes
        public void UpdateSearch(string searchQuery)
        {
            FilterData(searchQuery);
        }
    }
}
