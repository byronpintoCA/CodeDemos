using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleLookupApi.Tests
{

    [TestClass]
    public class UTVehicleRepositoryV2 : UTVehicleRepositoryV1
    {
        public UTVehicleRepositoryV2() : base("v2") { }

        //overide base Test Class to enable White box Unit Testing context
        [TestMethod]
        public override void GetVehicle()
        {
            base.GetVehicle();
        }


        [TestMethod]
        public override void AddVehicle()
        {
            base.AddVehicle();
        }

        [TestMethod]
        public override void UpdateVehicle()
        {
            base.UpdateVehicle();
        }
    }
}
