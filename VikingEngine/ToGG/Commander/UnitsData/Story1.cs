using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    class Story1_DrillSergeant : CmdUnitData
    {
        public Story1_DrillSergeant()
            : base("Drill Sergeant", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdHero_Magician,
                2, 4, 0, 0, 6, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Leader(), new Valuable());
        }

        public override UnitType Type => UnitType.Story1_DrillSergeant;
    }

    class Story1_Engineer : CmdUnitData
    {
        public Story1_Engineer()
            : base("Engineer", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdHero_Archer,
                0, 0, 0, 0, 6, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Cant_retreat(), new Valuable());
        }

        public override UnitType Type => UnitType.Story1_Engineer;
    }

    class Story1_ElfPrincess : CmdUnitData
    {
        public Story1_ElfPrincess()
            : base("Princess", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdHero_Archer,
               2, 2, 3, 3, 6, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Leader(), new Valuable());
        }

        public override UnitType Type => UnitType.Story1_ElfPrincess;
    }

    class Story1_NecroDragon : CmdUnitData
    {
        public Story1_NecroDragon()
            : base("Necromancer", UnitMainType.Cavalry, UnitUnderType.Cavalry_Flying, false, SpriteName.cmdUnitBoneDragon,
               6, 5, 4, 3, 8, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Leader(), new Block(), new Flying());
        }

        public override UnitType Type => UnitType.Story1_NecroDragon;
    }

    class SleepingTent : CmdUnitData
    {
        public SleepingTent()
            : base("Sleeping Tent", UnitMainType.StaticObject, UnitUnderType.Tent, true, SpriteName.cmdUnitTent,
                    0, 0, 0, 0, 4, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Spawn_point(), new Static_target(), new Valuable());
        }

        public override UnitType Type => UnitType.SleepingTent;
    }

    class Story1_Catapult : CmdUnitData
    {
        public Story1_Catapult()
            : base("Catapult", UnitMainType.Warmashine, UnitUnderType.Warmashine_Ballista, false, SpriteName.cmdUnitBallista,
                    1, 0, 0, 5, 2, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Catapult(), new Cant_retreat(), new Valuable());
        }

        public override UnitType Type => UnitType.Story1_Catapult;
    }

    class Story1_ElfCatapult : CmdUnitData
    {
        public Story1_ElfCatapult()
            : base("Elf Catapult", UnitMainType.Warmashine, UnitUnderType.Warmashine_Ballista, false, SpriteName.cmdUnitBallista,
                    1, 0, 0, 5, 2, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Catapult_Plus(), new Cant_retreat());
        }

        public override UnitType Type => UnitType.Story1_ElfCatapult;
    }

}
