using AdminUI.Models.HCAssignment;
using AdminUI.Models.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminUI.Common
{
    public class TreeViewHelper
    {

        public static List<TreeViewNode> Transform(List<MSH> inBatch)
        {
            List<TreeViewNode> treeRet = new List<TreeViewNode>();

            if (inBatch == null || inBatch.Count <=0)
            {
                return treeRet;
            }

            foreach (var item in inBatch)
            {
                TreeViewNode node = GetMsh(item);
                treeRet.Add(node);
            }

            return treeRet;
        }

        private static TreeViewNode GetMsh(MSH mshInfo)
        {

            TreeViewNode mshNode = new TreeViewNode()
            {
                sequenceID = mshInfo.SequenceID,
                type = NodeType.MSH,
                msh_id = mshInfo.MSH_ID,
                text = $"MSH {mshInfo.MSH_ID}",
                tags = new List<string>(),
                //icon = NodeIcons.MSH,
                color = NodeColors.MSH
            };
            mshInfo.NodeType = NodeType.MSH; 

            AddObrNodes(ref mshNode, mshInfo);

            //mshNode.text = $"{mshInfo.MSH_ID} ({mshNode.nodes.Count()})";
            return mshNode;
        }

        private static void AddObrNodes(ref TreeViewNode mshNode, MSH mshInfo)
        {
            List<TreeViewNode> obrNodes = new List<TreeViewNode>();

            if (mshInfo.ObrList != null)
            {
                foreach (var item in mshInfo.ObrList)
                {
                    obrNodes.Add(GetSingleObrNode(mshInfo, item));
                }
            }
            
            mshNode.nodes = obrNodes;
        }

        private static TreeViewNode GetSingleObrNode(MSH mshInfo, OBR obr)
        {
            TreeViewNode obrNode = new TreeViewNode()
            {
                sequenceID = obr.SequenceID,
                type = NodeType.OBR,
                msh_id = mshInfo.MSH_ID,
                obr_id = obr.OBR_ID,
                text = $"OBR {obr.OBR_ID}",
                tags = new List<string>(),
                //icon = NodeIcons.OBR,
                color = NodeColors.OBR
            };
            obr.NodeType = NodeType.OBR;

            AddObXNodes(ref obrNode, obr.ObxList);

            return obrNode;
        }

        private static void AddObXNodes( ref TreeViewNode obrNode, List<OBX> obxList)
        {
            List<TreeViewNode> obXNodes = new List<TreeViewNode>();

            if (obxList != null)
            {
                foreach (var item in obxList)
                {
                    obXNodes.Add(GetSingleObXNode(obrNode, item));
                }
            }
            
            obrNode.nodes = obXNodes;
        }

        private static TreeViewNode GetSingleObXNode(TreeViewNode obrNode ,OBX obx )
        {
            TreeViewNode obxNode = new TreeViewNode()
            {
                sequenceID = obx.SequenceID,
                type = NodeType.OBX,
                msh_id= obrNode.msh_id,
                obr_id = obrNode.obr_id,
                obx_id =  obx.OBX_ID,
                text = $"OBX {obx.OBX_ID}",
                tags = new List<string>(),
                //icon = NodeIcons.OBX,
                color = NodeColors.OBX
            };
            obx.NodeType = NodeType.OBX;
            return obxNode;
        }

        public static string TruncString(string myStr, int THRESHOLD)
        {
            if (myStr.Length > THRESHOLD)
                return myStr.Substring(0, THRESHOLD) + "...";
            return myStr;
        }
    }
}