using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.DSSWars.GameObject
{
    class ArcherData : AbsSoldierData
    {
        public ArcherData()
        {
            unitType = UnitType.Archer;

            modelScale = StandardModelScale * 0.9f;
            boundRadius = StandardBoundRadius * 0.9f;

            walkingSpeed = StandardWalkingSpeed * 1.05f;
            rotationSpeed = StandardRotatingSpeed;
            attackRange = 1.7f;
            ArmyFrontToBackPlacement =  ArmyPlacement.Mid;
            targetSpotRange = attackRange + StandardTargetSpotRange;
            basehealth = DefaultHealth;
            mainAttack = AttackType.Arrow;
            attackDamage = 50;
            attackDamageStructure = 60;
            attackDamageSea = 60;
            basehealth = 50;
            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 3f;

            modelName = LootFest.VoxelModelName.war_archer;
            modelVariationCount = 2;

            description = DssRef.lang.UnitType_Description_Archer;
            icon = SpriteName.WarsUnitIcon_Archer;
        }        
    }

}
