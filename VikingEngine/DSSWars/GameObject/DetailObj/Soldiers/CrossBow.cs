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

            modelScale = StandardModelScale;
            boundRadius = StandardBoundRadius;

            walkingSpeed = StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 1.7f;
            ArmyFrontToBackPlacement = ArmyPlacement.Mid;
            basehealth = DefaultHealth;
            mainAttack = AttackType.Bolt;
            attackDamage = 50;
            attackDamageStructure = 40;
            attackDamageSea = 100;
            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 4f;

            modelName = LootFest.VoxelModelName.little_crossboworc;

            description = "";
        }
        
    }
}
