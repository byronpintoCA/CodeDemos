using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiscHelper;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using MiscHelper.Encryption;

namespace MiscHelperTest.Encryption
{
    [TestClass]
    public class UTAllTogether
    {
        private string _senderPrivatePath;
        private string _senderPublicPath;

        public UTAllTogether()
        {
            _senderPrivatePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Certificates\\DCSender.pfx";


            _senderPublicPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Certificates\\DCSender.cer";
        }

     
        [TestMethod]
        public void EncryptAndDecrypt()
        {
            String target = " Jack and jill went up the hill to fetch a pale of water";

            var payload = EncryptedPayload.Encrypt(new X509Certificate2(_senderPublicPath), target);

            string data = EncryptedPayload.Decrypt(new X509Certificate2(_senderPrivatePath), payload);

            Assert.AreEqual(target, data, false);
            
        }
    }
}
