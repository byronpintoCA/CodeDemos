using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLookupHelper
{
    public class CityInfo
    {
        static TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        private bool _appliedAlready;

        public String City { get; set; }
        public String Region { get; set; }
        public String Country { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public void ApplyCityNameTransform()
        {
            if (_appliedAlready== false)
            {
                _appliedAlready = true;

                City = textInfo.ToTitleCase(City);
                
            }
            
        }
    }
}
