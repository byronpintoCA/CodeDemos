using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AdminUI.DataProvider.HCAssignment
{
    public class MSHDataTransformation
    {
        public static String FilterTransform(string inData)
        {

            if (inData ==null || inData.Length <=0)
            {
                return inData;
            }

            // Move to a config file later .. If the list grows ..
            List<Tuple<String, String>> filterList = new List<Tuple<string, string>>()
            {
                new Tuple<String,String>( "&gt;",">")
            };

            StringBuilder builder = new StringBuilder( inData);

            foreach (var item in filterList)
            {
                builder.Replace(item.Item1, item.Item2);
            }

            return builder.ToString();
        }

    }
}