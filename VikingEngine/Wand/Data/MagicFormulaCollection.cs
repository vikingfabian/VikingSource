using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.Wand.Data
{
    class MagicFormulaCollection
    {
        static MagicFormula SmallDamage = new MagicFormula(MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Damage, 1);
        static MagicFormula MediumDamage = new MagicFormula(MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Damage, 2);

        static MagicFormula ThinWallSmasher = new MagicFormula(MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Thin_Wall, MagicEffect.Open_wall, MagicFormula.NoTier);
        static MagicFormula WallSmasher = new MagicFormula(MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Any_Wall, MagicEffect.Open_wall, MagicFormula.NoTier);


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
    }
}
