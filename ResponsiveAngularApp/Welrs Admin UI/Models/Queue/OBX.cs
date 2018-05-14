using AdminUI.Common;
using HL7.Reportable.Common.Business.HL7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminUI.Models.HCAssignment;

namespace AdminUI.Models.Queue
{
    [Serializable]
    public class OBX
    {
        
        #region MultiLineTextFields
        public MultiLineTextField ResultTest { get; set; }
        public MultiLineTextField Result { get; set; }
        public MultiLineTextField ResultUnits { get; set; }
        public MultiLineTextField TestMethods { get; set; }
        public MultiLineTextField TestResultNotes { get; set; }
        
        #endregion
        
        public NodeType NodeType { get; set; }
        public string OBX_ID { get; set; }

        public string SequenceID { get; set; }


        private HealthConditionAssignment _hcAssignment;
        public HealthConditionAssignment HCAssignment
        {
            get { return _hcAssignment; }
            set { _hcAssignment = value; ShowCancel = true;  }
        }
        public bool ShowCancel { get; set; } = false;

        public static OBX GetObx(TestResult tResult , int sequenceID)
        {
            OBX obx = new OBX();

            obx.SequenceID = sequenceID.ToString();
            obx.OBX_ID = tResult.OBX_ID;
            obx.ResultTest = new MultiLineTextField(BatchHelper.ConcatCE(tResult.ObservationIdentifier));
            obx.Result = new MultiLineTextField(BatchHelper.ConcatOVRList(tResult.ObservationValues));
            obx.ResultUnits = new MultiLineTextField(BatchHelper.ConcatCE(tResult.ResultUnits));
            obx.TestMethods = new MultiLineTextField(BatchHelper.ConcatCEList(tResult.TestMethods));
            obx.TestResultNotes = new MultiLineTextField(BatchHelper.ConcatNotes(tResult.TestResultNotes));

            CodedElement ceHCC = tResult.PHRED_AssignedHCC;
            CodedElement ceHC = tResult.PHRED_AssignedHC;

            obx.HCAssignment = GetHealthCondition(ceHCC , ceHC);
            if (obx.HCAssignment.ChildName =="")
            {
                obx.ShowCancel = false;
            }

            return obx;
        }

        public static HealthConditionAssignment GetHealthCondition(CodedElement ceHCC , CodedElement ceHC)
        {
            try
            {
               
                return new HealthConditionAssignment()
                {
                    HCAType = HealthConditionAssignment.HCATypeKind.HealthCode,
                    ChildKey = ceHC.Code == null ? "" : ceHC.Code,
                    ChildName = ceHC.Description == null ? "" : ceHC.Description,
                    ChildCodingSystem = ceHC.CodingSystem == null ? "" : ceHC.CodingSystem,
                    ParentKey = ceHCC.Code == null ? "" : ceHCC.Code,
                    ParentName = ceHCC.Description == null ? "" : ceHCC.Description,
                    ParentCodingSystem = ceHCC.CodingSystem == null ? "" : ceHCC.CodingSystem
                };
            }
            catch
            {
                return null;
            }
        }

        public OBX DeepCopy()
        {
            return (OBX)this.MemberwiseClone();
        }
    }

}