using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Commander.UnitsData
{


    class Practice_Spearman : CmdUnitData
    {
        public Practice_Spearman()
            : base("Recruit Spear", UnitMainType.Infantry, UnitUnderType.Infantry_CC, true, SpriteName.cmdUnitHuman_LightSpearman,
                2, 2, 0, 0, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
        }

        public override UnitType Type => UnitType.Practice_Spearman;
    }

    class Practice_Archer : CmdUnitData
    {
        public Practice_Archer()
            : base("Recruit Archer", UnitMainType.Infantry, UnitUnderType.Infantry_CC, true, SpriteName.cmdUnitHuman_LightArcher,
                2, 1, 1, 2, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
        }

        public override UnitType Type => UnitType.Practice_Archer;
    }

    class Practice_Dummy : CmdUnitData
    {
        public Practice_Dummy()
            : base("Practice dummy", UnitMainType.StaticObject, UnitUnderType.Dummy, true, SpriteName.cmdUnitPracticeDummy,
                0, 0, 0, 0, OneMan, false)
        {
            modelSettings.facingRight = false;
            properties.set(new Static_target());
        }

        public override UnitType Type => UnitType.Practice_Dummy;
    }

    class Practice_Dummy_Group : CmdUnitData
    {
        public Practice_Dummy_Group()
            : base("Dummy group", UnitMainType.StaticObject, UnitUnderType.Dummy, true, SpriteName.cmdUnitPracticeDummy,
                0, 0, 0, 0, 4, false)
        {
            modelSettings.facingRight = false;
            properties.set(new Static_target());
        }

        public override UnitType Type => UnitType.Practice_Dummy_Group;
    }

    class Practice_Dummy_Armored : CmdUnitData
    {
        public Practice_Dummy_Armored()
            : base("Armored Dummy", UnitMainType.StaticObject, UnitUnderType.Dummy, true, SpriteName.cmdUnitPracticeDummyArmor,
                0, 0, 0, 0, OneMan, false)
        {
            modelSettings.facingRight = false;
            properties.set(new Static_target(), new Block());
        }

        public override UnitType Type => UnitType.Practice_Dummy_Armored;
    }

    class Practice_Sneaky_Goblin : CmdUnitData
    {
        public Practice_Sneaky_Goblin()
            : base("Sneaky Goblin", UnitMainType.Infantry, UnitUnderType.Infantry_CC, true, SpriteName.cmdUnitOrc_LightArcher,
                2, 2, StandardRangedAttacks, 2, OneMan, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Backstab_expert(), new Slippery());
        }

        public override UnitType Type => UnitType.Practice_Sneaky_Goblin;
    }



}
