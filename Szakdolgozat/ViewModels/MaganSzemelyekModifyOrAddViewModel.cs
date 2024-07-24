using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;

namespace Szakdolgozat.ViewModels
{
    public class MaganSzemelyekModifyOrAddViewModel : ViewModelBase
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

        public ICommand SaveCommand { get; }

        public MaganSzemelyekModifyOrAddViewModel()
        {
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
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

        private void AddMaganSzemely()
        {
            try
            {
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
        private void CloseWindow()
        {
            RequestClose?.Invoke();
        }
    }
}
