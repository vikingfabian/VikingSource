using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Engine;

namespace VikingEngine.Core.DSSWars.XP.TechTree
{

    struct TechTreeField
    {
        public TechFieldType techField;

        public PartialList2<TechFieldBransch> branshes;
    }

    struct TechFieldBransch
    {
        public int inFieldIndex;

        /// <summary>
        /// Starts locked under a parent bransh
        /// </summary>
        public bool locked;

        public int childNodeLevel;
        public bool isOption;
        public PartialList2<TechTreeNode> nodes;
        public PartialList2<TechNodeType> unlockBranshOnComplete;

    }

    struct TechTreeNode
    {
        public const int NoParent = -1;
        public int inBranschIndex;
        public SpriteName icon;
        public TechNodeType type;
        public int pointsCost;

        public TechTreeNode(TechNodeType type, SpriteName icon, double pointsCost)
        {
            this.icon = icon;
            this.type = type;
            this.pointsCost = (int)pointsCost;
#if DEBUG
            if (this.pointsCost > UInt12_flag4.Max12BitValue)
            {
                throw new ArgumentOutOfRangeException();
            }
#endif
        }
    }


    struct TechTreeNodeProgress_Faction
    {
        public const ushort FactionWideUnlock = ushort.MaxValue;
        public ushort cityUnlocks;
    }

    enum TechFieldType
    {
        Buildings,
        FarmsAndAnimals,
        Casting,
        MetalWork,
        Projectiles,
        Chemistry,
        NUM_NONE
    }


    struct FieldResearch
    {
        public TechNodeType selected;
        public TechNodeType parent;
        public ushort pointsNeeded;

        public FieldResearch(TechTreeNode node)
        {
            selected = node.type;
            parent = selected;
            pointsNeeded = (ushort)node.pointsCost;
        }
    }

    enum TechNodeType : byte
    {
        ironProduction,
        steelProduction,
        mithrilProduction,

        ironCraft_base,
        shortsword,
        sword,
        longsword,
        steelsword,
        zweihänder,

        HeaterShield, BucklerShield, TowerShield,

        lightIronArmor,
        heavyIronArmor,
        steelarmor,
        warhammer,
        heavySteelarmor,

        mechanics_base,
        wagon4,
        wagonClosed,
        ironwagon,
        steelwagon,


        toolkit,
        NutLockMechanic,
        MatchLockMechanic,

        NUM_NONE
    }
}
