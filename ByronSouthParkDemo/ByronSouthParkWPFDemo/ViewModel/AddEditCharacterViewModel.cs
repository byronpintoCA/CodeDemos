using ByronSouthParkDemo.Common;
using ByronSouthParkDemo.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByronSouthParkDemo.ViewModel
{
    public enum CharacterViewMode { Add, Edit }
    public class AddEditCharacterViewModel : ViewModelBase  
    {
        private bool _NoErrors;
        private List<string> _errorsList;

        public CharacterViewMode ViewMode { get; }
        public Character TargetCharacter { get; }

        public RelayCommand Save { get; private set; }

        public RelayCommand Cancel { get; private set; }

        public RelayCommand ValidateCmd { get; private set; }

        public RelayCommand SelectAnImage { get; private set; }

        public bool SavedSelected { get; private set; }

        public List<String> GenderList { get; private set; }

        public AddEditCharacterViewModel(Character ch, CharacterViewMode viewMode)
        {
            Errors = false;
            GenderList = GetGenderList();
            this.ViewMode = viewMode;
            this.TargetCharacter = ch;
            SetupCommands();
        }

        private List<string> GetGenderList()
        {
            List<String> temp = new List<string>();

            foreach (var name in Enum.GetNames(typeof(Gender)))
            {
                temp.Add(name);
            }

            return temp;
        }

        private void SetupCommands()
        {
            Save = new RelayCommand(SaveCharacter);
            Cancel = new RelayCommand(CancelEdit);
            ValidateCmd = new RelayCommand(ValidateWork);
            SelectAnImage = new RelayCommand(ImageSelect);
        }

        private void ImageSelect()
        {
            if (true == SouthParkViewModelFactory.GetInstance().ViewManager.SelectFile(out String newFile))
            {
                Image = new Uri(newFile, UriKind.Absolute);
            }
        }

        private void ValidateWork()
        {
            Validate();
        }

        private void SaveCharacter()
        {
            if (Validate() == false)
            {
                
                SavedSelected = false;
            }
            else
            {  
                SavedSelected = true;
                SavedClicked?.Invoke();
            }
        }

        private void CancelEdit()
        {
            SavedSelected = false;
        }

        public bool Errors
        {
            get { return _NoErrors; }
            set { _NoErrors = value; OnPropertyChanged(); }
        }


        public bool Validate()
        {
            bool valid = true;
            List<String> errors = new List<string>();
            if (String.IsNullOrWhiteSpace(TargetCharacter.FirstName) == true)
            {
                errors.Add("First Name cannot be blank");
                valid = false;
            }

            if (String.IsNullOrWhiteSpace(TargetCharacter.LastName) == true)
            {
                errors.Add("Last Name cannot be blank");
                valid = false;
            }

            ErrorsList = errors;
            Errors = !valid;
            return valid;
        }

        public List<String> ErrorsList
        {
            get { return _errorsList; }
            set { _errorsList = value; OnPropertyChanged(); }
        }

        public Action SavedClicked { get; internal set; }

        public Uri Image
        {
            get { return TargetCharacter.Image; }
            set { TargetCharacter.Image = value; OnPropertyChanged(); }
        }
    }
}
