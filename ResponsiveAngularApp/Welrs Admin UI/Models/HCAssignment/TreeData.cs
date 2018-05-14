using AdminUI.Common;
using AdminUI.Models.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminUI.Models.HCAssignment
{
    public class TreeViewData
    {
        
        public List<MSH> MSHData { get; set; }

        public List<TreeViewNode> TreeData { get; set; }

        public List<HealthConditionAssignment> AssignCodes { get; set; }

        public TreeConfig TreeConfig { get; internal set; }
    }
}