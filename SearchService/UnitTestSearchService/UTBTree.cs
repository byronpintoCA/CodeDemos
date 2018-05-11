using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchService;
using System.Collections.Generic;

namespace UnitTestSearchService
{
    [TestClass]
    public class UTBTree
    {

        [TestMethod]
        public void Testadd()
        {
            BTree<String> root = new BTree<string>();

            string toAdd = "a";

            root.Add(toAdd, toAdd);
            toAdd = "b";
            root.Add(toAdd, toAdd);
            toAdd = "ba";
            root.Add(toAdd, toAdd);
            toAdd = "bat";
            root.Add(toAdd, toAdd);
            toAdd = "bac";
            root.Add(toAdd, toAdd);
            toAdd = "back";
            root.Add(toAdd, toAdd);

            Assert.IsTrue(CheckIfExists(root,"back"));
            Assert.IsTrue(CheckIfExists(root, "bad"));


        }

        private static bool CheckIfExists(BTree<string> root, string searchString  )
        {
            List<string> result;
            if (false == root.Seek(searchString, out result))
            {
                return false;
            }
            else
            {
                return result[0] == searchString;
            }

        }

        [TestMethod]
        public void TestRemove()
        {
            BTree<String> root = new BTree<string>();

            string toAdd ="ba";
            root.Add(toAdd, toAdd);
            toAdd = "bat";
            root.Add(toAdd, toAdd);
            toAdd = "bac";
            root.Add(toAdd, toAdd);
            toAdd = "back";
            root.Add(toAdd, toAdd);

            Assert.IsTrue(root.Remove("back"));
            Assert.IsTrue(CheckIfExists(root, "bat"));
            Assert.IsTrue(CheckIfExists(root, "bac"));

            Assert.IsTrue(root.Remove("ba"));
            Assert.IsTrue(CheckIfExists(root, "bat"));
            Assert.IsTrue(CheckIfExists(root, "bac"));

            Assert.IsFalse(root.Remove("bad"));
            Assert.IsTrue(CheckIfExists(root, "bat"));
            Assert.IsTrue(CheckIfExists(root, "bac"));

            Assert.IsTrue(false == CheckIfExists(root, "back"));
            Assert.IsTrue(false == CheckIfExists(root, "ba"));
            Assert.IsTrue(false == CheckIfExists(root, "bad"));

            Assert.IsTrue(CheckIfExists(root, "bat"));
            Assert.IsTrue(CheckIfExists(root, "bac"));


        }
    }
}
