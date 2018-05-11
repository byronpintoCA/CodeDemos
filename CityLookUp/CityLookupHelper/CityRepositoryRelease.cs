using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CityLookupHelper
{
    public class CityRepositoryRelease : CityRepository
    {
        public override List<CityInfo> GetAllCities()
        {
            List<CityInfo> list = new List<CityInfo>();
            string line;
            string path = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)).AbsolutePath+ "/uscityLocationInfo.txt";

            StreamReader file =new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                list.Add(Parse(line));
            }

            file.Close();
            
            return list;
        }

        private CityInfo Parse(string line)
        {
            CityInfo ret = new CityInfo();
            string[] data = line.Split(new char[] { ',' });
            ret.Country = data[0];
            ret.City = data[1];
            ret.Region = data[2];
            ret.Latitude = Convert.ToDouble( data[3]);
            ret.Longitude = Convert.ToDouble(data[4]);

            return ret;
        }
    }
}
