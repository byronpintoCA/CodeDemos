using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CityLookupHelper;
using System.Collections.Generic;
using System.Linq;

namespace CitySearchTests
{
    [TestClass]
    public class CitySearchTest
    {
        [TestMethod]
        public void SQlRepositoryDataRetrieval()
        {
            CityRepositoryDebug sqr = new CityRepositoryDebug();
            var a =  sqr.GetAllCities();
        }


        [TestMethod]
        public void DictionaryTest()
        {
            CitySearchService.Load();
        }

    }
}
