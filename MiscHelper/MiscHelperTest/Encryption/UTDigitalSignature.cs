using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using MiscHelper;

namespace MiscHelperTest
{
    [TestClass]
    public class UTDigitalSignature
    {

        private string _senderPrivatePublicPath;
        private string _recieverPublicPath;
        private string _recieverPrivatePublicPath;

        public UTDigitalSignature()
        {
            _senderPrivatePublicPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Certificates\\DCSender.pfx";

            _recieverPublicPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Certificates\\DCReciever.cer";

            _recieverPrivatePublicPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Certificates\\DCReciever.pfx";
        }


        [TestMethod]
        public DigitalSignature Sign(string DATA_TO_ENCRYPT = "How are you")
        {
            // Encrypt with the certificate and decrypt with the combo pfx
            var signature = DigitalSignature.BuildSignedMessage(new X509Certificate2(_senderPrivatePublicPath), new X509Certificate2(_recieverPublicPath), DATA_TO_ENCRYPT);

            var untampered = X509Encryption.VerifySignature(new X509Certificate2(_senderPrivatePublicPath), signature.Cipher, signature.Signature);

            Assert.IsTrue(untampered);

            return signature;
        }

        [TestMethod]
        public void Decrypt()
        {
            const string target = "What the hell .. How are you";

            var result = Sign(target);

            var untampered = X509Encryption.VerifySignature(new X509Certificate2(_senderPrivatePublicPath), result.Cipher, result.Signature);

            Assert.IsTrue(untampered);

            string decrypted = X509Encryption.DecryptAsString(new X509Certificate2(_recieverPrivatePublicPath), result.Cipher);

            Assert.AreEqual(target, decrypted, false);

        }
    }
}
