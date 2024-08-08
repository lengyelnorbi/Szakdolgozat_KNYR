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
    public class KoltsegvetesViewModel : ViewModelBase
    {
        private KoltsegvetesRepository _koltsegvetesRepository;
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

        private BevetelKiadas _selectedRow;

        public BevetelKiadas SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        public Dictionary<string, bool> checkboxStatuses = new Dictionary<string, bool>();

        public KoltsegvetesViewModel()
        {
            _koltsegvetesRepository = new KoltsegvetesRepository();
            checkboxStatuses.Add("mindCB", true);
            checkboxStatuses.Add("idCB", true);
            checkboxStatuses.Add("osszegCB", true);
            checkboxStatuses.Add("penznemCB", true);
            checkboxStatuses.Add("beKiKodCB", true);
            checkboxStatuses.Add("teljesitesiDatumCB", true);
            checkboxStatuses.Add("kotelKovetIDCB", true);
            checkboxStatuses.Add("partnerIDCB", true);
            BevetelekKiadasok = _koltsegvetesRepository.GetKoltsegvetesek();
            FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(BevetelekKiadasok);
            FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(
               BevetelekKiadasok.Select(d => new BevetelKiadas(d.ID, d.Osszeg, d.Penznem, d.BeKiKod, d.TeljesitesiDatum, d.KotelKovetID, d.KotelKovetID)).ToList()
           );
            DeleteBevetelKiadasCommand = new ViewModelCommand(ExecuteDeleteBevetelKiadasCommand, CanExecuteDeleteBevetelKiadasCommand);

            OpenBevetelKiadasModifyOrAddWindowCommand = new ViewModelCommand(ExecuteOpenBevetelKiadasModifyOrAddWindowCommand, CanExecuteOpenBevetelKiadasModifyOrAddWindowCommand);

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
                        if (d.BeKiKod.ToString().Contains(searchQuery))
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

        public void DeleteBevetelKiadas(int id)
        {
            try
            {
                _koltsegvetesRepository.DeleteKoltsegvetes(id);
                for (int i = 0; i < FilteredBevetelekKiadasok.Count; i++)
                {
                    if (FilteredBevetelekKiadasok.ElementAt(i).ID == id)
                        FilteredBevetelekKiadasok.RemoveAt(i);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ICommand DeleteBevetelKiadasCommand { get; }



        public ICommand OpenBevetelKiadasModifyOrAddWindowCommand { get; }


        private bool CanExecuteDeleteBevetelKiadasCommand(object obj)
        {
            if (SelectedRow != null)
                return true;
            return false;
        }
        private void ExecuteDeleteBevetelKiadasCommand(object obj)
        {
            System.Windows.MessageBox.Show(SelectedRow.ID.ToString() + SelectedRow.ID.ToString());
            DeleteBevetelKiadas(SelectedRow.ID);
        }

        private bool CanExecuteOpenBevetelKiadasModifyOrAddWindowCommand(object obj)
        {
            if ((string)obj == "Modify")
                return SelectedRow != null;
            return true;
        }

        private void ExecuteOpenBevetelKiadasModifyOrAddWindowCommand(object obj)
        {
            if (obj is string mode)
            {
                switch (mode)
                {
                    case "Add":
                        KoltsegvetesModifyOrAddView existingWindow;
                        if (!WindowHelper.IsKoltsegvetesAddWindowOpen(out existingWindow))
                        {
                            // The window is not open, create and show a new instance
                            KoltsegvetesModifyOrAddView koltsegvetesModifyOrAddView = new KoltsegvetesModifyOrAddView(EditMode.Add);
                            koltsegvetesModifyOrAddView.Show();
                            Mediator.NewBevetelKiadasAdded += RefreshBevetelKiadas;
                        }
                        else
                        {
                            // The window is already open, bring it to the foreground
                            if (existingWindow.WindowState == WindowState.Minimized)
                            {
                                existingWindow.WindowState = WindowState.Normal;
                            }
                            existingWindow.Activate();        // Activate the window
                            Mediator.NewBevetelKiadasAdded += RefreshBevetelKiadas;
                        }
                        break;
                    case "Modify":
                        KoltsegvetesModifyOrAddView koltsegvetesModifyOrAddView2 = new KoltsegvetesModifyOrAddView(EditMode.Modify, SelectedRow);
                        koltsegvetesModifyOrAddView2.Show();
                        Mediator.BevetelKiadasModified += RefreshBevetelKiadasAfterModify;
                        break;
                }
            }
        }

        private void RefreshBevetelKiadas(BevetelKiadas bevetelKiadas)
        {
            BevetelekKiadasok = _koltsegvetesRepository.GetKoltsegvetesek();
            //bad example for not making deep copy, also good example for making collection references:
            //FilteredBevetelekKiadasok = BevetelekKiadasok, in this case when clearing the FilteredBevetelekKiadasok in later times, it will affect the BevetelekKiadasok collection too
            FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(BevetelekKiadasok);
        }

        private void RefreshBevetelKiadasAfterModify(BevetelKiadas bevetelKiadas)
        {
            for (int i = 0; i < BevetelekKiadasok.Count; i++)
            {
                if (BevetelekKiadasok.ElementAt(i).ID == bevetelKiadas.ID)
                {
                    BevetelekKiadasok.ElementAt(i).Osszeg = bevetelKiadas.Osszeg;
                    BevetelekKiadasok.ElementAt(i).Penznem = bevetelKiadas.Penznem;
                    BevetelekKiadasok.ElementAt(i).BeKiKod = bevetelKiadas.BeKiKod;
                    BevetelekKiadasok.ElementAt(i).TeljesitesiDatum = bevetelKiadas.TeljesitesiDatum;
                    BevetelekKiadasok.ElementAt(i).KotelKovetID = bevetelKiadas.KotelKovetID;
                    BevetelekKiadasok.ElementAt(i).PartnerID = bevetelKiadas.PartnerID;
                    break;
                }
            }
            FilteredBevetelekKiadasok = new ObservableCollection<BevetelKiadas>(BevetelekKiadasok);
        }
    }
}
