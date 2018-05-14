using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class VehicleDBRepository : VehicleRepository
    {
        public override IList<Vehicle> Get()
        {

            VehicleDBEntities db = new VehicleDBEntities();

            try
            {
                var vehQuery = from veh in db.VehicleModelYear
                               select veh;

                var ret = vehQuery.Select(vh => new Vehicle() { Id = vh.id, Manufacturer = vh.make, Model = vh.model, Year = vh.year }).Take(1000).ToList();
                //Lets only return 1000 records 
                return ret;

            }
            finally
            {
                db.Dispose();
            }

        }

        public IList<Vehicle> GetAll()
        {

            VehicleDBEntities db = new VehicleDBEntities();

            try
            {
                var vehQuery = from veh in db.VehicleModelYear
                               select veh;

                var ret = vehQuery.Select(vh => new Vehicle() { Id = vh.id, Manufacturer = vh.make, Model = vh.model, Year = vh.year }).ToList();
                //Lets only return 1000 records 
                return ret;

            }
            finally
            {
                db.Dispose();
            }

        }

        public override Vehicle Get(int id)
        {
            VehicleDBEntities db = new VehicleDBEntities();

            try
            {
                var vehQuery = from veh in db.VehicleModelYear
                               where veh.id == id
                               select veh;

                var vehicle = vehQuery.Select(vh => new Vehicle() { Id = vh.id, Manufacturer = vh.make, Model = vh.model, Year = vh.year }).ToList().FirstOrDefault();

                return vehicle;
            }
            finally
            {
                db.Dispose();
            }
        }

        public override IEnumerable<Vehicle> Get(int page, int size)
        {
            int skip = page==1 ? 0 : (page -1 ) * size + 1;

            VehicleDBEntities db = new VehicleDBEntities();

            try
            {
                var vehQuery = db.VehicleModelYear.OrderBy(vmy => vmy.id ).Skip(skip).Take(size);

                var ret = vehQuery.Select(vh => new Vehicle() { Id = vh.id, Manufacturer = vh.make, Model = vh.model, Year = vh.year }).ToList();
                //Lets only return 1000 records 
                return ret;

            }
            finally
            {
                db.Dispose();
            }
        }

        public override int Add(Vehicle vh)
        {
            var vmy = Mapper.IMap.Map<VehicleModelYear>(vh);

            using (var dbCtx = new VehicleDBEntities())
            {
                dbCtx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[VehicleModelYear] ON");
                dbCtx.VehicleModelYear.Add(vmy);

                dbCtx.SaveChanges();

                dbCtx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[VehicleModelYear] OFF");
            }

            return vmy.id;
        }

        public override bool Delete(int id)
        {
            using (var dbCtx = new VehicleDBEntities())
            {

                var vehicle = (from veh in dbCtx.VehicleModelYear
                               where veh.id == id
                               select veh).FirstOrDefault();

                if (vehicle == null)
                {
                    return false;
                }
                else
                {
                    dbCtx.VehicleModelYear.Remove(vehicle);
                    dbCtx.SaveChanges();
                    return true;
                }
            }
        }

        public override bool Update(Vehicle vh)
        {
            using (var dbCtx = new VehicleDBEntities())
            {

                var vehicle = (from veh in dbCtx.VehicleModelYear
                               where veh.id == vh.Id
                               select veh).FirstOrDefault();

                if (vehicle == null)
                {
                    return false;
                }
                else
                {
                    vehicle.make = vh.Manufacturer;
                    vehicle.model = vh.Model ;
                    vehicle.year = vh.Year;
                    dbCtx.SaveChanges();
                    return true;
                }
            }
        }

        public override IList<string> SearchModel(string search)
        {
            string criteria = search.Length > 9 ? search.Substring(0, 10) : search;
            // Only allow search for 10 characters to prevent sql injection
            string sql = $"SELECT Model FROM dbo.VehicleModelYear where Model Like '{criteria}%'";
            using (var dbCtx = new VehicleDBEntities())
            {
                var searchRet = dbCtx.Database.SqlQuery<string>(sql).ToList();

                if (searchRet == null )
                {
                    return new List<String>();
                }
                else
                {
                    
                    return searchRet;
                }
            }
        }
    }
}
