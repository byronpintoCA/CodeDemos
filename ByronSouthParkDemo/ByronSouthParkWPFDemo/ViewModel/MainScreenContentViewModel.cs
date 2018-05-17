using ByronSouthParkDemo.Common;
using ByronSouthParkDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ByronSouthParkDemo.ViewModel
{
    public class MainScreenContentViewModel : ViewModelBase
    {
        public MainScreenViewModel.ViewModeKind DaView;

        public RelayCommand Remove { get; private set; }

        public RelayCommand Add { get; private set; }

        public RelayCommand Edit { get; private set; }

        public MainScreenContentViewModel(MainScreenViewModel.ViewModeKind value, Character current, TimeService whatsDaTime)
        {
            CharacterList = SouthParkViewModelFactory.GetInstance().CharacterProvider.GetAllCharacters();
            this.DaView = value;
            SetupCommands();

            if (current == null)
            {
                SelectedCharacter = CharacterList.FirstOrDefault();
            }
            else
            {
                SelectedCharacter = current;
            }
            whatsDaTime.TimesUp += WhatsDaTime_TimesUp;

        }

        #region RelayCommands

        private void SetupCommands()
        {
            Remove = new RelayCommand(RemoveCharacter);
            Add = new RelayCommand(AddCharacter);
            Edit = new RelayCommand(EditCharacter);
        }

        private void AddCharacter()
        {
            if (true == SouthParkViewModelFactory.GetInstance().ViewManager.AddCharacter(out Character newCharacter))
            {
                FindAndSelectCharacter(newCharacter);
            }
        }

        private void EditCharacter()
        {
            if (true == SouthParkViewModelFactory.GetInstance().ViewManager.EditCharacter(SelectedCharacter))
            {
                var tempChar = SelectedCharacter;
                CharacterList = null;
                CharacterList = SouthParkViewModelFactory.GetInstance().CharacterProvider.GetAllCharacters();
                FindAndSelectCharacter(tempChar);

                //  
            }
        }

        private void FindAndSelectCharacter(Character tempChar)
        {
            var sel = CharacterList.Where(c => c.FirstName == tempChar.FirstName && c.LastName == tempChar.LastName &&
                           c.StreetAddress == tempChar.StreetAddress && c.Image == tempChar.Image).FirstOrDefault();

            if (sel != null) SelectedCharacter = sel;
        }

        private void RemoveCharacter()
        {
            if (SouthParkViewModelFactory.GetInstance().ViewManager.ConfirmDelete() == true)
            {
                if (true == SouthParkViewModelFactory.GetInstance().CharacterProvider.RemoveCharacter(SelectedCharacter))
                {
                    SelectedCharacter = CharacterList.FirstOrDefault();
                }
            }
        }
        #endregion

        private void WhatsDaTime_TimesUp(DateTime obj)
        {
            TimeChanged = obj.ToString("MMM dd yyyy HH:mm:ss");
        }

        public void UnSubscribe(TimeService pTimeService)
        {
            pTimeService.TimesUp -= WhatsDaTime_TimesUp;
        }

        //return SouthParkViewModelFactory.GetInstance().CharacterProvider.GetAllCharacters();

        ObservableCollection<Character> _characterList;

        public ObservableCollection<Character> CharacterList
        {
            get
            {
                return _characterList;
            }
            set
            {
                _characterList = value;
                OnPropertyChanged();
            }
        }

        private Character _selectedCharacter;
        public Character SelectedCharacter
        {
            get { return _selectedCharacter; }
            set { _selectedCharacter = value; OnPropertyChanged(); }
        }

        private String _time;
        public String TimeChanged
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged(); }
        }
    }
}
