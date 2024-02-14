using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    class Orc_Spearman : CmdUnitData
    {
        public Orc_Spearman()
            : base("Spearman", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdUnitOrc_GoblinSoldier,
                2, 2, 0, 0, OrcInfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Shield_dash(), new Expendable());
        }

        public override UnitType Type => UnitType.Orc_Spearman;
    }

    class Orc_LightInfantery : CmdUnitData
    {
        public Orc_LightInfantery()
            : base("Light Infantery", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdUnitOrc_LightSwordsman,
                2, 2, 0, 0, OrcInfantryHealth -1, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Charge(), new Frenzy());
        }

        public override UnitType Type => UnitType.Orc_LightInfantery;
    }

    class Orc_HeavyInfantery : CmdUnitData
    {
        public Orc_HeavyInfantery()
            : base("Heavy Infantery", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdUnitOrc_HeavySwordsman,
                1, 4, 0, 0, OrcInfantryHealth, false)
        {
            modelSettings.modelScale *= 0.95f;
            modelSettings.facingRight = true;
            properties.set(new Block());
        }

        public override UnitType Type => UnitType.Orc_HeavyInfantery;
    }

    class Orc_LongBow : CmdUnitData
    {//EJ KLAR
        public Orc_LongBow()
            : base("Long bow", UnitMainType.Infantry, UnitUnderType.Infantry_Ranged, false, SpriteName.cmdUnitOrc_LightArcher,
                2, 1, StandardRangedAttacks, 3, OrcInfantryHealth, false)
        {
            modelSettings.facingRight = true;
        }

        public override UnitType Type => UnitType.Orc_LongBow;
    }

    class Orc_ShortBow : CmdUnitData
    {
        public Orc_ShortBow()
            : base("Short bow", UnitMainType.Infantry, UnitUnderType.Infantry_Ranged, false, SpriteName.cmdUnitOrc_LightArcher,
                2, 1, StandardRangedAttacks, 2, OrcInfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Expendable());
        }

        public override UnitType Type => UnitType.Orc_ShortBow;
    }

    class Orc_Cavalry : CmdUnitData
    {
        public Orc_Cavalry()
            : base("Cavalry", UnitMainType.Cavalry, UnitUnderType.Cavalry_Horseback, false, SpriteName.cmdUnitOrc_LightWolfRider,
                4, 2, 0, 0, OrcCavalryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Flank_support());
        }

        public override UnitType Type => UnitType.Orc_Cavalry;
    }

    class Orc_HeavyCavalry : CmdUnitData
    {
        public Orc_HeavyCavalry()
            : base("Heavy Cavalry", UnitMainType.Cavalry, UnitUnderType.Cavalry_Horseback, false, SpriteName.cmdUnitOrc_HeavyWolfRider,
                3, 3, 0, 0, OrcCavalryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Block(), new Flank_support());
        }

        public override UnitType Type => UnitType.Orc_HeavyCavalry;
    }

    class Orc_Hero : CmdUnitData
    {
        public Orc_Hero()
            : base("Hero", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdUnitOrc_Hero,
                2, 4, 0, 0, 6, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Leader(), new Valuable());
        }

        public override UnitType Type => UnitType.Orc_Hero;
    }


    class Goblin_MountedBallista : CmdUnitData
    {
        public Goblin_MountedBallista()
            : base("Goblin Ballista", UnitMainType.Warmashine, UnitUnderType.Warmashine_Ballista, false, SpriteName.cmdUnitBallista,
                0, 0, 2, 5, 2, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Pierce(), new Static_target());
        }

        public override UnitType Type => UnitType.Goblin_MountedBallista;
    }
}
