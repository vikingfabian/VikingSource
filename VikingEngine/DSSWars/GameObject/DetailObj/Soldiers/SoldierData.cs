using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.DSSWars.GameObject
{
    class SoldierData : AbsSoldierData
    {
        public SoldierData()
        {
            unitType = UnitType.Soldier;

            modelScale = StandardModelScale;
            boundRadius = StandardBoundRadius;

            walkingSpeed = StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 0.04f;
            basehealth = DefaultHealth;
            mainAttack = AttackType.Melee;
            attackDamage = 50;
            attackDamageStructure = attackDamage;
            attackDamageSea = attackDamage;
            attackTimePlusCoolDown = StandardAttackAndCoolDownTime;

            setupJavelinCommand();
            modelName = LootFest.VoxelModelName.wars_soldier;
            modelVariationCount = 3;

            description = "A general purpose unit.";
            icon = SpriteName.WarsUnitIcon_Soldier;
        }
    }

}
