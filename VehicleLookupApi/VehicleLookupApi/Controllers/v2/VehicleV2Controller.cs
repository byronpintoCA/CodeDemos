using Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace VehicleLookupApi.Controllers
{
    /// <summary>
    /// Version  Two of the Vehicle Lookup API
    /// </summary>
    public class VehiclesV2Controller : VehiclesController
    {
        public VehiclesV2Controller()
        {
            Version = "v2";
        }

        /// <summary>
        /// Version 2 - Uses No Sql based Azure Table Storage
        /// Returns an unfiltered list of all Vehicles 
        /// </summary>
        /// <returns></returns>
        [Route("v2/Vehicles")]
        public override IEnumerable<Vehicle> Get()
        {
            return base.Get();
        }

        /// <summary>
        /// Demonstrates usage of asynchronous Calls
        /// This wraps the "GET" Method into an async call via the Vehicle Repository abstract class 
        /// Its NOT real world usage and only highlights knowledge of aysnc calls and polymorphism :)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override async Task<Vehicle> GetAsync(int id)
        {
            var result = await Repository().GetAsync(id);
            return result;
        }

        [Route("v2/Vehicles/Search")]
        [HttpGet]
        public override IHttpActionResult Search(String searchCriteria)
        {
            return base.Search(searchCriteria);
        }
        /// <summary>
        /// Get a single Vehicle based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("v2/Vehicles/{id}")]
        public override IHttpActionResult Get(int id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// Pagination Support for client side data grids
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Route("v2/Vehicles")]
        public override IHttpActionResult Get(int page, int limit)
        {
            return base.Get(page, limit);
        }

        /// <summary>
        /// Adds a new Vehicle 
        /// </summary>
        /// <param name="veh"></param>
        [Route("v2/Vehicles")]
        public override IHttpActionResult Post([FromBody] Vehicle veh)
        {
            return base.Post(veh);
        }

        /// <summary>
        /// Updates a Vehicle
        /// </summary>
        /// <param name="veh"></param>
        [Route("v2/Vehicles")]
        [HttpPut]
        public override IHttpActionResult Put([FromBody] Vehicle veh)
        {
            return base.Put(veh);
        }

        /// <summary>
        /// Deletes a Vehicle based on ID
        /// </summary>
        /// <param name="id"></param>
        [Route("v2/Vehicles/{id}")]
        [HttpDelete]
        public override IHttpActionResult Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
