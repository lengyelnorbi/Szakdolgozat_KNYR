using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;

namespace Szakdolgozat.ViewModels
{
    public class GazdalkodoSzervezetModifyOrAddViewModel : ViewModelBase, IDataErrorInfo
    {
        public event Action RequestClose;
        private string _name;
        private string _contactPerson;
        private string _phonenumber;
        private string _email;
        private string _title;
        private EditMode _editMode;
        private GazdalkodoSzervezetRepository _gazdalkodoSzervezetRepository = new GazdalkodoSzervezetRepository();
        private GazdalkodoSzervezet _modifiableGazdalkodoSzervezet;
        public GazdalkodoSzervezet ModifiableGazdalkodoSzervezet
        {
            get
            {
                return _modifiableGazdalkodoSzervezet;
            }
            set
            {
                _modifiableGazdalkodoSzervezet = value;
                Name = value.Nev;
                ContactPerson = value.Kapcsolattarto;
                Email = value.Email;
                Phonenumber = value.Telefonszam;
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
                    Title = "Új Gazdálkodó Szervezet Létrehozása";
                else if (_editMode == EditMode.Modify)
                    Title = "Gazdálkodó Szervezet Módosítása";
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string ContactPerson
        {
            get
            {
                return _contactPerson;
            }
            set
            {
                _contactPerson = value;
                OnPropertyChanged(nameof(ContactPerson));
            }
        }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Phonenumber
        {
            get
            {
                return _phonenumber;
            }
            set
            {
                _phonenumber = value;
                OnPropertyChanged(nameof(Phonenumber));
            }
        }
        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(Name))
                            error = "Név nem lehet üres.";
                        break;
                    case nameof(ContactPerson):
                        if (string.IsNullOrWhiteSpace(ContactPerson))
                            error = "A Kapcsolattartó nem lehet üres.";
                        break;
                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            error = "Email nem lehet üres.";
                        else if (!IsValidEmail(Email))
                            error = "Érvénytelen email formátum.";
                        break;
                    case nameof(Phonenumber):
                        if (string.IsNullOrWhiteSpace(Phonenumber))
                            error = "Telefonszám nem lehet üres.";
                        else if (!IsValidPhoneNumber(Phonenumber))
                            error = "Érvénytelen telefonszám formátum.";
                        break;
                }
                return error;
            }
        }

        public string Error => null;

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            if (phoneNumber.StartsWith("+"))
            {
                return phoneNumber.Length > 1 &&
                       phoneNumber.Substring(1).All(char.IsDigit) &&
                       phoneNumber.Length >= 9 &&
                       phoneNumber.Length <= 12;
            }

            return phoneNumber.All(char.IsDigit) &&
                   phoneNumber.Length >= 9 &&
                   phoneNumber.Length <= 12;
        }
        public ICommand SaveCommand { get; }
        public ICommand ResetEditCommand { get; }

        public GazdalkodoSzervezetModifyOrAddViewModel()
        {
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
            ResetEditCommand = new ViewModelCommand(ExecuteResetEditCommand);
        }

        public GazdalkodoSzervezet NotificationAboutAddedDolgozo()
        {
            return null;
        }

        private void ExecuteSaveCommand(object parameter)
        {
            switch (this.EditMode)
            {
                case EditMode.Add:
                    AddGazdalkodoSzervezet();
                    break;
                case EditMode.Modify:
                    ModifyGazdalkodoSzervezet();
                    break;
            }
        }
        private void ExecuteResetEditCommand(object parameter)
        {
            if (this.EditMode == EditMode.Modify)
            {
                Name = ModifiableGazdalkodoSzervezet.Nev;
                ContactPerson = ModifiableGazdalkodoSzervezet.Kapcsolattarto;
                Email = ModifiableGazdalkodoSzervezet.Email;
                Phonenumber = ModifiableGazdalkodoSzervezet.Telefonszam;
            }
        }
        private void AddGazdalkodoSzervezet()
        {
            try
            {
                if (HasValidationErrors())
                {
                    MessageBox.Show("Kérjük, javítsa ki a hibákat a mentés előtt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Name != null && ContactPerson != null && Email != null && Phonenumber != null)
                {
                    GazdalkodoSzervezet gazdalkodoSzervezet = new GazdalkodoSzervezet(Name, ContactPerson, Email, Phonenumber);
                    bool isSuccess = _gazdalkodoSzervezetRepository.AddGazdalkodoSzervezet(gazdalkodoSzervezet);
                    if (isSuccess)
                    {
                        Mediator.NotifyNewGazdalkodoSzervezetAdded(gazdalkodoSzervezet);
                        System.Windows.MessageBox.Show("Gazdálkodó szervezet hozzáadása sikeres.", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                        CloseWindow();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Hiba a Gazdálkodó szervezet hozzáadása során.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        private void ModifyGazdalkodoSzervezet()
        {
            try
            {
                if (HasValidationErrors())
                {
                    MessageBox.Show("Kérjük, javítsa ki a hibákat a mentés előtt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Name != null && ContactPerson != null && Email != null && Phonenumber != null)
                {
                    GazdalkodoSzervezet gazdalkodoSzervezet = new GazdalkodoSzervezet(ModifiableGazdalkodoSzervezet.ID, Name, ContactPerson, Email, Phonenumber);
                    bool isSuccess = _gazdalkodoSzervezetRepository.ModifyGazdalkodoSzervezet(gazdalkodoSzervezet);
                    if (isSuccess)
                    {
                        Mediator.NotifyModifiedGazdalkodoSzervezet(gazdalkodoSzervezet);
                        System.Windows.MessageBox.Show("Gazdálkodó szervezet módosítása sikeres.", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                        CloseWindow();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Hiba a Gazdálkodó szervezet módosítása során.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
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
            return !string.IsNullOrEmpty(this[nameof(Name)]) ||
                   !string.IsNullOrEmpty(this[nameof(ContactPerson)]) ||
                   !string.IsNullOrEmpty(this[nameof(Email)]) ||
                   !string.IsNullOrEmpty(this[nameof(Phonenumber)]);
        }
        private void CloseWindow()
        {
            RequestClose?.Invoke();
        }
    }
}
