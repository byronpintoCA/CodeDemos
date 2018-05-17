using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByronSouthParkDemo.Common;
using ByronSouthParkDemo.Model;

namespace ByronSouthParkDemo.DataProvider
{
    public class TestCharacterProvider : SouthParkDataProvider
    {
        private ObservableCollection<Character> _testCollection;

        public TestCharacterProvider()
        {
            var tempCollection = new CharacterCollectionModel();
            tempCollection.PopulateCollection();

            _testCollection = new ObservableCollection<Character>(tempCollection);

        }
        public bool AddCharacter(Character c)
        {
            _testCollection.Add(c);
            return true;
        }

        public ObservableCollection<Character> GetAllCharacters()
        {
            return _testCollection;
        }

        public bool RemoveCharacter(Character target)
        {
            bool success = false;
            var itemToRemove = _testCollection.Where(charac => charac.FirstName == target.FirstName && charac.LastName == target.LastName
                                    && charac.Age == target.Age && charac.City == target.City && charac.State == target.State && charac.ZipCode == target.ZipCode
                                    && charac.StreetAddress == target.StreetAddress).FirstOrDefault();

            if (itemToRemove != null)
            {
                _testCollection.Remove(itemToRemove);
                success = true;
            }


            return success;
        }

        public bool UpdateCharacter(Character original , Character newValues)
        {
            bool success = false;
            var itemToUpdate = _testCollection.Where(charac => charac.FirstName == original.FirstName && charac.LastName == original.LastName
                                    && charac.Age == original.Age && charac.City == original.City && charac.State == original.State && charac.ZipCode == original.ZipCode
                                    && charac.StreetAddress == original.StreetAddress).FirstOrDefault();

            if (itemToUpdate != null)
            {
                MiscUtilities.Copy(itemToUpdate, newValues);
                success = true;
            }


            return success;
        }

       
    }
}
