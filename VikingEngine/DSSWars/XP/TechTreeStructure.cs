using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.EngineSpace.Engine.Extensions.Struct;

namespace VikingEngine.DSSWars.XP
{
    class TechTreeStructure
    {
        TechTreeField ironField;

        public TechTreeStructure() => ironField = new TechTreeField()
        {
            techField = TechFieldType.Iron,
            branshes = new TechFieldBransch[]
            {
                new TechFieldBransch() { 
                    locked = false, 
                    isOption = false,
                    nodes = new FlatArray_Eight<TechTreeNode>(
                        new TechTreeNode( TechNodeType.iron, 200)
                        ),
                    unlockBranshOnComplete = new FlatArray_Three<TechNodeType>(TechNodeType.shortsword, TechNodeType.lightIronArmor),
                },

                new TechFieldBransch() {
                    locked = true,
                    isOption = true,
                    nodes = new FlatArray_Eight<TechTreeNode>(
                        new TechTreeNode( TechNodeType.shortsword, 100),
                        new TechTreeNode( TechNodeType.sword, 100),
                        new TechTreeNode( TechNodeType.longsword, 100)
                        ),
                    unlockBranshOnComplete = new FlatArray_Three<TechNodeType>(TechNodeType.steel),
                },

                new TechFieldBransch() {
                    locked = true,
                    isOption = true,
                    nodes = new FlatArray_Eight<TechTreeNode>(
                        new TechTreeNode( TechNodeType.lightIronArmor, 100),
                        new TechTreeNode( TechNodeType.heavyIronArmor, 100)
                        ),
                    unlockBranshOnComplete = new FlatArray_Three<TechNodeType>(TechNodeType.steel),
                },

                new TechFieldBransch() {
                    locked = true,
                    isOption = false,
                    nodes = new FlatArray_Eight<TechTreeNode>(
                        new TechTreeNode( TechNodeType.steel, 400)
                        ),
                    unlockBranshOnComplete = new FlatArray_Three<TechNodeType>(TechNodeType.steelsword, TechNodeType.steelarmor),
                },
            },
        };
    }

    class TechTreeField
    {
        public TechFieldType techField;

        public TechFieldBransch[] branshes;
    }

    struct TechFieldBransch
    {
        public bool locked;
        public bool isOption;
        public FlatArray_Eight<TechTreeNode> nodes;

        public  FlatArray_Three<TechNodeType> unlockBranshOnComplete;
    }

    struct TechTreeNode
    {
        public TechNodeType type;
        public int pointsCost;

        public TechTreeNode(TechNodeType type, int pointsCost)
        { 
            this.type = type;
            this.pointsCost = pointsCost;
        }
    }

    struct TechTreeNodeProgress
    {
        public static readonly int NodeCount = (int)TechNodeType.NUM_NONE;
        public UInt12_flag4 points_bUnlocked_bSelected_bComplete;
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
        iron,
        
        shortsword,
        sword,
        longsword,

        lightIronArmor,
        heavyIronArmor,

        steel,

        steelsword,
        steelarmor,

        NUM_NONE
    }
}
