using CityLookupHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLookUpAPI.Controllers
{
    public class LookupController : Controller
    {
        public ContentResult Index()
        {
            //ViewBag.Title = "Home Page";
            
            return Content(JsonConvert.SerializeObject(new { Startup = "Successful" }), "application/json"); 
        }

        [AllowCrossSiteJson]
        public ContentResult Cities(string searchCity, string searchCountry)
        {
            String result = JsonConvert.SerializeObject(CitySearchServiceBTree.SearchForCities(searchCity));

            return Content(result, "application/json");

            
            //localhost:49492/Home/Cities?searchCity=cac&searchCountry=US
           // return 
        }

        [AllowCrossSiteJson]
        public ContentResult CitiesBTree(string searchCity, string searchCountry)
        {
            String result = JsonConvert.SerializeObject(CitySearchServiceBTree.SearchForCities(searchCity));

            return Content(result, "application/json");


            //localhost:49492/Home/CitiesBTree?searchCity=cac&searchCountry=US
            // return 
        }
    }
}
