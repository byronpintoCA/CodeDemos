using System;

namespace ByronSouthParkDemo.Model
{
    public class Character
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public Gender Gender { get; set; }
        public Uri Image { get; set; }
        public int Age { get; set; }
        public string EmailAddress { get; set; }
        public string Occupation { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
    }

    public enum Gender
    {
        Unknown,
        Male, 
        Female,
    }
}