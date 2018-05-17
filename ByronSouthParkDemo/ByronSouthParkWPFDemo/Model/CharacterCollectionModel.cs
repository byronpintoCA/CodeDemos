using System;
using System.Collections.Generic;

namespace ByronSouthParkDemo.Model
{
    public class CharacterCollectionModel : List<Character>
    {
        public void PopulateCollection()
        {
            Add(new Character
            {
                FirstName = "Stan",
                LastName = "Marsh",
                Age = 10,
                City = "South Park",
                State = "Colorado",
                StreetAddress = "1234 Moncada Dr.",
                ZipCode = 80440, 
                EmailAddress = "Stan.Marsh@gmail.com",
                Gender = Gender.Male,
                NickName = "Toolshed",
                Occupation = "Student",
                Image = new Uri("/Resources/stan-marsh.png", UriKind.Relative)
            });

            Add(new Character
            {
                FirstName = "Kyle",
                LastName = "Broflovski",
                Age = 10,
                City = "South Park",
                State = "Colorado",
                StreetAddress = "666 Sergey Rd.",
                ZipCode = 80442, 
                EmailAddress = "Kyle.Broflovski@gmail.com",
                Gender = Gender.Male,
                NickName = "The Human Kite",
                Occupation = "Student",
                Image = new Uri("/Resources/kyle-broflovski.png", UriKind.Relative)
            });

            Add(new Character
            {
                FirstName = "Eric",
                LastName = "Cartman",
                Age = 10,
                City = "South Park",
                State = "Colorado",
                StreetAddress = "6718 Hubert Ln.",
                ZipCode = 80440, 
                EmailAddress = "Eric.Cartman@gmail.com",
                Gender = Gender.Male,
                NickName = "A.W.E.S.O.M.-O 4000",
                Occupation = "Student",
                Image = new Uri("/Resources/eric-cartman.png", UriKind.Relative)
            });

            Add(new Character
            {
                FirstName = "Kenny",
                LastName = "McCormick",
                Age = 10,
                City = "South Park",
                State = "Colorado",
                StreetAddress = "2000 Brandon St.",
                ZipCode = 80441, 
                EmailAddress = "Kenny.McCormick@gmail.com",
                Gender = Gender.Male,
                NickName = "Mysterion",
                Occupation = "Student",
                Image = new Uri("/Resources/kenny-mccormick.png", UriKind.Relative)
            });
        }
    }
}