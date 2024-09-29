using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.DSSWars.GameObject
{
    class SoldierData : AbsSoldierProfile
    {
        public SoldierData()
        {
            unitType = UnitType.Soldier;

            modelScale = DssConst.Men_StandardModelScale;
            boundRadius =DssVar.StandardBoundRadius;

            walkingSpeed = DssConst.Men_StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 0.04f;
            basehealth = DssConst.Soldier_DefaultHealth;
            mainAttack = AttackType.Melee;
            attackDamage = 50;
            attackDamageStructure = attackDamage;
            attackDamageSea = attackDamage;
            attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;

            setupJavelinCommand();
            modelName = LootFest.VoxelModelName.wars_soldier;
            modelVariationCount = 3;

            description = DssRef.lang.UnitType_Description_Soldier;
            icon = SpriteName.WarsUnitIcon_Soldier;
        }
    }

}
