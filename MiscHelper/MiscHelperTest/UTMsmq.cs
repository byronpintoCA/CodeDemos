using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiscHelper;

namespace MiscHelperTest
{
    [TestClass]
    public class UTMsmq
    {
        [TestMethod]
        public void Start()
        {

            //TestQueWithStrings();
            //TestQueWithObjs();
            TestLocalQue();
        }


        private static void TestQueWithStrings()
        {
            MSMQHelper.PostMessageToLocalQueue<String>("Byron", "What");
            MSMQHelper.PostMessageToLocalQueue<String>("Byron", "The");
            MSMQHelper.PostMessageToLocalQueue<String>("Byron", "Hell");

            String result = "";
            MSMQHelper.ProcessMessagesFromLocalQueue<String>("Byron", (input) =>
            {
                result += input;
                return false;
            });

            Assert.IsTrue(result == "WhatTheHell");

            result = "";
            MSMQHelper.ProcessMessagesFromLocalQueue<String>("Byron", (input) =>
            {
                result += input;
                return true;
            });

            Assert.IsTrue(result == "WhatTheHell");

            result = "";
            MSMQHelper.ProcessMessagesFromLocalQueue<String>("Byron", (input) =>
            {
                result += input;
                return true;
            });

            Assert.IsTrue(result == "");
        }

        private static void TestQueWithObjs()
        {
            MSMQHelper.PostMessageToLocalQueue<TestData>("Byron", new TestData("What"));
            MSMQHelper.PostMessageToLocalQueue<TestData>("Byron", new TestData("The"));
            MSMQHelper.PostMessageToLocalQueue<TestData>("Byron", new TestData("Hell"));

            String result = "";
            MSMQHelper.ProcessMessagesFromLocalQueue<TestData>("Byron", (input) =>
            {
                result += input.Data;
                return false;
            });

            Assert.IsTrue(result == "WhatTheHell");

            result = "";
            MSMQHelper.ProcessMessagesFromLocalQueue<TestData>("Byron", (input) =>
            {
                result += input.Data;
                return true;
            });

            Assert.IsTrue(result == "WhatTheHell");

            result = "";
            MSMQHelper.ProcessMessagesFromLocalQueue<TestData>("Byron", (input) =>
            {
                result += input.Data;
                return true;
            });

            Assert.IsTrue(result == "");
        }

        private static void TestLocalQue()
        {

            bool retVal = MSMQHelper.PostMessageToLocalQueue<String>("FAILED", "What");
            retVal = MSMQHelper.DeleteLocalQueue("FAILED");

            //Assert.IsTrue(MSMQHelper.TestLocalQueueCreation(out errorMessage) == true);
        }
    }
}
