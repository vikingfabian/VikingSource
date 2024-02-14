using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class AttackTarget
    {
        public static List<AttackTarget> Conv(List<AbsUnit> units)
        {
            List<AttackTarget> targets = new List<AttackTarget>(units.Count);
            foreach (var m in units)
            {
                targets.Add(new AttackTarget(m));
            }
            return targets;
        }

        public static List<AttackTarget> Conv(SpottedArrayCounter<AbsUnit> units)
        {
            List<AttackTarget> targets = new List<AttackTarget>(units.array.Count);
            while (units.Next())
            {
                targets.Add(new AttackTarget(units.sel));
            }
            return targets;
        }

        public AttackTargetType type = AttackTargetType.Unit;
        public AbsUnit unit;
        public HeroQuest.AbsTrap terrain;
        public AttackType attackType;
        public IntVector2 position = IntVector2.NegativeOne;
        public IntVector2 groupAttackStart = IntVector2.NegativeOne;
        public bool groupHasValidTarget = false;

        public WalkingPath path = null;

        public HeroQuest.RecordedDamageEvent recordedDamage = null;

        public AttackTarget(AbsUnit unit, bool isMelee)
            : this(unit, isMelee ? AttackType.Melee : AttackType.Ranged)
        {
        }

        public AttackTarget(AbsUnit unit, AttackType attackType)
        {
            this.unit = unit;
            if (unit != null)
            {
                this.position = unit.squarePos;
            }
            this.attackType = attackType;
        }

        public AttackTarget(AbsUnit unit)
        {
            this.unit = unit;
            if (unit != null)
            {
                this.position = unit.squarePos;
            }
        }

        public AttackTarget(IntVector2 position, AttackTargetType type = AttackTargetType.Unit)
        {
            this.type = type;
            this.position = position;
            this.unit = null;
            this.attackType = AttackType.Melee;
        }

        public AttackTarget(HeroQuest.AbsTrap terrain)
        {
            this.position = terrain.position;
            type = AttackTargetType.Terrain;
            attackType = AttackType.Melee;
        }

        public AttackTarget(System.IO.BinaryReader r)
        {
            read(r);
        }

        public void setAttackType(bool isMelee)
        {
            attackType = isMelee ? AttackType.Melee : AttackType.Ranged;
        }

        public override string ToString()
        {
            string targetName = TextLib.EmptyString;

            switch (type)
            {
                case AttackTargetType.Unit:
                    if (unit != null)
                    {
                        targetName = unit.NameAndId();
                    }
                    else
                    {
                        targetName = "Non";
                    }
                    break;

                case AttackTargetType.Objective:
                    targetName = "Objective" + position.ToString();
                    break;
            }

            return "Target " + targetName + " - " + attackType.ToString();
        }

        public float StrengthValue()
        {
            if (type == AttackTargetType.Unit)
            {
                return unit.StrengthValue();
            }
            else
            {
                return 0;
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)type);
            attackType.write(w);

            switch (type)
            {
                case AttackTargetType.Unit:
                    if (toggRef.mode == GameMode.HeroQuest)
                    {
                        unit.hq().netWriteUnitId(w);
                    }
                    break;
                case AttackTargetType.Objective:
                    toggRef.board.WritePosition(w, position);
                    break;
            }
        }

        public void read(System.IO.BinaryReader r)
        {
            type = (AttackTargetType)r.ReadByte();
            attackType.read(r);

            switch (type)
            {
                case AttackTargetType.Unit:
                    if (toggRef.mode == GameMode.HeroQuest)
                    {
                        unit = HeroQuest.Unit.NetReadUnitId(r);
                        if (unit != null)
                        {
                            position = unit.squarePos;
                        }
                    }
                    break;
                case AttackTargetType.Objective:
                    position = toggRef.board.ReadPosition(r);
                    break;
            }
        }
        
        public override bool Equals(object obj)
        {
            return ((AttackTarget)obj).position == this.position;
        }

        public bool LosTerrainTarget => type != AttackTargetType.Unit;

        //public bool IsMelee => attackType.IsMelee == Battle.AttackType.Melee;
    }

    enum AttackTargetType
    {
        Unit,
        Terrain,
        Objective,

        Mixed,
    }
}
