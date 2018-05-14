using System;
using System.Collections.Generic;

namespace AdminUI.Models.HCAssignment
{
    public static class IdGenerator
    {
        static int _id = 0;
        static object _lock = new object();
        public static int NewId
        {
            get
            {
                lock (_lock)
                {
                    _id++;
                }
                return _id;
            }
            private set { }
        }
    }
    public class TreeViewNode
    {
        public int nodeUniqueID { get; private set; } = IdGenerator.NewId;

        public string sequenceID;

        public string color { get; set; }

        public String text { get; set; }
        public List<String> tags { get; set; }
        public NodeType type { get; set; }

        public String icon { get; set; }

        public State state { get; internal set; } = new State();

        public List<TreeViewNode> nodes { get; set; }
        public string msh_id { get; internal set; }
        public string obr_id { get; internal set; }
        public string obx_id { get; internal set; }
    }

    public class State
    {
        public bool @checked { get; set; } = false;
        public bool disabled { get; set; } = false;
        public bool expanded { get; set; } = false;
        public bool selected { get; set; } = false;
    }

    public enum NodeType
    {
        MSH = 1, OBR, OBX
    }

    public class NodeIcons
    {
        public const String MSH = "glyphicon glyphicon-user";
        public const String OBR = "glyphicon glyphicon-list-alt";
        public const String OBX = "glyphicon glyphicon-bookmark";
    }

    public class NodeColors
    {
        public const String MSH = "#225fc1";
        public const String OBR = "DarkOliveGreen ";
        public const String OBX = "#633300";
    }
}