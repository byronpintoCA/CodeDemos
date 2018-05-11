using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MiscHelper
{
    public class AESEncryption
    {
        public byte[] HexToByteArray(string hexString)
        {
            if (0 != (hexString.Length % 2))
            {
                throw new ApplicationException("Hex string must be multiple of 2 in length");
            }

            int byteCount = hexString.Length / 2;
            byte[] byteValues = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                byteValues[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return byteValues;
        }

        public static byte [] CreateKey()
        {
            byte[] retVal = new byte[32];

            Buffer.BlockCopy(Guid.NewGuid().ToByteArray(), 0, retVal, 0, 16);
            Buffer.BlockCopy(Guid.NewGuid().ToByteArray(), 0, retVal, 16, 16);

            return retVal;
        }

        public static String CreateKeyString()
        {
            return $"{Guid.NewGuid().ToString("N")}{Guid.NewGuid().ToString("N")}";
        }

       private static RijndaelManaged CreateCipher(byte[] bkey)
        {
            RijndaelManaged cipher = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.ISO10126,
                Mode = CipherMode.CBC,
                Key = bkey
            };
            return cipher;
        }

        public static byte[] Encrypt( string plainText,byte[] key, out byte[] IV)
        {
            RijndaelManaged rijndael = CreateCipher(key);
            IV =rijndael.IV;
            ICryptoTransform cryptoTransform = rijndael.CreateEncryptor();
            byte[] plain = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherText = cryptoTransform.TransformFinalBlock(plain, 0, plain.Length);
            return cipherText;
        }

        public static String EncryptToString(string plainText, byte[] key, out byte[] IV)
        {
            byte[] cipherText = Encrypt(plainText, key, out IV);
            return Convert.ToBase64String(cipherText);
        }

        public static String Decrypt(byte[] key, byte[] IV, string cipherText)
        {
            RijndaelManaged cipher = CreateCipher(key);
            cipher.IV = IV;
            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] plainText = cryptTransform.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length);

            return Encoding.UTF8.GetString(plainText);
        }


    }
}
