using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminUI.Models.HCAssignment;
using AdminUI.Common;

namespace AdminUI.Models.Queue
{
    [Serializable]
    public class OBR
    {

        #region MultiLineTextFields

        public MultiLineTextField OrderedTest { get; set; }

        public MultiLineTextField SpecimenSourceSite { get; set; }
        public MultiLineTextField ParentResult { get; set; }
        public MultiLineTextField ReasonsForStudy { get; set; }
        public MultiLineTextField RelevantClinicInfo { get; set; }

        public MultiLineTextField Notes { get; set; }

        #endregion


        public string OBR_ID { get; set; }

        public List<OBX> ObxList { get; set; }
        public string TestRequestIdentifier { get; set; }
        public string SequenceID { get; set; }
        public NodeType NodeType { get; set; }

        private HealthConditionAssignment _hcAssignment;
        public HealthConditionAssignment HCAssignment
        {
            get { return _hcAssignment; }
            set { _hcAssignment = value; ShowCancel = true; }
        }
        public bool ShowCancel { get; set; } = false;

        public bool ShowOBRCancelAll { get; set; } = false;

        public OBR DeepCopy()
        {
            var retVal = (OBR)this.MemberwiseClone();
            retVal.ObxList = new List<OBX>();

            foreach (var item in ObxList)
            {
                retVal.ObxList.Add(item.DeepCopy());
            }

            return retVal;
        }
    }


}