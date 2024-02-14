using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Players;

namespace VikingEngine.LootFest.Data
{
    class LootBoxes
    {
        

        public static readonly LootBoxData[] Boxes = new LootBoxData[]
        {
            new LootBoxData(LootBoxType.Hat, (int)HatType.None, LootBoxUnlockGroup.StartsUnlocked),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Vendel, LootBoxUnlockGroup.StartsUnlocked),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Viking1, LootBoxUnlockGroup.StartsUnlocked),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Viking2, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Viking3, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Viking4, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Knight, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Spartan, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Football, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Witch, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Cap, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Pirate1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Pirate2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Pirate3, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Girly1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Girly2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Archer,  LootBoxUnlockGroup.StartsUnlocked),

            new LootBoxData(LootBoxType.Hat, (int)HatType.WolfHead,  LootBoxUnlockGroup.StartsUnlocked),
            new LootBoxData(LootBoxType.Hat, (int)HatType.BearHead,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.PoodleHead, LootBoxUnlockGroup.First),

            new LootBoxData(LootBoxType.Hat, (int)HatType.Future1,  LootBoxUnlockGroup.StartsUnlocked),
            new LootBoxData(LootBoxType.Hat, (int)HatType.FutureMask1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.FutureMask2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.GasMask, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Baby1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Baby2, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Santa1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Santa2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Santa3, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Arrow,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Coif1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Coif2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.High1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.High2, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Hunter1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Hunter2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Hunter3, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Low1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Low2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Mini1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Mini2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Mini3, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Turban1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Turban2, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Headband1,  LootBoxUnlockGroup.StartsUnlocked),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Headband2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Headband3, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.MaskTurtle1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.MaskZorro1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.MaskZorro2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Zelda, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Cone1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Cone2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Cone3,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Cone4, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.crown1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.crown2,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.crown3, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.princess1,  LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.princess2, LootBoxUnlockGroup.First),
            new LootBoxData(LootBoxType.Hat, (int)HatType.Bucket, LootBoxUnlockGroup.First),
        };

        static Dictionary<int, int> hashToIndex = null;

        public int[] unlockedBoxes;

        public LootBoxes()
        {
            if (hashToIndex == null)
            {
                hashToIndex = new Dictionary<int,int>(Boxes.Length);

                for (int i = 0; i < Boxes.Length; ++i)
                {
                    hashToIndex.Add(LootBoxData.Hash(Boxes[i].type, Boxes[i].underType), i);
                }
            }

            unlockedBoxes = new int[Boxes.Length];
            for (int i = 0; i < Boxes.Length; ++i)
            {
                if (Boxes[i].group == LootBoxUnlockGroup.StartsUnlocked)
                {
                    unlockedBoxes[i] = Boxes[i].maxUnlockCount;
                }
            }

        }

        public bool unlocked(LootBoxType type, int underType)
        {
            if (DebugSett.DebugAllAppearanceUnlocked && PlatformSettings.DevBuild)
            {
                return true;
            }

            int ix = hashToIndex[LootBoxData.Hash(type, underType)];
            return unlockedBoxes[ix] > 0;
        }

        public void unlockedItemsToList<TEnum>(LootBoxType type, List<TEnum> checklist, List<TEnum> addToList)
        {
            foreach (var m in checklist)
            {
                int underType = m.GetHashCode();

                if (unlocked(type, underType))
                {
                    addToList.Add(m);
                }
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(unlockedBoxes.Length);
            for (int i = 0; i < unlockedBoxes.Length; ++i)
            {
                w.Write(LootBoxData.Hash(Boxes[i].type, Boxes[i].underType));
                w.Write((byte)unlockedBoxes[i]);
            }
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            int length = r.ReadInt32();
            for (int i = 0; i < length; ++i)
            {
                int hash = r.ReadInt32();
                int value = r.ReadByte();

                int ix = hashToIndex[hash];
                if (Boxes[ix].group != LootBoxUnlockGroup.StartsUnlocked)
                {
                    unlockedBoxes[ix] = value;
                }
            }
        }
    }

    struct LootBoxData
    {
        public static readonly LootBoxData Empty = new LootBoxData(LootBoxType.Empty, 0, LootBoxUnlockGroup.Empty, 0);

        public LootBoxType type;
        public int underType;

        //public int unlocked;
        public int maxUnlockCount;

        //public int hash;
        public LootBoxUnlockGroup group;

        public LootBoxData(LootBoxType type, int underType, LootBoxUnlockGroup group, int maxUnlockCount = 1)
        {
            this.type = type;
            this.underType = underType;
            this.maxUnlockCount = maxUnlockCount;
            //unlocked = 0;
            this.group = group;
        }

        public static int Hash(LootBoxType type, int underType)
        {
            return (int)type * 1000 + underType;
        }
    }

    enum LootBoxType
    {
        Empty,

        Hat,
        Hair,
        Eyes,
        Mouth,
        Belt,

        Expression,
        UnlockCape,
    }

    enum LootBoxUnlockGroup
    {
        Empty,

        StartsUnlocked,
        First,
        Second,
        Gold,
        NUM,
    }
}
