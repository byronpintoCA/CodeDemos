using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminUI.Common
{
    public class GeneralHelper
    {
        static String _connectionString;

        public static String ConnectionString
        {
            get
            {
                if (null == _connectionString)
                {
                    _connectionString = ConfigurationManager.AppSettings["HL7ReportableDB"];

                    //_connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Development\\SCM\\WELRS\\AdminUI\\Welrs Admin UI\\App_Data\\Test.mdf\";Integrated Security=True;MultipleActiveResultSets=True;";

                    //_connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"App_Data\\Test.mdf\";Integrated Security=True;MultipleActiveResultSets=True;";
                }

                return _connectionString;
            }
            private set { }
        }
        
    }
}