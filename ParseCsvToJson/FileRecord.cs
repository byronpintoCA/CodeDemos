using System.Collections.Generic;
using System.Diagnostics;

namespace ParseCSVToJson
{
    public class FileRecord
    {

        public Dictionary<string,string> HeaderFields { get; set; }

        public Dictionary<string, string> FooterFields { get; set; }

        public List<Order> Orders { get; set; }

        List<List<string[]>> parcelCollection = new List<List<string[]>>(); // Can support multiple File Records if needed

        List<string[]> parcel = null;

        public FileRecord(List<string[]> lst)
        {
            int NoOfTimes = lst.Count - 1;
            HeaderFields = Root.FillDynamicData(lst[0]);
            FooterFields = Root.FillDynamicData(lst[ lst.Count- 1 ]);

            for (int i = 1; i < NoOfTimes; i++)
            {
                string collumnDescriptor = Root.Clean(lst[i][0]);

                switch (collumnDescriptor)
                {
                    case "O":
                        parcel = new List<string[]>();
                        parcelCollection.Add(parcel);
                        parcel.Add(lst[i]);
                        break;
                    default:
                        parcel.Add(lst[i]);
                        break;
                }

            }

            Orders = new List<Order>();

            foreach (var item in parcelCollection)
            {
                Orders.Add(new Order(item));
            }

        }
    }
}