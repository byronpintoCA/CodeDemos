using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLookupHelper
{
    public static class Factory
    {
        public static List<CityInfo> GetAllCities()
        {

#if DEBUG
            return new CityRepositoryDebug().GetAllCities();

#else
            return new CityRepositoryRelease().GetAllCities();
#endif

        }

    }

}
