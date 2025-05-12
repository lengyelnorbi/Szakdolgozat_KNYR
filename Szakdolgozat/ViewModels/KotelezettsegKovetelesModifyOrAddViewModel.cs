using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public class KotelezettsegKovetelesModifyOrAddViewModel : ViewModelBase, IDataErrorInfo
    {
        public event Action RequestClose;
        //Obligation - kötelezettség, Claim - követelés
        private string _oblClaimType;
        private int _amount;
        private Penznem _currency;
        private DateTime _paymentDeadline = DateTime.Now;
        private Int16 _completed;
        private string _title;
        private EditMode _editMode;
        private KotelezettsegKoveteles _modifiableKotelezettsegKoveteles;
        private KotelezettsegKovetelesRepository _kotelezettsegKovetelesRepository = new KotelezettsegKovetelesRepository();
        public KotelezettsegKoveteles ModifiableKotelezettsegKoveteles
        {
            get
            {
                return _modifiableKotelezettsegKoveteles;
            }
            set
            {
                _modifiableKotelezettsegKoveteles = value;
                OblClaimType = value.Tipus;
                Amount = value.Osszeg;
                Currency = value.Penznem;
                PaymentDeadline = value.KifizetesHatarideje;
                Completed = value.Kifizetett;
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
                    Title = "Új Kötelezettség/Követelés Létrehozása";
                else if (_editMode == EditMode.Modify)
                    Title = "Kötelezettség/Követelés Módosítása";
            }
        }
        public string OblClaimType
        {
            get
            {
                return _oblClaimType;
            }
            set
            {
                _oblClaimType = value;
                OnPropertyChanged(nameof(OblClaimType));
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
        public DateTime PaymentDeadline
        {
            get
            {
                return _paymentDeadline;
            }
            set
            {
                _paymentDeadline = value;
                OnPropertyChanged(nameof(PaymentDeadline));
            }
        }
        public Int16 Completed
        {
            get
            {
                return _completed;
            }
            set
            {
                _completed = value;
                OnPropertyChanged(nameof(Completed));
                //MessageBox.Show($"SelectedOption set to: {_completed}");
            }
        }
        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(OblClaimType):
                        if (string.IsNullOrWhiteSpace(OblClaimType))
                            error = "Kötelezettség és követelés típus nem lehet üres.";
                        break;
                    case nameof(Amount):
                        if (string.IsNullOrWhiteSpace(Amount.ToString()) && Amount == 0)
                            error = "Összeg nem lehet üres és 0.";
                        break;
                }
                return error;
            }
        }

        public string Error => null;

        public ICommand SaveCommand { get; }
        public ICommand ResetEditCommand { get; }

        public KotelezettsegKovetelesModifyOrAddViewModel()
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
                    AddKotelezettsegKoveteles();
                    break;
                case EditMode.Modify:
                    ModifyKotelezettsegKoveteles();
                    break;
            }
        }
        private void ExecuteResetEditCommand(object parameter)
        {
            if (this.EditMode == EditMode.Modify)
            {
                Amount = ModifiableKotelezettsegKoveteles.Osszeg;
                Currency = ModifiableKotelezettsegKoveteles.Penznem;
                Completed = ModifiableKotelezettsegKoveteles.Kifizetett;
                PaymentDeadline= ModifiableKotelezettsegKoveteles.KifizetesHatarideje;
                OblClaimType = ModifiableKotelezettsegKoveteles.Tipus;
            }
        }
        private void AddKotelezettsegKoveteles()
        {
            try
            {
                if (HasValidationErrors())
                {
                    System.Windows.MessageBox.Show("Kérjük, javítsa ki a hibákat a mentés előtt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Amount != null && PaymentDeadline != null && OblClaimType != null)
                {
                    KotelezettsegKoveteles kotelezettsegKoveteles = new KotelezettsegKoveteles(OblClaimType, Amount, Currency, PaymentDeadline, Completed);
                    _kotelezettsegKovetelesRepository.AddKotelezettsegKoveteles(kotelezettsegKoveteles);
                    Mediator.NotifyNewKotelezettsegKovetelesAdded(kotelezettsegKoveteles);
                    CloseWindow();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        private void ModifyKotelezettsegKoveteles()
        {
            try
            {
                if (HasValidationErrors())
                {
                    System.Windows.MessageBox.Show("Kérjük, javítsa ki a hibákat a mentés előtt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Amount != null && OblClaimType != null && PaymentDeadline != null)
                {
                    KotelezettsegKoveteles kotelezettsegKoveteles = new KotelezettsegKoveteles(ModifiableKotelezettsegKoveteles.ID, OblClaimType, Amount, Currency, PaymentDeadline, Completed);
                    _kotelezettsegKovetelesRepository.ModifyKotelezettsegKoveteles(kotelezettsegKoveteles);
                    Mediator.NotifyModifiedKotelezettsegKoveteles(kotelezettsegKoveteles);
                    CloseWindow();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        private bool HasValidationErrors()
        {
            return !string.IsNullOrEmpty(this[nameof(OblClaimType)]) ||
                   !string.IsNullOrEmpty(this[nameof(Amount)]);
        }
        private void CloseWindow()
        {
            RequestClose?.Invoke();
        }
    }
}
