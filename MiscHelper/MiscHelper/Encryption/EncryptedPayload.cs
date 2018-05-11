using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MiscHelper.Encryption
{
    public class EncryptedPayload
    {
        public string Data { get; private set; }
        public string Key { get; private set; }
        public string Salt { get; private set; }

        public static EncryptedPayload Encrypt(X509Certificate2 publicCert, String target)
        {
            byte[] key = AESEncryption.CreateKey();
            string encryptedData = AESEncryption.EncryptToString(target, key, out byte[] salt);
            string encryptedKey = X509Encryption.Encrypt(publicCert, key);

            return new EncryptedPayload()
            {
                Data = encryptedData,
                Key = encryptedKey,
                Salt = Convert.ToBase64String(salt)
                
            };
        }

        public static string Decrypt(X509Certificate2 privateKey, EncryptedPayload payload)
        {
            byte [] decryptedKey =X509Encryption.Decrypt(privateKey, payload.Key);

            string retVal = AESEncryption.Decrypt(decryptedKey, Convert.FromBase64String(payload.Salt), payload.Data);

            return retVal;
        }
    }
}
