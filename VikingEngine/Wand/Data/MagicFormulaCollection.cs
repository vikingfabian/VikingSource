using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace VikingEngine.Core.Wand.Data
{
    class MagicFormulaCollection
    {
        static MagicFormula Heal = new MagicFormula("Heal", MagicTrigger.OnComplete, MagicTrigger_ObjectType.None, MagicEffect.Heal_user, 1);
        static MagicFormula HealTarget = new MagicFormula("Heal target", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Heal_target, 2);
        static MagicFormula SmallDamage = new MagicFormula("Small damage", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Damage, 1);
        static MagicFormula MediumDamage = new MagicFormula("Medium damage", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Damage, 2);
        static MagicFormula MassiveDamage = new MagicFormula("Massive damage", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Damage, 5);
        static MagicFormula Backfire = new MagicFormula("Backfire", MagicTrigger.OnComplete, MagicTrigger_ObjectType.None, MagicEffect.Back_fire, 1);
        static MagicFormula Leecher = new MagicFormula("Leecher", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Leech, 2);
        static MagicFormula Executer = new MagicFormula("Executer", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Execute_damaged, MagicFormula.NoTier);
        static MagicFormula Explosion = new MagicFormula("Explosion", MagicTrigger.OnComplete, MagicTrigger_ObjectType.None, MagicEffect.Explode, 1);
        static MagicFormula BigExplosion = new MagicFormula("Big Explosion", MagicTrigger.OnComplete, MagicTrigger_ObjectType.None, MagicEffect.Explode, 3);
        static MagicFormula Teleport = new MagicFormula("Teleport", MagicTrigger.OnComplete, MagicTrigger_ObjectType.None, MagicEffect.Teleport_user, MagicFormula.NoTier);

        static MagicFormula ThinWallSmasher = new MagicFormula("Thin wall smasher", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Thin_Wall, MagicEffect.Open_wall, 1);
        static MagicFormula Thin2WallSmasher = new MagicFormula("Thin wall smasher II", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Thin_Wall, MagicEffect.Open_wall, 2);
        static MagicFormula WallSmasher = new MagicFormula("Wall smasher", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Any_Wall, MagicEffect.Open_wall, 1);

        static MagicFormula ExplosiveBlood = new MagicFormula("Explosive blood", MagicTrigger.SpendFluid, MagicTrigger_ObjectType.Blood, MagicEffect.Explode, 1);
        static MagicFormula SuckBlood = new MagicFormula("Suck blood", MagicTrigger.SpendFluid, MagicTrigger_ObjectType.Blood, MagicEffect.Heal_user, 1);
        static MagicFormula BloodMagic = new MagicFormula("Blood magic", MagicTrigger.SpendFluid, MagicTrigger_ObjectType.Blood, MagicEffect.Magic_up, 1);
        static MagicFormula BloodRevival = new MagicFormula("Blood revival", MagicTrigger.SpendFluid, MagicTrigger_ObjectType.Blood, MagicEffect.Heal_target, 1);
        static MagicFormula EndOnCreature = new MagicFormula("End on creature", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.End, MagicFormula.NoTier);
        static MagicFormula Cloner = new MagicFormula("Cloner", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Any, MagicEffect.Clone, 1);
        static MagicFormula Upper = new MagicFormula("Upper", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Any, MagicEffect.Level_up, 1);

        static MagicFormula BloodActivate = new MagicFormula("Blood activate", MagicTrigger.SpendFluid, MagicTrigger_ObjectType.Blood, MagicEffect.Activate, 1);
        static MagicFormula CreatureActivate = new MagicFormula("Creature activate", MagicTrigger.PassObject, MagicTrigger_ObjectType.Creature, MagicEffect.Activate, 1);
        static MagicFormula ThinWallActivate = new MagicFormula("Wall activate", MagicTrigger.PassObject, MagicTrigger_ObjectType.Thin_Wall, MagicEffect.Activate, 1);
        static MagicFormula NorthActivate = new MagicFormula("North fire", MagicTrigger.Fire_north, MagicTrigger_ObjectType.None, MagicEffect.Activate, 1);
        static MagicFormula SouthActivate = new MagicFormula("South fire", MagicTrigger.Fire_south, MagicTrigger_ObjectType.None, MagicEffect.Activate, 1);
        List<MagicFormula> activators;

        List<MagicFormula> collection;

        public MagicFormulaCollection()
        {
            activators = new List<MagicFormula>
            {
                BloodActivate,
                CreatureActivate,
                ThinWallActivate,
                NorthActivate,
                SouthActivate,
            };

            collection = new List<MagicFormula>
            {
                Heal,
                HealTarget,
                SmallDamage,
                MediumDamage,
                MassiveDamage,
                Backfire,
                Leecher,
                Executer,
                ThinWallSmasher,
                WallSmasher,
                Explosion,
                BigExplosion,
                Teleport,
                ExplosiveBlood,
                SuckBlood,
                //BloodMagic,
                BloodRevival,
                EndOnCreature,
                Cloner,
                Upper,
            };
        }

        public List2<MagicFormula> CreateMagicEffectList(int count)
        {
            List2<MagicFormula> formulas = new List2<MagicFormula>(count);

            if (Ref.rnd.Chance(0.25))
            {
                var activation = arraylib.RandomListMember(activators);
                formulas.Add(activation);
            }

            if (Ref.rnd.Chance(0.1))
            {
                formulas.Add(BloodMagic);
            }

            AddToCount(count, ref formulas);

            return formulas;
        }

        private void AddToCount(int count, ref List2<MagicFormula> formulas)
        {
            int maxLoops = 100;
            while (formulas.Count < count && maxLoops > 0)
            {
                --maxLoops;
                var form = arraylib.RandomListMember(collection);

                bool canBeUsed = true;

                for (int i = 0; i < formulas.Count; ++i)
                {
                    if (!canBeCombined(form, formulas.Array[i]))
                    {
                        canBeUsed = false;
                        break;
                    }
                }

                if (canBeUsed)
                {
                    formulas.Add(form);
                }
            }
        }

        public void AddToEffectList(ref List2<MagicFormula> list)
        {
            AddToCount( list.Count + 1, ref list);
        }

        bool canBeCombined(MagicFormula form1, MagicFormula form2)
        {
            return form1.effect != form2.effect;
        }
    }
}
