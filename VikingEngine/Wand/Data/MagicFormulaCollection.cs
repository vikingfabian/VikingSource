using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace VikingEngine.Core.Wand.Data
{
    class MagicFormulaCollection
    {
        static MagicFormula SmallDamage = new MagicFormula(MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Damage, 1);
        static MagicFormula MediumDamage = new MagicFormula(MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Damage, 2);

        static MagicFormula ThinWallSmasher = new MagicFormula(MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Thin_Wall, MagicEffect.Open_wall, 1);
        static MagicFormula Thin2WallSmasher = new MagicFormula(MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Thin_Wall, MagicEffect.Open_wall, 2);
        static MagicFormula WallSmasher = new MagicFormula(MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Any_Wall, MagicEffect.Open_wall, 1);


        List<MagicFormula> collection;

        public MagicFormulaCollection()
        {
            collection = new List<MagicFormula>
            {
                SmallDamage,
                MediumDamage,
                ThinWallSmasher,
                WallSmasher,
            };
        }

        public List2<MagicFormula> CreateMagicEffectList(int count)
        {
            List2<MagicFormula> formulas = new List2<MagicFormula>(count);

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

            return formulas;
        }

        bool canBeCombined(MagicFormula form1, MagicFormula form2)
        {
            return form1.effect != form2.effect;
        }
    }
}
