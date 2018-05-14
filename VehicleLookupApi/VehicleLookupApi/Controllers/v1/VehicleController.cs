using Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace VehicleLookupApi.Controllers
{
    /// <summary>
    /// Version  One of the Vehicle Lookup API
    /// </summary>
    [CORSHeader]
    public class VehiclesController : ApiController
    {
        /// <summary>
        /// property based Inversion of Control
        /// To support testing .. Since during testing we don't have access to HttpContext ..
        /// </summary>
        public Func<VehicleRepository> Repository { get; set; }


        public VehiclesController()
        {
            Repository = GetRepository;
        }

        protected String Version = "v1";

        protected VehicleRepository GetRepository()
        {
            var factory = (VehicleFactory)HttpContext.Current.Application[ApplicationKeys.VehicleFactory];

            return factory.GetRepository(Version);
        }

        /// <summary>
        /// Uses a Local file base Sql Server database and Entity Framework for ORM 
        /// Returns an unfiltered list of all Vehicles 
        /// </summary>
        /// <returns></returns>
        [Route("v1/Vehicles")]
        public virtual IEnumerable<Vehicle> Get()
        {
            return Repository().Get();
        }

        /// <summary>
        /// Demonstrates usage of asynchronous Calls
        /// This wraps the "GET" Method into an async call via the Vehicle Repository abstract class 
        /// Its NOT real world usage and only highlights knowledge of aysnc calls and polymorphism :)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<Vehicle> GetAsync(int id)
        {

            var result = await Repository().GetAsync(id);
            return result;

        }

        /// <summary>
        /// Get a single Vehicle based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("v1/Vehicles/{id}")]
        public virtual IHttpActionResult Get(int id)
        {
            if (ModelState.IsValid)
            {
                var result = Repository().Get(id);

                if (null == result)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            else
            {
                return BadRequest($"Validation failed ..");
            }
        }

        [Route("v1/Vehicles/Search")]
        [HttpGet]
        public virtual IHttpActionResult Search(String searchCriteria)
        {
            dynamic dynamicObj = new ExpandoObject();

            var result = Repository().SearchModel(searchCriteria);

            dynamicObj.total = result.Count;
            dynamicObj.results = result;

            return Ok(dynamicObj);
        }

        [Route("v1/Vehicles")]
        public virtual IHttpActionResult Get(int page, int limit)
        {
            if (ModelState.IsValid)
            {
                var result = Repository().Get(page, limit);

                if (null == result)
                {
                    return NotFound();
                }
                else
                {
                    dynamic dynamicObj = new ExpandoObject();
                    dynamicObj.records = result;
                    dynamicObj.total = 200; // Lets limit to 200 records

                    return Ok(dynamicObj);
                }
            }
            else
            {
                return BadRequest($"Validation failed ..");
            }
        }

        // POST: api/Vehicle
        /// <summary>
        /// Adds a new Vehicle 
        /// </summary>
        /// <param name="veh"></param>

        [Route("v1/Vehicles")]
        public virtual IHttpActionResult Post([FromBody] Vehicle veh)
        {
            if (ModelState.IsValid && veh != null)
            {

                var result = Repository().Add(veh);

                if (result <= 0)
                {
                    return Conflict();
                    //return NotFound();
                }
                else
                {
                    veh.Id = result;
                    return Ok(veh);
                }
            }
            else
            {
                return BadRequest($"Validation failed ");
            }
        }

        /// <summary>
        /// Updates a Vehicle
        /// </summary>
        /// <param name="veh"></param>
        [Route("v1/Vehicles")]
        [HttpPut]
        public virtual IHttpActionResult Put([FromBody] Vehicle veh)
        {
            if (ModelState.IsValid && veh != null)
            {
                var result = Repository().Update(veh);

                if (result == false)
                {
                    //return Conflict();
                    return NotFound();
                }
                else
                {
                    return Ok(veh);
                }
            }
            else
            {
                return BadRequest($"Validation failed ..");
            }

        }

        /// <summary>
        /// Deletes a Vehicle based on ID
        /// </summary>
        /// <param name="id"></param>
        [Route("v1/Vehicles/{id}")]
        [HttpDelete]
        public virtual IHttpActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var result = Repository().Delete(id);

                if (result == false)
                {
                    //return Conflict();
                    return NotFound();
                }
                else
                {
                    return Ok();
                }
            }
            else
            {
                return BadRequest($"Validation failed ..");
            }
        }

    }

}
