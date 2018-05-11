using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiscHelper;

namespace MiscHelperTest
{
    [TestClass]
    public class UTParameterAction
    {
        private static void TestOne(string[] args)
        {
            ParameterActionMapper<String>.Register("/eod", () =>
            {
                return "eod";
            });


            ParameterActionMapper<String>.Register("/day ", () =>
            {
                return "day";
            });

            ParameterActionMapper<String>.Register("/funda ", () =>
            {
                return "funda";
            });

            var result = ParameterActionMapper<string>.Run(new string[] { "/day", "/eod" });
            Assert.IsTrue(result[0] == "day" && result[1] == "eod" && result.Count == 2);

            result = ParameterActionMapper<string>.Run(new string[] { "/eod" });
            Assert.IsTrue(result[0] == "eod" && result.Count == 1);

            result = ParameterActionMapper<string>.Run(new string[] { "/funda" });
            Assert.IsTrue(result[0] == "funda" && result.Count == 1);

            result = ParameterActionMapper<string>.Run(new string[] { "/funda", "/eod" });
            Assert.IsTrue(result[0] == "eod" && result[1] == "funda" && result.Count == 2);

            result = ParameterActionMapper<string>.Run(new string[] { "/day", "/eod", "/funda" });
            Assert.IsTrue(result[0] == "day" && result[1] == "eod" && result[2] == "funda" && result.Count == 3);
        }
    }
}
