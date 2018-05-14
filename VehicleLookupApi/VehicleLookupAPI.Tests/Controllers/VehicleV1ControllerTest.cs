using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VehicleLookupApi;
using VehicleLookupApi.Controllers;
using Services;
using System.Web.Http.Controllers;
using Moq;

namespace VehicleLookupApi.Tests.Controllers
{
    [TestClass]
    public class VehicleV1ControllerTest
    {
        private string _version;

        public VehicleV1ControllerTest() : this("v1") { }
        public VehicleV1ControllerTest(String version)
        {
            _version = version;
        }

        [TestMethod]
        public void Get()
        {
            // Arrange
            VehiclesController controller = new VehiclesController() { Repository = () => new VehicleFactory().GetRepository(_version) };

            // Act
            IEnumerable<Vehicle> result = controller.Get();

            // Could perform model validation etc .
            // However most of the testing is done in UTVehicleRespositoryV1 , UTNoSql

          
        }

        [TestMethod]
        public void GetById()
        {

            // Arrange
            VehiclesController controller = new VehiclesController() { Repository = () => new VehicleFactory().GetRepository(_version) };
            // Act
            var result =  controller.Get(10000);

            // Assert
           // Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            VehiclesController controller = new VehiclesController() { Repository = () => new VehicleFactory().GetRepository(_version) };

            // Act
            //controller.Post("value");

            // Assert
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            VehiclesController controller = new VehiclesController() { Repository = () => new VehicleFactory().GetRepository(_version) };

            // Act
            //controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            VehiclesController controller = new VehiclesController() { Repository = () => new VehicleFactory().GetRepository("v1") };

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
