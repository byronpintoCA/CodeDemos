using SearchService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLookupHelper
{
    public static class CitySearchServiceBTree
    {
        private static bool _loaded;
        private static BTree<CityInfo> _master;

        public static void Load()
        {
            if (_loaded == false)
            {
                var cityList = Factory.GetAllCities();
                _loaded = true;
                _master = new BTree<CityInfo>();
                _master.Add( cityList , ci => ci.City );
            }

        }


        public static List<CityInfo> SearchForCities(String cityName)
        {
            //List<CityInfo> retVal = new List<CityInfo>() { new CityInfo() { City = "Not Found", Region = "Not Found" } };

            return _master.Children(cityName);

            //
        }

    }
}
