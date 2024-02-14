using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine;

namespace VikingEngine.ToGG.Commander
{
    static class UnitLib
    {
        public static SpriteName ArmyRaceIcon(ArmyRace race)
        {
            switch (race)
            {
                case ArmyRace.Human:
                    return SpriteName.cmdUnitHuman_LightSpearman;
                case ArmyRace.Undead:
                    return SpriteName.cmdUnitUndead_LightInfantry;
                case ArmyRace.Elf:
                    return SpriteName.cmdUnitElf_Archer;
                case ArmyRace.Orc:
                    return SpriteName.cmdUnitOrc_LightSwordsman;
            }
            return SpriteName.MissingImage;
        }
    }

    enum UnitType
    {
        Human_Spearman =0,
        Human_Warrior =1,
        Human_HeavySpearman =2, 
        Human_Scout =3,
        Human_LongBow,
        Human_ShortBow,
        Human_HorseScout,
        Human_Cavalry,
        Human_HeavyCavalry,
        Human_Chariot,
        Human_Hero,


        Undead_Spearman,
        Undead_Warrior,
        Undead_SpearBeast,
        Undead_Scout,
        Undead_LongBow,
        Undead_ShortBow,
        Undead_HorseScout,
        Undead_Cavalry,
        Undead_HeavyCavalry,
        Undead_Hero,

        Elf_Spearman,
        Elf_Faun,
        Elf_Wardacer,
        Elf_Scout,
        Elf_LongBow,
        Elf_ShortBow,
        Elf_HorseScout,
        Elf_Cavalry,
        Elf_HeavyCavalry,
        Elf_Hero,

        Orc_Spearman,
        Orc_LightInfantery,
        Orc_HeavyInfantery,
        Orc_Scout,
        Orc_LongBow,
        Orc_ShortBow,
        Orc_HorseScout,
        Orc_Cavalry,
        Orc_HeavyCavalry,
        Orc_Hero,

        TacticalBase,

        Practice_Spearman,
        Practice_Archer,
        Practice_Dummy,
        Practice_Dummy_Group,
        Practice_Dummy_Armored,
        Practice_Sneaky_Goblin,
        non1,
        non2,
        non3,
        non4,

        SupplyWagon,
        Undead_HeavyBeast,
        Undead_HeavySpearman,
        BatteringRam,
        SleepingTent,
        Story1_DrillSergeant,
        Story1_Engineer,
        Story1_Catapult,
        Goblin_MountedBallista,
        Story1_ElfCatapult,
        Story1_ElfPrincess,
        Story1_NecroDragon,
        SupplyPile,
        NUM_NONE,
    }

    enum ArmyRace
    {
        Practice,

        Human,
        Undead,
        Elf,
        Orc,
        All,
        NUM_NON,
    }

   

    enum UnitMainType
    {
        Infantry,
        Cavalry,
        Warmashine,
        Special,
        StaticObject,
    }
    enum UnitUnderType
    {
        Infantry_CC,
        Infantry_Ranged,
        Infantry_Mixed,
        Cavalry_Horseback,
        Cavalry_Chariot,
        Cavalry_Flying,
        Warmashine_Ballista,
        Warmashine_BatteringRam,
        Special_TacticalBase,
        Special_Supplies,
        Tent,
        Dummy,
    }

    //enum DefenceType
    //{
    //    Dodge,
    //    Armor,
    //    HeavyArmor,
    //}
}
