using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using SearchService;
using System.Linq;

namespace VehicleLookupApi.Tests
{
    [TestClass]
    public class UTMiscTest
    {
        [TestMethod]
        public void MoqDemo()
        {
            var mockDependency = new Mock<Dependency>();
            mockDependency.Setup(mock => mock.Work()).Returns("Testing");

            DoSomething doIt = new DoSomething(mockDependency.Object);
            Assert.IsTrue("Testing" ==  doIt.DoWork());
        }

        [TestMethod]
        public void BtreeTest()
        {

            var allVehicles = ((VehicleDBRepository)new VehicleFactory().GetRepository("v1")).GetAll();
            var tupleList = allVehicles.Select(vh => new Tuple<String, Vehicle>(vh.Model, vh)).ToList();
            var btree = new BTree<Vehicle>(tupleList);
            var results = btree.Children("corv");
        }
    }

    public abstract class Dependency
    {
        public abstract String Work();
    }

    public class ProductionDependency : Dependency
    {
        public override string Work()
        {
            return "Production Environment : HANDS OFF";
        }
    }
    
    public class DoSomething
    {
        private Dependency _depends;

        public DoSomething(Dependency d)
        {
            _depends = d;
        }

        public String DoWork()
        {
            return _depends.Work();
        }
    }
}
