using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;

namespace Szakdolgozat.ViewModels
{
    public class DolgozokModifyOrAddViewModel : ViewModelBase, IDataErrorInfo
    {
        public event Action RequestClose;
        private string _lastname;
        private string _firstname;
        private string _email;
        private string _phonenumber;
        private string _title;
        private EditMode _editMode;
        private Dolgozo _modifiableDolgozo;
        private DolgozoRepository _dolgozoRepository = new DolgozoRepository();
        public Dolgozo ModifiableDolgozo
        {
            get 
            {
                return _modifiableDolgozo;
            }
            set 
            { 
                _modifiableDolgozo = value;
                Lastname = value.Vezeteknev;
                Firstname = value.Keresztnev;
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
                    Title = "Új Dolgozó Létrehozása";
                else if (_editMode == EditMode.Modify)
                    Title = "Dolgozó Módosítása";
            }
        }
        public string Lastname
        {
            get
            {
                return _lastname;
            }
            set
            {
                _lastname = value;
                OnPropertyChanged(nameof(Lastname));
            }
        }
        public string Firstname
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
                OnPropertyChanged(nameof(Firstname));
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
                    case nameof(Lastname):
                        if (string.IsNullOrWhiteSpace(Lastname))
                            error = "Vezetéknév nem lehet üres.";
                        break;
                    case nameof(Firstname):
                        if (string.IsNullOrWhiteSpace(Firstname))
                            error = "Keresztnév nem lehet üres.";
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

        public DolgozokModifyOrAddViewModel()
        {
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
            ResetEditCommand = new ViewModelCommand(ExecuteResetEditCommand);
        }

        public Dolgozo NotificationAboutAddedDolgozo()
        {
            return null;
        }

        private void ExecuteSaveCommand(object parameter)
        {
            switch (this.EditMode)
            {
                case EditMode.Add:
                    AddDolgozo();
                    break;
                case EditMode.Modify:
                    ModifyDolgozo();
                    break;
            }
        }
        private void ExecuteResetEditCommand(object parameter)
        {
            if (this.EditMode == EditMode.Modify)
            {
                Lastname = ModifiableDolgozo.Vezeteknev;
                Firstname = ModifiableDolgozo.Keresztnev;
                Email = ModifiableDolgozo.Email;
                Phonenumber = ModifiableDolgozo.Telefonszam;
            }
        }
        private void AddDolgozo()
        {
            try
            {
                if (HasValidationErrors())
                {
                    MessageBox.Show("Kérjük, javítsa ki a hibákat a mentés előtt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Dolgozo dolgozo = new Dolgozo(Lastname, Firstname, Email, Phonenumber);
                bool isSuccess = false;
                int newDolgozoID = 0;
                (isSuccess, newDolgozoID) = _dolgozoRepository.AddDolgozo(dolgozo);
                if (isSuccess)
                {
                    dolgozo.ID = newDolgozoID;
                    UserRepository userRepository = new UserRepository();
                    string username = "";
                    string password = "";
                    bool creationSuccess = false;
                    (creationSuccess, username, password) = userRepository.CreateAndAddUser(dolgozo);
                    if (creationSuccess)
                    {
                        MessageBox.Show($"Dolgozó hozzáadva. Az új felhasználóhoz generált adatok: \n Felhasználónév: {username}, Jelszó: {password}", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Dolgozó hozzáadva, de a felhasználó létrehozása nem sikerült.", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    Mediator.NotifyNewDolgozoAdded(dolgozo);
                    CloseWindow();
                }
                else
                {
                    MessageBox.Show("Dolgozó hozzáadása nem sikerült.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Hiba történt a dolgozó hozzáadása során: {e.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ModifyDolgozo()
        {
            try
            {
                if (HasValidationErrors())
                {
                    MessageBox.Show("Kérjük, javítsa ki a hibákat a mentés előtt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Dolgozo dolgozo = new Dolgozo(ModifiableDolgozo.ID, Lastname, Firstname, Email, Phonenumber);
                bool isSuccess = _dolgozoRepository.ModifyDolgozo(dolgozo);
                if (isSuccess)
                {
                    Mediator.NotifyModifiedDolgozo(dolgozo);
                    MessageBox.Show("Dolgozó módosítása sikeres.", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseWindow();
                }
                else
                {
                    MessageBox.Show("Dolgozó módosítása nem sikerült.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Hiba történt a dolgozó módosítása során: {e.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool HasValidationErrors()
        {
            return !string.IsNullOrEmpty(this[nameof(Lastname)]) ||
                   !string.IsNullOrEmpty(this[nameof(Firstname)]) ||
                   !string.IsNullOrEmpty(this[nameof(Email)]) ||
                   !string.IsNullOrEmpty(this[nameof(Phonenumber)]);
        }
        private void CloseWindow()
        {
            RequestClose?.Invoke();
        }
    }
}
