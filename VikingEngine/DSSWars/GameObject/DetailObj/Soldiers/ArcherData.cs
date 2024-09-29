using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.DSSWars.GameObject
{
    class ArcherData : AbsSoldierProfile
    {
        public ArcherData()
        {
            unitType = UnitType.Archer;

            modelScale = DssConst.Men_StandardModelScale * 0.9f;
            boundRadius = DssVar.StandardBoundRadius * 0.9f;

            walkingSpeed = DssConst.Men_StandardWalkingSpeed * 1.05f;
            rotationSpeed = StandardRotatingSpeed;
            attackRange = 1.7f;
            ArmyFrontToBackPlacement =  ArmyPlacement.Mid;
            targetSpotRange = attackRange + StandardTargetSpotRange;
            basehealth =    DssConst.Soldier_DefaultHealth;
            mainAttack = AttackType.Arrow;
            attackDamage = 40;
            attackDamageStructure = 50;
            attackDamageSea = 50;
            basehealth = 50;
            attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;

            modelName = LootFest.VoxelModelName.war_archer;
            modelVariationCount = 2;

            description = DssRef.lang.UnitType_Description_Archer;
            icon = SpriteName.WarsUnitIcon_Archer;
        }        
    }

}
