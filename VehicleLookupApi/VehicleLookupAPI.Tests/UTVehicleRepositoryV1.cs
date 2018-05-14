using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System.Linq;

namespace VehicleLookupApi.Tests
{
    [TestClass]
    public class UTVehicleRepositoryV1
    {
        private string _version;

        public UTVehicleRepositoryV1() : this("v1") { }
        public UTVehicleRepositoryV1(String version)
        {
            _version = version;
        }

        public VehicleRepository GetRepository()
        {
            VehicleFactory vf = new VehicleFactory();
            return vf.GetRepository(_version);
        }

        [TestMethod]
        public virtual void GetVehicle()
        {
            VehicleRepository repo = GetRepository();

            Vehicle vh = new Vehicle() { Manufacturer = "Byron", Model = "Pinto", Year = 1975 };

            var result = repo.Get();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }


        [TestMethod]
        public virtual void AddVehicle()
        {
            VehicleRepository repo = GetRepository();

            Vehicle vh = new Vehicle() { Manufacturer = "Byron", Model = "Pinto", Year = 1975 };

            int id = repo.Add(vh);

            Assert.IsNotNull(repo.Get(id));

            Assert.IsTrue(repo.Delete(id));

            Assert.IsNull(repo.Get(id));
        }

        [TestMethod]
        public virtual void UpdateVehicle()
        {

            VehicleRepository repo = GetRepository();

            Vehicle vh = new Vehicle() { Manufacturer = "Byron", Model = "Pinto", Year = 1975 };

            vh.Id = repo.Add(vh);

            vh.Manufacturer = "Shane"; vh.Model = "Garbanzo"; vh.Year = 1972;

            Assert.IsTrue(repo.Update(vh));

            var dbVeh = repo.Get(vh.Id);

            Assert.IsTrue(dbVeh.Manufacturer == "Shane");
            Assert.IsTrue(dbVeh.Model == "Garbanzo");
            Assert.IsTrue(dbVeh.Year == 1972);

            repo.Delete(vh.Id);
        }
    }
}
