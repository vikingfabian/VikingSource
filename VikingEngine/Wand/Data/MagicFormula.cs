using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.Wand.Data
{
    struct MagicFormula
    {
        public const int NoTier = 0;
        public MagicTrigger trigger;
        public MagicTrigger_ObjectType triggerObject;
        public MagicEffect effect;

        public int triggerTier;

        public MagicFormula(MagicTrigger trigger, MagicTrigger_ObjectType triggerObject, MagicEffect effect, int triggerTier)
        {
            this.trigger = trigger;
            this.triggerObject = triggerObject;
            this.effect = effect;
            this.triggerTier = triggerTier;
        }
    }

    enum MagicTrigger
    {
        Auto,

        ObjectHit,
    }

    enum MagicTrigger_ObjectType
    { 
        Any,

        Creature,
        Thin_Wall,
        Any_Wall,
    }

    enum MagicEffect
    {
        Damage,
        Open_wall,
    }
}
