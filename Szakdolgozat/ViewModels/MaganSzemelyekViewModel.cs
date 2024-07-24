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
using Szakdolgozat.Repositories;
using Szakdolgozat.Views;

namespace Szakdolgozat.ViewModels
{
    public class MaganSzemelyekViewModel : ViewModelBase
    {

        private ObservableCollection<MaganSzemely> _maganSzemelyek;
        private MaganSzemelyRepository _maganSzemelyRepository;

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
        private MaganSzemely _selectedRow;

        public MaganSzemely SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
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
            _maganSzemelyRepository = new MaganSzemelyRepository();
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("nevCB", true);
            checkboxStatuses.Add("telefonszamCB", true);
            checkboxStatuses.Add("emailCB", true);
            checkboxStatuses.Add("lakcimCB", true);
            MaganSzemelyek = GetYourData();
            //Deep Copy - to ensure that the FilteredDolgozok does not affect the Dolgozok collection, and vica versa
            FilteredMaganSzemelyek = new ObservableCollection<MaganSzemely>(
                MaganSzemelyek.Select(d => new MaganSzemely(d.ID, d.Nev, d.Lakcim, d.Email, d.Telefonszam)).ToList()
            );

            DeleteMaganSzemelyCommand = new ViewModelCommand(ExecuteDeleteMaganSzemelyCommand, CanExecuteDeleteMaganSzemelyCommand);

            OpenMaganSzemelyModifyOrAddWindowCommand = new ViewModelCommand(ExecuteOpenMaganSzemelyModifyOrAddWindowCommand, CanExecuteOpenMaganSzemelyModifyOrAddWindowCommand);
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

        public void DeleteMaganSzemely(int id)
        {
            try
            {
                _maganSzemelyRepository.DeleteMaganSzemely(id);
                for (int i = 0; i < FilteredMaganSzemelyek.Count; i++)
                {
                    if (FilteredMaganSzemelyek.ElementAt(i).ID == id)
                        FilteredMaganSzemelyek.RemoveAt(i);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ICommand DeleteMaganSzemelyCommand { get; }



        public ICommand OpenMaganSzemelyModifyOrAddWindowCommand { get; }


        private bool CanExecuteDeleteMaganSzemelyCommand(object obj)
        {
            if (SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteMaganSzemelyCommand(object obj)
        {
            System.Windows.MessageBox.Show(SelectedRow.ID.ToString() + SelectedRow.Nev);
            DeleteMaganSzemely(SelectedRow.ID);
        }

        private bool CanExecuteOpenMaganSzemelyModifyOrAddWindowCommand(object obj)
        {
            if ((string)obj == "Modify")
                return SelectedRow != null;
            return true;
        }

        private void ExecuteOpenMaganSzemelyModifyOrAddWindowCommand(object obj)
        {
            if (obj is string mode)
            {
                switch (mode)
                {
                    case "Add":
                        MaganSzemelyekModifyOrAddView existingWindow;
                        if (!WindowHelper.IsMaganSzemelyAddWindowOpen(out existingWindow))
                        {
                            // The window is not open, create and show a new instance
                            MaganSzemelyekModifyOrAddView maganSzemelyekModifyOrAddView = new MaganSzemelyekModifyOrAddView(EditMode.Add);
                            maganSzemelyekModifyOrAddView.Show();
                            Mediator.NewMaganSzemelyAdded += RefreshMaganSzemely;
                        }
                        else
                        {
                            // The window is already open, bring it to the foreground
                            if (existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();        // Activate the window
                            Mediator.NewMaganSzemelyAdded += RefreshMaganSzemely;
                        }
                        break;
                    case "Modify":
                        MaganSzemelyekModifyOrAddView maganSzemelyekModifyOrAddView2 = new MaganSzemelyekModifyOrAddView(EditMode.Modify, SelectedRow);
                        maganSzemelyekModifyOrAddView2.Show();
                        Mediator.MaganSzemelyModified += RefreshMaganSzemelyAfterModify;
                        break;
                }
            }
        }

        private void RefreshMaganSzemely(MaganSzemely maganSzemely)
        {
            MaganSzemelyek = GetYourData();
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredDolgozok = Dolgozok, in this case when clearing the FilteredDolgozok in later times, it will affect the Dolgozok collection too
            FilteredMaganSzemelyek = new ObservableCollection<MaganSzemely>(MaganSzemelyek);
        }

        private void RefreshMaganSzemelyAfterModify(MaganSzemely maganSzemely)
        {
            for (int i = 0; i < MaganSzemelyek.Count; i++)
            {
                if (MaganSzemelyek.ElementAt(i).ID == maganSzemely.ID)
                {
                    MaganSzemelyek.ElementAt(i).Nev = maganSzemely.Nev;
                    MaganSzemelyek.ElementAt(i).Lakcim = maganSzemely.Lakcim;
                    MaganSzemelyek.ElementAt(i).Email = maganSzemely.Email;
                    MaganSzemelyek.ElementAt(i).Telefonszam = maganSzemely.Telefonszam;
                    break;
                }
            }
            FilteredMaganSzemelyek = new ObservableCollection<MaganSzemely>(MaganSzemelyek);
        }
    }
}
