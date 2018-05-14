using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HL7.Reportable.DAL;
using AdminUI.Common;
using static HL7.Reportable.Common.Common;
using AdminUI.Models.Queue;
using AdminUI.Models.HCAssignment;
using HL7.Reportable.Common.Business.HL7;

namespace AdminUI.DataProvider.HCAssignment
{
    public class HCAssignmentRealDataProvider : HCAssignmentDataProvider
    {
        public override List<MSH> GetMSH()
        {
            const int NO_OF_RECORDS = 25;

            using (ExportQueueTable exp = new ExportQueueTable(GeneralHelper.ConnectionString, 0, ProcessStageFilterEnum.Lookup, ProcessStateFilterEnum.Failed, "ResolveHealthConditions"))
            {
                // Return 25 for now 
                return BatchHelper.LoadBatches(exp.Items_ExportQueue.Take(NO_OF_RECORDS).ToList());
            }
        }

        public override string GetHL7Message(int msh_id)
        {
            HL7Message hl7Message = new HL7Message(GeneralHelper.ConnectionString, Convert.ToInt64(msh_id));
            return  hl7Message.ToString();
        }

        public override List<HealthConditionAssignment> GetAssignmentCodes()
        {
            List<HealthConditionAssignment> lstHealth = new List<HealthConditionAssignment>();
            TRCClient trc = new TRCClient();
            var result = trc.GetHealthConditionCatagories();

            foreach (var parent in result.HealthConditionCatagories)
            {
                foreach (var child in parent.HealthConditions)
                {
                    lstHealth.Add(new HealthConditionAssignment()
                    {
                        HCAType = HealthConditionAssignment.HCATypeKind.HealthCode,
                        ChildKey = child.Key,
                        ChildName = child.Name,
                        ChildCodingSystem = child.CodingSystem,
                        ParentKey = parent.Key,
                        ParentName = parent.Name,
                        ParentCodingSystem = parent.CodingSystem
                    });
                }
            }
            var filteredList = lstHealth.GroupBy(hca => hca.ChildName).Select(g => g.First()).ToList();
            return filteredList.Union(base.GetAssignmentCodes()).ToList();
        }

        public override bool Save(string Username, long MSH_ID, String Note, List<ChangeSetData> changeSet, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                using (ExportQueueTable exp = new ExportQueueTable(GeneralHelper.ConnectionString, 0, ProcessStageFilterEnum.Lookup, ProcessStateFilterEnum.Failed, "ResolveHealthConditions"))
                {
                    var update_msh_id = MSH_ID;
                    var exportQueueItem = exp.Items_ExportQueue.Where(eq => eq.MSH_ID == update_msh_id).FirstOrDefault();

                    if (exportQueueItem == null)
                    { // Record was already saved by another user . So lets just ignore these changes 
                        errorMessage = " Already saved by another user";
                        return true;
                    }

                    var batchItem = HL7.Reportable.Common.Common.DeserializeXmlToObject<HL7.Reportable.Common.Business.HL7.ExportBatch>(exportQueueItem.Process_Message);
                    var labReport = batchItem.LabReports.Where(lr => lr.MSH_ID == update_msh_id.ToString()).FirstOrDefault();

                    labReport.PHRED_Variables.PHRED_AssignmentNote = Note;

                    foreach (var item in changeSet)
                    {
                        UpdateReport(Username, labReport, item);
                    }
                    // Update the Status
                    exportQueueItem.Process_State = ProcessStateEnum.Pending;
                    exportQueueItem.Save();
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private static void UpdateReport(string Username, HL7.Reportable.Common.Business.HL7.LabReport labReport , ChangeSetData change)
        {

            var obr = labReport.TestRequests.Where(tRequest => tRequest.OBR_ID == change.OBR_ID).FirstOrDefault();

            CodedElement ceHCC = new CodedElement() { Code = change.HealthCondition.ParentKey, Description = change.HealthCondition.ParentName, CodingSystem = change.HealthCondition.ParentCodingSystem };
            CodedElement ceHC = new CodedElement() { Code = change.HealthCondition.ChildKey, Description = change.HealthCondition.ChildName, CodingSystem = change.HealthCondition.ChildCodingSystem };

            if (change.OBX_ID == "NONE")
            { // update the OBR
                obr.PHRED_AssignedHCManually = true;
                obr.PHRED_AssignerUserKey = Username;
                //obx.PHRED_AssignmentNote = "Another Note";
                obr.PHRED_AssignedDtTm = DateTime.Now.ToString();
                obr.PHRED_AssignedHCC = ceHCC;
                obr.PHRED_AssignedHC = ceHC;

            }
            else
            {
                var obx = obr.TestResults.Where(tResult => tResult.OBX_ID == change.OBX_ID).FirstOrDefault();

                obx.PHRED_AssignedHCManually = true;
                obx.PHRED_AssignerUserKey = Username;
                //obx.PHRED_AssignmentNote = "Another Note";
                obx.PHRED_AssignedDtTm = DateTime.Now.ToString();
                obx.PHRED_AssignedHCC = ceHCC;
                obx.PHRED_AssignedHC = ceHC;
            }

        }
    }
}