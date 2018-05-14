using HL7.Reportable.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HL7.Reportable.Common.Business.HL7;
using static HL7.Reportable.Common.Common;
using System.Text;
using AdminUI.Models.HCAssignment;

namespace AdminUI.Models.Queue
{
    [Serializable]
    public class MSH
    {
        public string SequenceID { get; set; }
        public string DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MessageControlID { get; set; }
        public string MessageDateTime { get; set; }
        public string MiddleNameOrInitial { get; set; }
        public string MSH_ID { get; set; }
        public string PID_ID { get; set; }
        public string Sender { get; set; }
        public List<OBR> ObrList { get; set; }
        public string HL7 { get; set; }

        public string Note { get; set; }
        public string FileName { get; set; }
        public NodeType NodeType { get; set; }

        public string HCAssignment { get;  set; }

        public bool ShowCancel { get; set; } = false;

        public MSH DeepCopy ()
        {
            var retVal = (MSH)this.MemberwiseClone();
            retVal.ObrList = new List<OBR>();

            foreach (var item in ObrList)
            {
                retVal.ObrList.Add(item.DeepCopy());
            }
            return retVal; 

        }
    }

}