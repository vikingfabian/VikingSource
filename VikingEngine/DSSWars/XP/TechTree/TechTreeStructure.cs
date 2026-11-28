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

        public List2<TechTreeField> fields;
        public List2<TechFieldBransch> bransches_all;
        public List2<TechTreeNode> nodes_all;

        public TechTreeStructure()
        {
            bransches_all = new List2<TechFieldBransch>(32);
            nodes_all = new List2<TechTreeNode>((int)TechNodeType.NUM_NONE);

            double dPoints = DefaultProcessPoints / DssRef.storage.ruleset_instance.TechMultiProperty(null, false, 0);

            fields = new List2<TechTreeField>((int)TechFieldType.NUM_NONE);

            buildingFieldStructure(dPoints);
            farmFieldStructure(dPoints);
            castFieldStructure(dPoints);
            metalFieldStructure(dPoints);
            woodFieldStructure(dPoints);
            chemistryFieldStructure(dPoints);

            for (int fieldIx = 0; fieldIx < fields.Array.Length; fieldIx++)
            {
                var field = fields.Array[fieldIx];
                int[] currentParentOnLevel = new int[4];

                for (int branschIx = field.branshes.StartIndex; branschIx < field.branshes.ExEndIndex; branschIx++)
                {
                    bransches_all.Array[branschIx].inFieldIndex = fieldIx;
                    
                    var bransch = bransches_all[branschIx];
                    for (int nodeIx = bransch.nodes.StartIndex; nodeIx < bransch.nodes.ExEndIndex; nodeIx++)
                    {
                        nodes_all[nodeIx].inBranschIndex = branschIx;
                    }
                }
            }
        }
        void buildingFieldStructure(double dPoints)
        {
            var field = new TechTreeField() { techField = TechFieldType.Buildings };
            {
                field.branshes = new PartialList2<TechFieldBransch>(bransches_all);
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = false,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.Logistics, 0.4 * dPoints),
                            new TechTreeNode(TechNodeType.CraftBuildings, 0.6 * dPoints),
                            new TechTreeNode(TechNodeType.Logistics2, 1 * dPoints)
                            )
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.CityHall_village, 1 * dPoints),
                            new TechTreeNode(TechNodeType.CityHall_town, 1.2 * dPoints),
                            new TechTreeNode(TechNodeType.CityHall_capital, 1.4 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.ImmigrantTent, 0.4 * dPoints),
                            new TechTreeNode(TechNodeType.Delivery, 1 * dPoints),
                            new TechTreeNode(TechNodeType.DeliveryUpgraded, 1.4 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.CommonHall, 1 * dPoints),
                            new TechTreeNode(TechNodeType.LordSeat1, 1.2 * dPoints),
                            new TechTreeNode(TechNodeType.LordSeat2, 1.4 * dPoints),
                            new TechTreeNode(TechNodeType.LordSeat3, 1.6 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.Stakes, 0.5 * dPoints),
                            new TechTreeNode(TechNodeType.WoodWall, 1 * dPoints),
                            new TechTreeNode(TechNodeType.StoneWall, 1.4 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.NobelHouse, 1 * dPoints),
                            new TechTreeNode(TechNodeType.Embassy, 1.2 * dPoints),
                            new TechTreeNode(TechNodeType.Bank, 1.4 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.GravelRoad, 1 * dPoints),
                            new TechTreeNode(TechNodeType.CobblestoneRoad, 1.2 * dPoints),
                            new TechTreeNode(TechNodeType.PavedRoad, 1.4 * dPoints),
                            new TechTreeNode(TechNodeType.DecorRoad, 1.8 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
            }
            fields.Array[(int)field.techField] = field;
        }

        void farmFieldStructure(double dPoints)
        {
            var field = new TechTreeField() { techField = TechFieldType.FarmsAndAnimals };
            {
                field.branshes = new PartialList2<TechFieldBransch>(bransches_all);
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = false,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.Trapper, 1 * dPoints))
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.PenTier1, 1 * dPoints),
                            new TechTreeNode(TechNodeType.PenTier2, 1.2 * dPoints),
                            new TechTreeNode(TechNodeType.DraftPen, 1.4 * dPoints),
                            new TechTreeNode(TechNodeType.WarPen, 1.6 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.WildPenTier1, 0.4 * dPoints),
                            new TechTreeNode(TechNodeType.WildPenTier2, 0.6 * dPoints),
                            new TechTreeNode(TechNodeType.WildWarPen, 0.8 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.BasePlant, 1 * dPoints),
                            new TechTreeNode(TechNodeType.Hemp, 1.2 * dPoints),
                            new TechTreeNode(TechNodeType.TreePlant, 1.4 * dPoints),
                            new TechTreeNode(TechNodeType.UpgradedFarm, 0.6 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.GardenGrass, 1.2 * dPoints),
                            new TechTreeNode(TechNodeType.GardenPlants, 1.4 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
            }
            fields.Array[(int)field.techField] = field;
        }

        void castFieldStructure(double dPoints)
        {
            var field = new TechTreeField() { techField = TechFieldType.Casting };
            {
                field.branshes = new PartialList2<TechFieldBransch>(bransches_all);
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.CastCopper, 0.4 * dPoints),
                            new TechTreeNode(TechNodeType.CastBronze, 0.8 * dPoints),
                            new TechTreeNode(TechNodeType.CastBronzeGun, 3 * dPoints),
                            new TechTreeNode(TechNodeType.CastBronzeCannon, 2 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.CastIron, 1 * dPoints),
                            new TechTreeNode(TechNodeType.CastIronGun, 5 * dPoints),
                            new TechTreeNode(TechNodeType.CastIronCannon, 3 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.CastSilverGold, 0.5 * dPoints),
                            new TechTreeNode(TechNodeType.CastTinLead, 1 * dPoints),
                            new TechTreeNode(TechNodeType.CastMithril, 3 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
            }
            fields.Array[(int)field.techField] = field;
        }

        void metalFieldStructure(double dPoints)
        {
            var field = new TechTreeField() { techField = TechFieldType.MetalWork };
            {
                field.branshes = new PartialList2<TechFieldBransch>(bransches_all);
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = false,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.ironProduction, 4 * dPoints),
                            new TechTreeNode(TechNodeType.steelProduction, 6 * dPoints)
                        )
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = false,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.ironCraft_base, 2 * dPoints))
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.shortsword, 1 * dPoints),
                            new TechTreeNode(TechNodeType.sword, 1.2 * dPoints),
                            new TechTreeNode(TechNodeType.longsword, 1.4 * dPoints),
                            new TechTreeNode(TechNodeType.zweihänder, 1.6 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.HeaterShield, 0.4 * dPoints),
                            new TechTreeNode(TechNodeType.TowerShield, 0.6 * dPoints),
                            new TechTreeNode(TechNodeType.BucklerShield, 0.8 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.lightIronArmor, 1 * dPoints),
                            new TechTreeNode(TechNodeType.heavyIronArmor, 1.2 * dPoints),
                            new TechTreeNode(TechNodeType.steelarmor, 1.4 * dPoints),
                            new TechTreeNode(TechNodeType.warhammer, 0.6 * dPoints),
                            new TechTreeNode(TechNodeType.heavySteelarmor, 1.8 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = false,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.mechanics_base, 2 * dPoints)
                        )
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.wagon4, 0.8 * dPoints),
                            new TechTreeNode(TechNodeType.wagonClosed, 1 * dPoints),
                            new TechTreeNode(TechNodeType.ironwagon, 1.2 * dPoints),
                            new TechTreeNode(TechNodeType.steelwagon, 1.4 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.toolkit, 2 * dPoints),
                            new TechTreeNode(TechNodeType.NutLockMechanic, 4 * dPoints),
                            new TechTreeNode(TechNodeType.MatchLockMechanic, 5 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
            }
            fields.Array[(int)field.techField] = field;
        }

        void woodFieldStructure(double dPoints)
        {
            var field = new TechTreeField() { techField = TechFieldType.Wood };
            {
                field.branshes = new PartialList2<TechFieldBransch>(bransches_all);
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = false,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.woodBase, 0.6 * dPoints))
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.javelin, 0.6 * dPoints),
                            new TechTreeNode(TechNodeType.shortBow, 1.2 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 2,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.LongBow, 2 * dPoints),
                            new TechTreeNode(TechNodeType.CrossBow, 3 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 2,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.ballista, 1 * dPoints),
                            new TechTreeNode(TechNodeType.catapult, 2 * dPoints),
                            new TechTreeNode(TechNodeType.manuballista, 2.2 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
            }
            fields.Array[(int)field.techField] = field;
        }

        void chemistryFieldStructure(double dPoints)
        {
            var field = new TechTreeField() { techField = TechFieldType.Chemistry };
            {
                field.branshes = new PartialList2<TechFieldBransch>(bransches_all);
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = false,
                        childNodeLevel = 0,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.coalPit, 0.4 * dPoints))
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.foodConservation, 0.8 * dPoints),
                            new TechTreeNode(TechNodeType.foodStorage, 1.2 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.beer, 1 * dPoints),
                            new TechTreeNode(TechNodeType.coolingFluid, 1.4 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }

                {
                    var branch = new TechFieldBransch()
                    {
                        locked = true,
                        childNodeLevel = 1,
                        isOption = true,
                        nodes = new PartialList2<TechTreeNode>(ref nodes_all,
                            new TechTreeNode(TechNodeType.blackPowder, 6 * dPoints),
                            new TechTreeNode(TechNodeType.gunPowder, 10 * dPoints)
                            ),
                    };

                    field.branshes.Add(ref bransches_all, branch);
                }
            }
            fields.Array[(int)field.techField] = field;
        }
        public bool findNode(TechFieldType fieldtype, TechNodeType nodeType, out TechTreeNode node, 
            out int branschIx, out int nodeIx)
        {
            int start, exend;
            if (fieldtype == TechFieldType.NUM_NONE)
            {
                start = 0; exend = XpLib.TechTree.bransches_all.Count;
            }
            else
            {
                var field = XpLib.TechTree.fields.Array[(int)fieldtype];
                start = field.branshes.StartIndex;
                exend = field.branshes.ExEndIndex;
            }

            for (branschIx = start; branschIx < exend; branschIx++)
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
                    return TechFieldType.Wood;

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
