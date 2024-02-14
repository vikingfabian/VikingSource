using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    struct AttackRollResult
    {
        public int hits;
        public int critiqualHits;
        public int pierce;

        public int surges;
        public int retreats;

        public AttackRollResult(System.IO.BinaryReader r)
            : this()
        {
            read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)hits);
            w.Write((byte)critiqualHits);
            w.Write((byte)pierce);
            w.Write((byte)surges);
        }

        public void read(System.IO.BinaryReader r)
        {
            hits = r.ReadByte();
            critiqualHits = r.ReadByte();
            pierce = r.ReadByte();
            surges = r.ReadByte();
        }
    }

    struct DefenceRollResult
    {
        public int blocks;
        public bool dodge;
    }

    class DefenderRoll
    {
        //public Unit unit;
        public ToggEngine.BattleEngine.AttackRollDiceDisplay rollDisplay;
        public DefenceRollResult result = new DefenceRollResult();
        public bool gotFirstDefenceUpdate = false;
        public AttackTarget target;

        public DefenderRoll(AttackTarget target)
        {
            this.target = target;
            //unit = target.unit as Unit;
        }

        public void DeleteMe()
        {
            rollDisplay.DeleteMe();
        }

        public AbsUnit unit => target.unit;
    }
}
