using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Conscript;

namespace VikingEngine.DSSWars.GameObject
{
    class ConscriptedSoldierData : AbsSoldierData
    {
        public ConscriptedSoldierData(SoldierProfile profile)
        {
            init(profile);
        }

        public ConscriptedSoldierData(System.IO.BinaryReader r)
        {
            SoldierProfile profile = new SoldierProfile();
            profile.readGameState(r);

            init(profile);
        }

        void init(SoldierProfile profile)
        {
            this.profile = profile;

            unitType = UnitType.Conscript;

            modelScale = DssConst.Men_StandardModelScale;
            boundRadius = DssVar.StandardBoundRadius;

            walkingSpeed = DssConst.Men_StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;


            basehealth = ConscriptProfile.ArmorHealth(profile.conscript.armorLevel);

            attackDamage = Convert.ToInt32(ConscriptProfile.WeaponDamage(profile.conscript.weapon) * profile.skillBonus);
            attackDamageStructure = attackDamage;
            attackDamageSea = attackDamage;

            attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;

            switch (profile.conscript.weapon)
            {
                case MainWeapon.SharpStick:
                    mainAttack = AttackType.Melee;
                    attackRange = 0.03f;
                    modelName = LootFest.VoxelModelName.war_folkman;
                    icon = SpriteName.WarsUnitIcon_Folkman;
                    break;
                case MainWeapon.Sword:
                    mainAttack = AttackType.Melee;
                    attackRange = 0.04f;
                    modelName = LootFest.VoxelModelName.wars_soldier;
                    modelVariationCount = 3;
                    icon = SpriteName.WarsUnitIcon_Soldier;
                    break;
                case MainWeapon.TwoHandSword:
                    mainAttack = AttackType.Melee;
                    attackRange = 0.08f;
                    modelName = LootFest.VoxelModelName.wars_twohand;
                    modelVariationCount = 1;
                    icon = SpriteName.WarsUnitIcon_Soldier;
                    break;
                case MainWeapon.Bow:
                    mainAttack = AttackType.Arrow;
                    ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    attackRange = 1.7f;
                    modelName = LootFest.VoxelModelName.war_archer;
                    modelVariationCount = 2;
                    icon = SpriteName.WarsUnitIcon_Archer;
                    attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                    break;
            }

            switch (profile.conscript.specialization)
            {
                case SpecializationType.Field:
                    attackDamage = MathExt.AddPercentage(attackDamage, DssConst.Conscript_SpecializePercentage);
                    attackDamageSea = MathExt.SubtractPercentage(attackDamageSea, DssConst.Conscript_SpecializePercentage);                   
                    attackDamageStructure = MathExt.SubtractPercentage(attackDamageStructure, DssConst.Conscript_SpecializePercentage);
                    break;

                case SpecializationType.Viking:
                case SpecializationType.Sea:
                    attackDamage = MathExt.SubtractPercentage(attackDamage, DssConst.Conscript_SpecializePercentage);
                    float seaDamagePerc = profile.conscript.specialization == SpecializationType.Sea ?
                        DssConst.Conscript_SpecializePercentage : DssConst.Conscript_SpecializePercentage * 3f;
                    attackDamageSea = MathExt.AddPercentage(attackDamageSea, seaDamagePerc);                    
                    attackDamageStructure = MathExt.SubtractPercentage(attackDamageStructure, DssConst.Conscript_SpecializePercentage);
                    modelVariationCount = 1;

                    if (!profile.RangedUnit())
                    { 
                        modelName = LootFest.VoxelModelName.war_sailor;
                        icon = SpriteName.WarsUnitIcon_Viking;
                    }
                    break;
                
                case SpecializationType.Siege:
                    attackDamage = MathExt.SubtractPercentage(attackDamage, DssConst.Conscript_SpecializePercentage);
                    attackDamageSea = MathExt.SubtractPercentage(attackDamageSea, DssConst.Conscript_SpecializePercentage);
                    attackDamageStructure = MathExt.AddPercentage(attackDamageStructure, DssConst.Conscript_SpecializePercentage);
                    break;

                case SpecializationType.HonorGuard:
                    energyPerSoldier = 0;
                    modelName = LootFest.VoxelModelName.little_hirdman;
                    modelVariationCount = 1;
                    icon = SpriteName.WarsUnitIcon_Honorguard;
                    break;

                case SpecializationType.Traditional:
                    energyPerSoldier *= 0.5f;
                    break;

                case SpecializationType.Green:
                    secondaryAttack = AttackType.Arrow;
                    secondaryAttackDamage = 100;
                    secondaryAttackRange = 1.7f;
                    bonusProjectiles = 2;
                    icon = SpriteName.WarsUnitIcon_Greensoldier;
                    
                    break;
            }

            attackTimePlusCoolDown /= ConscriptProfile.TrainingAttackSpeed(profile.conscript.training);
            attackTimePlusCoolDown /= 1f + profile.skillBonus;

        }

        override public void writeGameState(System.IO.BinaryWriter w)
        {
            profile.writeGameState(w);
        }
        override public void readGameState(System.IO.BinaryReader r)
        {
            profile.readGameState(r);
        }
    }
}
