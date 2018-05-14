using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Reuseable
{
    public static class WeatherHelper
    {
        private static string _severPath = "forecast.weather.gov/MapClick.php?";

        //public WeatherHelper(String serverPath)
        //{
        //    _severPath = serverPath;
        //}

        public static async Task<String> GetWeatherData(decimal latitude, decimal longitude)
        {
            String retVal = "";
            String url = $"https://{_severPath}lat={latitude}&lon={longitude}&FcstType=xml";


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //request.Headers
           


            request.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:53.0) Gecko/20100101 Firefox/53.0";
            request.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";


            WebResponse response = await request.GetResponseAsync();
            Stream receiveStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            
            retVal = reader.ReadToEnd();
            
            response.Dispose();
            receiveStream.Dispose();
            reader.Dispose();


            return retVal.Replace("http://", "https://"); ;

        }
    }
}
