using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLookupHelper
{
    public class CityRepositoryDebug : CityRepository
    {
        public override List<CityInfo> GetAllCities()
        {
            List<CityInfo> retList = new List<CityInfo>();

            SqlConnection conn = new SqlConnection("Data Source=CHUI;Initial Catalog=Test;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select * from worldcities where country ='us'", conn);
            conn.Open();

            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                CityInfo city = new CityInfo();
                // get the results of each column

                city.City = (string)rdr["City"];
                city.Region = (string)rdr["Region"];
                city.Country = (string)rdr["Country"];
                city.Latitude = Convert.ToDouble((string)rdr["Latitude"]);
                city.Longitude = Convert.ToDouble((string)rdr["Longitude"]);

                retList.Add(city);
            }


            rdr.Close();

            return retList;
        }
    }
}
