using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class VehicleNoSqlRepository : VehicleRepository
    {
        public const string AppName = "VehicleNoSqlRepository";
        public const string MaxIdKey = "MaxIdKey";

        private static CloudTable _Table;

        static VehicleNoSqlRepository()
        {
            _Table = GetTable();
        }

        private static VehicleTE GetVTEByID(int id)
        {
            var queryResult = _Table.ExecuteQuery(new TableQuery<VehicleTE>()
                                    .Where(TableQuery.GenerateFilterConditionForInt("Id", QueryComparisons.Equal, id)));

            return queryResult.FirstOrDefault();
        }

        private static CloudTable GetTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("vehicleLookup");
            //table.CreateIfNotExists();
            return table;
        }

        public VehicleNoSqlRepository()
        {

        }
        
        public override IList<Vehicle> Get()
        {
            
            TableContinuationToken token = null;
            var lstVehiclesTE = new List<VehicleTE>();

            do
            {
                var queryResult = _Table.ExecuteQuerySegmented(new TableQuery<VehicleTE>(), token);
                lstVehiclesTE.AddRange(queryResult.Results);
                //token = queryResult.ContinuationToken; // For Now lets just return 1000 records and not get all the data
            } while (token != null);


            List<Vehicle> lst = lstVehiclesTE.Select(vh => new Vehicle() { Id = vh.Id, Manufacturer = vh.Manufacturer, Model = vh.Model, Year = vh.Year }).ToList();

            return lst;

        }

        public override Vehicle Get(int id)
        {
            Vehicle retVal = null;
            VehicleTE veh = GetVTEByID(id);

            if (null != veh)
            {
                retVal = Mapper.IMap.Map<Vehicle>(veh);
            }

            return retVal;
        }
        public override IEnumerable<Vehicle> Get(int page, int size)
        {
            int skip = page == 1 ? 0 : (page - 1) * size + 1;

            var lstVehiclesTE = new List<VehicleTE>();

            var queryResult = _Table.ExecuteQuerySegmented(new TableQuery<VehicleTE>(), null);
            lstVehiclesTE.AddRange(queryResult.Results.Skip(skip).Take(size));

            List<Vehicle> lst = lstVehiclesTE.Select(vh => new Vehicle() { Id = vh.Id, Manufacturer = vh.Manufacturer, Model = vh.Model, Year = vh.Year }).ToList();
            return lst;
        }

        public override int Add(Vehicle inVehicle)
        {
            // This is just a demo And doesn't make much sense.. NoSql Tables typically should not have an Identity column .. They use Partition + Row Key as a composite... But in keeping with the Interface .. I'm simulating
            // Also The Make and Manufacturer should not accept special characters as they are part of the composite key
            
            VehicleTE vehInsert = Mapper.IMap.Map<VehicleTE>(inVehicle);

            /// if Make and Model exists  then delete first
            VehicleTE existing = (VehicleTE)  _Table.Execute(TableOperation.Retrieve<VehicleTE>(inVehicle.Manufacturer,inVehicle.Model)).Result;
            if (existing != null)
            {
                vehInsert.Id = existing.Id;
                _Table.Execute(TableOperation.Delete(existing));
            }
            else
            {
                int  newID = Convert.ToInt32(PersistedKeyValueHelper.GetValue(VehicleNoSqlRepository.AppName, VehicleNoSqlRepository.MaxIdKey));
                newID++;
                PersistedKeyValueHelper.SaveValue(VehicleNoSqlRepository.AppName, VehicleNoSqlRepository.MaxIdKey, newID.ToString());
                vehInsert.Id = newID;
            }
            _Table.Execute(TableOperation.Insert(vehInsert));
            
            return vehInsert.Id;
        }

        public override bool Delete(int id)
        {
            bool retVal = false;
            VehicleTE vte = GetVTEByID(id);
            ///vte = (VehicleTE)_Table.Execute(TableOperation.Retrieve<VehicleTE>(vte.Manufacturer, vte.Model)).Result;
            if (null != vte)
            {
                try
                {
                    _Table.Execute(TableOperation.Delete(vte));
                    retVal = true;
                }
                catch( Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    // Object does not exist
                }
            }

            return retVal;
        }


        public override bool Update(Vehicle vh)
        {
            bool retVal = false;
            VehicleTE existing = GetVTEByID(vh.Id);

            
            if (null != existing)
            {
                try
                {
                    VehicleTE insertVte = Mapper.IMap.Map<VehicleTE>(vh);
                    insertVte.Id = existing.Id;
                    _Table.Execute(TableOperation.Delete(existing));
                    _Table.Execute(TableOperation.Insert(insertVte));
                    
                    retVal = true;
                }
                catch
                {
                    // Object does not exist
                }
            }

            return retVal;
        }

        public override IList<string> SearchModel(string search)
        {  
            return new List<String>() { "Not Supported" };
        }
    }
}
