using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.DSSWars.XP.TechTree;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.XP;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.GameObject
{
    //TECH TREE
    partial class City
    {
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

        public void SelectProgressNode(TechTreeNode node)
        {
            var field = XpLib.TechTree.bransches_all.Array[node.parentBranschIndex].parentFieldIndex;

            var branshes = XpLib.TechTree.fields.Array[field].branshes;
            for (int branschIx = branshes.StartIndex; branschIx < branshes.ExEndIndex; branschIx++)
            {
                var nodes = XpLib.TechTree.bransches_all.Array[branschIx].nodes;

                for (int nodeIx = nodes.StartIndex; nodeIx < nodes.ExEndIndex; nodeIx++)
                {
                    var nType = XpLib.TechTree.nodes_all.Array[nodeIx].type;
                    bool set = nType == node.type;
                    var progress = GetProgress(nType);
                    progress.points_bUnlocked_bSelected_bComplete.Flag2 = set;
                    SetProgress(nType, progress);
                }
            }
        }

        //foreach (var br in XpLib.TechTree.ironField.branshes)
        //{
        //    for (int nIx = 0; nIx < br.nodes.Count; nIx++)
        //    { 

        //    }
        //}
        //city.GetRefProgress(node.type).points_bUnlocked_bSelected_bComplete.Flag2 = true;

    }

    struct TechTreeNodeProgress_City
    {
        public static readonly TechTreeNodeProgress_City Unlocked = new TechTreeNodeProgress_City() { points_bUnlocked_bSelected_bComplete = UInt12_flag4.MaxValue };

        public static readonly int NodeCount = (int)TechNodeType.NUM_NONE;

        /// <summary>
        /// Unlocked: May research, Complete: Fully researched
        /// </summary>
        public UInt12_flag4 points_bUnlocked_bSelected_bComplete;

        public bool Complete()
        {
            return points_bUnlocked_bSelected_bComplete.Flag3;
        }
        public bool Selected { get { return points_bUnlocked_bSelected_bComplete.Flag2; } }
    }

}
