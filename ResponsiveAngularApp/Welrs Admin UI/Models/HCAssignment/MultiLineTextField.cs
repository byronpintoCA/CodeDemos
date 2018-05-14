using AdminUI.DataProvider.HCAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminUI.Models.HCAssignment
{
    [Serializable]
    public class MultiLineTextField
    {
        public String DataField { get; set; }

        public String DataFieldLength
        {
            get
            {
                if (DataField != null)
                {
                    var length = DataField.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Length + 1 ;
                    return length.ToString();
                }
                return "1";
                
            }
        }

        public MultiLineTextField(string dataField)
        {
            DataField = MSHDataTransformation.FilterTransform(dataField);
        }

    }
}