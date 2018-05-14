using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using System.Data.SqlClient;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace VehicleLookupApi.Tests
{
    [TestClass]
    public class UTNoSql
    {
        public List<VehicleTE> GetSqlTestData()
        {
            List<VehicleTE> retLst = new List<VehicleTE>();
            string sqlCon = @"data source=(LocalDB)\MSSQLLocalDB;attachdbfilename=|DataDirectory|\TestDB.mdf;integrated security=True;MultipleActiveResultSets=True;Connect Timeout=3";
            SqlConnection conn = new SqlConnection(sqlCon);
            conn.Open();
            SqlCommand command = new SqlCommand("select * from VehicleModelYear", conn);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    VehicleTE vte = new VehicleTE((int)reader[0], (int)reader[1], (String)reader[2], (String)reader[3]);
                    retLst.Add(vte);
                }

            }
            catch { }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return retLst;
        }

        [TestMethod]
        public void TestGetSqlData()
        {
            Assert.IsTrue(GetSqlTestData().Count > 0);
        }

        [TestMethod]
        public void LoadDataToCloudTable()
        {
            CloudTable table = GetTable();

            var testData = GetSqlTestData();

            foreach (var item in testData)
            {
                TableOperation insertOperation = TableOperation.InsertOrReplace(item);

                try
                {
                    table.Execute(insertOperation);
                    //Debug.WriteLine($"Added - {item.Manufacturer} - {item.Model}");
                }
                catch ( Exception ex)
                {
                    Debug.WriteLine($"Failed - {ex.Message} - {item.Manufacturer} - {item.Model}");
                }
            }
        }


        private static CloudTable GetTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("vehicleLookup");
            //table.CreateIfNotExists();
            //table.Delete();
            //table.CreateIfNotExists();
            return table;
        }

        [TestMethod]
        public void ReadAllData()
        {
            CloudTable table = GetTable();

            TableContinuationToken token = null;
            var lstVehicles = new List<VehicleTE>();

            do
            {
                var queryResult = table.ExecuteQuerySegmented(new TableQuery<VehicleTE>(), token);
                lstVehicles.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

        }

        [TestMethod]
        public void UpdateData()
        {
            int id = -1;
            CloudTable table = GetTable();

            VehicleTE vte = new VehicleTE(id, id, "Ding", "Dong");

            var result = table.Execute(TableOperation.InsertOrReplace(vte));


            TableQuery<VehicleTE> searchQuery = new TableQuery<VehicleTE>()
                .Where(TableQuery.GenerateFilterConditionForInt("Id", QueryComparisons.Equal, id));

            var queryResult = table.ExecuteQuery(searchQuery);

            Assert.IsNotNull(queryResult.FirstOrDefault());

            result = table.Execute(TableOperation.Delete(vte));

            vte = new VehicleTE(id, id, "John", "Doe");

            result = table.Execute(TableOperation.Insert(vte));

            result = table.Execute(TableOperation.Delete(vte));

            //TableOperation insertOperation = TableOperation.InsertOrReplace(test);

        }

        [TestMethod]
        public void MaxValueSetup()
        {
            //PersistedKeyValueHelper.CreateTable();

            String result = PersistedKeyValueHelper.GetValue(VehicleNoSqlRepository.AppName, VehicleNoSqlRepository.MaxIdKey);

            if (result == "")
            {
                PersistedKeyValueHelper.SaveValue(VehicleNoSqlRepository.AppName, VehicleNoSqlRepository.MaxIdKey, "18000");
            }
            result = PersistedKeyValueHelper.GetValue(VehicleNoSqlRepository.AppName, VehicleNoSqlRepository.MaxIdKey);

            Assert.IsTrue(result == "18000");
        }



    }
}
