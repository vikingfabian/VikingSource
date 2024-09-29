using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.DSSWars.GameObject
{
    class HonorGuardData : AbsSoldierProfile
    {
        public HonorGuardData()
        {
            unitType = UnitType.HonorGuard;

            modelScale = DssConst.Men_StandardModelScale;
            boundRadius = DssVar.StandardBoundRadius;

            walkingSpeed = DssConst.Men_StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 0.06f;
            basehealth = DssConst.Soldier_DefaultHealth * 2;
            mainAttack = AttackType.Melee;
            attackDamage = 50;
            attackDamageStructure = attackDamage;
            attackDamageSea = attackDamage;
            attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 1.5f;

            setupJavelinCommand();

            upkeepPerSoldier = 0;

            modelName = LootFest.VoxelModelName.little_hirdman;
            icon = SpriteName.WarsUnitIcon_Honorguard;
            description = DssRef.lang.UnitType_Description_HonorGuard;
        }
    }
}
