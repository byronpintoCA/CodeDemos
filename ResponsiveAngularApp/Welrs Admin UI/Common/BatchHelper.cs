using AdminUI.Models.HCAssignment;
using AdminUI.Models.Queue;
using HL7.Reportable.Common.Business.HL7;
using HL7.Reportable.DAL;
using System;
using System.Collections.Generic;
using System.Text;


namespace AdminUI.Common
{
    public class BatchHelper
    {
        private const string CE_DELIMMITER = " | ";
        private const string CONTROL_LINEFEED = "\r\n";

        public static List<MSH> LoadBatches(List<ExportQueue> list)
        {
            List<MSH> retList = new List<MSH>();
            int sequenceID = 1;
            foreach (var item in list)
            {
                HL7.Reportable.Common.Business.HL7.ExportBatch batch =
                    HL7.Reportable.Common.Common.DeserializeXmlToObject<HL7.Reportable.Common.Business.HL7.ExportBatch>(item.Process_Message);

                foreach (var subItem in batch.LabReports)
                {
                    retList.Add(ProcessLabReportItem(subItem, sequenceID++));
                }
            }

            return retList;
        }

        private static MSH ProcessLabReportItem(HL7.Reportable.Common.Business.HL7.LabReport report, int sequenceID)
        {
            MSH msh = new MSH();

            msh.SequenceID = sequenceID.ToString();
            msh.Sender = report.Submission.SubmitterName;
            msh.MessageControlID = report.Submission.MessageControlID;
            msh.MSH_ID = report.MSH_ID;
            msh.PID_ID = report.Patient.PID_ID;
            msh.MessageDateTime = report.Submission.MessageDateTime;
            msh.FileName = report.Submission.Filename;

            msh.FirstName = report.Patient.PatientNames.Count > 0 ? (report.Patient.PatientNames[0].FirstName != null ? report.Patient.PatientNames[0].FirstName : "") : "";
            msh.LastName = report.Patient.PatientNames.Count > 0 ? (report.Patient.PatientNames[0].LastName != null ? report.Patient.PatientNames[0].LastName : "") : "";
            msh.MiddleNameOrInitial = report.Patient.PatientNames.Count > 0 ? (report.Patient.PatientNames[0].MiddleNameOrInitial != null ? report.Patient.PatientNames[0].MiddleNameOrInitial : "") : "";

            msh.DateOfBirth = report.Patient.DateOfBirth;

            AddOBRAndOBXs(ref msh, report);

            return msh;
        }
        private static void AddOBRAndOBXs(ref MSH msh, HL7.Reportable.Common.Business.HL7.LabReport report)
        {
            int sequenceID = 1;
            List<OBR> OBRs = new List<OBR>();
            foreach (var item in report.TestRequests)
            {
                OBRs.Add(GetObr(item, sequenceID++));
            }

            msh.ObrList = OBRs;
        }

        //public static OBX GetObx(TestResult tResult, int sequenceID)
        //{
        //    OBX obx = new OBX();
        //    obx.SequenceID = sequenceID.ToString();
        //    obx.OBX_ID = tResult.OBX_ID;
        //    obx.ResultTest = new MultiLineTextField(BatchHelper.ConcatCE(tResult.ObservationIdentifier));
        //    obx.Result = new MultiLineTextField(BatchHelper.ConcatOVRList(tResult.ObservationValues));
        //    obx.ResultUnits = new MultiLineTextField(BatchHelper.ConcatCE(tResult.ResultUnits));
        //    obx.TestMethods = new MultiLineTextField(BatchHelper.ConcatCEList(tResult.TestMethods));
        //    obx.TestResultNotes = new MultiLineTextField(BatchHelper.ConcatNotes(tResult.TestResultNotes));
        //    obx.HCAssignment = "DingDong";

        //    return obx;
        //}

        public static OBR GetObr(TestRequest tReq, int sequenceID)
        {
            OBR obr = new OBR();

            obr.SequenceID = sequenceID.ToString();
            obr.OBR_ID = tReq.OBR_ID;
            obr.TestRequestIdentifier = (tReq.TestRequestIdentifierGroups.Count > 0 &&
                                            !String.IsNullOrWhiteSpace(tReq.TestRequestIdentifierGroups[0].TestRequestIdentifier)
                                         )
                                        ? tReq.TestRequestIdentifierGroups[0].TestRequestIdentifier : "";
            obr.SpecimenSourceSite = new MultiLineTextField(BatchHelper.ConcatCE(tReq.Specimen.SpecimenSourceSite));
            obr.OrderedTest = new MultiLineTextField(BatchHelper.ConcatCE(tReq.OrderedTest));
            obr.ParentResult = new MultiLineTextField(BatchHelper.ConcatCE(tReq.ParentResult.ParentObservationID));
            obr.ReasonsForStudy = new MultiLineTextField(BatchHelper.ConcatCEList(tReq.ReasonsForStudy));
            obr.RelevantClinicInfo = new MultiLineTextField(tReq.OtherClinicalInformation);
            obr.Notes = new MultiLineTextField(BatchHelper.ConcatNotes(tReq.TestRequestNotes));

            List<OBX> OBXs = new List<OBX>();

            int childSequence = 1;

            if (tReq.TestResults.Count <=0)
            {
                CodedElement ceHCC = tReq.PHRED_AssignedHCC;
                CodedElement ceHC = tReq.PHRED_AssignedHC;
                obr.HCAssignment = OBX.GetHealthCondition(ceHCC, ceHC);
            }
            else
            {
                foreach (var item in tReq.TestResults)
                {
                    OBXs.Add(OBX.GetObx(item, childSequence++));
                }
            }
            
            obr.ObxList = OBXs;

            return obr;
        }

        public static string ConcatOVRList(List<ObservationValueRow> observationValues)
        {
            StringBuilder output = new StringBuilder();
            String temp;

            if (observationValues == null) return "";

            foreach (var item in observationValues)
            {
                temp = ConcatOVR(item);
                if (String.IsNullOrWhiteSpace(temp) == false)
                {
                    output.Append(output.Length > 0 ? CONTROL_LINEFEED + temp : temp);
                }
            }

            return output.ToString();
        }

        public static string ConcatOVR(ObservationValueRow ovr)
        {
            StringBuilder output = new StringBuilder();

            if (!(String.IsNullOrWhiteSpace(ovr.ObsValRowNumber) && String.IsNullOrWhiteSpace(ovr.ObsValLabel) && String.IsNullOrWhiteSpace(ovr.ObsValue)))
            {
                AddIfNotEmpty_CE(ref output, ovr.ObsValRowNumber);
                AddIfNotEmpty_CE(ref output, ovr.ObsValLabel);
                AddIfNotEmpty_CE(ref output, ovr.ObsValue);
            }
            return output.ToString();
        }

        public static string ConcatNotes(List<NotesAndComments> testRequestNotes)
        {
            StringBuilder output = new StringBuilder();

            if (testRequestNotes == null) return "";

            foreach (var item in testRequestNotes)
            {
                //temp = ConcatCE(item);
                if (String.IsNullOrWhiteSpace(item.Comment) == false)
                {
                    output.Append(output.Length > 0 ? CONTROL_LINEFEED + item.Comment : item.Comment);
                }
            }

            return output.ToString(); ;
        }

        public static string ConcatCEList(List<CodedElement> ceList)
        {
            StringBuilder output = new StringBuilder();
            String temp;

            if (ceList == null) return "";

            foreach (var item in ceList)
            {
                temp = ConcatCE(item);
                if (String.IsNullOrWhiteSpace(temp) == false)
                {
                    output.Append(output.Length > 0 ? CONTROL_LINEFEED + temp : temp);
                }
            }

            return output.ToString();
        }

        public static string ConcatCE(CodedElement ce)
        {
            StringBuilder output = new StringBuilder();

            if (!(String.IsNullOrWhiteSpace(ce.Code) && String.IsNullOrWhiteSpace(ce.Description) && String.IsNullOrWhiteSpace(ce.CodingSystem)))
            {
                AddIfNotEmpty_CE(ref output, ce.Code);
                AddIfNotEmpty_CE(ref output, ce.Description);
                AddIfNotEmpty_CE(ref output, ce.CodingSystem);
            }

            if (!(String.IsNullOrWhiteSpace(ce.AltCode) && String.IsNullOrWhiteSpace(ce.AltDescription) && String.IsNullOrWhiteSpace(ce.AltCodingSystem)))
            {
                if (output.Length > 0) output.Append(CONTROL_LINEFEED);

                AddIfNotEmpty_CE(ref output, ce.AltCode);
                AddIfNotEmpty_CE(ref output, ce.AltDescription);
                AddIfNotEmpty_CE(ref output, ce.AltCodingSystem);
            }

            return output.ToString();
        }

        public static void AddIfNotEmpty_CE(ref StringBuilder output, string daValue)
        {
            if (String.IsNullOrWhiteSpace(daValue) == false)
            {   // Prepend Delimmiter if not empty or theres
                output.Append(
                    output.Length > 0 && (output[output.Length - 1] != CONTROL_LINEFEED[CONTROL_LINEFEED.Length - 1])
                        ? CE_DELIMMITER + daValue : daValue);
            }
        }

    }
}