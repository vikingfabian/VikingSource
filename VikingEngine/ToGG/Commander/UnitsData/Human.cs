using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    class Human_Scout : CmdUnitData
    {
        public Human_Scout()
            : base("Scout", UnitMainType.Infantry, UnitUnderType.Infantry_CC, true, SpriteName.cmdUnitHuman_Scout,
                2, 1, 1, 1, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Ignore_terrain(), new Slippery());
        }

        public override UnitType Type => UnitType.Human_Scout;
    }

    class Human_Spearman : CmdUnitData
    {
        public Human_Spearman()
            : base("Spearman", UnitMainType.Infantry, UnitUnderType.Infantry_CC, true, 
                  SpriteName.cmdUnitHuman_LightSpearman, 2, 3, 0, 0, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Shield_dash());
        }

        public override UnitType Type => UnitType.Human_Spearman;
    }

    class Human_Warrior : CmdUnitData
    {
        public Human_Warrior()
            : base("Warrior", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdUnitHuman_Warrior,
                2, 3, 0, 0, InfantryHealth - 1, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Charge(), new Frenzy());
        }

        public override UnitType Type => UnitType.Human_Warrior;
    }

    class Human_HeavySpearman : CmdUnitData
    {
        public Human_HeavySpearman()
            : base("Heavy Spearman", UnitMainType.Infantry, UnitUnderType.Infantry_CC, true, SpriteName.cmdUnitHuman_HeavySpearman,
                1, 5, 0, 0, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Block());
        }

        public override UnitType Type => UnitType.Human_HeavySpearman;
    }

    class Human_LongBow : CmdUnitData
    {
        public Human_LongBow()
            : base("Long bow", UnitMainType.Infantry, UnitUnderType.Infantry_Ranged, true, SpriteName.cmdUnitHuman_HeavyArcher,
                2, 1, StandardRangedAttacks, 4, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Aim());
        }

        public override UnitType Type => UnitType.Human_LongBow;
    }

    class Human_ShortBow : CmdUnitData
    {
        public Human_ShortBow()
            : base("Short bow", UnitMainType.Infantry, UnitUnderType.Infantry_Ranged, true, SpriteName.cmdUnitHuman_LightArcher,
                2, 1, StandardRangedAttacks, 2, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Over_shoulder());
        }

        public override UnitType Type => UnitType.Human_ShortBow;
    }

    class Human_HorseScout : CmdUnitData
    {
        public Human_HorseScout()
            : base("Horse scout", UnitMainType.Cavalry, UnitUnderType.Cavalry_Horseback, false, SpriteName.cmdUnitHuman_LightCavalry,
                5, 1, StandardRangedAttacks, 1, CavalryHealth, false)
        {
            modelSettings.shadowOffset = -0.05f;
            properties.set(new Slippery());
            }

        public override UnitType Type => UnitType.Human_HorseScout;
    }

    class Human_Cavalry : CmdUnitData
    {
        public Human_Cavalry()
            : base("Cavalry", UnitMainType.Cavalry, UnitUnderType.Cavalry_Horseback, true, SpriteName.cmdUnitHuman_LightCavalry,
                4, 2, 0, 0, CavalryHealth, false)
        {
            modelSettings.facingRight = true;
            modelSettings.shadowOffset = -0.1f;
            properties.set(new Flank_support());
            }

        public override UnitType Type => UnitType.Human_Cavalry;
    }

    class Human_HeavyCavalry : CmdUnitData
    {
        public Human_HeavyCavalry()
            : base("Heavy Cavalry", UnitMainType.Cavalry, UnitUnderType.Cavalry_Horseback, true, SpriteName.cmdUnitHuman_HeavyCavalry,
                3, 3, 0, 0, CavalryHealth, false)
        {
            modelSettings.facingRight = true;
            modelSettings.shadowOffset = -0.05f;
            properties.set(new Block(), new Flank_support());
        }

        public override UnitType Type => UnitType.Human_HeavyCavalry;
    }


    class Human_Chariot : CmdUnitData
    {
        public Human_Chariot()
            : base("Chariot", UnitMainType.Cavalry, UnitUnderType.Cavalry_Chariot, false, SpriteName.cmdUnitChariot,
                2, 4, 0, 0, ChariotHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Flank_support(), new Leader());
        }

        public override UnitType Type => UnitType.Human_Chariot;
    }

    class Human_Hero : CmdUnitData
    {
        public Human_Hero()
            : base("Hero", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdHero_Magician,
                2, 4, 0, 0, 6, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Leader(), new Valuable());
        }

        public override UnitType Type => UnitType.Human_Hero;
    }
}
