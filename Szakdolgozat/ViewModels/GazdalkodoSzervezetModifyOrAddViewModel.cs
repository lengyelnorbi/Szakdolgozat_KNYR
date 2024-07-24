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
    public class GazdalkodoSzervezetModifyOrAddViewModel : ViewModelBase
    {
        public event Action RequestClose;
        private int _id;
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

        public ICommand SaveCommand { get; }

        public GazdalkodoSzervezetModifyOrAddViewModel()
        {
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
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

        private void AddGazdalkodoSzervezet()
        {
            try
            {
                if (Name != null && ContactPerson != null && Email != null && Phonenumber != null)
                {
                    GazdalkodoSzervezet gazdalkodoSzervezet = new GazdalkodoSzervezet(Name, ContactPerson, Email, Phonenumber);
                    _gazdalkodoSzervezetRepository.AddGazdalkodoSzervezet(gazdalkodoSzervezet);
                    Mediator.NotifyNewGazdalkodoSzervezetAdded(gazdalkodoSzervezet);
                    CloseWindow();
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
                if (Name != null && ContactPerson != null && Email != null && Phonenumber != null)
                {
                    GazdalkodoSzervezet gazdalkodoSzervezet = new GazdalkodoSzervezet(ModifiableGazdalkodoSzervezet.ID, Name, ContactPerson, Email, Phonenumber);
                    _gazdalkodoSzervezetRepository.AddGazdalkodoSzervezet(gazdalkodoSzervezet);
                    Mediator.NotifyModifiedGazdalkodoSzervezet(gazdalkodoSzervezet);
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
