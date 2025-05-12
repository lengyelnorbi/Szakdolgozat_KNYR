using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;

namespace Szakdolgozat.ViewModels
{
    public class MaganSzemelyekModifyOrAddViewModel : ViewModelBase, IDataErrorInfo
    {
        public event Action RequestClose;
        private string _name;
        private string _homeAddress;
        private string _email;
        private string _phonenumber;
        private string _title;
        private EditMode _editMode;
        private MaganSzemely _modifiableMaganSzemely;
        private MaganSzemelyRepository _maganSzemelyRepository = new MaganSzemelyRepository();
        public MaganSzemely ModifiableMaganSzemely
        {
            get
            {
                return _modifiableMaganSzemely;
            }
            set
            {
                _modifiableMaganSzemely = value;
                Name = value.Nev;
                HomeAddress = value.Lakcim;
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
                    Title = "Új Magán Személy Létrehozása";
                else if (_editMode == EditMode.Modify)
                    Title = "Magán Személy Módosítása";
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
        public string HomeAddress
        {
            get
            {
                return _homeAddress;
            }
            set
            {
                _homeAddress = value;
                OnPropertyChanged(nameof(HomeAddress));
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
                    case nameof(HomeAddress):
                        if (string.IsNullOrWhiteSpace(HomeAddress))
                            error = "Lakcím nem lehet üres.";
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
            return phoneNumber.All(char.IsDigit) && phoneNumber.Length >= 8 && phoneNumber.Length <= 15;
        }
        public ICommand SaveCommand { get; }
        public ICommand ResetEditCommand { get; }

        public MaganSzemelyekModifyOrAddViewModel()
        {
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
            ResetEditCommand = new ViewModelCommand(ExecuteResetEditCommand);
        }

        public MaganSzemely NotificationAboutAddedMaganSzemely()
        {
            return null;
        }

        private void ExecuteSaveCommand(object parameter)
        {
            switch (this.EditMode)
            {
                case EditMode.Add:
                    AddMaganSzemely();
                    break;
                case EditMode.Modify:
                    ModifyMaganSzemely();
                    break;
            }
        }
        private void ExecuteResetEditCommand(object parameter)
        {
            if (this.EditMode == EditMode.Modify)
            {
                Name = ModifiableMaganSzemely.Nev;
                HomeAddress = ModifiableMaganSzemely.Lakcim;
                Email = ModifiableMaganSzemely.Email;
                Phonenumber = ModifiableMaganSzemely.Telefonszam;
            }
        }
        private void AddMaganSzemely()
        {
            try
            {
                if (HasValidationErrors())
                {
                    MessageBox.Show("Kérjük, javítsa ki a hibákat a mentés előtt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Name != null && HomeAddress != null && Email != null && Phonenumber != null)
                {
                    MaganSzemely maganSzemely = new MaganSzemely(Name, HomeAddress, Email, Phonenumber);
                    _maganSzemelyRepository.AddMaganSzemely(maganSzemely);
                    Mediator.NotifyNewMaganSzemelyAdded(maganSzemely);
                    CloseWindow();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        private void ModifyMaganSzemely()
        {
            try
            {
                if (HasValidationErrors())
                {
                    MessageBox.Show("Kérjük, javítsa ki a hibákat a mentés előtt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Name != null && HomeAddress != null && Email != null && Phonenumber != null)
                {
                    MaganSzemely maganSzemely = new MaganSzemely(ModifiableMaganSzemely.ID, Name, HomeAddress, Email, Phonenumber);
                    _maganSzemelyRepository.ModifyMaganSzemely(maganSzemely);
                    Mediator.NotifyModifiedMaganSzemely(maganSzemely);
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
            return !string.IsNullOrEmpty(this[nameof(Name)]) ||
                   !string.IsNullOrEmpty(this[nameof(HomeAddress)]) ||
                   !string.IsNullOrEmpty(this[nameof(Email)]) ||
                   !string.IsNullOrEmpty(this[nameof(Phonenumber)]);
        }
        private void CloseWindow()
        {
            RequestClose?.Invoke();
        }
    }
}
