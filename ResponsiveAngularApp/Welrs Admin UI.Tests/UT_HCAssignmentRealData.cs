using AdminUI.Common;
using AdminUI.DataProvider.HCAssignment;
using HL7.Reportable.Common.Business.HL7;
using HL7.Reportable.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HL7.Reportable.Common.Common;

namespace Welrs_Admin_UI.Tests
{
    [TestClass]
    public class UT_HCAssignmentRealData
    {
        //HCAssignmentRealDataProvider hc = new HCAssignmentRealDataProvider();

        //var res = hc.GetMSH();
        //Assert.IsTrue( res.Count)

        [TestMethod]
        public void SaveHealthCondition()
        {
            String _CodingSystem = "Anthrax";
            String Key = "Anthrax";
            String Name = "Anthrax";


            using (ExportQueueTable exp = new ExportQueueTable(GeneralHelper.ConnectionString, 0, ProcessStageFilterEnum.Lookup, ProcessStateFilterEnum.Failed, "ResolveHealthConditions"))
            {

                var update_msh_id = 100;
                var exportQueueItem = exp.Items_ExportQueue.Where(eq => eq.MSH_ID == update_msh_id).FirstOrDefault();
                var batchItem = HL7.Reportable.Common.Common.DeserializeXmlToObject<HL7.Reportable.Common.Business.HL7.ExportBatch>(exportQueueItem.Process_Message);
                var labReport = batchItem.LabReports.Where(lr => lr.MSH_ID == update_msh_id.ToString()).FirstOrDefault();

                labReport.PHRED_Variables.PHRED_AssignmentNote = "The Note";
                

                string obr_id = "108";
                string obx_id = "110";

                var obr = labReport.TestRequests.Where(tRequest => tRequest.OBR_ID == obr_id).FirstOrDefault();
                var obx = obr.TestResults.Where(tResult => tResult.OBX_ID == obx_id).FirstOrDefault();

                CodedElement ceHCC = new CodedElement() { Code = Key, Description = Name, CodingSystem = _CodingSystem };

                CodedElement ceHC = new CodedElement() { Code = Key, Description = Name, CodingSystem = _CodingSystem };

                obx.PHRED_AssignedHCManually = true;
                obx.PHRED_AssignerUserKey = "Active Directory Username";
                obx.PHRED_AssignmentNote = "Another Note";
                obx.PHRED_AssignedDtTm = DateTime.Now.ToString();
                obx.PHRED_AssignedHCC = ceHCC;
                obx.PHRED_AssignedHC = ceHC;


                // Update the Status
                exportQueueItem.Process_State = ProcessStateEnum.Pending;
                exportQueueItem.Save();



                // Revert it back
                //exportQueueItem.Process_Stage = ProcessStageEnum.Lookup;
                //exportQueueItem.Process_State = ProcessStateEnum.Failed;
                //exportQueueItem.Save();

            }
        }


        [TestMethod]
        public void GetHealthConditionCategories()
        {
            List<HealthConditionAssignment> lstHealth = new List<HealthConditionAssignment>();
            TRCClient trc = new TRCClient();
            var result =trc.GetHealthConditionCatagories();

            foreach (var parent in result.HealthConditionCatagories)
            {
                foreach (var child in parent.HealthConditions)
                {
                    lstHealth.Add(new HealthConditionAssignment()
                    {
                        ChildKey = child.Key, ChildName = child.Name , ChildCodingSystem = child.CodingSystem,
                        ParentKey = parent.Key , ParentName = parent.Name, ParentCodingSystem = parent.CodingSystem
                    });
                }
            }

            var a = lstHealth.Select(hca => hca.ChildName).Distinct();
        }
    }
}
