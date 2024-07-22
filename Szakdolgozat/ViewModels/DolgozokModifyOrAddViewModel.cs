using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Repositories;

namespace Szakdolgozat.ViewModels
{
    public class DolgozokModifyOrAddViewModel : ViewModelBase
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
        
        public ICommand SaveCommand { get;}

        public DolgozokModifyOrAddViewModel()
        {
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
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

        private void AddDolgozo()
        {
            try
            {
                if (Lastname != null && Firstname != null && Email != null && Phonenumber != null)
                {
                    Dolgozo dolgozo = new Dolgozo(Lastname, Firstname, Email, Phonenumber);
                    _dolgozoRepository.AddDolgozo(dolgozo);
                    Mediator.NotifyNewDolgozoAdded(dolgozo);
                    CloseWindow();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        private void ModifyDolgozo()
        {
            try
            {
                if (Lastname != null && Firstname != null && Email != null && Phonenumber != null)
                {
                    Dolgozo dolgozo = new Dolgozo(ModifiableDolgozo.ID, Lastname, Firstname, Email, Phonenumber);
                    _dolgozoRepository.ModifyDolgozo(dolgozo);
                    Mediator.NotifyModifiedDolgozo(dolgozo);
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
