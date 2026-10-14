using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.XP
{
    class TechTreeStructure
    {
        public TechTreeField ironField;

        public TechTreeStructure()
        {
            ironField = new TechTreeField()
            {
                techField = TechFieldType.Iron,

                branshes = new TechFieldBransch[]
                {
                     new TechFieldBransch() {
                        locked = false,
                        isOption = true,
                        nodes = new FlatArray_Eight<TechTreeNode>(
                            new TechTreeNode( TechNodeType.ironProduction, SpriteName.WarsResource_Iron, 400),
                            new TechTreeNode( TechNodeType.steelProduction, SpriteName.WarsResource_Steel, 400)
                            ),
                        //unlockBranshOnComplete = new FlatArray_Three<TechNodeType>(TechNodeType.steelsword, TechNodeType.steelarmor),
                    },


                    new TechFieldBransch() {
                        locked = false,
                        isOption = false,
                        nodes = new FlatArray_Eight<TechTreeNode>(
                            new TechTreeNode( TechNodeType.ironCraft, SpriteName.WarsBuild_Smith, 200)
                            ),
                        unlockBranshOnComplete = new FlatArray_Three<TechNodeType>(TechNodeType.shortsword, TechNodeType.lightIronArmor),
                    },

                    new TechFieldBransch() {
                        locked = true,
                        childNode = 1,
                        isOption = true,
                        nodes = new FlatArray_Eight<TechTreeNode>(
                            new TechTreeNode( TechNodeType.shortsword, SpriteName.WarsResource_ShortSword, 100),
                            new TechTreeNode( TechNodeType.sword, SpriteName.WarsResource_Sword, 100),
                            new TechTreeNode( TechNodeType.longsword, SpriteName.WarsResource_Longsword, 100)
                            ),
                        //unlockBranshOnComplete = new FlatArray_Three<TechNodeType>(TechNodeType.steel),
                    },

                    new TechFieldBransch() {
                        locked = true,
                        childNode = 1,
                        isOption = true,
                        nodes = new FlatArray_Eight<TechTreeNode>(
                            new TechTreeNode( TechNodeType.lightIronArmor, SpriteName.WarsResource_IronArmor, 100),
                            new TechTreeNode( TechNodeType.heavyIronArmor, SpriteName.WarsResource_HeavyIronArmor, 100),
                            new TechTreeNode( TechNodeType.steelarmor, SpriteName.WarsResource_LightPlateArmor, 100)
                            ),
                    },

                    new TechFieldBransch() {
                        locked = true,
                        childNode = 1,
                        isOption = true,
                        nodes = new FlatArray_Eight<TechTreeNode>(
                            new TechTreeNode( TechNodeType.wagon4, SpriteName.WarsResource_Wagon4Wheel, 100),
                            new TechTreeNode( TechNodeType.toolkit, SpriteName.WarsResource_Toolkit, 100),
                            new TechTreeNode( TechNodeType.ironwagon, SpriteName.WarsResource_WagonIron, 100),
                            new TechTreeNode( TechNodeType.steelwagon, SpriteName.WarsResource_WagonSteel, 100)
                            ),
                    },

                   
                },
            };
        }

        public void GetNode(TechNodeType nodeType, out TechTreeNode node, out TechFieldBransch bransch)
        {

            if (checkfield(ironField, out node, out bransch))
            {
                return;
            }
            throw new NotImplementedException("GetNode " + nodeType.ToString());

            bool checkfield(TechTreeField field, out TechTreeNode node, out TechFieldBransch bransch)
            {
                node = new TechTreeNode();
                bransch = new TechFieldBransch();

                foreach (var fieldbransh in field.branshes)
                {
                    if (fieldbransh.nodes.value1.type == nodeType)
                    {
                        bransch = fieldbransh;
                        node = fieldbransh.nodes.value1;
                        return true;
                        
                    }
                }

                return false;
            }
        }
    }

    class TechTreeField
    {
        public TechFieldType techField;

        public TechFieldBransch[] branshes;
    }

    struct TechFieldBransch
    {
        /// <summary>
        /// Starts locked under a parent bransh
        /// </summary>
        public bool locked;

        public int childNode;
        public bool isOption;
        public FlatArray_Eight<TechTreeNode> nodes;

        public  FlatArray_Three<TechNodeType> unlockBranshOnComplete;

        public SpriteName Icon()
        {
            return nodes.value1.icon;
        }
    }

    struct TechTreeNode
    {
        public SpriteName icon;
        public TechNodeType type;
        public int pointsCost;

        public TechTreeNode(TechNodeType type, SpriteName icon, int pointsCost)
        { 
            this.icon = icon;
            this.type = type;
            this.pointsCost = pointsCost;
        }
    }

    struct TechTreeNodeProgress_City
    {
        public static readonly int NodeCount = (int)TechNodeType.NUM_NONE;
        public UInt12_flag4 points_bUnlocked_bSelected_bComplete;

        public bool Complete()
        {
            return points_bUnlocked_bSelected_bComplete.Flag3;
        }
        public bool Selected { get { return points_bUnlocked_bSelected_bComplete.Flag2; } }
    }

    struct TechTreeNodeProgress_Faction
    {
        public const ushort FactionWideUnlock = ushort.MaxValue;
        public ushort cityUnlocks;
    }

    enum TechFieldType
    { 
        Buildings,
        Casting,
        Iron,
        Bow,
        Chemistry,
    }

    enum TechNodeType
    {
        ironProduction,
        steelProduction,

        ironCraft,
        shortsword,
        sword,
        longsword,
        steelsword,
        lightIronArmor,
        heavyIronArmor,
        steelarmor,

        wagon4,

        toolkit,

        ironwagon,
        steelwagon,


        NUM_NONE
    }
}
