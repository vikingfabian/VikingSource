using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using VikingEngine.Core.DSSWars.XP.TechTree;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.GameObject
{
    //TECH TREE
    partial class City
    {
        bool generalizedResearch;

        public bool GeneralizedResearchProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                generalizedResearch = value;
            }
            return generalizedResearch;
        }

        public TechTreeNodeProgress_City GetProgress(TechNodeType nodeType)
        {
            return DssRef.world.city_techNodes[myIndex * TechTreeNodeProgress_City.NodeCount + (int)nodeType];
        }
        public ref TechTreeNodeProgress_City GetRefProgress(TechNodeType nodeType)
        {
            return ref DssRef.world.city_techNodes[myIndex * TechTreeNodeProgress_City.NodeCount + (int)nodeType];
        }
        public void SetProgress(TechNodeType nodeType, TechTreeNodeProgress_City progress)
        {
            DssRef.world.city_techNodes[myIndex * TechTreeNodeProgress_City.NodeCount + (int)nodeType] = progress;
        }
        public void UnlockNode(TechNodeType nodeType)
        {
            SetProgress(nodeType, TechTreeNodeProgress_City.Unlocked);
        }
        public FieldResearch GetFieldResearch(TechFieldType fieldType)
        {
            return DssRef.world.city_researchNode_inField[myIndex * (int)TechFieldType.NUM_NONE + (int)fieldType];
        }
        public void SetFieldResearch(int fieldType, FieldResearch value)
        {
            DssRef.world.city_researchNode_inField[myIndex * (int)TechFieldType.NUM_NONE + fieldType] = value;
        }

        void unlockChildNodes(TechFieldType fieldtype, TechNodeType nodeType)
        {
            XpLib.TechTree.findNode(fieldtype, nodeType, out var node, out int branschIx, out int nodeIx);
            XpLib.TechTree.branschStatus(this, branschIx, out var unlocked, out var completed, out int nextNode);
            if (completed)
            {
                var level = XpLib.TechTree.bransches_all[branschIx].childNodeLevel;
                for (int i = branschIx + 1; i < XpLib.TechTree.bransches_all.Count; ++i)
                {
                    if (XpLib.TechTree.bransches_all[i].childNodeLevel == level + 1)
                    {
                        var childNodes = XpLib.TechTree.bransches_all[i].nodes;
                        for (nodeIx = childNodes.StartIndex; nodeIx < childNodes.ExEndIndex; nodeIx++)
                        {
                            GetRefProgress(XpLib.TechTree.nodes_all[nodeIx].type).points_bUnlocked_non_bComplete.Flag1 = true;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        void refreshProgressNode(TechFieldType fieldtype)
        {
            var research = GetFieldResearch(fieldtype);
            
            XpLib.TechTree.findNode(fieldtype, research.selected, out var node, out int branschIx, out int nodeIx);
            XpLib.TechTree.branschStatus(this, branschIx, out var unlocked, out var completed, out int nextNode);

            if (completed)
            {
                //Track on to find the next available bransch
                autoSelectNextProgressNode(fieldtype);
                return;
            }
            else if (unlocked)
            {
                var nextResearch = XpLib.TechTree.nodes_all[nextNode];
                research = new FieldResearch(nextResearch);
            }
            else
            {
                //Find parent bransch
                bool foundParent = false;
                int bransch = node.inBranschIndex;
                while (XpLib.TechTree.findParentBransch(bransch, out var parentBranschIx))
                {
                    XpLib.TechTree.branschStatus(this, parentBranschIx, out bool p_unlocked, out bool p_completed, out int p_nextNode);
                    if (p_completed)
                    { 
                        //Should have unlocked all child nodes
                    }
                    else if (p_unlocked)
                    {
                        foundParent = true;
                        var nextResearch = XpLib.TechTree.nodes_all[p_nextNode];
                        research.parent = nextResearch.type;
                        research.pointsNeeded = (ushort)nextResearch.pointsCost;
                        break;
                    }
                    else
                    {
                        bransch = parentBranschIx;
                    }
                }

                if (!foundParent)
                {
                    autoSelectNextProgressNode(fieldtype);
                    return;
                }
            }

            SetFieldResearch((int)fieldtype, research);
            //node.inBranschIndex

            //SelectProgressNode(research.selected);
        }

        void autoSelectNextProgressNode(TechFieldType fieldtype)
        {
            FieldResearch research = new FieldResearch() { parent = TechNodeType.NUM_NONE, selected = TechNodeType.NUM_NONE };

            var branshes = XpLib.TechTree.fields.Array[(int)fieldtype].branshes;
            for (int branschIx = branshes.StartIndex; branschIx < branshes.ExEndIndex; branschIx++)
            {
                XpLib.TechTree.branschStatus(this, branschIx, out bool unlocked, out bool completed, out int nextNode);
                if (unlocked && !completed)
                {
                    research = new FieldResearch(XpLib.TechTree.nodes_all[nextNode]);
                    break;
                }
            }

            SetFieldResearch((int)fieldtype, research);
        }

        public void ProfileSelectProgressNode(TechNodeType node)
        {
            XpLib.TechTree.findNode(TechFieldType.NUM_NONE, node, out var nodeResult, out _, out _);
            SelectProgressNode(nodeResult);
        }

        public void SelectProgressNode(TechTreeNode node)
        {
            var bransch = XpLib.TechTree.bransches_all.Array[node.inBranschIndex];
            var field = bransch.inFieldIndex;
            var progress = GetProgress(node.type);

            var select = new FieldResearch() { selected = node.type, parent = node.type, pointsNeeded = (ushort)node.pointsCost };
            if (!NodeIsUnlocked(node))
            {
                for (int i = node.inBranschIndex - 1; i >= 0; i--)
                {
                    if (XpLib.TechTree.bransches_all.Array[i].childNodeLevel < bransch.childNodeLevel)
                    {
                        var parentBransch = XpLib.TechTree.bransches_all.Array[i];
                        var parentNode = parentBransch.nodes.First(XpLib.TechTree.nodes_all);
                        if (NodeIsUnlocked(parentNode))
                        {
                            //Will fail if the parent bransch has more than one node
                            select.parent = parentNode.type;
                            select.pointsNeeded = (ushort)parentNode.pointsCost;
                            break;
                        }
                    }
                }
                
            }

            SetFieldResearch(field, select);

            bool NodeIsUnlocked(TechTreeNode node)
            {
                var progress = GetProgress(node.type);
                return (XpLib.TechTree.bransches_all.Array[node.inBranschIndex].childNodeLevel == 0 ||
                     progress.points_bUnlocked_non_bComplete.Flag1) && !progress.points_bUnlocked_non_bComplete.Flag3;
            }
        }
    }

    struct TechTreeNodeProgress_City
    {
        public static readonly TechTreeNodeProgress_City Unlocked = new TechTreeNodeProgress_City() { points_bUnlocked_non_bComplete = UInt12_flag4.MaxValue };

        public static readonly int NodeCount = (int)TechNodeType.NUM_NONE;

        /// <summary>
        /// Unlocked: May research, Complete: Fully researched
        /// </summary>
        public UInt12_flag4 points_bUnlocked_non_bComplete;

        public bool Complete()
        {
            return points_bUnlocked_non_bComplete.Flag3;
        }
        //public bool Selected { get { return points_bUnlocked_non_bComplete.Flag2; } }
    }

}
