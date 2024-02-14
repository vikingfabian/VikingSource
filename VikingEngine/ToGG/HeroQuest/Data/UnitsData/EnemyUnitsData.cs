using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Data.Property;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class GoblinKnightBoss : HqUnitData
    {
        public GoblinKnightBoss()
            : base()
        {
            move = 3;
            wep.meleeStrength = 5;
            startHealth = 10;

            defence.set(hqLib.HeavyArmorDie, 2);
            properties.set(new Swing(3), new MonsterBoss());

            modelSettings.image = SpriteName.cmdGoblinKnight;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 1.1f;
            modelSettings.shadowOffset = -0.05f;
        }

        public override HqUnitType Type => HqUnitType.GoblinKnightBoss;
        public override string Name => "Goblin Knight";
    }


    class GuardDog : HqUnitData
    {
        public GuardDog()
            : base()
        {
            move = 5;
            wep.meleeStrength = 5;
            startHealth = 2;

            //defence.set(hqLib.ArmorDie);
            properties.set(new Players.Ai.Bark());

            modelSettings.image = SpriteName.cmdGuardDog;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 0.7f;
            modelSettings.shadowOffset = -0.05f;
        }

        public override HqUnitType Type => HqUnitType.GuardDog;
        public override string Name => "Guard dog";
        public override float UnitDifficulty => 0.8f;
    }

    class Beastman : HqUnitData
    {
        public Beastman()
            : base()
        {
            move = 3;
            wep.meleeStrength = 2;
            startHealth = 3;

            defence.set(hqLib.ArmorDie);
            modelSettings.image = SpriteName.cmdUnitUndead_Warrior;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 1f;
            modelSettings.shadowOffset = -0.05f;
        }

        public override HqUnitType Type => HqUnitType.Beastman;
        public override string Name => "Beast";
        public override float UnitDifficulty => 0.5f;
    }

    class HeavyBeastman : HqUnitData
    {
        public HeavyBeastman()
            : base()
        {
            move = 2;
            wep.meleeStrength = 3;
            startHealth = 4;

            defence.set(hqLib.HeavyArmorDie);

            properties.Add(new SurgeOptionGain(new PierceSurgeOption(2, 1)), null);

            modelSettings.image = SpriteName.cmdUnitUndead_HeavySwordBeast;
            modelSettings.facingRight = true;    
            modelSettings.modelScale = 1f;
            modelSettings.shadowOffset = -0.05f;
        }

        public override HqUnitType Type => HqUnitType.HeavyBeastman;
        public override string Name => "Heavy Beast";
        public override float UnitDifficulty => 1f;
    }

    class OrcGuard : HqUnitData
    {
        public OrcGuard()
            : base()
        {
            move = 2;
            wep.meleeStrength = 2;
            wep.projectileStrength = 4;
            wep.projectileRange = 2;
            startHealth = 4;

            defence.set(hqLib.HeavyArmorDie);

            properties.Add(new SurgeOptionGain(new PierceSurgeOption(2, 1)));

            modelSettings.image = SpriteName.cmdUnitOrc_HeavySwordsman;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 1f;
            modelSettings.shadowOffset = -0.05f;
        }

        public override HqUnitType Type => HqUnitType.OrcGuard;
        public override string Name => "Orc guard";
        public override float UnitDifficulty => 1.3f;
    }

    class Cyclops : HqUnitData
    {
        public Cyclops()
            : base()
        {
            move = 3;
            wep.meleeStrength = 4;
            startHealth = 8;

            defence.set(hqLib.HeavyArmorDie, 2);
            modelSettings.image = SpriteName.hqCyclops;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 1f;
        }

        public override HqUnitType Type => HqUnitType.Cyclops;
        public override string Name => "Cyclops";
    }

    class FireLizard : HqUnitData
    {
        public FireLizard()
            : base()
        {
            move = 2;
            wep.meleeStrength = 2;
            wep.projectileStrength = 4;
            wep.projectileRange = 2;
            startHealth = 7;

            defence.set(hqLib.HeavyArmorDie, 1);
            properties.Add(new SurgeOptionGain(new PierceSurgeOption(1, 1)));
            //weapons = new List<Gadgets.AbsMonsterWeapon>
            //{
            //    new FireBreath(wep.projectileStrength, wep.projectileRange)
            //};

            modelSettings.image = SpriteName.hqFireLizard;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 0.75f;
            modelSettings.shadowOffset = -0.1f; 
        }

        public override HqUnitType Type => HqUnitType.Firelizard;
        public override string Name => "Fire Lizard";
        public override float UnitDifficulty => 1.5f;
    }

    class Bat : HqUnitData
    {
        public Bat()
            : base()
        {
            move = 4;
            wep.meleeStrength = 2;
            startHealth = 1;

            defence.set(hqLib.DodgeDie);
            modelSettings.image = SpriteName.hqBat;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 1f;

            properties.set(new Flying(), new DullWeapon(), new Sleepy());
        }

        public override HqUnitType Type => HqUnitType.Bat;
        public override string Name => "Bat";

        public override float UnitDifficulty => 0.4f;
    }

    class Ogre : HqUnitData
    {
        public Ogre()
            : base()
        {
            move = 3;
            wep.meleeStrength = 4;
            startHealth = 8;

            defence.set(hqLib.HeavyArmorDie, 2);
            modelSettings.image = SpriteName.hqOgre;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 0.8f;

            properties.set(new UnitActionProperty(new ThrowAction(2, 3)), new Swing(3));
        }

        public override HqUnitType Type => HqUnitType.Ogre;
        public override string Name => "Ogre";
        public override float UnitDifficulty => 2f;
    }

    class CannonTroll : HqUnitData
    {
        public CannonTroll()
            : base()
        {
            move = 3;
            wep.meleeStrength = 2;
            wep.projectileStrength = 4;
            wep.projectileRange = 5;
            startHealth = 10;

            defence.set(hqLib.ArmorDie);
            modelSettings.image = SpriteName.cmdCannonTroll;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 1.5f;
            modelSettings.shadowOffset = -0.05f;
            modelSettings.centerOffset.X += 0.1f;

            //Slow attack +1 dodge på defender
            properties.set(new Regenerate(3), new SlowAttacker());
        }

        public override HqUnitType Type => HqUnitType.CannonTroll;
        public override string Name => "Cannon troll";
        public override float UnitDifficulty => 2.4f;
    }

    class DarkPriest : HqUnitData
    {
        public DarkPriest()
            : base()
        {
            move = 3;
            wep.meleeStrength = 1;
            wep.projectileStrength = 2;
            wep.projectileRange = 4;
            startHealth = 5;

            defence.set(hqLib.DodgeDie);
            modelSettings.image = SpriteName.hqDarkPriest;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 1f;
            modelSettings.shadowOffset = -0.1f;

            properties.set(new UnitActionProperty(new DarkHeal(2, 4)));
        }

        public override HqUnitType Type => HqUnitType.DarkPriest;
        public override string Name => "Dark Priest";
        public override float UnitDifficulty => 0.8f;
    }

    class SkeletonArcher : HqUnitData
    {
        public SkeletonArcher()
            : base()
        {
            move = 3;
            wep.meleeStrength = 1;
            wep.projectileStrength = 3;
            wep.projectileRange = 3;
            startHealth = 3;

            defence.set(hqLib.ArmorDie);
            modelSettings.image = SpriteName.cmdUnitUndead_LightArcher;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 1f;
            modelSettings.shadowOffset = -0.1f;

            properties.set(new Undead());
        }

        public override HqUnitType Type => HqUnitType.SkeletonArcher;
        public override string Name => "Skeleton Archer";
        public override float UnitDifficulty => 0.7f;
    }

    class SkeletonPeasant : HqUnitData
    {
        public SkeletonPeasant()
            : base()
        {
            move = 2;
            wep.meleeStrength = 2;
            startHealth = 4;

            defence.set(hqLib.ArmorDie);
            modelSettings.image = SpriteName.cmdUnitUndead_LightInfantry;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 1f;
            modelSettings.shadowOffset = -0.1f;

            properties.set(new Undead());
        }

        public override HqUnitType Type => HqUnitType.SkeletonPeasant;
        public override string Name => "Skeleton Peasant";
        public override float UnitDifficulty => 0.7f;
    }

    class SkeletonSoldier : HqUnitData
    {
        public SkeletonSoldier()
            : base()
        {
            move = 2;
            wep.meleeStrength = 3;
            startHealth = 5;

            defence.set(hqLib.ArmorDie);
            modelSettings.image = SpriteName.cmdUnitUndead_MedInfantry;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 1f;
            modelSettings.shadowOffset = -0.1f;

            properties.set(new Undead());
        }

        public override HqUnitType Type => HqUnitType.SkeletonSoldier;
        public override string Name => "Skeleton Soldier";
        public override float UnitDifficulty => 0.9f;
    }

    class Naga : HqUnitData
    {
        public Naga()
            : base()
        {
            move = 4;
            wep.meleeStrength = 4;
            startHealth = 6;

            defence.set(hqLib.ArmorDie);
            modelSettings.image = SpriteName.hqNaga;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 0.8f;

            properties.set(new UnitActionProperty(new Grapple()));
        }

        public override HqUnitType Type => HqUnitType.Naga;
        public override string Name => "Naga";
        public override float UnitDifficulty => 1.5f;
    }

    class NagaCommander : Naga
    {
        public NagaCommander()
            : base()
        {
            wep.meleeStrength++;
            startHealth++;

            defence.set(hqLib.ArmorDie, 2);
            modelSettings.image = SpriteName.hqNagaCommander;
            modelSettings.facingRight = true;
            //modelSettings.modelScale = 1f;

            properties.Add(new MonsterCommander());
        }

        public override HqUnitType Type => HqUnitType.NagaCommander;
        public override string Name => "Commander Naga";
        public override float UnitDifficulty => 2f;
    }

    class NagaBoss : NagaCommander
    {
        public NagaBoss()
            : base()
        {
            defence.set(hqLib.HeavyArmorDie, 2);
            startHealth += 1;
            modelSettings.image = SpriteName.hqNagaBoss;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 1f;
            modelSettings.centerOffset.Z = 0.12f;

            properties.Add(new Parry(4));
        }

        public override HqUnitType Type => HqUnitType.NagaBoss;
        public override string Name => "Naga Boss";

        public override float UnitDifficulty => 2.5f;
    }

    class CaveSpider : HqUnitData
    {
        public CaveSpider()
            : base()
        {
            move = 3;
            wep.meleeStrength = 2;
            wep.projectileStrength = 2;
            wep.projectileRange = 3;
            startHealth = 3;

            defence.set(hqLib.ArmorDie);
            //properties.set(new SurgeOptionGain(new WebbSurgeOption(2)));

            modelSettings.image = SpriteName.hqCaveSpider;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 0.85f;
            modelSettings.shadowOffset = -0.12f;

           // weapons = new List<AbsMonsterWeapon> { new NetspitWeapon(wep.projectileStrength, wep.projectileRange) };
        }

        public override HqUnitType Type => HqUnitType.CaveSpider;
        public override string Name => "Cave Spider";

        public override float UnitDifficulty => 0.6f;
    }

    class RabidLizzard : HqUnitData
    {
        public RabidLizzard()
            : base()
        {
            move = 3;
            wep.meleeStrength = 3;
            startHealth = 4;
            defence.set(hqLib.ArmorDie, 1);

            modelSettings.image = SpriteName.hqRabidLizard;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 1.1f;
            modelSettings.shadowOffset = -0.04f;

            properties.set(
                new DoubleAttack(),
                new Retaliate(2));
                //new SurgeOptionGain(new PoisionSurgeOption(2, 1)));
        }

        public override HqUnitType Type => HqUnitType.RabidLizard;
        public override string Name => "Rabid Lizard";
        public override float UnitDifficulty => 1.6f;
    }

    class FleshGhoul : HqUnitData
    {
        public FleshGhoul()
            : base()
        {
            move = 1;
            wep.meleeStrength = 5;
            startHealth = 12;

            modelSettings.image = SpriteName.MissingImage;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 1f;

            properties.set(
                new Undead(), 
                new Regenerate(2), 
                new SurgeOptionGain(new IncreaseMaxHealthSurgeOption(2, 1)));
        }

        public override HqUnitType Type => HqUnitType.FleshGhoul;
        public override string Name => "Flesh Ghoul";
    }

    class GreenSlime : HqUnitData
    {
        /*
         * Surge: Spawnar små slime vid attack
         * Små slime kan merge-heala dom stora
         * 4små kan merga till stor
         */
        public GreenSlime()
            : base()
        {
            move = 2;
            wep.meleeStrength = 3;
            startHealth = 8;

            modelSettings.image = SpriteName.MissingImage;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 1f;

            //properties.set(
            //    new Undead(),
            //    new Regenerate(2),
            //    new SurgeOptionGain(new IncreaseMaxHealthSurgeOption(2, 1)));
        }

        public override HqUnitType Type => HqUnitType.GreenSlime;
        public override string Name => "Green Slime";
    }
}
