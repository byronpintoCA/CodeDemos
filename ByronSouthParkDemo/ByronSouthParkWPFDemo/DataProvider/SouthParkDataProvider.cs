using ByronSouthParkDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByronSouthParkDemo.DataProvider
{
    public interface SouthParkDataProvider
    {
        ObservableCollection<Character> GetAllCharacters();

        bool AddCharacter(Character target);

        bool RemoveCharacter(Character target);

        bool UpdateCharacter(Character old ,Character newValues);
    }
}
