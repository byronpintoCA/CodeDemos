using AdminUI.Models.Queue;
using HL7.Reportable.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminUI.Models.HCAssignment;
using AdminUI.Common;

namespace AdminUI.DataProvider.HCAssignment
{
    public abstract class HCAssignmentDataProvider
    {
        public const String PATIENT_CENTRIC = "Patient Centric";




        public abstract List<MSH> GetMSH();

        public abstract String GetHL7Message(int msh_id);



        public virtual List<HealthConditionAssignment> GetAssignmentCodes()
        {
            return new List<HealthConditionAssignment>() { new HealthConditionAssignment() { HCAType = HealthConditionAssignment.HCATypeKind.PatientCentric, ChildName = PATIENT_CENTRIC } };
        }

        public abstract bool Save(string Username, long mSH_ID, String Note, List<ChangeSetData> changeSet, out string errorMessage);

    }
}