using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;

namespace Szakdolgozat.ViewModels
{
    public class KoltsegvetesModifyOrAddViewModel : ViewModelBase
    {
        public event Action RequestClose;
        private int _osszeg;
        private Penznem _penznem;
        private string _beKiKod;
        private DateTime _teljesitesiDatum = DateTime.Now;
        private int? _kotelKovetID;
        private int? _partnerID;
        private string _title;
        private EditMode _editMode;
        private BevetelKiadas _modifiableKoltsegvetes;
        private KoltsegvetesRepository _koltsegvetesRepository= new KoltsegvetesRepository();
        public BevetelKiadas ModifiableKoltsegvetes
        {
            get
            {
                return _modifiableKoltsegvetes;
            }
            set
            {
                _modifiableKoltsegvetes = value;
                Osszeg = value.Osszeg;
                Penznem = value.Penznem;
                BeKiKod = value.BeKiKod;
                TeljesitesiDatum = value.TeljesitesiDatum;
                KotelKovetID = value.KotelKovetID;
                PartnerID = value.PartnerID;
            }
        }
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public EditMode EditMode
        {
            get
            {
                return _editMode;
            }
            set
            {
                _editMode = value;
                if (_editMode == EditMode.Add)
                    Title = "Új Bevétel/Kiadás Létrehozása";
                else if (_editMode == EditMode.Modify)
                    Title = "Bevétel/Kiadás Módosítása";
            }
        }
        public int Osszeg
        {
            get
            {
                return _osszeg;
            }
            set
            {
                _osszeg = value;
                OnPropertyChanged(nameof(Osszeg));
            }
        }
        public Penznem Penznem
        {
            get
            {
                return _penznem;
            }
            set
            {
                _penznem = value;
                OnPropertyChanged(nameof(Penznem));
            }
        }
        public string BeKiKod
        {
            get
            {
                return _beKiKod;
            }
            set
            {
                _beKiKod = value;
                OnPropertyChanged(nameof(BeKiKod));
            }
        }
        public DateTime TeljesitesiDatum
        {
            get
            {
                return _teljesitesiDatum;
            }
            set
            {
                _teljesitesiDatum = value;
                OnPropertyChanged(nameof(TeljesitesiDatum));
            }
        }
        public int? KotelKovetID
        {
            get
            {
                return _kotelKovetID;
            }
            set
            {
                _kotelKovetID = value;
                OnPropertyChanged(nameof(KotelKovetID));
            }
        }
        public int? PartnerID
        {
            get
            {
                return _partnerID;
            }
            set
            {
                _partnerID = value;
                OnPropertyChanged(nameof(PartnerID));
            }
        }

        public ICommand SaveCommand { get; }

        public KoltsegvetesModifyOrAddViewModel()
        {
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
        }

        public BevetelKiadas NotificationAboutAddedBevetelKiadas()
        {
            return null;
        }

        private void ExecuteSaveCommand(object parameter)
        {
            switch (this.EditMode)
            {
                case EditMode.Add:
                    AddBevetelKiadas();
                    break;
                case EditMode.Modify:
                    ModifyBevetelKiadas();
                    break;
            }
        }

        private void AddBevetelKiadas()
        {
            try
            {
                if (Osszeg != null && BeKiKod != null && Penznem != null && TeljesitesiDatum != null)
                {
                    MessageBox.Show(TeljesitesiDatum.ToString());
                    BevetelKiadas bevetelKiadas = new BevetelKiadas(Osszeg, Penznem, BeKiKod, TeljesitesiDatum, KotelKovetID, PartnerID);
                    _koltsegvetesRepository.AddKoltsegvetes(bevetelKiadas);
                    Mediator.NotifyNewBevetelKiadasAdded(bevetelKiadas);
                    CloseWindow();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        private void ModifyBevetelKiadas()
        {
            try
            {
                if (Osszeg != null && BeKiKod != null && Penznem != null && TeljesitesiDatum != null)
                {
                    BevetelKiadas bevetelKiadas = new BevetelKiadas(ModifiableKoltsegvetes.ID, Osszeg, Penznem, BeKiKod, TeljesitesiDatum, KotelKovetID, PartnerID);
                    _koltsegvetesRepository.ModifyKoltsegvetes(bevetelKiadas);
                    Mediator.NotifyModifiedBevetelKiadas(bevetelKiadas);
                    CloseWindow();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        private void CloseWindow()
        {
            RequestClose?.Invoke();
        }
    }
}
