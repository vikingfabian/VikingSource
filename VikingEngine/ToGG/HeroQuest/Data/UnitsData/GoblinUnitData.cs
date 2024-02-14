using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Data.Property;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class GoblinArcher : HqUnitData
    {
        public GoblinArcher()
            : base()
        {
            move = 4;
            wep.meleeStrength = 1;
            wep.projectileStrength = 2;
            wep.projectileRange = 4;
            startHealth = 2;

            defence.set(hqLib.DodgeDie);
            properties.set(new DullWeapon());

            modelSettings.image = SpriteName.cmdUnitOrc_LightArcher;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 0.8f;
            modelSettings.shadowOffset = -0.05f;
        }

        public override HqUnitType Type => HqUnitType.GoblinArcher;
        public override string Name => "Goblin Archer";
        public override float UnitDifficulty => 0.6f;
    }

    class GoblinSoldier : HqUnitData
    {
        public GoblinSoldier()
            : base()
        {
            move = 4;
            wep.meleeStrength = 2;
            wep.projectileStrength = 0;
            wep.projectileRange = 0;
            startHealth = 3;

            defence.set(hqLib.ArmorDie);
            properties.set(new DullWeapon());

            modelSettings.image = SpriteName.cmdUnitOrc_GoblinSoldier;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 0.8f;
            modelSettings.shadowOffset = -0.03f;
        }

        public override HqUnitType Type => HqUnitType.GoblinSoldier;
        public override string Name => "Goblin Soldier";
        public override float UnitDifficulty => 0.5f;
    }

    class GoblinGuard : HqUnitData
    {
        public GoblinGuard()
            : base()
        {
            move = 4;
            wep.meleeStrength = 1;
            wep.projectileStrength = 2;
            wep.projectileRange = 2;
            startHealth = 3;

            defence.set(hqLib.ArmorDie);
            properties.set(new DullWeapon());

            modelSettings.image = SpriteName.cmdUnitOrc_GoblinGuard;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 0.8f;
            modelSettings.shadowOffset = -0.1f;
        }

        public override HqUnitType Type => HqUnitType.GoblinSoldier;
        public override string Name => "Goblin Guard";
        public override float UnitDifficulty => 0.6f;
    }

    class GoblinBloated : HqUnitData
    {
        public GoblinBloated()
            : base()
        {
            move = 2;
            wep.meleeStrength = 4;
            wep.projectileStrength = 0;
            wep.projectileRange = 0;
            startHealth = 8;

            properties.set(new MonsterCommander(), new DeathPoisionArea());

            modelSettings.image = SpriteName.cmdGoblinBloated;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 0.8f;
            modelSettings.shadowOffset = -0.01f;
        }

        public override HqUnitType Type => HqUnitType.GoblinBloated;
        public override string Name => "Bloated goblin";
        public override float UnitDifficulty => 1.2f;
    }

    class GoblinRunner : HqUnitData
    {
        public GoblinRunner()
            : base()
        {
            move = 6;
            wep.meleeStrength = 1;
            wep.projectileStrength = 0;
            wep.projectileRange = 0;
            startHealth = 1;

            defence.set(hqLib.DodgeDie);
            properties.set(new SlipThrough());

            modelSettings.image = SpriteName.cmdGoblinRunner;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 0.85f;
            modelSettings.shadowOffset = -0.1f;
        }

        public override HqUnitType Type => HqUnitType.GoblinRunner;
        public override string Name => "Goblin Runner";
        public override float UnitDifficulty => 0.4f;
    }

    class GoblinWolfRider : HqUnitData
    {
        public GoblinWolfRider()
            : base()
        {
            move = 5;
            wep.meleeStrength = 3;
            wep.projectileStrength = 0;
            wep.projectileRange = 0;
            startHealth = 5;

            defence.set(hqLib.ArmorDie);
            properties.set(new TargetX(2));

            modelSettings.image = SpriteName.cmdUnitOrc_LightWolfRider;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 0.8f;
            modelSettings.shadowOffset = -0.1f;
        }

        public override HqUnitType Type => HqUnitType.GoblinWolfRider;
        public override string Name => "Wolf rider";
        public override float UnitDifficulty => 1.6f;
    }

    class GoblinWolfRiderCommander : GoblinWolfRider
    {
        public GoblinWolfRiderCommander()
            : base()
        {
            wep.meleeStrength += 1;
            startHealth += 1;

            properties.Add(new MonsterCommander(), null);

            modelSettings.image = SpriteName.cmdUnitOrc_HeavyWolfRider;
            modelSettings.modelScale += 0.06f;
        }

        public override HqUnitType Type => HqUnitType.GoblinWolfRiderCommander;
        public override string Name => "Wolf rider chief";
        public override float UnitDifficulty => 1.9f;
    }

    class GoblinBoss : HqUnitData
    {
        public GoblinBoss()
            : base()
        {
            move = 3;
            wep.meleeStrength = 3;
            startHealth = 24;

            defence.set(hqLib.ArmorDie, 2);
            modelSettings.image = SpriteName.cmdGoblinFoodBoss;
            modelSettings.facingRight = false;
            modelSettings.modelScale = 0.8f;
            modelSettings.shadowOffset = -0.05f;
            modelSettings.centerOffset.Z = 0.08f;

            //stumack bash (push all adjacent, stun/confuse)
            //Belch, posion all
            //2surge - push1
            properties.set(new MonsterBoss(), new Swing(3));
        }

        public override HqUnitType Type => HqUnitType.GoblinBoss;
        public override string Name => "Goblin Boss";
        public override float UnitDifficulty => 3f;
    }

}
