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
        private int _amount;
        private Penznem _currency;
        //Expenditure - kiadás, Income - bevétel
        private BeKiKod _incExpID;
        private DateTime _completionDate = DateTime.Now;
        //Obligation - kötelezettség, Claim - követelés
        private int? _oblClaimID;
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
                Amount = value.Osszeg;
                Currency = value.Penznem;
                IncExpID = value.BeKiKod;
                CompletionDate = value.TeljesitesiDatum;
                OblClaimID = value.KotelKovetID;
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
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }
        public Penznem Currency
        {
            get
            {
                return _currency;
            }
            set
            {
                _currency = value;
                OnPropertyChanged(nameof(Currency));
            }
        }
        public BeKiKod IncExpID
        {
            get
            {
                return _incExpID;
            }
            set
            {
                _incExpID = value;
                OnPropertyChanged(nameof(IncExpID));
            }
        }
        public DateTime CompletionDate
        {
            get
            {
                return _completionDate;
            }
            set
            {
                _completionDate = value;
                OnPropertyChanged(nameof(CompletionDate));
            }
        }
        public int? OblClaimID
        {
            get
            {
                return _oblClaimID;
            }
            set
            {
                _oblClaimID = value;
                OnPropertyChanged(nameof(OblClaimID));
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
                if (Amount != null && IncExpID != null && Currency != null && CompletionDate != null)
                {
                    BevetelKiadas bevetelKiadas = new BevetelKiadas(Amount, Currency, IncExpID, CompletionDate, OblClaimID, PartnerID);
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
                if (Amount != null && IncExpID != null && Currency != null && CompletionDate != null)
                {
                    BevetelKiadas bevetelKiadas = new BevetelKiadas(ModifiableKoltsegvetes.ID, Amount, Currency, IncExpID, CompletionDate, OblClaimID, PartnerID);
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
