using ByronSouthParkDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByronSouthParkDemo.Common
{
    public class MiscUtilities
    {
        public static void Copy(Character itemToUpdate, Character target)
        {

            itemToUpdate.FirstName = target.FirstName;
            itemToUpdate.LastName = target.LastName;
            itemToUpdate.Age = target.Age;
            itemToUpdate.City = target.City;
            itemToUpdate.State = target.State;
            itemToUpdate.StreetAddress = target.StreetAddress;
            itemToUpdate.ZipCode = target.ZipCode;
            itemToUpdate.EmailAddress = target.EmailAddress;
            itemToUpdate.Gender = target.Gender;
            itemToUpdate.NickName = target.NickName;
            itemToUpdate.Occupation = target.Occupation;
            itemToUpdate.Image = target.Image;
        }
    }
}
