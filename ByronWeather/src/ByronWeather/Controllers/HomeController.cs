//using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reuseable;
using Newtonsoft.Json;
using System.Xml;
using System.Net.Http;
using System.Threading.Tasks;

namespace ByronWeather.Controllers
{
    public class HomeController : Controller
    {

        public  IActionResult Index()
        {
            //return View("TestResponsiveUI");

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Usage of an aync controller
        /// </summary>

        public async Task<IActionResult> WeatherFromCoordinates(decimal latitude, decimal longitude)
        {
            //Task<String> task = WeatherHelper.GetWeatherData(40.78158, -73.96648);
            //string xml = task.Result;

            string xml = await WeatherHelper.GetWeatherData(latitude, longitude);

            XmlDocument doc = new XmlDocument();    
            doc.LoadXml(xml);

            string jsonString  = JsonConvert.SerializeXmlNode(doc.LastChild);

            //dynamic json = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(doc.LastChild.InnerXml);
            

            return Content(jsonString, "application/json");

            //http://localhost:30897/Home/WeatherFromCoordinates?latitude=47.037874099999996&longitude=-122.9006951
        }


        public async Task<ContentResult> Cities(string searchCity, string searchCountry)
        {
            string result = "";
            //ViewBag.Title = "Home Page";
            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"http://citylookupapi.azurewebsites.net/lookup/cities?searchCity={searchCity}&searchCountry={searchCountry}";
                result = await httpClient.GetStringAsync(url);
                return Content(result, "application/json");
            }

            
        }

        //// GET api/Student  
        //public IEnumerable<CityInfo> Cities(string searchCity , string searchCountry)
        //{
        //    return new List<CityInfo>() {  new CityInfo {City="Olympia",State="WA", Country="US" },
        //                                new CityInfo {City="Bellevue",State="WA", Country="US" },
        //                                new CityInfo {City="Richardson",State="TX", Country="US" },
        //                                new CityInfo {City="Addison",State="TX", Country="US"}
        //    };
        //}




    }
}
