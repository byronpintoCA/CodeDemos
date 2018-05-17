using System;
using System.Windows;
using ByronSouthParkDemo.Model;
using ByronSouthParkDemo.View;
using ByronSouthParkDemo.ViewModel;
using Microsoft.Win32;


namespace ByronSouthParkDemo.Common
{
    /// <summary>
    /// Could use a different ViewController in the case that we were using Silverlight
    /// </summary>
    public class ViewController
    {
        /// <summary>
        /// Could display a custom delete confirmation dialog . But for now . Let's keep it simple
        /// But either ways .. The confirmation dialog should NOT be in the View Model
        /// </summary>
        /// <returns></returns>
        public bool ConfirmDelete()
        {
            var result = MessageBox.Show("Are You sure you want to delete ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }

            return false;
        }

        public bool SelectFile(out String theFile)
        {
            theFile = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Png files (*.png)|*.png";
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory + "Resources";
            if (openFileDialog.ShowDialog() == true)
            {
                theFile = openFileDialog.FileName;
                return true;
            }

            return false;
        }

        public bool EditCharacter(Character selectedCharacter)
        {
            bool ret = false;
            Character temp = new Character();
            MiscUtilities.Copy(temp, selectedCharacter);

            AddEditCharacterViewModel vm = new AddEditCharacterViewModel(temp, CharacterViewMode.Edit );
            AddEditCharacter window = new AddEditCharacter(vm);

            vm.SavedClicked = new Action(() => window.Close() );

            window.ShowDialog();

            if (vm.SavedSelected == true)
            { // Save
                ret = SouthParkViewModelFactory.GetInstance().CharacterProvider.UpdateCharacter(selectedCharacter, vm.TargetCharacter);
            }

            return ret;
            //MessageBox.Show($"Edit : {selectedCharacter.FirstName}");
        }

        public bool AddCharacter(out Character newCharacter)
        {
            bool ret = false;
            newCharacter = null;
            AddEditCharacterViewModel vm = new AddEditCharacterViewModel(new Character(), CharacterViewMode.Add);
            AddEditCharacter window = new AddEditCharacter(vm);

            vm.SavedClicked = new Action(() => window.Close());

            window.ShowDialog();

            if (vm.SavedSelected == true)
            { // Save
                ret = SouthParkViewModelFactory.GetInstance().CharacterProvider.AddCharacter(vm.TargetCharacter);

                newCharacter = vm.TargetCharacter;
            }

            return ret;
        }
    }
}
