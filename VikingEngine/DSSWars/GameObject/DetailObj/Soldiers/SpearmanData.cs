using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.DSSWars.GameObject
{
    class SpearmanData : AbsSoldierData
    {
        public SpearmanData()
        {
            unitType = UnitType.Spearman;

            modelScale = StandardModelScale;
            boundRadius = StandardBoundRadius;

            walkingSpeed = StandardWalkingSpeed * 0.8f;
            rotationSpeed = StandardRotatingSpeed * 0.5f;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 0.06f;
            basehealth = DefaultHealth;
            mainAttack = AttackType.Melee;
            attackDamage = 5;
            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 1.5f;

            modelName = LootFest.VoxelModelName.war_spearman;

            idleBlinkFrame = 0;
            //shieldDamageReduction = 4;
            turnTowardsDamage = true;
        }
    }
}
