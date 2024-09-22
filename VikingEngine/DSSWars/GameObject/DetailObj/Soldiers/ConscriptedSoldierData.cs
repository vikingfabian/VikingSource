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
        public SoldierProfile profile;

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
                case MainWeapon.Bow:
                    mainAttack = AttackType.Arrow;
                    ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    attackRange = 1.7f;
                    modelName = LootFest.VoxelModelName.war_archer;
                    modelVariationCount = 3;
                    icon = SpriteName.WarsUnitIcon_Archer;
                    attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                    break;
            }

            attackTimePlusCoolDown /= ConscriptProfile.TrainingAttackSpeed(profile.conscript.training);
            attackTimePlusCoolDown /= profile.skillBonus;

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
