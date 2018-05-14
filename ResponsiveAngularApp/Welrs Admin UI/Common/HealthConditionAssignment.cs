using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminUI.Common
{
    [Serializable]
    public class HealthConditionAssignment
    {
        public enum HCATypeKind {  HealthCode =0 , PatientCentric=1}

        public HCATypeKind HCAType { get; set; } = HCATypeKind.HealthCode;

        public String label { get { return ChildName; } }

        public String ChildKey { get; set; }
        public String ChildName { get; set; }
        public String ChildCodingSystem { get; set; }
        public String ParentKey { get; set; }
        public String ParentName { get; set; }
        public String ParentCodingSystem { get; set; }
    }
}