using AdminUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminUI.Models.HCAssignment
{
    public class ChangeSetData
    {
        public int NodeType { get; set; }
        public String MSH_ID { get; set; }

        public String OBR_ID { get; set; }

        public String OBX_ID { get; set; }

        public HealthConditionAssignment HealthCondition { get; set; }

    }
}