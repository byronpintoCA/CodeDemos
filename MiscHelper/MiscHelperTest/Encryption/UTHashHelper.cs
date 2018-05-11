using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiscHelper;

namespace MiscHelperTest
{
    [TestClass]
    public class UTHashHelper
    {
        [TestMethod]
        public void HashSHA512Test()
        {
            var hash = HashHelper.GenerateSHA512Hash("DamnIt", out string salt);

            var secondhash = HashHelper.GenerateSHA512Hash("DamnIt" + salt);

            Assert.IsTrue(hash == secondhash);

        }

        [TestMethod]
        public void HashHMACTest()
        {
            string key = "1234567890123456";
            var hash = HashHelper.GenerateHMACHash(key, "DamnIt");

            var secondhash = HashHelper.GenerateHMACHash(key, "DamnIt" );

            Assert.IsTrue(hash == secondhash);

            secondhash = HashHelper.GenerateHMACHash(key+"32", "DamnIt");

            Assert.IsTrue(hash != secondhash);

        }
    }
}
