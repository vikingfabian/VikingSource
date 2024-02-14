using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    class Elf_Spearman : CmdUnitData
    {
        public Elf_Spearman()
            : base("Infantry", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.MissingImage,
                2, 2, 2, 2, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Ignore_terrain());
        }

        public override UnitType Type => UnitType.Elf_Spearman;
    }

    class Elf_Faun : CmdUnitData
    {
        public Elf_Faun()
            : base("Faun", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdUnitElf_Faun,
                3, 3, 0, 0, InfantryHealth - 1, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Charge(), new Ignore_terrain());
        }

        public override UnitType Type => UnitType.Elf_Faun;
    }

    class Elf_Wardacer : CmdUnitData
    {
        public Elf_Wardacer()
            : base("Heavy Infantry", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdUnitElf_Wardancer,
                2, 4, 0, 0, InfantryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Block());
        }

        public override UnitType Type => UnitType.Elf_Wardacer;
    }

    class Elf_LongBow : CmdUnitData
    {
        public Elf_LongBow()
            : base("Long bow", UnitMainType.Infantry, UnitUnderType.Infantry_Ranged, false, SpriteName.cmdUnitElf_Archer,
                2, 1, 3, 4, InfantryHealth -1, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Aim());
        }

        public override UnitType Type => UnitType.Elf_LongBow;
    }

    class Elf_ShortBow : CmdUnitData
    {
        public Elf_ShortBow()
            : base("Short bow", UnitMainType.Infantry, UnitUnderType.Infantry_Ranged, false, SpriteName.cmdUnitElf_Scout,
                2, 1, 3, 3, InfantryHealth -1, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Over_shoulder(), new Ignore_terrain());
        }

        public override UnitType Type => UnitType.Elf_ShortBow;
    }

    class Elf_Cavalry : CmdUnitData
    {
        public Elf_Cavalry()
            : base("Cavalry", UnitMainType.Cavalry, UnitUnderType.Cavalry_Horseback, false, SpriteName.cmdUnitElf_LightCavalry,
                4, 1, 2, 2, CavalryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Flank_support(), new Over_shoulder());
        }

        public override UnitType Type => UnitType.Elf_Cavalry;
    }

    class Elf_HeavyCavalry : CmdUnitData
    {
        public Elf_HeavyCavalry()
            : base("Heavy Cavalry", UnitMainType.Cavalry, UnitUnderType.Cavalry_Horseback, false, SpriteName.cmdUnitElf_HeavyCavalry,
                3, 2, 0, 0, CavalryHealth, false)
        {
            modelSettings.facingRight = true;
            properties.set(new Flank_support(), new Block());
        }

        public override UnitType Type => UnitType.Elf_HeavyCavalry;
    }

    class Elf_Hero : CmdUnitData
    {
        public Elf_Hero()
            : base("Hero", UnitMainType.Infantry, UnitUnderType.Infantry_CC, false, SpriteName.cmdHero_Archer,
                2, 2, 3, 3, 6, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Leader(), new Valuable());
        }

        public override UnitType Type => UnitType.Elf_Hero;
    }



}
