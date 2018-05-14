using AdminUI.DataProvider.HCAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminUI.DataProvider
{
    public class HCAssignmentDataProviderFactory
    {
        static HCAssignmentDataProvider _instance;

        public static HCAssignmentDataProvider GetProvider()
        {
            if (null == _instance)
            {   //_instance = new HCAssignmentRealDataProvider();
                _instance = new HCAssignmentTestDataProvider();
            }
            return _instance;
        }

        public static HCAssignmentDataProvider GetLiveProvider()
        {
            return new HCAssignmentRealDataProvider();
        }

    }

}