using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;

namespace Szakdolgozat.ViewModels
{
    public class KotelezettsegKovetelesModifyOrAddViewModel : ViewModelBase
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
                    Title = "Új Bevétel/Kiadás Létrehozása";
                else if (_editMode == EditMode.Modify)
                    Title = "Bevétel/Kiadás Módosítása";
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
                MessageBox.Show($"SelectedOption set to: {_completed}");
            }
        }

        public ICommand SaveCommand { get; }

        public KotelezettsegKovetelesModifyOrAddViewModel()
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
                    AddKotelezettsegKoveteles();
                    break;
                case EditMode.Modify:
                    ModifyKotelezettsegKoveteles();
                    break;
            }
        }

        private void AddKotelezettsegKoveteles()
        {
            try
            {
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
                if (Amount != null && OblClaimType != null && PaymentDeadline != null)
                {
                    KotelezettsegKoveteles kotelezettsegKoveteles = new KotelezettsegKoveteles(OblClaimType, Amount, Currency, PaymentDeadline, Completed);
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
        private void CloseWindow()
        {
            RequestClose?.Invoke();
        }
    }
}
