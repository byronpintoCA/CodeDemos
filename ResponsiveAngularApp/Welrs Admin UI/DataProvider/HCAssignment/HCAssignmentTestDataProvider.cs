using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminUI.Models.Queue;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using AdminUI.Models.HCAssignment;
using AdminUI.Common;

namespace AdminUI.DataProvider.HCAssignment
{
    public class HCAssignmentTestDataProvider : HCAssignmentDataProvider
    {
        private List<Tuple<string, string>> _hl7List;
        private List<MSH> _mshList;
        private List<HealthConditionAssignment> _assignCodes;

        public HCAssignmentTestDataProvider()
        {
            string appPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            string root = appPath + "\\TestData";

            string inHl7Text = File.ReadAllText(root + @"\hl7Messages.json");
            _hl7List = JsonConvert.DeserializeObject<List<Tuple<String, String>>>(inHl7Text);

            FileStream stream = File.OpenRead(root + @"\msh.bin");
            _mshList = (List<MSH>)new BinaryFormatter().Deserialize(stream);
            stream.Close();


            stream = File.OpenRead(root + @"\hca.bin");
            _assignCodes = (List<HealthConditionAssignment>)new BinaryFormatter().Deserialize(stream);
            stream.Close();

            //string assignCodes = File.ReadAllText(root + @"\assignmentCodes.txt");
            //_assignCodes = assignCodes.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        }

        public override List<HealthConditionAssignment> GetAssignmentCodes()
        {
            return _assignCodes.Union(base.GetAssignmentCodes()).ToList();
        }

        public override string GetHL7Message(int msh_id)
        {
            return _hl7List.Where(tup => tup.Item1 == msh_id.ToString()).FirstOrDefault().Item2;
        }

        public override List<MSH> GetMSH()
        {
            return _mshList;
        }

        public override bool Save(string Username, long MSH_ID, String Note, List<ChangeSetData> changeSet, out string errorMessage)
        {
            errorMessage = "";
            return true;
        }
    }
}