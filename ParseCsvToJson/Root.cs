using System;
using System.Collections;
using System.Collections.Generic;

namespace ParseCSVToJson
{
    internal class Root
    {

        public List<FileRecord> FileRecords { get; set; }

        public Root(string fileContents)
        {
            string[] lines = fileContents.Split( new char[] { '\n' } , StringSplitOptions.RemoveEmptyEntries);

            List<List<string[]>> parcelCollection = new List<List<string[]>>(); // Can support multiple File Records if needed

            List<string[]> parcel = null;

            foreach (var item in lines)
            {
                string[] col = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string collumnDescriptor = Clean(col[0]);

                switch (collumnDescriptor)
                {
                    case "F":
                        parcel = new List<string[]>();
                        parcel.Add(col);
                        break;
                    case "E":
                        parcel.Add(col);
                        parcelCollection.Add(parcel);
                        break;
                    default:
                        parcel.Add(col);
                        break;
                }

            }

            FileRecords = new List<FileRecord>();

            foreach (var item in parcelCollection)
            {
                FileRecords.Add(new FileRecord(item));
            }


        }

        internal static string Clean(string col)
        {
            return col.Trim().ToUpper().Replace("\"", "");
        }

        internal static Dictionary<string, string> FillDynamicData(string[] input)
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();

            int count = 1;

            foreach (var item in input)
            {
                retVal.Add(count.ToString(), Clean(item));
                count++;
            }


            return retVal;

        }
    }
}