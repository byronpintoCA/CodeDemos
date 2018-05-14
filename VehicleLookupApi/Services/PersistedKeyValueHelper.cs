using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class PersistedKeyValueHelper
    {
        private static CloudTable _Table;

        private class AppKeyValues : TableEntity
        {
            private string _appName;
            private string _key;

            public String AppName { get { return _appName; } set { _appName = value; PartitionKey = value; } }
            public String Key { get { return _key; } set { _key = value; RowKey = value; } }

            public String Value { get; set; }
        }

        static PersistedKeyValueHelper()
        {
            _Table = GetTable();
        }

        public static void CreateTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("AppKeyValues");

            table.CreateIfNotExists();
        }

        private static CloudTable GetTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("AppKeyValues");

            return table;
        }

        public static String GetValue(String appName, String key)
        {
            String retVal = "";
            
            var result = _Table.Execute(TableOperation.Retrieve<AppKeyValues>(appName, key)).Result;

            if (result != null)
            {
                retVal = ((AppKeyValues)result).Value;
            }

            return retVal;

        }

        public static void SaveValue(String appName, String key , String value)
        {
            AppKeyValues kv = new AppKeyValues() { AppName = appName, Key = key , Value = value };

            _Table.Execute(TableOperation.InsertOrReplace(kv));

        }

    }
}
