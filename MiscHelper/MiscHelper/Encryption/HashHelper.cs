using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MiscHelper
{
    public class HashHelper
    {
        public static String GenerateSHA512Hash(String input, out String AppendedSalt)
        {
            AppendedSalt = Guid.NewGuid().ToString("N");
            return GenerateSHA512Hash(input + AppendedSalt);
        }

        public static string GenerateSHA512Hash(string input)
        {
            byte[] hashedValueOfTextOne = new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(input));

            String retVal = Convert.ToBase64String(hashedValueOfTextOne);

            return retVal;
        }

        public static string GenerateHMACHash(String key , string input)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = new HMACSHA1(Encoding.UTF8.GetBytes(key)).ComputeHash(textBytes);
            return Convert.ToBase64String(hash);
        }
    }
}
