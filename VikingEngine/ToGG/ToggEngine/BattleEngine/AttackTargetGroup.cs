using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Battle;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{    
    class AttackTargetGroup : List2<AttackTarget>
    {
        public AttackSettings attackSettings;

        public AttackTargetGroup(List<AttackTarget> units, bool isMelee)
            :base(units.Count)
        {
            foreach (var m in units)
            {
                m.setAttackType(isMelee);
                Add(m);
            }
            selectFirst();
        }

        public AttackTargetGroup(List<AbsUnit> units, bool isMelee)
            :base(units.Count)
        {
            foreach (var m in units)
            {
                Add(new AttackTarget(m, isMelee));
            }
            selectFirst();
        }

        public AttackTargetGroup(AttackTarget target)
            :base(1)
        {
            if (target != null)
            {
                Add(target);
                selectFirst();
            }
        }

        public AttackTargetGroup(List<AttackTarget> targets)
            :base(targets.Count)
        {
            AddRange(targets);
            selectFirst();
        }

        public void clearOutInvalid()
        {
            loopBegin(false);
            while (loopNext())
            {
                if (sel.unit == null)
                {
                    loopRemove();
                }
            }

            selectFirst();
        }

        public void write(System.IO.BinaryWriter w)
        {
            //AttackType.write(w);

            w.Write((byte)Count);

            foreach (var m in this)
            {
                m.write(w);
            }

            //if (toggRef.mode == GameMode.HeroQuest)
            //{
            //    foreach (var m in this)
            //    {
            //        ((HeroQuest.Unit)m.unit).netWriteUnit(w);
            //    }
            //}
        }

        public AttackTargetGroup(System.IO.BinaryReader r)
        {
            //Battle.AttackType attacktype = new Battle.AttackType();
            //attacktype.read(r);

            int listCount = r.ReadByte();
            Clear();

            for (int i = 0; i < listCount; ++i)
            {
                Add(new AttackTarget(r));
            }
            //if (toggRef.mode == GameMode.HeroQuest)
            //{
            //    for (int i = 0; i < listCount; ++i)
            //    {
            //        var u = HeroQuest.Unit.NetReadUnit(r);
            //        Add(new AttackTarget(u, attacktype));
            //    }
            //}
        }

        public AttackTarget Get(AbsUnit unit)
        {
            foreach (var m in this)
            {
                if (m.unit == unit)
                {
                    return m;
                }
            }
            return null;
        }

        public bool IsMelee => sel.attackType.IsMelee;

        public bool IsProjectile => sel.attackType.IsRanged;

        public bool HasMods => attackSettings != null && attackSettings.modifications != null;

        public AttackType AttackType
        {
            get { return sel.attackType; }
        }

        public List<AbsUnit> listUnits()
        {
            List<AbsUnit> result = new List<AbsUnit>(this.Count);

            foreach (var m in this)
            {
                result.Add(m.unit);
            }

            return result;
        }

        public AttackTargetType attackTargetType()
        {
            AttackTargetType type = this[0].type;
            for (int i = 1; i < this.Count; ++i)
            {
                if (this[i].type != type)
                {
                    type = AttackTargetType.Mixed;
                }
            }

            return type;
        }

        public bool LosTerrainTarget => attackTargetType() != AttackTargetType.Unit;
    }
    class AttackSettings
    {
        public List<AbsBattleModification> modifications = null;
        public int attackStrength;
        public HeroQuest.Gadgets.AbsWeapon weapon = null;

        public AttackSettings(int attackStrength, HeroQuest.Gadgets.AbsWeapon weapon)
        {
            this.attackStrength = attackStrength;
            this.weapon = weapon;
        }

        public void Add(AbsBattleModification mod)
        {
            arraylib.AddOrCreate(ref modifications, mod);
        }
    }
}
