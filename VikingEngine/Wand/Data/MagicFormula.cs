using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace VikingEngine.Core.Wand.Data
{
    struct MagicFormula
    {
        public const int NoTier = 0;

        public int charges;
        public bool isFound;
        public MagicTrigger trigger;
        public MagicTrigger_ObjectType triggerObject;
        public MagicEffect effect;
        public int triggerTier;

        public int triggerBoost;

        public SpecialTarget specialTarget;
        public int specialTargetId;

        public string name;

        public MagicFormula(string name, MagicTrigger trigger, MagicTrigger_ObjectType triggerObject, MagicEffect effect, int triggerTier)
        {
            this.name = name;
            this.trigger = trigger;
            this.triggerObject = triggerObject;
            this.effect = effect;
            this.triggerTier = triggerTier;
        }

        public void Recharge()
        {
            charges = triggerTier;
            triggerBoost = 0;
        }

        public void MagicUp()
        {
            if (triggerTier != NoTier)
            {
                triggerBoost++;
            }
        }

        public string FullName()
        {
            if (isFound)
            {
                if (triggerTier == NoTier)
                {
                    return name;
                }
                else
                {
                    return $"{name}({triggerTier})";
                }
            }
            else
            {
                return "???";
            }
        }

        public int PowerAndBoost=>triggerTier + triggerBoost;
    }

    enum MagicTrigger
    {
        Auto,

        ObjectHit,

        PassObject,

        OnComplete,

        SpendFluid,

        Fire_north,
        Fire_south,
    }

    enum MagicTrigger_ObjectType
    { 
        None,
        Any,

        Creature,
        Thin_Wall,
        Any_Wall,

        Blood,
    }

    enum MagicEffect
    {
        Activate,
        Heal_user,
        Heal_target,
        Damage,
        Execute_damaged,
        Back_fire,
        Leech,
        Explode,
        Open_wall,
        Teleport_user,
        Magic_up,
        Clone,
        Level_up,

        End,
    }

    enum SpecialTarget
    { 
        None,
        Sides,
    }
}
