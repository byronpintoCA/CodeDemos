using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using MiscHelper;
using System.Diagnostics;

namespace MiscHelperTest
{
    [TestClass]
    public class UTX509
    {
        private string _certificatePath;
        private string _public_private_package_path;

        public UTX509()
        {
            _certificatePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Certificates\\X509Test.cer";

            _public_private_package_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Certificates\\X509Test.pfx";
        }

        [TestMethod]
        public void PublicPrivateKeyFromFile()
        {
            string DATA_TO_ENCRYPT = "How are you";

            // Encrypt with the certificate and decrypt with the combo pfx
            X509Certificate2 publicX509 = new X509Certificate2(_certificatePath);
            String encryptedData = X509Encryption.Encrypt(publicX509, DATA_TO_ENCRYPT);

            X509Certificate2 comboKey = new X509Certificate2(_public_private_package_path);
            String decryptedData = X509Encryption.DecryptAsString(comboKey, encryptedData);
            Assert.AreEqual(DATA_TO_ENCRYPT, decryptedData, false, "Strings don't match");


            // Encrypt with the combo and decrypt with the combo pfx
            encryptedData = X509Encryption.Encrypt(comboKey, DATA_TO_ENCRYPT);
            decryptedData = X509Encryption.DecryptAsString(comboKey, encryptedData);
            Assert.AreEqual(DATA_TO_ENCRYPT, decryptedData, false, "Strings don't match");


        }

        #region Generate Root Certificate
        //makecert.exe -r -n "CN=ByronRoot" -pe -sv ByronRoot.pvk -a sha1 -len 2048 -b 01/01/2015 -e 01/01/2030 -cy authority ByronRoot.cer
        //"C:\Program Files (x86)\Windows Kits\8.0\bin\x86\pvk2pfx.exe" -pvk "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\ByronRoot.pvk" -spc "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\ByronRoot.cer" -pfx "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\ByronRoot.pfx" 
        #endregion

        #region Generate Child Certificate

        //makecert.exe -ic ByronRoot.cer -iv ByronRoot.pvk -pe -sv ByronChild.pvk -a sha1 -n “CN=ByronChild” -len 2048 -b 01/01/2015 -e 01/01/2030 -sky exchange ByronChild.cer -eku 1.3.6.1.5.5.7.3.1
        //"C:\Program Files (x86)\Windows Kits\8.0\bin\x86\pvk2pfx.exe" -pvk "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\ByronChild.pvk" -spc "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\ByronChild.cer" -pfx "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\ByronChild.pfx"

        #endregion

        [TestMethod]
        public void LoadCertificateFromStore()
        {
            var cert = X509Encryption.LoadCertificate(StoreLocation.LocalMachine, "CN=ByronChild");

            String data = "Helojnmj kuhkjh ii";

            var encrypted =X509Encryption.Encrypt(cert, data);

            var decrypted = X509Encryption.DecryptAsString(cert, encrypted);

            Assert.AreEqual(data, decrypted, false);
        }

    }
}
