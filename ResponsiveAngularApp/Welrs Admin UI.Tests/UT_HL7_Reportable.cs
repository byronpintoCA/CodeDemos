using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HL7.Reportable.DAL;
using System.Configuration;
using static HL7.Reportable.Common.Common;
using AdminUI.Models.Queue;
using AdminUI.Models;
using AdminUI.Common;
using System.Collections.Generic;
using AdminUI.Models.HCAssignment;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using AdminUI.DataProvider;

namespace Welrs_Admin_UI.Tests
{
    [TestClass]
    public class UT_HL7_Reportable
    {

        [TestMethod]
        public void DbConnectionTest()
        {
            List<MSH> msh;

            using (ExportQueueTable exp = new ExportQueueTable(GeneralHelper.ConnectionString, 0, ProcessStageFilterEnum.Lookup, ProcessStateFilterEnum.Failed, "ResolveHealthConditions"))
            {
                msh = BatchHelper.LoadBatches(exp.Items_ExportQueue);
            }

        }

        [TestMethod]
        public void FirstSetupTest()
        {
            String dbConnection = ConfigurationManager.AppSettings["HL7ReportableDB"];

            List<MSH> msh;

            using (ExportQueueTable exp = new ExportQueueTable(dbConnection, 0, ProcessStageFilterEnum.Lookup, ProcessStateFilterEnum.Failed, "ResolveHealthConditions"))
            {
                msh = BatchHelper.LoadBatches(exp.Items_ExportQueue);
            }

            var result = TreeViewHelper.Transform(msh);

            TreeViewData td = new TreeViewData() { MSHData = msh, TreeData = result };
        }

        [TestMethod]
        public void SerializeMSHQueue()
        {
            String filename = @"c:\temp\msh.bin";

            var prov = HCAssignmentDataProviderFactory.GetLiveProvider();

            List<MSH> mshList = prov.GetMSH();


            //mshList[0].ObrList[0].ObxList[0].HCAssignment = "Anthrax";


            MSH anotherOne = mshList[4].DeepCopy();

            anotherOne.MSH_ID = "1000";
            anotherOne.ObrList[0].OBR_ID = "1001";
            anotherOne.ObrList[0].ObxList[0].OBX_ID = "1002";
            anotherOne.ObrList[0].ObxList = null;

            mshList.Add(anotherOne);

            FileStream stream = File.Create(filename);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, mshList);
            stream.Close();

            
            stream = File.OpenRead(filename);
            Console.WriteLine("Deserializing vector");
            var v1 = (List<MSH>)formatter.Deserialize(stream);
            stream.Close();
            
        }

        [TestMethod]
        public void SerializeHealthCode()
        {
            String filename = @"c:\temp\hca.bin";

            var prov = HCAssignmentDataProviderFactory.GetLiveProvider();

            var hcaList = prov.GetAssignmentCodes();


            FileStream stream = File.Create(filename);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, hcaList);
            stream.Close();


            stream = File.OpenRead(filename);
            Console.WriteLine("Deserializing");
            var v1 = (List<HealthConditionAssignment>)formatter.Deserialize(stream);
            stream.Close();

        }


        [TestMethod]
        public void SerializeHL7()
        {

            var prov = HCAssignmentDataProviderFactory.GetProvider();

            List<MSH> msh = prov.GetMSH();

            List<Tuple<String, String>> hl7Messages = new List<Tuple<string, string>>();

            foreach (var item in msh)
            {
                hl7Messages.Add(new Tuple<string, string>(
                    item.MSH_ID, prov.GetHL7Message(Convert.ToInt32(item.MSH_ID)
                    )));

            }

            string output = JsonConvert.SerializeObject(hl7Messages);

            File.WriteAllText(@"c:\temp\hl7Messages.json", output);

            string input = File.ReadAllText(@"c:\temp\hl7Messages.json");


            List<Tuple<String, String>> two = JsonConvert.DeserializeObject<List<Tuple<String, String>>>(input);

            Assert.IsTrue(two.Count > 0);

        }
    }
}
