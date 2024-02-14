using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    class AllUnits
    {
        public AllUnits()
        {
            cmdRef.units = this;
           
        }

        public CmdUnitData GetUnit(UnitType type)
        {
            CmdUnitData unit = null;

            switch (type)
            {
                case UnitType.Practice_Spearman:
                    unit = new Practice_Spearman();
                    break;
                case UnitType.Practice_Archer:
                    unit = new Practice_Archer();
                    break;
                case UnitType.Practice_Dummy:
                    unit = new Practice_Dummy();
                    break;
                case UnitType.Practice_Dummy_Group:
                    unit = new Practice_Dummy_Group();
                    break;
                case UnitType.Practice_Dummy_Armored:
                    unit = new Practice_Dummy_Armored();
                    break;
                case UnitType.Practice_Sneaky_Goblin:
                    unit = new Practice_Sneaky_Goblin();
                    break;

                //HUMAN
                case UnitType.Human_Scout:
                    unit = new Human_Scout();
                    break;
                case UnitType.Human_Spearman:
                    unit = new Human_Spearman();
                    break;
                case UnitType.Human_Warrior:
                    unit = new Human_Warrior();
                    break;
                case UnitType.Human_HeavySpearman:
                    unit = new Human_HeavySpearman();
                    break;
                case UnitType.Human_LongBow:
                    unit = new Human_LongBow();
                    break;
                case UnitType.Human_ShortBow:
                    unit = new Human_ShortBow();
                    break;
                case UnitType.Human_HorseScout:
                    unit = new Human_HorseScout();
                    break;
                case UnitType.Human_Cavalry:
                    unit = new Human_Cavalry();
                    break;
                case UnitType.Human_HeavyCavalry:
                    unit = new Human_HeavyCavalry();
                    break;
                case UnitType.Human_Chariot:
                    unit = new Human_Chariot();
                    break;
                case UnitType.Human_Hero:
                    unit = new Human_Hero();
                    break;

                //UNDEAD
                case UnitType.Undead_Spearman:
                    unit = new Undead_Spearman();
                    break;
                case UnitType.Undead_Warrior:
                    unit = new Undead_Warrior();
                    break;
                case UnitType.Undead_SpearBeast:
                    unit = new Undead_SpearBeast();
                    break;
                case UnitType.Undead_HeavyBeast:
                    unit = new Undead_HeavyBeast();
                    break;
                case UnitType.Undead_HeavySpearman:
                    unit = new Undead_HeavySpearman();
                    break;
                case UnitType.Undead_LongBow:
                    unit = new Undead_LongBow();
                    break;
                case UnitType.Undead_ShortBow:
                    unit = new Undead_ShortBow();
                    break;
                case UnitType.Undead_Cavalry:
                    unit = new Undead_Cavalry();
                    break;
                case UnitType.Undead_HeavyCavalry:
                    unit = new Undead_HeavyCavalry();
                    break;
                case UnitType.Undead_Hero:
                    unit = new Undead_Hero();
                    break;

                //ELF
                case UnitType.Elf_Spearman:
                    unit = new Elf_Spearman();
                    break;
                case UnitType.Elf_Faun:
                    unit = new Elf_Faun();
                    break;
                case UnitType.Elf_Wardacer:
                    unit = new Elf_Wardacer();
                    break;
                case UnitType.Elf_LongBow:
                    unit = new Elf_LongBow();
                    break;
                case UnitType.Elf_ShortBow:
                    unit = new Elf_ShortBow();
                    break;
                case UnitType.Elf_Cavalry:
                    unit = new Elf_Cavalry();
                    break;
                case UnitType.Elf_HeavyCavalry:
                    unit = new Elf_HeavyCavalry();
                    break;
                case UnitType.Elf_Hero:
                    unit = new Elf_Hero();
                    break;

                //ORC
                case UnitType.Orc_Spearman:
                    unit = new Orc_Spearman();
                    break;
                case UnitType.Orc_LightInfantery:
                    unit = new Orc_LightInfantery();
                    break;
                case UnitType.Orc_HeavyInfantery:
                    unit = new Orc_HeavyInfantery();
                    break;
                case UnitType.Orc_LongBow:
                    unit = new Orc_LongBow();
                    break;
                case UnitType.Orc_ShortBow:
                    unit = new Orc_ShortBow();
                    break;
                case UnitType.Orc_Cavalry:
                    unit = new Orc_Cavalry();
                    break;
                case UnitType.Orc_HeavyCavalry:
                    unit = new Orc_HeavyCavalry();
                    break;
                case UnitType.Orc_Hero:
                    unit = new Orc_Hero();
                    break;
                case UnitType.Goblin_MountedBallista:
                    unit = new Goblin_MountedBallista();
                    break;

                //STRUCTURE
                case UnitType.TacticalBase:
                    unit = new TacticalBase();
                    break;
                case UnitType.SupplyWagon:
                    unit = new SupplyWagon();
                    break;
                case UnitType.SupplyPile:
                    unit = new SupplyPile();
                    break;
                case UnitType.BatteringRam:
                    unit = new BatteringRam();
                    break;

                //STORY1
                case UnitType.Story1_DrillSergeant:
                    unit = new Story1_DrillSergeant();
                    break;
                case UnitType.Story1_Engineer:
                    unit = new Story1_Engineer();
                    break;
                case UnitType.Story1_ElfPrincess:
                    unit = new Story1_ElfPrincess();
                    break;
                case UnitType.Story1_NecroDragon:
                    unit = new Story1_NecroDragon();
                    break;
                case UnitType.SleepingTent:
                    unit = new SleepingTent();
                    break;
                case UnitType.Story1_Catapult:
                    unit = new Story1_Catapult();
                    break;
                case UnitType.Story1_ElfCatapult:
                    unit = new Story1_ElfCatapult();
                    break;    

                default:
                    throw new NotImplementedException("cmd unit type get: " + type.ToString());
            }

            return unit;
        }
    }
}
