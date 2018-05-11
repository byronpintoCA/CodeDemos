using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MiscHelper
{
    //makecert -n "CN=X509Test" -r -sky exchange -sv X509Test.pvk X509Test.cer
    //cd C:\Program Files (x86)\Windows Kits\8.0\bin\x86
    // pvk2pfx.exe -pvk "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\X509Test.pvk" -spc "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\X509Test.cer" -pfx "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\X509Test.pfx"
    // C:\Program Files (x86)\Windows Kits\8.0\bin\x86>pvk2pfx.exe

    public class X509Encryption
    {

        public static void Sign(X509Certificate2 senderPrivate, X509Certificate2 recieverPublic, string message, out byte[] cipherBytes, out byte[] signatureHash)
        {
            cipherBytes = ((RSACryptoServiceProvider)recieverPublic.PublicKey.Key).Encrypt(Encoding.UTF8.GetBytes(message), false);
            byte[] cipherHash = new SHA1Managed().ComputeHash(cipherBytes);
            signatureHash = ((RSACryptoServiceProvider)senderPrivate.PrivateKey).SignHash(cipherHash, CryptoConfig.MapNameToOID("SHA1"));
        }

        public static bool VerifySignature(X509Certificate2 senderPublicKey, string cipher, string signatureHash)
        {
            byte[] cipherHash = new SHA1Managed().ComputeHash(Convert.FromBase64String(cipher));

            return ((RSACryptoServiceProvider)senderPublicKey.PublicKey.Key).VerifyHash(cipherHash, CryptoConfig.MapNameToOID("SHA1"), Convert.FromBase64String(signatureHash));
        }

        public static X509Certificate2 LoadCertificate(StoreLocation storeLocation, string certificateName)
        {
            X509Store store = new X509Store(storeLocation);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = store.Certificates;
            X509Certificate2 cert = certCollection.Cast<X509Certificate2>().FirstOrDefault(c => c.Subject == certificateName);
            if (cert == null)
                Console.WriteLine($"No Certificate named {certificateName} was found in your certificate store");
            store.Close();
            return cert;
        }

        public static string Encrypt(X509Certificate2 x509, string stringToEncrypt)
        {
            if (x509 == null || string.IsNullOrEmpty(stringToEncrypt))
                throw new Exception("A x509 certificate and string for encryption must be provided");

            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509.PublicKey.Key;
            byte[] bytestoEncrypt = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt);
            byte[] encryptedBytes = rsa.Encrypt(bytestoEncrypt, false);
            return Convert.ToBase64String(encryptedBytes);
        }
        public static string Encrypt(X509Certificate2 x509, byte[] dataEncrypt)
        {
            if (x509 == null || dataEncrypt == null || dataEncrypt.Length <= 0)
                throw new Exception("A x509 certificate and string for encryption must be provided");

            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509.PublicKey.Key;
            byte[] encryptedBytes = rsa.Encrypt(dataEncrypt, false);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptAsString(X509Certificate2 x509, string stringTodecrypt)
        {
            byte[] plainbytes = Decrypt(x509, stringTodecrypt);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(plainbytes);
        }

        public static byte [] Decrypt(X509Certificate2 x509, string stringTodecrypt)
        {
            if (x509 == null || string.IsNullOrEmpty(stringTodecrypt))
                throw new Exception("A x509 certificate and string for decryption must be provided");

            if (!x509.HasPrivateKey)
                throw new Exception("x509 certicate does not contain a private key for decryption");

            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509.PrivateKey;
            byte[] bytestodecrypt = Convert.FromBase64String(stringTodecrypt);
            return  rsa.Decrypt(bytestodecrypt, false);
        }
    }
}
