using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.XP;
using VikingEngine.Engine;
using VikingEngine.ToGG;

namespace VikingEngine.Core.DSSWars.XP.TechTree
{
    class TechTreeStructure
    {
        const double DefaultProcessPoints = 100;

        //public TechTreeField ironField;
        public List2<TechTreeField> fields;
        public List2<TechFieldBransch> bransches_all;
        public List2<TechTreeNode> nodes_all;
        public List2<TechNodeType> unlocktech_all;

        public TechTreeStructure()
        {
            bransches_all = new List2<TechFieldBransch>(32);
            nodes_all = new List2<TechTreeNode>((int)TechNodeType.NUM_NONE);
            unlocktech_all = new List2<TechNodeType>(16);

            double dPoints = DefaultProcessPoints / DssRef.storage.ruleset_instance.TechMultiProperty(null, false, 0);

            fields = new List2<TechTreeField>((int)TechFieldType.NUM_NONE);


            var ironField = new TechTreeField() { techField = TechFieldType.MetalWork };
            {
                ironField.branshes = new PartialList2<TechFieldBransch>(bransches_all);
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = false,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.ironProduction, SpriteName.WarsResource_Iron, 4 * dPoints),
                            new TechTreeNode(TechNodeType.steelProduction, SpriteName.WarsResource_Steel, 4 * dPoints),
                            new TechTreeNode(TechNodeType.mithrilProduction, SpriteName.WarsResource_Mithril, 4 * dPoints)
                        )
                    };

                    ironField.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = false,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.ironCraft_base, SpriteName.WarsBuild_Smith, 2 * dPoints)),
                        unlockBranshOnComplete = new PartialList2<TechNodeType>(ref unlocktech_all,
                            TechNodeType.shortsword, TechNodeType.HeaterShield, TechNodeType.lightIronArmor)
                    };

                    ironField.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.shortsword, SpriteName.WarsResource_ShortSword, 1 * dPoints),
                            new TechTreeNode(TechNodeType.sword, SpriteName.WarsResource_Sword, 1 * dPoints),
                            new TechTreeNode(TechNodeType.longsword, SpriteName.WarsResource_Longsword, 1 * dPoints),
                            new TechTreeNode(TechNodeType.zweihänder, SpriteName.WarsResource_TwoHandSword, 1 * dPoints)
                            ),
                    };

                    ironField.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.HeaterShield, SpriteName.WarsResource_HeaterShield, 0.5 * dPoints),
                            new TechTreeNode(TechNodeType.TowerShield, SpriteName.WarsResource_TowerShield, 0.5 * dPoints),
                            new TechTreeNode(TechNodeType.BucklerShield, SpriteName.WarsResource_BucklerShield, 0.5 * dPoints)
                            ),
                    };

                    ironField.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.lightIronArmor, SpriteName.WarsResource_IronArmor, 1 * dPoints),
                            new TechTreeNode(TechNodeType.heavyIronArmor, SpriteName.WarsResource_HeavyIronArmor, 1 * dPoints),
                            new TechTreeNode(TechNodeType.steelarmor, SpriteName.WarsResource_LightPlateArmor, 1 * dPoints),
                            new TechTreeNode(TechNodeType.warhammer, SpriteName.WarsResource_Warhammer, 0.6 * dPoints),
                            new TechTreeNode(TechNodeType.heavySteelarmor, SpriteName.WarsResource_FullPlateArmor, 1.5 * dPoints)
                            ),
                    };

                    ironField.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = false,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                        new TechTreeNode(TechNodeType.mechanics_base, SpriteName.IconMenuSettings, 2 * dPoints)
                        ),
                        unlockBranshOnComplete = new PartialList2<TechNodeType>(ref unlocktech_all,
                        TechNodeType.wagon4, TechNodeType.toolkit),
                    };

                    ironField.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.wagon4, SpriteName.WarsResource_Wagon4Wheel, 1 * dPoints),
                            new TechTreeNode(TechNodeType.wagonClosed, SpriteName.WarsResource_WagonClosed, 1 * dPoints),
                            new TechTreeNode(TechNodeType.ironwagon, SpriteName.WarsResource_WagonIron, 1 * dPoints),
                            new TechTreeNode(TechNodeType.steelwagon, SpriteName.WarsResource_WagonSteel, 1 * dPoints)
                            ),
                    };

                    ironField.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.toolkit, SpriteName.WarsResource_Toolkit, 2 * dPoints),
                            new TechTreeNode(TechNodeType.NutLockMechanic, SpriteName.MissingImage, 4 * dPoints),
                            new TechTreeNode(TechNodeType.MatchLockMechanic, SpriteName.MissingImage, 5 * dPoints)
                            ),
                    };

                    ironField.branshes.Add(ref bransches_all, branch);
                }
            } fields.Array[(int)ironField.techField] = ironField;

          
            for (int fieldIx = 0; fieldIx < fields.Array.Length; fieldIx++)
            {
                var field = fields.Array[fieldIx];
                int[] currentParentOnLevel = new int[4];

                for (int branschIx = field.branshes.StartIndex; branschIx < field.branshes.ExEndIndex; branschIx++)
                {
                    bransches_all.Array[branschIx].inFieldIndex = fieldIx;
                    //int level = bransches_all.Array[branschIx].childNode;
                    //if (level == 0)
                    //{
                    //    bransches_all.Array[branschIx].childNode = 
                    //}
                    //else
                    //{ 
                    
                    //}
                    //currentParentOnLevel[level] = branschIx;

                    var bransch = bransches_all[branschIx];
                    for (int nodeIx = bransch.nodes.StartIndex; nodeIx < bransch.nodes.ExEndIndex; nodeIx++)
                    {
                        nodes_all[nodeIx].inBranschIndex = branschIx;
                        //if (bransch.childNodeLevel == 0)
                        //{ 
                            
                        //}
                    }
                }
            }
        }

        public bool findNode(TechFieldType fieldtype, TechNodeType nodeType, out TechTreeNode node, 
            out int branschIx, out int nodeIx)
        {
            //var research = GetFieldResearch(fieldtype);
            //var progress = GetProgress(research.parent);
            var field = XpLib.TechTree.fields.Array[(int)fieldtype];

            for (branschIx = field.branshes.StartIndex; branschIx < field.branshes.ExEndIndex; branschIx++)
            {
                var bransch = XpLib.TechTree.bransches_all[branschIx];
                for (nodeIx = bransch.nodes.StartIndex; nodeIx < bransch.nodes.ExEndIndex; nodeIx++)
                {
                    if (XpLib.TechTree.nodes_all[nodeIx].type == nodeType)
                    {
                        node = XpLib.TechTree.nodes_all[nodeIx];
                        return true;
                    }
                }
            }
            nodeIx = -1;
            node = new TechTreeNode();
            return false;
        }

        public bool findParentBransch(int inBranschIndex, out int parentBranschIx)
        {
            int childLevel = XpLib.TechTree.bransches_all[inBranschIndex].childNodeLevel;
            if (childLevel > 0)
            {
                for (int branschIx = inBranschIndex - 1; branschIx >= 0; branschIx--)
                {
                    if (XpLib.TechTree.bransches_all[branschIx].childNodeLevel < childLevel)
                    {
                        parentBranschIx = branschIx;
                        return true;
                    }
                }
            }
            parentBranschIx = -1;
            return false;
        }

        public void branschStatus(City city, int branschIx, out bool unlocked, out bool completed, out int nextNode)
        {
            completed = true;
            unlocked = true;
            nextNode = -1;
            var bransch = XpLib.TechTree.bransches_all[branschIx];
            for (int nIx = bransch.nodes.StartIndex; nIx < bransch.nodes.ExEndIndex; nIx++)
            {
                var node =  XpLib.TechTree.nodes_all[nIx];
                var progress = city.GetProgress(node.type);
                unlocked &= !bransch.locked || progress.points_bUnlocked_non_bComplete.Flag1;
                completed &= progress.points_bUnlocked_non_bComplete.Flag3;

                if (unlocked && !completed && nextNode < 0)
                {
                    nextNode = nIx;
                }
            }
        }

        public static TechFieldType ExperienceToTechField(WorkExperienceType experienceType)
        {
            switch (experienceType)
            {
                case WorkExperienceType.HouseBuilding:
                case WorkExperienceType.StoneCutter:
                    return TechFieldType.Buildings;

                case WorkExperienceType.Farm:
                case WorkExperienceType.AnimalCare:
                    return TechFieldType.FarmsAndAnimals;

                case WorkExperienceType.Mining:
                case WorkExperienceType.Smelting:
                case WorkExperienceType.CastMetal:
                    return TechFieldType.Casting;

                case WorkExperienceType.CraftMetal:
                //case WorkExperienceType.CraftWeapon:
                case WorkExperienceType.CraftArmor:
                    return TechFieldType.MetalWork;

                case WorkExperienceType.WoodWork:
                case WorkExperienceType.Fletcher:
                    return TechFieldType.Projectiles;

                case WorkExperienceType.CraftFuel:
                case WorkExperienceType.Chemistry:
                    return TechFieldType.Chemistry;

                default:
                    return TechFieldType.NUM_NONE;
                    //throw new ArgumentOutOfRangeException(nameof(experienceType), $"Unhandled experience type: {experienceType}");
            }
        }
    }

}
