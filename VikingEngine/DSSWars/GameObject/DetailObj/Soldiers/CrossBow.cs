//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
    class CrossBow : AbsSoldierData
    {
        public CrossBow() 
        {
            unitType = UnitType.CrossBow;
            factionUniqueType = 0;

            modelScale = StandardModelScale;
            boundRadius = StandardBoundRadius;

            walkingSpeed = StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 1.7f;
            ArmyFrontToBackPlacement = ArmyPlacement.Mid;
            basehealth = DefaultHealth;
            mainAttack = AttackType.Bolt;
            attackDamage = 100;
            attackDamageStructure = 80;
            attackDamageSea = 120;
            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 4f;

            modelName = LootFest.VoxelModelName.wars_crossbow;

            description = "Powerful ranged soldier";
            goldCost = MathExt.MultiplyInt(2, DssLib.GroupDefaultCost);
        }
        
    }
}
