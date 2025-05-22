using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;
using Szakdolgozat.Specials;

namespace Szakdolgozat.ViewModels
{
    public class KoltsegvetesModifyOrAddViewModel : ViewModelBase, IDataErrorInfo
    {
        public event Action RequestClose;
        private int _amount;
        private Penznem _currency;
        //Expenditure - kiadás, Income - bevétel
        private BeKiKod _incExpID;
        private DateTime _completionDate = DateTime.Now;
        //Obligation - kötelezettség, Claim - követelés
        private int? _oblClaimID;
        private int? _companyID;
        private int? _privatePersonID;
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
                CompanyID = value.GazdalkodasiSzervID;
                PrivatePersonID = value.MaganszemelyID;
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
        public int? CompanyID
        {
            get
            {
                return _companyID;
            }
            set
            {
                _companyID = value;
                OnPropertyChanged(nameof(CompanyID));
            }
        }
        public int? PrivatePersonID
        {
            get
            {
                return _privatePersonID;
            }
            set
            {
                _privatePersonID = value;
                OnPropertyChanged(nameof(PrivatePersonID));
            }
        }
        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(CompanyID):
                        if ((string.IsNullOrWhiteSpace(CompanyID.ToString()) && CompanyID == 0) && (string.IsNullOrWhiteSpace(PrivatePersonID.ToString()) && PrivatePersonID == 0))
                            error = "Szükséges megadni partner id-t.";
                        break;
                    case nameof(PrivatePersonID):
                        if ((string.IsNullOrWhiteSpace(CompanyID.ToString()) && CompanyID == 0) && (string.IsNullOrWhiteSpace(PrivatePersonID.ToString()) && PrivatePersonID == 0))
                            error = "Szükséges megadni partner id-t.";
                        break;
                    case nameof(Amount):
                        if (string.IsNullOrWhiteSpace(Amount.ToString()) && Amount == 0)
                            error = "Telefonszám nem lehet üres és 0.";
                        break;
                }
                return error;
            }
        }

        public string Error => null;

        public ICommand SaveCommand { get; }
        public ICommand ResetEditCommand { get; }

        public KoltsegvetesModifyOrAddViewModel()
        {
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
            ResetEditCommand = new ViewModelCommand(ExecuteResetEditCommand);
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
        private void ExecuteResetEditCommand(object parameter)
        {
            if (this.EditMode == EditMode.Modify)
            {
                Amount = ModifiableKoltsegvetes.Osszeg;
                Currency = ModifiableKoltsegvetes.Penznem;
                IncExpID = ModifiableKoltsegvetes.BeKiKod;
                CompletionDate = ModifiableKoltsegvetes.TeljesitesiDatum;
                OblClaimID = ModifiableKoltsegvetes.KotelKovetID;
                PrivatePersonID = ModifiableKoltsegvetes.MaganszemelyID;
                CompanyID = ModifiableKoltsegvetes.GazdalkodasiSzervID;
            }
        }
        private void AddBevetelKiadas()
        {
            try
            {
                if (HasValidationErrors())
                {
                    System.Windows.MessageBox.Show("Kérjük, javítsa ki a hibákat a mentés előtt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Amount != null && IncExpID != null && Currency != null && CompletionDate != null)
                {
                    BevetelKiadas bevetelKiadas = new BevetelKiadas(Amount, Currency, IncExpID, CompletionDate, OblClaimID, CompanyID, PrivatePersonID);
                    bool isSuccess = _koltsegvetesRepository.AddKoltsegvetes(bevetelKiadas);
                    if (isSuccess)
                    {
                        Mediator.NotifyNewBevetelKiadasAdded(bevetelKiadas);
                        System.Windows.MessageBox.Show("Költségvetés hozzáadása sikeres.", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                        CloseWindow();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Hiba a Költségvetés hozzáadása során.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
                if (HasValidationErrors())
                {
                    System.Windows.MessageBox.Show("Kérjük, javítsa ki a hibákat a mentés előtt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Amount != null && IncExpID != null && Currency != null && CompletionDate != null)
                {
                    BevetelKiadas bevetelKiadas = new BevetelKiadas(ModifiableKoltsegvetes.ID, Amount, Currency, IncExpID, CompletionDate, OblClaimID, CompanyID, PrivatePersonID);
                    bool isSuccess = _koltsegvetesRepository.ModifyKoltsegvetes(bevetelKiadas);
                    if (isSuccess)
                    {
                        Mediator.NotifyModifiedBevetelKiadas(bevetelKiadas);
                        System.Windows.MessageBox.Show("Költségvetés módosítása sikeres.", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                        CloseWindow();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Hiba a Költségvetés módosítása során.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        private bool HasValidationErrors()
        {
            return !string.IsNullOrEmpty(this[nameof(Amount)]) ||
                   !string.IsNullOrEmpty(this[nameof(CompanyID)]) ||
                   !string.IsNullOrEmpty(this[nameof(PrivatePersonID)]);
        }
        private void CloseWindow()
        {
            RequestClose?.Invoke();
        }
    }
}
