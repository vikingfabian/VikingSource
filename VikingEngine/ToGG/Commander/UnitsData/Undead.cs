using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    class Undead_Spearman : CmdUnitData
    {
        public Undead_Spearman()
            : base("Swordsman", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false,
                SpriteName.cmdUnitUndead_LightInfantry,
                2, 3, 0, 0, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Cant_retreat(), new Body_snatcher());
        }

        public override UnitType Type => UnitType.Undead_Spearman;
    }

    class Undead_Warrior : CmdUnitData
    {
        public Undead_Warrior()
            : base("Warrior Beast", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false,
                SpriteName.cmdUnitUndead_Warrior,
                3, 4, 0, 0, InfantryHealth - 1, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Fear(), new Sucide_attack());
        }

        public override UnitType Type => UnitType.Undead_Warrior;
    }

    class Undead_SpearBeast : CmdUnitData
    {
        public Undead_SpearBeast()
            : base("Spear Beast", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false,
                SpriteName.cmdUnitUndead_SpearBeast,
                2, 4, 2, 2, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Charge(), new Sucide_attack());
        }

        public override UnitType Type => UnitType.Undead_SpearBeast;
    }

    class Undead_HeavyBeast : CmdUnitData
    {
        public Undead_HeavyBeast()
            : base("Heavy Beast", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false,
                SpriteName.cmdUnitUndead_HeavySwordBeast,
                2, 5, 0, 0, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Shield_dash());
        }

        public override UnitType Type => UnitType.Undead_HeavyBeast;
    }

    class Undead_HeavySpearman : CmdUnitData
    {
        public Undead_HeavySpearman()
            : base("Heavy swordsman", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false,
                SpriteName.MissingImage,
                1, 5, 0, 0, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Cant_retreat(), new Fear(), new Body_snatcher());
        }

        public override UnitType Type => UnitType.Undead_HeavySpearman;
    }

    class Undead_LongBow : CmdUnitData
    {
        public Undead_LongBow()
            : base("Long bow", UnitMainType.Infantry, UnitUnderType.Infantry_Ranged, false, SpriteName.cmdUnitUndead_HeavyArcher,
                2, 1, StandardRangedAttacks, 3, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Aim());
        }

        public override UnitType Type => UnitType.Undead_LongBow;
    }

    class Undead_ShortBow : CmdUnitData
    {
        public Undead_ShortBow()
            : base("Short bow", UnitMainType.Infantry, UnitUnderType.Infantry_Ranged, false, SpriteName.cmdUnitUndead_LightArcher,
                2, 2, StandardRangedAttacks, 2, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Cant_retreat(), new Body_snatcher());
        }

        public override UnitType Type => UnitType.Undead_ShortBow;
    }


    class Undead_Cavalry : CmdUnitData
    {
        public Undead_Cavalry()
            : base("Cavalry", UnitMainType.Cavalry, UnitUnderType.Cavalry_Horseback, false, SpriteName.cmdUnitUndead_LightCavalry,
                4, 2, 0, 0, CavalryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Fear_support());
        }

        public override UnitType Type => UnitType.Undead_Cavalry;
    }

    class Undead_HeavyCavalry : CmdUnitData
    {
        public Undead_HeavyCavalry()
            : base("Heavy Cavalry", UnitMainType.Cavalry, UnitUnderType.Cavalry_Horseback, false, SpriteName.cmdUnitUndead_HeavyCavalry,
                3, 3, 0, 0, CavalryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Block(), new Fear_support());
        }

        public override UnitType Type => UnitType.Undead_HeavyCavalry;
    }

    class Undead_Hero : CmdUnitData
    {
        public Undead_Hero()
            : base("Necromancer", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdUnitUndead_Necromancer,
                2, 3, 0, 0, 4, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Necromancer(), new Valuable());
        }

        public override UnitType Type => UnitType.Undead_Hero;
    }
}
