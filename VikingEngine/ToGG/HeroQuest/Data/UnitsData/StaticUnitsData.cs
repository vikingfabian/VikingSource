using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class Dummy : HqUnitData
    {
        public Dummy()
        {
            move = 0;
            startHealth = 2;

            defence.set(hqLib.ArmorDie);
            //defenceArmor = 1;
            properties.set(new StaticTarget());
            oneManArmy = false;
            modelSettings.image = SpriteName.cmdUnitPracticeDummy;
            modelSettings.modelScale = 0.72f;
        }

        public override HqUnitType Type => HqUnitType.Dummy;
        public override string Name => "Practice dummy";
    }

    class Decoy : HqUnitData
    {
        public Decoy()
        {
            move = 0;
            startHealth = 4;

            properties.set(new StaticTarget());
            modelSettings.image = SpriteName.cmdUnitPracticeDummy;
            modelSettings.modelScale = 0.72f;
        }

        public override HqUnitType Type => HqUnitType.Decoy;
        public override string Name => "Decoy";
    }

    class TrapDecoy : HqUnitData
    {
        public const string TrapDecoyName = "Trap Decoy";

        public TrapDecoy()
        {
            move = 0;
            startHealth = 4;

            properties.set(new StaticTarget());
            modelSettings.image = SpriteName.hqTrapDecoy;
            modelSettings.modelScale = 0.72f;
            modelSettings.facingRight = true;

            properties.set(new Retaliate(2));
        }

        public override HqUnitType Type => HqUnitType.TrapDecoy;
        public override string Name => TrapDecoyName;
    }

    class ArmoredDecoy : HqUnitData
    {
        public ArmoredDecoy()
        {
            move = 0;
            startHealth = 6;

            defence.set(hqLib.HeavyArmorDie);
            properties.set(new StaticTarget());
            modelSettings.image = SpriteName.cmdUnitPracticeDummyArmor;
            modelSettings.modelScale = 0.72f;
        }

        public override HqUnitType Type => HqUnitType.ArmoredDecoy;
        public override string Name => "Armored Decoy";
    }
}
