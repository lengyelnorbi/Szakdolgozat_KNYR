using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Szakdolgozat.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Szakdolgozat.Repositories;
using System.Windows.Input;
using System.Windows;
using Szakdolgozat.Views;

namespace Szakdolgozat.ViewModels
{
    class KotelezettsegKovetelesViewModel : ViewModelBase
    {
        private KotelezettsegKovetelesRepository _kotelezettsegKovetelesRepository;
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

        private KotelezettsegKoveteles _selectedRow;

        public KotelezettsegKoveteles SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
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
                            Int16 kifizetett = Convert.ToInt16(reader["kifizetett"]);

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
            _kotelezettsegKovetelesRepository = new KotelezettsegKovetelesRepository();
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("tipusCB", true);
            checkboxStatuses.Add("osszegCB", true);
            checkboxStatuses.Add("penznemCB", true);
            checkboxStatuses.Add("kifizetesHataridejeCB", true);
            checkboxStatuses.Add("kifizetettCB", true);
            KotelezettsegekKovetelesek = GetYourData();
            FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(KotelezettsegekKovetelesek);

            FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(
               KotelezettsegekKovetelesek.Select(d => new KotelezettsegKoveteles(d.ID, d.Tipus, d.Osszeg, d.Penznem, d.KifizetesHatarideje, d.Kifizetett)).ToList()
            );

            DeleteKotelezettsegKovetelesCommand = new ViewModelCommand(ExecuteDeleteKotelezettsegKovetelesCommand, CanExecuteDeleteKotelezettsegKovetelesCommand);

            OpenKotelezettsegKovetelesModifyOrAddWindowCommand = new ViewModelCommand(ExecuteOpenKotelezettsegKovetelesModifyOrAddWindowCommand, CanExecuteOpenKotelezettsegKovetelesModifyOrAddWindowCommand);
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

        public void DeleteKotelezettsegKoveteles(int id)
        {
            try
            {
                _kotelezettsegKovetelesRepository.DeleteKotelezettsegKoveteles(id);
                for (int i = 0; i < FilteredKotelezettsegekKovetelesek.Count; i++)
                {
                    if (FilteredKotelezettsegekKovetelesek.ElementAt(i).ID == id)
                        FilteredKotelezettsegekKovetelesek.RemoveAt(i);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ICommand DeleteKotelezettsegKovetelesCommand { get; }



        public ICommand OpenKotelezettsegKovetelesModifyOrAddWindowCommand { get; }


        private bool CanExecuteDeleteKotelezettsegKovetelesCommand(object obj)
        {
            if (SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteKotelezettsegKovetelesCommand(object obj)
        {
            System.Windows.MessageBox.Show(SelectedRow.ID.ToString() + SelectedRow.ID.ToString());
            DeleteKotelezettsegKoveteles(SelectedRow.ID);
        }

        private bool CanExecuteOpenKotelezettsegKovetelesModifyOrAddWindowCommand(object obj)
        {
            if ((string)obj == "Modify")
                return SelectedRow != null;
            return true;
        }

        private void ExecuteOpenKotelezettsegKovetelesModifyOrAddWindowCommand(object obj)
        {
            if (obj is string mode)
            {
                switch (mode)
                {
                    case "Add":
                        KotelezettsegKovetelesModifyOrAddView existingWindow;
                        if (!WindowHelper.IsKoltsegvetesAddWindowOpen(out existingWindow))
                        {
                            // The window is not open, create and show a new instance
                            KotelezettsegKovetelesModifyOrAddView kotelezettsegKovetelesModifyOrAddView = new KotelezettsegKovetelesModifyOrAddView(EditMode.Add);
                            kotelezettsegKovetelesModifyOrAddView.Show();
                            Mediator.NewKotelezettsegKovetelesAdded += RefreshKotelezettsegKoveteles;
                        }
                        else
                        {
                            // The window is already open, bring it to the foreground
                            if (existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();        // Activate the window
                            Mediator.NewKotelezettsegKovetelesAdded += RefreshKotelezettsegKoveteles;
                        }
                        break;
                    case "Modify":
                        KotelezettsegKovetelesModifyOrAddView kotelezettsegKovetelesModifyOrAddView2 = new KotelezettsegKovetelesModifyOrAddView(EditMode.Modify, SelectedRow);
                        kotelezettsegKovetelesModifyOrAddView2.Show();
                        Mediator.KotelezettsegKovetelesModified += RefreshKotelezettsegKovetelesAfterModify;
                        break;
                }
            }
        }

        private void RefreshKotelezettsegKoveteles(KotelezettsegKoveteles kotelezettsegKoveteles)
        {
            KotelezettsegekKovetelesek = GetYourData();
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredKotelezettsegekKovetelesek = KotelezettsegekKovetelesek, in this case when clearing the FilteredKotelezettsegekKovetelesek in later times, it will affect the KotelezettsegekKovetelesek collection too
            FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(KotelezettsegekKovetelesek);
        }

        private void RefreshKotelezettsegKovetelesAfterModify(KotelezettsegKoveteles kotelezettsegKoveteles)
        {
            for (int i = 0; i < KotelezettsegekKovetelesek.Count; i++)
            {
                if (KotelezettsegekKovetelesek.ElementAt(i).ID == kotelezettsegKoveteles.ID)
                {
                    KotelezettsegekKovetelesek.ElementAt(i).Tipus = kotelezettsegKoveteles.Tipus;
                    KotelezettsegekKovetelesek.ElementAt(i).Osszeg = kotelezettsegKoveteles.Osszeg;
                    KotelezettsegekKovetelesek.ElementAt(i).Penznem = kotelezettsegKoveteles.Penznem;
                    KotelezettsegekKovetelesek.ElementAt(i).KifizetesHatarideje = kotelezettsegKoveteles.KifizetesHatarideje;
                    KotelezettsegekKovetelesek.ElementAt(i).Kifizetett = kotelezettsegKoveteles.Kifizetett;
                    break;
                }
            }
            FilteredKotelezettsegekKovetelesek = new ObservableCollection<KotelezettsegKoveteles>(KotelezettsegekKovetelesek);
        }
    }
}